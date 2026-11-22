using LogiMarket.Application.Comun;
using LogiMarket.Application.Eventos;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Devoluciones;
using LogiMarket.Domain.Inventario;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Pedidos.Estados;

namespace LogiMarket.Application.Devoluciones;

public sealed class ServiciosDevolucion
{
    public ServiciosDevolucion(IPedidoEscritura pedidos, ICatalogoEscritura catalogo, IInventarioRepositorio inventario,
        IDevolucionRepositorio devoluciones, IOutboxRepositorio outbox, IUnidadTrabajo unidad,
        ProcesadorOutbox procesador, ServicioAutorizacion autorizacion, ISesionUsuario sesion, IBitacora bitacora)
    {
        Pedidos = pedidos;
        Catalogo = catalogo;
        Inventario = inventario;
        Devoluciones = devoluciones;
        Outbox = outbox;
        Unidad = unidad;
        Procesador = procesador;
        Autorizacion = autorizacion;
        Sesion = sesion;
        Bitacora = bitacora;
    }

    public IPedidoEscritura Pedidos { get; }
    public ICatalogoEscritura Catalogo { get; }
    public IInventarioRepositorio Inventario { get; }
    public IDevolucionRepositorio Devoluciones { get; }
    public IOutboxRepositorio Outbox { get; }
    public IUnidadTrabajo Unidad { get; }
    public ProcesadorOutbox Procesador { get; }
    public ServicioAutorizacion Autorizacion { get; }
    public ISesionUsuario Sesion { get; }
    public IBitacora Bitacora { get; }
}

// Template Method: la secuencia es fija, las variantes solo cambian la inspeccion
public abstract class ProcesoDevolucion
{
    protected ServiciosDevolucion Servicios { get; }

    protected ProcesoDevolucion(ServiciosDevolucion servicios)
    {
        Servicios = servicios;
    }

    public abstract MotivoDevolucion Motivo { get; }

    public abstract int PlazoDias { get; }

    public async Task<ResultadoDevolucion> ProcesarAsync(SolicitudDevolucion solicitud)
    {
        var pasos = new List<string>();
        try
        {
            await Servicios.Autorizacion.ExigirAsync(Operacion.ProcesarDevolucion);
            var pedido = await Servicios.Pedidos.ObtenerAsync(solicitud.PedidoId)
                ?? throw new ReglaNegocioException($"El pedido {solicitud.PedidoId} no existe.");

            await VerificarElegibilidadAsync(pedido);
            pasos.Add("1. Elegibilidad verificada");

            var dias = (solicitud.FechaSolicitud.Date - pedido.FechaEntrega!.Value.Date).Days;
            InspeccionarCondicion(solicitud, dias);
            pasos.Add($"2. Condición aceptada ({Motivo.ToString().ToUpperInvariant()}, día {dias} de {PlazoDias})");

            var reembolso = CalcularReembolso(pedido);
            pasos.Add($"3. Reembolso calculado: {Presentacion.Monto(reembolso)}");

            await RegistrarDevolucionYStockAsync(pedido, solicitud, dias, reembolso);
            pasos.Add("4. Devolución y reintegro de stock registrados");

            var outbox = await Servicios.Procesador.ProcesarPendientesAsync();
            pasos.Add($"5. Evento DevolucionAceptada generado ({outbox.Procesados} procesado(s))");

            return new ResultadoDevolucion(true,
                $"Devolución aceptada para el pedido {pedido.Id}. Reembolso USD {Presentacion.Monto(reembolso)} (sin tarifa de envío).",
                pedido.Id, reembolso, pasos);
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException or OperacionRechazadaException)
        {
            await Servicios.Bitacora.RegistrarAsync(CategoriaAuditoria.Pedido,
                $"Devolución {Motivo.ToString().ToUpperInvariant()} del pedido {solicitud.PedidoId} rechazada: {ex.Message}");
            pasos.Add($"Rechazada: {ex.Message}");
            return new ResultadoDevolucion(false, $"Devolución rechazada: {ex.Message}", solicitud.PedidoId, 0, pasos);
        }
    }

    private async Task VerificarElegibilidadAsync(Pedido pedido)
    {
        if (pedido.Estado != EstadosPedido.Entregado || pedido.FechaEntrega is null)
            throw new ReglaNegocioException($"Solo se devuelven pedidos ENTREGADOS; el pedido {pedido.Id} está {pedido.EstadoCodigo}.");
        if (pedido.Devuelto || await Servicios.Devoluciones.ObtenerPorPedidoAsync(pedido.Id) is not null)
            throw new ReglaNegocioException($"El pedido {pedido.Id} ya fue devuelto.");
    }

    // Paso que cambia en cada variante
    protected abstract void InspeccionarCondicion(SolicitudDevolucion solicitud, int diasDesdeEntrega);

    // Productos despues del descuento; la tarifa de envio no se devuelve
    protected virtual decimal CalcularReembolso(Pedido pedido) => Dinero.Redondear(pedido.ImporteProductos);

    private Task RegistrarDevolucionYStockAsync(Pedido pedido, SolicitudDevolucion solicitud, int dias, decimal reembolso) =>
        Servicios.Unidad.EnTransaccionAsync(async () =>
        {
            var usuario = Servicios.Sesion.NombreUsuario;
            var fecha = solicitud.FechaSolicitud;
            var requerimiento = pedido.RequerimientoStock();
            var productos = await Servicios.Catalogo.ObtenerParaStockAsync(requerimiento.Keys);

            pedido.RegistrarDevolucion(fecha, usuario, $"Devolución {Motivo.ToString().ToUpperInvariant()}, reembolso {Presentacion.Monto(reembolso)}.");

            foreach (var producto in productos)
            {
                producto.ReintegrarStock(requerimiento[producto.Sku]);
                Servicios.Inventario.Agregar(new MovimientoInventario(producto.Sku, requerimiento[producto.Sku], producto.Stock,
                    MovimientoInventario.Devolucion, $"DEV-{pedido.Id}", fecha, usuario));
            }

            Servicios.Devoluciones.Agregar(new Devolucion(pedido.Id, Motivo, fecha, dias, reembolso, $"RMB-{pedido.Id:0000}-{fecha:yyyyMMdd}", usuario));
            Servicios.Outbox.Agregar(EventoNegocio.CrearOutbox(TiposEvento.DevolucionAceptada, pedido.Id,
                $"Aceptamos la devolución del pedido {pedido.Id}; reembolso USD {Presentacion.Monto(reembolso)}.", reembolso, fecha));

            await Servicios.Unidad.GuardarCambiosAsync();
            return true;
        });
}
