using LogiMarket.Domain.Pagos;

namespace LogiMarket.Application.Puertos;

public interface IPagoRepositorio
{
    Task<Pago?> ObtenerPorClaveAsync(string claveIdempotencia);

    Task<Pago?> ObtenerPorPedidoAsync(int pedidoId);

    Task<int> ContarPorPedidoAsync(int pedidoId);

    void Agregar(Pago pago);
}
