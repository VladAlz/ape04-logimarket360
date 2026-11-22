using LogiMarket.Application.Comun;
using LogiMarket.Application.Confirmacion;
using LogiMarket.Application.Devoluciones;
using LogiMarket.Application.Pedidos;

namespace LogiMarket.Application.Comandos;

// Punto unico para la pantalla y las pruebas: arma el comando y lo pasa al invocador
public sealed class ServicioComandosPedido
{
    private readonly InvocadorComandos _invocador;
    private readonly ConfirmacionPedidoFacade _confirmacion;
    private readonly CancelarPedidoUseCase _cancelar;
    private readonly DespacharPedidoUseCase _despachar;
    private readonly SelectorDevolucion _devoluciones;

    public ServicioComandosPedido(InvocadorComandos invocador, ConfirmacionPedidoFacade confirmacion,
        CancelarPedidoUseCase cancelar, DespacharPedidoUseCase despachar, SelectorDevolucion devoluciones)
    {
        _invocador = invocador;
        _confirmacion = confirmacion;
        _cancelar = cancelar;
        _despachar = despachar;
        _devoluciones = devoluciones;
    }

    public async Task<ResultadoConfirmacion> ConfirmarAsync(SolicitudConfirmacion solicitud)
    {
        var comando = new ConfirmarPedidoCommand(_confirmacion, solicitud);
        await _invocador.EjecutarAsync(comando);
        return comando.Resultado!;
    }

    public Task<ResultadoOperacion> CancelarAsync(int pedidoId, string motivo) =>
        _invocador.EjecutarAsync(new CancelarPedidoCommand(_cancelar, pedidoId, motivo));

    public Task<ResultadoOperacion> DespacharAsync(int pedidoId) =>
        _invocador.EjecutarAsync(new DespacharPedidoCommand(_despachar, pedidoId));

    public async Task<ResultadoDevolucion> DevolverAsync(SolicitudDevolucion solicitud)
    {
        var comando = new ProcesarDevolucionCommand(_devoluciones, solicitud);
        await _invocador.EjecutarAsync(comando);
        return comando.Resultado!;
    }
}
