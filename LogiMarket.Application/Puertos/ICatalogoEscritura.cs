using LogiMarket.Domain.Catalogo;

namespace LogiMarket.Application.Puertos;

public interface ICatalogoEscritura
{
    Task<Producto?> ObtenerProductoAsync(string sku);

    Task<Kit?> ObtenerKitAsync(string codigo);

    // Lee el stock vigente aunque la entidad ya estuviera cargada
    Task<IReadOnlyList<Producto>> ObtenerParaStockAsync(IEnumerable<string> skus);

    void AgregarProducto(Producto producto);

    void EliminarProducto(Producto producto);

    void AgregarKit(Kit kit);
}
