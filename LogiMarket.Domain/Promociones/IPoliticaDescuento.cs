namespace LogiMarket.Domain.Promociones;

// Strategy: cada politica dice si aplica y con que tasa
public interface IPoliticaDescuento
{
    string Nombre { get; }

    // Mayor numero gana cuando aplican varias
    int Prioridad { get; }

    decimal Tasa { get; }

    bool Aplica(ContextoDescuento contexto);
}
