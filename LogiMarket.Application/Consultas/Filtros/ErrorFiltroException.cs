namespace LogiMarket.Application.Consultas.Filtros;

public class ErrorFiltroException : Exception
{
    public int Posicion { get; }

    public ErrorFiltroException(string mensaje, int posicion)
        : base($"Error de sintaxis en la posición {posicion + 1}: {mensaje}")
    {
        Posicion = posicion;
    }
}
