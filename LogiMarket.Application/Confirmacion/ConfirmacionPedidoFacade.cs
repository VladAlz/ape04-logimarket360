using LogiMarket.Application.Comun;
using LogiMarket.Application.Eventos;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Application.Validacion;
using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Inventario;
using LogiMarket.Domain.Pagos;
using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Application.Confirmacion;

// Facade: una sola operacion para confirmar; las reglas quedan en sus colaboradores
public sealed class ConfirmacionPedidoFacade
{
    private readonly ServicioAutorizacion _autorizacion;
    private readonly CadenaValidacionPedido _cadena;
    private readonly IPedidoEscritura _pedidos;
    private readonly ICatalogoEscritura _catalogo;
    private readonly IPagoRepositorio _pagos;
    private readonly IInventarioRepositorio _inventario;
    private readonly IOutboxRepositorio _outbox;
    private readonly IPasarelaPago _pasarela;
    private readonly IUnidadTrabajo _unidad;
    private readonly ProcesadorOutbox _procesador;
    private readonly HistorialEdiciones _historial;
    private readonly ISesionUsuario _sesion;
    private readonly IReloj _reloj;
    private readonly IBitacora _bitacora;
    private readonly MetricasSesion _metricas;

    public ConfirmacionPedidoFacade(ServicioAutorizacion autorizacion, CadenaValidacionPedido cadena,
        IPedidoEscritura pedidos, ICatalogoEscritura catalogo, IPagoRepositorio pagos, IInventarioRepositorio inventario,
        IOutboxRepositorio outbox, IPasarelaPago pasarela, IUnidadTrabajo unidad, ProcesadorOutbox procesador,
        HistorialEdiciones historial, ISesionUsuario sesion, IReloj reloj, IBitacora bitacora, MetricasSesion metricas)
    {
        _autorizacion = autorizacion;
        _cadena = cadena;
        _pedidos = pedidos;
        _catalogo = catalogo;
        _pagos = pagos;
        _inventario = inventario;
        _outbox = outbox;
        _pasarela = pasarela;
        _unidad = unidad;
        _procesador = procesador;
        _historial = historial;
        _sesion = sesion;
        _reloj = reloj;
        _bitacora = bitacora;
        _metricas = metricas;
    }

