using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Catalogo;

// Definicion guardada del kit; el arbol se arma con ArmadorCatalogo
public class Kit
{
    private readonly List<KitComponente> _componentes = new();

    public int Id { get; private set; }
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public IReadOnlyList<KitComponente> Componentes => _componentes;

    private Kit()
    {
    }

    public Kit(string codigo, string nombre)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ReglaNegocioException("El código del kit es obligatorio.");

        Codigo = codigo.Trim().ToUpperInvariant();
        Renombrar(nombre);
    }

    public void Renombrar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ReglaNegocioException("El nombre del kit es obligatorio.");

        Nombre = nombre.Trim();
    }

    public void DefinirComponentes(IEnumerable<(string Codigo, int Cantidad)> componentes)
    {
        var lista = componentes
            .GroupBy(c => c.Codigo.Trim().ToUpperInvariant())
            .Select(g => (Codigo: g.Key, Cantidad: g.Sum(c => c.Cantidad)))
            .ToList();

        if (lista.Count == 0)
            throw new ReglaNegocioException("Un kit necesita al menos un componente.");
        if (lista.Any(c => c.Cantidad <= 0))
            throw new ReglaNegocioException("Las cantidades del kit deben ser positivas.");
        if (lista.Any(c => c.Codigo == Codigo))
            throw new ReglaNegocioException($"El kit {Codigo} no puede contenerse a sí mismo.");

        _componentes.Clear();
        _componentes.AddRange(lista.Select(c => new KitComponente(c.Codigo, c.Cantidad)));
    }
}
