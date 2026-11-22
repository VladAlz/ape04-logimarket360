namespace LogiMarket.Domain.Pedidos.Estados;

public sealed class EstadoDespachado : EstadoPedido
{
    public override string Codigo => "DESPACHADO";
    public override EstadoPedido Entregar() => EstadosPedido.Entregado;
}
