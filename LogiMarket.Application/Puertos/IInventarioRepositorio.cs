using LogiMarket.Domain.Inventario;

namespace LogiMarket.Application.Puertos;

public interface IInventarioRepositorio
{
    void Agregar(MovimientoInventario movimiento);

    Task<IReadOnlyList<MovimientoInventario>> ListarAsync(string? sku = null);
}
