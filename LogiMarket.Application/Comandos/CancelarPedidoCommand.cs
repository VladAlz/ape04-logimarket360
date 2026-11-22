using LogiMarket.Application.Comun;
using LogiMarket.Application.Pedidos;

namespace LogiMarket.Application.Comandos;

public sealed class CancelarPedidoCommand : IComandoPedido
{
    private readonly CancelarPedidoUseCase _receptor;
    private readonly int _pedidoId;
    private readonly string _motivo;

    public CancelarPedidoCommand(CancelarPedidoUseCase receptor, int pedidoId, string motivo)
    {
        _receptor = receptor;
        _pedidoId = pedidoId;
        _motivo = motivo;
    }

    public string Nombre => "CancelarPedido";
    public string Entrada => $"pedido={_pedidoId}; motivo={_motivo}";

    public Task<ResultadoOperacion> EjecutarAsync() => _receptor.EjecutarAsync(_pedidoId, _motivo);
}
