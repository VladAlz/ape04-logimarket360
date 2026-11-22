namespace LogiMarket.Domain.Pedidos.Estados;

public sealed class EstadoValidado : EstadoPedido
{
    public override string Codigo => "VALIDADO";
    public override EstadoPedido Pagar() => EstadosPedido.Pagado;
    public override EstadoPedido Cancelar() => EstadosPedido.Cancelado;
}
