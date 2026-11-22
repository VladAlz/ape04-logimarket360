using LogiMarket.Application.Comun;
using LogiMarket.Application.Confirmacion;

namespace LogiMarket.Application.Comandos;

public sealed class ConfirmarPedidoCommand : IComandoPedido
{
    private readonly ConfirmacionPedidoFacade _receptor;
    private readonly SolicitudConfirmacion _solicitud;

    public ConfirmarPedidoCommand(ConfirmacionPedidoFacade receptor, SolicitudConfirmacion solicitud)
    {
        _receptor = receptor;
        _solicitud = solicitud;
    }

    public string Nombre => "ConfirmarPedido";
    public string Entrada => $"pedido={_solicitud.PedidoId}; clave={_solicitud.ClaveIdempotencia}";

    // Queda disponible para mostrar las trazas del cobro
    public ResultadoConfirmacion? Resultado { get; private set; }

    public async Task<ResultadoOperacion> EjecutarAsync()
    {
        Resultado = await _receptor.ConfirmarAsync(_solicitud);
        return new ResultadoOperacion(Resultado.Exito, Resultado.Mensaje);
    }
}
