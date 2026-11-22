using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Catalogo;

public class Producto
{
    public int Id { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string Categoria { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public decimal Precio { get; private set; }
    public decimal PesoKg { get; private set; }
    public int Stock { get; private set; }

    // Token de concurrencia, sube con cada cambio de stock
    public int Version { get; private set; }

    private Producto()
    {
    }

    public Producto(string sku, string nombre, string categoria, string descripcion,
        decimal precio, decimal pesoKg, int stock)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ReglaNegocioException("El SKU es obligatorio.");
        if (stock < 0)
            throw new ReglaNegocioException("El stock no puede ser negativo.");

        Sku = sku.Trim().ToUpperInvariant();
        Stock = stock;
        Actualizar(nombre, categoria, descripcion, precio, pesoKg);
    }

    public void Actualizar(string nombre, string categoria, string descripcion, decimal precio, decimal pesoKg)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ReglaNegocioException("El nombre del producto es obligatorio.");
        if (precio <= 0)
            throw new ReglaNegocioException("El precio debe ser mayor que cero.");
        if (pesoKg < 0)
            throw new ReglaNegocioException("El peso no puede ser negativo.");

        Nombre = nombre.Trim();
        Categoria = string.IsNullOrWhiteSpace(categoria) ? "General" : categoria.Trim();
        Descripcion = descripcion.Trim();
        Precio = Dinero.Redondear(precio);
        PesoKg = pesoKg;
    }

    public void DescontarStock(int cantidad)
    {
        if (cantidad <= 0)
            throw new ReglaNegocioException("La cantidad a descontar debe ser positiva.");
        if (cantidad > Stock)
            throw new ReglaNegocioException($"Stock insuficiente de {Sku}: se requieren {cantidad} y hay {Stock}.");

        Stock -= cantidad;
        Version++;
    }

    public void ReintegrarStock(int cantidad)
    {
        if (cantidad <= 0)
            throw new ReglaNegocioException("La cantidad a reintegrar debe ser positiva.");

        Stock += cantidad;
        Version++;
    }

    // Devuelve la diferencia para registrar el movimiento
    public int AjustarStock(int nuevoStock)
    {
        if (nuevoStock < 0)
            throw new ReglaNegocioException("El stock no puede ser negativo.");

        var diferencia = nuevoStock - Stock;
        Stock = nuevoStock;
        Version++;
        return diferencia;
    }
}
