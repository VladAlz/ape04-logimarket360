namespace LogiMarket.Application.Eventos;

public interface IObservadorEvento
{
    string Nombre { get; }

    // Orden de aviso: el que mas puede fallar va primero
    int Orden { get; }

    Task AlRecibirAsync(EventoNegocio evento);
}
