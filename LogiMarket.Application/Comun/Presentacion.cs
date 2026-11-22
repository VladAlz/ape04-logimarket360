using System.Globalization;

namespace LogiMarket.Application.Comun;

public static class Presentacion
{
    // Siempre punto decimal y dos cifras, sin depender de la cultura del equipo
    public static string Monto(decimal valor) => valor.ToString("0.00", CultureInfo.InvariantCulture);

    public static string Peso(decimal valor) => valor.ToString("0.00", CultureInfo.InvariantCulture);
}
