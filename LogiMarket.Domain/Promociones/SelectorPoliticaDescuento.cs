namespace LogiMarket.Domain.Promociones;

// Se elige una sola politica; nunca se acumulan
public sealed class SelectorPoliticaDescuento
{
    private readonly IReadOnlyList<IPoliticaDescuento> _politicas;

    public SelectorPoliticaDescuento(IEnumerable<IPoliticaDescuento> politicas)
    {
        _politicas = politicas.OrderByDescending(p => p.Prioridad).ToList();
    }

    public IPoliticaDescuento Elegir(ContextoDescuento contexto) =>
        _politicas.FirstOrDefault(p => p.Aplica(contexto)) ?? new SinDescuento();
}
