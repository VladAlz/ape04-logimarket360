namespace LogiMarket.Domain.Promociones;

public sealed class SinDescuento : IPoliticaDescuento
{
    public string Nombre => "NORMAL";
    public int Prioridad => 0;
    public decimal Tasa => 0m;

    public bool Aplica(ContextoDescuento contexto) => true;
}
