using LogiMarket.Domain.Catalogo;
using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Pedidos;

public class PedidoDetalle : IPrototipo<PedidoDetalle>
{
    private readonly List<PedidoDetalleComponente> _componentes = new();

    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public bool EsKit { get; private set; }
    public int Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }
    public IReadOnlyList<PedidoDetalleComponente> Componentes => _componentes;

    public decimal Subtotal => PrecioUnitario * Cantidad;
    public int UnidadesFisicas => Cantidad * _componentes.Sum(c => c.CantidadPorUnidad);

    private PedidoDetalle()
    {
    }

    internal PedidoDetalle(IElementoCatalogo elemento, int cantidad)
    {
        Codigo = elemento.Codigo;
        Nombre = elemento.Nombre;
        EsKit = elemento.EsKit;
        PrecioUnitario = elemento.Precio;
        CambiarCantidad(cantidad);

        // Se congela la composicion de una unidad al momento de registrar
        foreach (var componente in elemento.ExpandirSku(1))
            _componentes.Add(new PedidoDetalleComponente(componente.Sku, componente.Nombre,
                componente.Cantidad, componente.PrecioUnitario, componente.PesoKg));
    }

    private PedidoDetalle(PedidoDetalle original)
    {
        Codigo = original.Codigo;
        Nombre = original.Nombre;
        EsKit = original.EsKit;
        Cantidad = original.Cantidad;
        PrecioUnitario = original.PrecioUnitario;
        _componentes.AddRange(original._componentes.Select(c => c.Clonar()));
    }

    internal void CambiarCantidad(int cantidad)
    {
        if (cantidad <= 0)
            throw new ReglaNegocioException("La cantidad debe ser mayor que cero.");

        Cantidad = cantidad;
    }

    // Copia profunda: lista de componentes nueva y sin Id
    public PedidoDetalle Clonar() => new(this);
}
