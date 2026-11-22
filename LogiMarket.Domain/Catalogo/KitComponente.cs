namespace LogiMarket.Domain.Catalogo;

public class KitComponente
{
    public int Id { get; private set; }
    public int KitId { get; private set; }
    public string CodigoComponente { get; private set; } = string.Empty;
    public int Cantidad { get; private set; }

    private KitComponente()
    {
    }

    internal KitComponente(string codigoComponente, int cantidad)
    {
        CodigoComponente = codigoComponente;
        Cantidad = cantidad;
    }
}
