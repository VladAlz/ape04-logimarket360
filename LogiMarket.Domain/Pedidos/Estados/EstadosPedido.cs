using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Pedidos.Estados;

// Los estados no guardan datos, basta una instancia de cada uno
public static class EstadosPedido
{
    public static readonly EstadoPedido Borrador = new EstadoBorrador();
    public static readonly EstadoPedido Validado = new EstadoValidado();
    public static readonly EstadoPedido Pagado = new EstadoPagado();
    public static readonly EstadoPedido Despachado = new EstadoDespachado();
    public static readonly EstadoPedido Entregado = new EstadoEntregado();
    public static readonly EstadoPedido Cancelado = new EstadoCancelado();

    private static readonly Dictionary<string, EstadoPedido> PorCodigo =
        new[] { Borrador, Validado, Pagado, Despachado, Entregado, Cancelado }.ToDictionary(e => e.Codigo);

    public static IReadOnlyCollection<string> Codigos => PorCodigo.Keys;

    public static EstadoPedido Desde(string codigo) =>
        PorCodigo.TryGetValue(codigo, out var estado)
            ? estado
            : throw new ReglaNegocioException($"Estado de pedido desconocido: {codigo}.");
}
