using LogiMarket.Domain.Devoluciones;

namespace LogiMarket.Application.Puertos;

public interface IDevolucionRepositorio
{
    void Agregar(Devolucion devolucion);

    Task<Devolucion?> ObtenerPorPedidoAsync(int pedidoId);

    Task<IReadOnlyList<Devolucion>> ListarAsync();
}
