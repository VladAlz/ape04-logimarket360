using LogiMarket.Domain.Despachos;

namespace LogiMarket.Application.Puertos;

public interface IDespachoRepositorio
{
    void Agregar(Despacho despacho);

    Task<Despacho?> ObtenerPorPedidoAsync(int pedidoId);

    Task<Despacho?> ObtenerPorGuiaAsync(string numeroGuia);
}
