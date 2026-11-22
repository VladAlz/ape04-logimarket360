using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Pedidos;

// Instantanea de un SKU final dentro de la linea
public class PedidoDetalleComponente : IPrototipo<PedidoDetalleComponente>
{
    public int Id { get; private set; }
    public int PedidoDetalleId { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public int CantidadPorUnidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }
    public decimal PesoKg { get; private set; }

    private PedidoDetalleComponente()
    {
    }

    internal PedidoDetalleComponente(string sku, string nombre, int cantidadPorUnidad, decimal precioUnitario, decimal pesoKg)
    {
        Sku = sku;
        Nombre = nombre;
        CantidadPorUnidad = cantidadPorUnidad;
        PrecioUnitario = precioUnitario;
        PesoKg = pesoKg;
    }

    public PedidoDetalleComponente Clonar() =>
        new(Sku, Nombre, CantidadPorUnidad, PrecioUnitario, PesoKg);
}
