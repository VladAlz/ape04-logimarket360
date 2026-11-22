namespace LogiMarket.Application.VistaCatalogo;

// Estado intrinseco del Flyweight: inmutable y compartido
public sealed class MetadatoSku
{
    public string Sku { get; }
    public string Nombre { get; }
    public string Categoria { get; }
    public string Descripcion { get; }

    public MetadatoSku(string sku, string nombre, string categoria, string descripcion)
    {
        Sku = sku;
        Nombre = nombre;
        Categoria = categoria;
        Descripcion = descripcion;
    }
}
