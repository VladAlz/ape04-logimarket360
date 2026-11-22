using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Comun;

namespace LogiMarket.Application.Pedidos;

public sealed class CancelarPedidoUseCase
{
    private readonly IPedidoEscritura _pedidos;
    private readonly HistorialEdiciones _historial;
    private readonly IUnidadTrabajo _unidad;
    private readonly ServicioAutorizacion _autorizacion;
    private readonly ISesionUsuario _sesion;
    private readonly IReloj _reloj;
    private readonly IBitacora _bitacora;

    public CancelarPedidoUseCase(IPedidoEscritura pedidos, HistorialEdiciones historial, IUnidadTrabajo unidad,
        ServicioAutorizacion autorizacion, ISesionUsuario sesion, IReloj reloj, IBitacora bitacora)
    {
        _pedidos = pedidos;
        _historial = historial;
        _unidad = unidad;
        _autorizacion = autorizacion;
        _sesion = sesion;
        _reloj = reloj;
        _bitacora = bitacora;
    }

    public async Task<ResultadoOperacion> EjecutarAsync(int pedidoId, string motivo)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.CancelarPedido);
            var pedido = await _pedidos.ObtenerAsync(pedidoId)
                ?? throw new ReglaNegocioException($"El pedido {pedidoId} no existe.");

            pedido.Cancelar(_reloj.Ahora, _sesion.NombreUsuario, motivo);
            await _unidad.GuardarCambiosAsync();
            _historial.Limpiar(pedidoId);
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Pedido, $"Pedido {pedidoId} CANCELADO. {motivo}");

            return ResultadoOperacion.Correcto($"Pedido {pedidoId} cancelado.");
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException)
        {
            return ResultadoOperacion.Fallo(ex.Message);
        }
    }
}
