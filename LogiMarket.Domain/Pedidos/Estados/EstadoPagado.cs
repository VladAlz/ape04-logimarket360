namespace LogiMarket.Domain.Pedidos.Estados;

public sealed class EstadoPagado : EstadoPedido
{
    public override string Codigo => "PAGADO";
    public override EstadoPedido Despachar() => EstadosPedido.Despachado;
}
