using System.Collections.Concurrent;
using LogiMarket.Domain.Catalogo;

namespace LogiMarket.Application.VistaCatalogo;

public sealed class CatalogoVisualFlyweightFactory
{
    private readonly ConcurrentDictionary<string, MetadatoSku> _cache = new();

    public int ObjetosCompartidos => _cache.Count;

    public MetadatoSku Obtener(Producto producto) =>
        _cache.GetOrAdd(producto.Sku, _ => new MetadatoSku(producto.Sku, producto.Nombre, producto.Categoria, producto.Descripcion));

    // Si el catalogo cambia los metadatos se vuelven a crear
    public void Reiniciar() => _cache.Clear();
}
