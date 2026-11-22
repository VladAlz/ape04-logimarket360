using LogiMarket.Domain.Eventos;

namespace LogiMarket.Application.Puertos;

public interface IOutboxRepositorio
{
    void Agregar(EventoOutbox evento);

    Task<IReadOnlyList<EventoOutbox>> ListarPendientesAsync();

    Task<IReadOnlyList<EventoOutbox>> ListarAsync();
}
