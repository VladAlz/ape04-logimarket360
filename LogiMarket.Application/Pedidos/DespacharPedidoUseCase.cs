using LogiMarket.Application.Comun;
using LogiMarket.Application.Eventos;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Despachos;
using LogiMarket.Domain.Entregas;

namespace LogiMarket.Application.Pedidos;

public sealed class DespacharPedidoUseCase
{
    private readonly IPedidoEscritura _pedidos;
    private readonly IDespachoRepositorio _despachos;
    private readonly IOutboxRepositorio _outbox;
    private readonly IEnumerable<IFabricaEntrega> _fabricas;
    private readonly IUnidadTrabajo _unidad;
    private readonly ProcesadorOutbox _procesador;
    private readonly ServicioAutorizacion _autorizacion;
    private readonly ISesionUsuario _sesion;
    private readonly IReloj _reloj;

    public DespacharPedidoUseCase(IPedidoEscritura pedidos, IDespachoRepositorio despachos, IOutboxRepositorio outbox,
        IEnumerable<IFabricaEntrega> fabricas, IUnidadTrabajo unidad, ProcesadorOutbox procesador,
        ServicioAutorizacion autorizacion, ISesionUsuario sesion, IReloj reloj)
    {
        _pedidos = pedidos;
        _despachos = despachos;
        _outbox = outbox;
        _fabricas = fabricas;
        _unidad = unidad;
        _procesador = procesador;
        _autorizacion = autorizacion;
        _sesion = sesion;
        _reloj = reloj;
    }

    public async Task<ResultadoOperacion> EjecutarAsync(int pedidoId)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.DespacharPedido);
            var pedido = await _pedidos.ObtenerAsync(pedidoId)
                ?? throw new ReglaNegocioException($"El pedido {pedidoId} no existe.");

            // La familia de la fabrica sale de la forma de entrega del pedido
            var entrega = ServicioEntrega.Para(pedido.Entrega, _fabricas);
            var ahora = _reloj.Ahora;
            var guia = entrega.Guia.Generar(pedido.Id, pedido.ClienteNombre, ahora, entrega.Plazo);

            await _unidad.EnTransaccionAsync(async () =>
            {
                pedido.Despachar(ahora, _sesion.NombreUsuario, guia.Numero);
                _despachos.Agregar(new Despacho(pedido.Id, guia, ahora));
                _outbox.Agregar(EventoNegocio.CrearOutbox(TiposEvento.DespachoRegistrado, pedido.Id,
                    $"Su pedido {pedido.Id} salió con la guía {guia.Numero}; llegada estimada {guia.FechaEstimada:dd/MM/yyyy}.",
                    pedido.Total, ahora));
                await _unidad.GuardarCambiosAsync();
                return true;
            });

            var outbox = await _procesador.ProcesarPendientesAsync();
            return ResultadoOperacion.Correcto(
                $"Pedido {pedidoId} DESPACHADO.{Environment.NewLine}Guía {guia.Numero} ({entrega.Plazo.Dias} día(s)).{Environment.NewLine}" +
                $"{guia.Etiqueta}{Environment.NewLine}Outbox: {outbox.Procesados} procesado(s), {outbox.Fallidos} pendiente(s).");
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException or OperacionRechazadaException)
        {
            return ResultadoOperacion.Fallo(ex.Message);
        }
    }
}