    public async Task<ResultadoConfirmacion> ConfirmarAsync(SolicitudConfirmacion solicitud)
    {
        var inicioTrazas = _bitacora.RegistrosDelAmbito.Count;

        if (string.IsNullOrWhiteSpace(solicitud.ClaveIdempotencia))
            return Rechazo(solicitud.PedidoId, "-", "La confirmación requiere una clave de idempotencia.", inicioTrazas);

        // Misma clave: se devuelve lo que ya paso, sin cobrar otra vez
        var previo = await _pagos.ObtenerPorClaveAsync(solicitud.ClaveIdempotencia);
        if (previo is not null)
            return await Repetido(previo, inicioTrazas);

        Pedido? pedido = null;
        try
        {
            await _autorizacion.ExigirAsync(Operacion.CobrarPedido);
            pedido = await _pedidos.ObtenerAsync(solicitud.PedidoId)
                ?? throw new ReglaNegocioException($"El pedido {solicitud.PedidoId} no existe.");

            if (await _pagos.ContarPorPedidoAsync(pedido.Id) > 0)
                throw new ReglaNegocioException($"El pedido {pedido.Id} ya tiene un cobro aprobado.");

            var validacion = await _cadena.ValidarAsync(pedido, Operacion.CobrarPedido);
            if (!validacion.Aprobado)
                throw new ReglaNegocioException(
                    $"Validación detenida en «{validacion.Eslabon}»: {validacion.Mensaje} (recorrido: {validacion.RecorridoTexto})");

            var cobro = await _unidad.EnTransaccionAsync(() => CobrarYRegistrarAsync(pedido, solicitud.ClaveIdempotencia));

            _historial.Limpiar(pedido.Id);
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Pedido,
                $"Pedido {pedido.Id} PAGADO por {Presentacion.Monto(pedido.Total)}. Ref {cobro.Referencia}.", solicitud.ClaveIdempotencia);

            // Los eventos ya quedaron guardados; si un aviso falla, sigue pendiente
            var outbox = await _procesador.ProcesarPendientesAsync();

            return new ResultadoConfirmacion(true, false,
                $"Pedido {pedido.Id} confirmado y PAGADO.{Environment.NewLine}{cobro.Mensaje}{Environment.NewLine}" +
                $"Outbox: {outbox.Procesados} procesado(s), {outbox.Fallidos} pendiente(s).",
                pedido.Id, pedido.EstadoCodigo, pedido.Total, cobro.Referencia, TrazasDesde(inicioTrazas));
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException or OperacionRechazadaException)
        {
            // Si otra solicitud con la misma clave gano la carrera, se devuelve su resultado
            var ganador = await _pagos.ObtenerPorClaveAsync(solicitud.ClaveIdempotencia);
            if (ganador is not null)
                return await Repetido(ganador, inicioTrazas);

            if (ex is OperacionRechazadaException)
                _metricas.Incrementar(MetricasSesion.CobrosRechazados);

            await _bitacora.RegistrarAsync(CategoriaAuditoria.Pedido,
                $"Confirmación del pedido {solicitud.PedidoId} rechazada: {ex.Message}", solicitud.ClaveIdempotencia);

            var estado = pedido is null ? "-" : await EstadoActualAsync(pedido.Id);
            return Rechazo(solicitud.PedidoId, estado, ex.Message, inicioTrazas);
        }
    }

    // Todo dentro de la transaccion: si algo falla no queda stock, pago ni evento
    private async Task<Pagos.ResultadoCobro> CobrarYRegistrarAsync(Pedido pedido, string clave)
    {
        var ahora = _reloj.Ahora;
        var requerimiento = pedido.RequerimientoStock();
        var productos = await _catalogo.ObtenerParaStockAsync(requerimiento.Keys);

        foreach (var (sku, cantidad) in requerimiento)
        {
            var producto = productos.SingleOrDefault(p => p.Sku == sku)
                ?? throw new ReglaNegocioException($"El SKU {sku} ya no existe en el catálogo.");
            producto.DescontarStock(cantidad);
        }

        // Se guarda antes de cobrar para detectar a tiempo una venta concurrente
        await _unidad.GuardarCambiosAsync();

        Pagos.ResultadoCobro cobro;
        try
        {
            cobro = await _pasarela.CobrarAsync(pedido.Total, clave);
        }
        catch (Exception ex)
        {
            // Un error tecnico del proveedor se trata como cobro no aprobado
            throw new OperacionRechazadaException($"Error de la pasarela: {ex.Message}");
        }

        if (!cobro.Aprobado)
            throw new OperacionRechazadaException(cobro.Mensaje);

        pedido.MarcarPagado(ahora, _sesion.NombreUsuario, cobro.Referencia);
        _pagos.Agregar(new Pago(pedido.Id, pedido.Total, clave, cobro.Referencia, cobro.Proveedor, ahora));

        foreach (var producto in productos)
            _inventario.Agregar(new MovimientoInventario(producto.Sku, -requerimiento[producto.Sku], producto.Stock,
                MovimientoInventario.Venta, $"PED-{pedido.Id}", ahora, _sesion.NombreUsuario));

        _outbox.Agregar(EventoNegocio.CrearOutbox(TiposEvento.PedidoPagado, pedido.Id,
            $"Recibimos el pago de su pedido {pedido.Id} por USD {Presentacion.Monto(pedido.Total)}.", pedido.Total, ahora));

        await _unidad.GuardarCambiosAsync();
        return cobro;
    }

    private async Task<ResultadoConfirmacion> Repetido(Pago pago, int inicioTrazas)
    {
        var estado = await EstadoActualAsync(pago.PedidoId);
        await _bitacora.RegistrarAsync(CategoriaAuditoria.Pedido,
            $"Clave {pago.ClaveIdempotencia} ya usada: se devuelve el resultado original sin cobrar otra vez.", pago.ClaveIdempotencia);

        return new ResultadoConfirmacion(true, true,
            $"El pedido {pago.PedidoId} ya fue confirmado con esta clave. Resultado original: pago {pago.Referencia} por {Presentacion.Monto(pago.Monto)}.",
            pago.PedidoId, estado, pago.Monto, pago.Referencia, TrazasDesde(inicioTrazas));
    }

    private async Task<string> EstadoActualAsync(int pedidoId) =>
        (await _pedidos.ObtenerAsync(pedidoId))?.EstadoCodigo ?? "-";

    private ResultadoConfirmacion Rechazo(int pedidoId, string estado, string mensaje, int inicioTrazas) =>
        new(false, false, mensaje, pedidoId, estado, 0, string.Empty, TrazasDesde(inicioTrazas));

    private IReadOnlyList<string> TrazasDesde(int inicio) =>
        _bitacora.RegistrosDelAmbito.Skip(inicio).Select(r => $"[{r.Categoria}] {r.Mensaje}").ToList();
}
