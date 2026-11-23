using LogiMarket.Application.Comun;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Comun;

namespace LogiMarket.Application.Pedidos;

public sealed class EntregarPedidoUseCase
{
    private readonly IPedidoEscritura _pedidos;
    private readonly IDespachoRepositorio _despachos;
    private readonly IUnidadTrabajo _unidad;
    private readonly ServicioAutorizacion _autorizacion;
    private readonly ISesionUsuario _sesion;
    private readonly IBitacora _bitacora;
    private readonly MetricasSesion _metricas;

    public EntregarPedidoUseCase(IPedidoEscritura pedidos, IDespachoRepositorio despachos, IUnidadTrabajo unidad,
        ServicioAutorizacion autorizacion, ISesionUsuario sesion, IBitacora bitacora, MetricasSesion metricas)
    {
        _pedidos = pedidos;
        _despachos = despachos;
        _unidad = unidad;
        _autorizacion = autorizacion;
        _sesion = sesion;
        _bitacora = bitacora;
        _metricas = metricas;
    }

    // La fecha se recibe para poder probar los plazos de devolucion
    public async Task<ResultadoOperacion> EjecutarAsync(int pedidoId, DateTime fechaEntrega)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.EntregarPedido);
            var pedido = await _pedidos.ObtenerAsync(pedidoId)
                ?? throw new ReglaNegocioException($"El pedido {pedidoId} no existe.");

            // La entrega no puede quedar antes del despacho
            var despacho = await _despachos.ObtenerPorPedidoAsync(pedidoId);

            pedido.Entregar(fechaEntrega, _sesion.NombreUsuario);
            despacho?.RegistrarEntrega(fechaEntrega);
            await _unidad.GuardarCambiosAsync();

            _metricas.Incrementar(MetricasSesion.Entregas);
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Pedido, $"Pedido {pedidoId} ENTREGADO el {fechaEntrega:dd/MM/yyyy}.");
            return ResultadoOperacion.Correcto($"Pedido {pedidoId} ENTREGADO el {fechaEntrega:dd/MM/yyyy}.");
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException)
        {
            return ResultadoOperacion.Fallo(ex.Message);
        }
    }
}
