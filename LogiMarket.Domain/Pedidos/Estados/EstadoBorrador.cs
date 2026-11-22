namespace LogiMarket.Domain.Pedidos.Estados;

public sealed class EstadoBorrador : EstadoPedido
{
    public override string Codigo => "BORRADOR";
    public override bool PermiteEdicion => true;
    public override EstadoPedido Validar() => EstadosPedido.Validado;
    public override EstadoPedido Cancelar() => EstadosPedido.Cancelado;
}
