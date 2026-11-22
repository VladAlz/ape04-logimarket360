namespace LogiMarket.Domain.Comun;

// Un solo punto de redondeo para todos los importes
public static class Dinero
{
    public static decimal Redondear(decimal valor) =>
        Math.Round(valor, 2, MidpointRounding.AwayFromZero);
}
