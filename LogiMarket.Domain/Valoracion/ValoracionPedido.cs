namespace LogiMarket.Domain.Valoracion;

public sealed record ValoracionPedido(
    decimal Subtotal,
    string Politica,
    decimal Tasa,
    decimal Descuento,
    decimal Tarifa,
    int PlazoDias,
    int UnidadesFisicas)
{
    public decimal Total => Subtotal - Descuento + Tarifa;
}
