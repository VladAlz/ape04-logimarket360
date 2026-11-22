using LogiMarket.Domain.Catalogo.Visitantes;

namespace LogiMarket.Domain.Catalogo;

public sealed class ProductoSimple : IElementoCatalogo
{
    public string Codigo { get; }
    public string Nombre { get; }
    public bool EsKit => false;
    public decimal Precio { get; }
    public decimal PesoKg { get; }

    public ProductoSimple(string sku, string nombre, decimal precio, decimal pesoKg)
    {
        Codigo = sku;
        Nombre = nombre;
        Precio = precio;
        PesoKg = pesoKg;
    }

    public static ProductoSimple Desde(Producto producto) =>
        new(producto.Sku, producto.Nombre, producto.Precio, producto.PesoKg);

    public bool Contiene(string codigo) => Codigo == codigo;

    public void Aceptar(IVisitanteCatalogo visitante, int cantidad) =>
        visitante.VisitarProducto(this, cantidad);

    public IIteradorComponentes CrearIterador(int cantidad) =>
        new IteradorComponentesFisicos(this, cantidad);
}
