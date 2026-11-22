namespace LogiMarket.Application.Pagos;

public sealed record ResultadoCobro(bool Aprobado, bool Denegado, string Referencia, string Proveedor, string Mensaje)
{
    public static ResultadoCobro Aprobacion(string referencia, string proveedor) =>
        new(true, false, referencia, proveedor, $"Cobro aprobado por {proveedor} con referencia {referencia}.");

    public static ResultadoCobro Rechazo(string proveedor, string motivo) =>
        new(false, false, string.Empty, proveedor, $"Cobro rechazado por {proveedor}: {motivo}");

    public static ResultadoCobro AccesoDenegado(string motivo) =>
        new(false, true, string.Empty, "-", motivo);
}
