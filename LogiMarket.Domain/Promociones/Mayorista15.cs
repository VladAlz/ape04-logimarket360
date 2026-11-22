namespace LogiMarket.Domain.Promociones;

// Aplica por volumen sin importar la categoria del cliente
public sealed class Mayorista15 : IPoliticaDescuento
{
    public const int UnidadesMinimas = 10;

    public string Nombre => "MAYORISTA";
    public int Prioridad => 20;
    public decimal Tasa => 0.15m;

    public bool Aplica(ContextoDescuento contexto) => contexto.UnidadesFisicas >= UnidadesMinimas;
}
