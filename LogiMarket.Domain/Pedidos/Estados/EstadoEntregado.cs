namespace LogiMarket.Domain.Pedidos.Estados;

// Ultimo estado del ciclo; la devolucion se registra aparte
public sealed class EstadoEntregado : EstadoPedido
{
    public override string Codigo => "ENTREGADO";
}
