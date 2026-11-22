using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Application.Puertos;

public interface IPedidoEscritura
{
    Task<Pedido?> ObtenerAsync(int id);

    void Agregar(Pedido pedido);
}
