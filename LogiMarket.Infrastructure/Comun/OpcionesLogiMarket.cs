namespace LogiMarket.Infrastructure.Comun;

public sealed record OpcionesLogiMarket(string RutaBaseDatos, string CarpetaSalidas)
{
    public string CadenaConexion => $"Data Source={RutaBaseDatos};Default Timeout=30";
}
