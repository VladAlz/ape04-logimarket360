namespace LogiMarket.Domain.Inventario;

public class MovimientoInventario
{
    public const string Venta = "VENTA";
    public const string Devolucion = "DEVOLUCION";
    public const string Ajuste = "AJUSTE";
    public const string Inicial = "INICIAL";

    public int Id { get; private set; }
    public string Sku { get; private set; } = string.Empty;

    // Positivo entra, negativo sale
    public int Cantidad { get; private set; }
    public int StockResultante { get; private set; }
    public string Motivo { get; private set; } = string.Empty;
    public string Referencia { get; private set; } = string.Empty;
    public DateTime Fecha { get; private set; }
    public string Usuario { get; private set; } = string.Empty;

    private MovimientoInventario()
    {
    }

    public MovimientoInventario(string sku, int cantidad, int stockResultante, string motivo, string referencia,
        DateTime fecha, string usuario)
    {
        Sku = sku;
        Cantidad = cantidad;
        StockResultante = stockResultante;
        Motivo = motivo;
        Referencia = referencia;
        Fecha = fecha;
        Usuario = usuario;
    }
}
