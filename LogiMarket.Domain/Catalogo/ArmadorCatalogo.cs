using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Catalogo;

// Convierte lo guardado en la base en el arbol Composite y detecta ciclos
public sealed class ArmadorCatalogo
{
    private readonly Dictionary<string, Producto> _productos;
    private readonly Dictionary<string, Kit> _kits;
    private readonly Dictionary<string, IElementoCatalogo> _armados = new();

    public ArmadorCatalogo(IEnumerable<Producto> productos, IEnumerable<Kit> kits)
    {
        _productos = productos.ToDictionary(p => p.Sku);
        _kits = kits.ToDictionary(k => k.Codigo);
    }

    public IElementoCatalogo Obtener(string codigo) =>
        Armar(codigo.Trim().ToUpperInvariant(), new HashSet<string>());

    public IReadOnlyList<IElementoCatalogo> Todos() =>
        _productos.Keys.Concat(_kits.Keys).Select(Obtener).ToList();

    private IElementoCatalogo Armar(string codigo, HashSet<string> enCamino)
    {
        if (_armados.TryGetValue(codigo, out var listo))
            return listo;

        if (_productos.TryGetValue(codigo, out var producto))
            return _armados[codigo] = ProductoSimple.Desde(producto);

        if (!_kits.TryGetValue(codigo, out var kit))
            throw new ReglaNegocioException($"El código {codigo} no existe en el catálogo.");

        if (!enCamino.Add(codigo))
            throw new ReglaNegocioException($"Referencia circular detectada en el kit {codigo}.");

        var compuesto = new KitCompuesto(kit.Codigo, kit.Nombre);
        foreach (var componente in kit.Componentes)
            compuesto.Agregar(Armar(componente.CodigoComponente, enCamino), componente.Cantidad);

        enCamino.Remove(codigo);
        return _armados[codigo] = compuesto;
    }
}
