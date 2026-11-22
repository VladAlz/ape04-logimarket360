using LogiMarket.Domain.Catalogo.Visitantes;
using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Catalogo;

public sealed class KitCompuesto : IElementoCatalogo
{
    private readonly List<(IElementoCatalogo Elemento, int Cantidad)> _hijos = new();

    public string Codigo { get; }
    public string Nombre { get; }
    public bool EsKit => true;

    // Precio y peso salen de los hijos, no se guardan aparte
    public decimal Precio => _hijos.Sum(h => h.Elemento.Precio * h.Cantidad);
    public decimal PesoKg => _hijos.Sum(h => h.Elemento.PesoKg * h.Cantidad);

    // Solo el iterador y el armador recorren los hijos
    internal IReadOnlyList<(IElementoCatalogo Elemento, int Cantidad)> Hijos => _hijos;

    public KitCompuesto(string codigo, string nombre)
    {
        Codigo = codigo;
        Nombre = nombre;
    }

    public void Agregar(IElementoCatalogo elemento, int cantidad)
    {
        if (cantidad <= 0)
            throw new ReglaNegocioException("La cantidad del componente debe ser positiva.");

        // Si el hijo ya contiene a este kit se cerraria un ciclo
        if (elemento.Contiene(Codigo))
            throw new ReglaNegocioException(
                $"Referencia circular: {elemento.Codigo} ya contiene a {Codigo}.");

        _hijos.Add((elemento, cantidad));
    }

    public bool Contiene(string codigo) =>
        Codigo == codigo || _hijos.Any(h => h.Elemento.Contiene(codigo));

    public void Aceptar(IVisitanteCatalogo visitante, int cantidad)
    {
        visitante.VisitarKit(this, cantidad);
        foreach (var (elemento, cantidadHijo) in _hijos)
            elemento.Aceptar(visitante, cantidad * cantidadHijo);
    }

    public IIteradorComponentes CrearIterador(int cantidad) =>
        new IteradorComponentesFisicos(this, cantidad);
}
