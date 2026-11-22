using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Pagos;

public class Pago
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public decimal Monto { get; private set; }
    public string ClaveIdempotencia { get; private set; } = string.Empty;
    public string Referencia { get; private set; } = string.Empty;
    public string Proveedor { get; private set; } = string.Empty;
    public DateTime Fecha { get; private set; }

    private Pago()
    {
    }

    public Pago(int pedidoId, decimal monto, string claveIdempotencia, string referencia, string proveedor, DateTime fecha)
    {
        if (monto <= 0)
            throw new ReglaNegocioException("El monto del pago debe ser positivo.");
        if (string.IsNullOrWhiteSpace(claveIdempotencia))
            throw new ReglaNegocioException("El pago requiere clave de idempotencia.");

        PedidoId = pedidoId;
        Monto = monto;
        ClaveIdempotencia = claveIdempotencia;
        Referencia = referencia;
        Proveedor = proveedor;
        Fecha = fecha;
    }
}
