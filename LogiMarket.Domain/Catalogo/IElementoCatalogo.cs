using LogiMarket.Domain.Catalogo.Visitantes;

namespace LogiMarket.Domain.Catalogo;

// Composite: producto y kit se usan igual desde fuera
public interface IElementoCatalogo
{
    string Codigo { get; }
    string Nombre { get; }
    bool EsKit { get; }
    decimal Precio { get; }
    decimal PesoKg { get; }

    bool Contiene(string codigo);

    void Aceptar(IVisitanteCatalogo visitante, int cantidad);

    IIteradorComponentes CrearIterador(int cantidad);
}
