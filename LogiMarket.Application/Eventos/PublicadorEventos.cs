namespace LogiMarket.Application.Eventos;

// Sujeto del Observer: no conoce a los suscriptores concretos
public sealed class PublicadorEventos
{
    private readonly List<IObservadorEvento> _observadores = new();

    public PublicadorEventos(IEnumerable<IObservadorEvento> observadores)
    {
        foreach (var observador in observadores)
            Suscribir(observador);
    }

    public IReadOnlyList<string> Suscriptores => _observadores.Select(o => o.Nombre).ToList();

    public void Suscribir(IObservadorEvento observador)
    {
        if (_observadores.Contains(observador))
            return;

        _observadores.Add(observador);
        _observadores.Sort((a, b) => a.Orden.CompareTo(b.Orden));
    }

    public void Desuscribir(IObservadorEvento observador) => _observadores.Remove(observador);

    public async Task PublicarAsync(EventoNegocio evento)
    {
        foreach (var observador in _observadores.ToList())
            await observador.AlRecibirAsync(evento);
    }
}
