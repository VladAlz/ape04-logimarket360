namespace LogiMarket.Domain.Pedidos.Estados;

public sealed class EstadoCancelado : EstadoPedido
{
    public override string Codigo => "CANCELADO";
}
