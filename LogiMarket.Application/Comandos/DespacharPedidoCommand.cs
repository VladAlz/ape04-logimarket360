using LogiMarket.Application.Comun;
using LogiMarket.Application.Pedidos;

namespace LogiMarket.Application.Comandos;

public sealed class DespacharPedidoCommand : IComandoPedido
{
    private readonly DespacharPedidoUseCase _receptor;
    private readonly int _pedidoId;

    public DespacharPedidoCommand(DespacharPedidoUseCase receptor, int pedidoId)
    {
        _receptor = receptor;
        _pedidoId = pedidoId;
    }

    public string Nombre => "DespacharPedido";
    public string Entrada => $"pedido={_pedidoId}";

    public Task<ResultadoOperacion> EjecutarAsync() => _receptor.EjecutarAsync(_pedidoId);
}
