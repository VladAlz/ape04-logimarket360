namespace LogiMarket.Application.VistaCatalogo;

// Fila con Flyweight: solo guarda lo que cambia por renglon
public sealed class FilaCatalogoVisual
{
    public FilaCatalogoVisual(int numero, MetadatoSku metadato, int stock, int cantidad, decimal precioVenta)
    {
        Numero = numero;
        Metadato = metadato;
        Stock = stock;
        Cantidad = cantidad;
        PrecioVenta = precioVenta;
    }

    public int Numero { get; }
    public MetadatoSku Metadato { get; }
    public int Stock { get; }
    public int Cantidad { get; }
    public decimal PrecioVenta { get; }

    public string Sku => Metadato.Sku;
    public string Nombre => Metadato.Nombre;
    public string Categoria => Metadato.Categoria;
    public string Descripcion => Metadato.Descripcion;
}

// Misma fila sin compartir nada, solo para comparar memoria
public sealed class FilaCatalogoCompleta
{
    public FilaCatalogoCompleta(string sku, string nombre, string categoria, string descripcion, int stock, int cantidad, decimal precioVenta)
    {
        Sku = sku;
        Nombre = nombre;
        Categoria = categoria;
        Descripcion = descripcion;
        Stock = stock;
        Cantidad = cantidad;
        PrecioVenta = precioVenta;
    }

    public string Sku { get; }
    public string Nombre { get; }
    public string Categoria { get; }
    public string Descripcion { get; }
    public int Stock { get; }
    public int Cantidad { get; }
    public decimal PrecioVenta { get; }
}
