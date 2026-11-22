using LogiMarket.Domain.Clientes;

namespace LogiMarket.Domain.Pedidos;

// Memento: solo el Pedido sabe leer su contenido
public sealed class MementoPedido
{
    internal int PedidoId { get; }
    internal int ClienteId { get; }
    internal string ClienteNombre { get; }
    internal CategoriaCliente Categoria { get; }
    internal FormaEntrega Entrega { get; }
    internal IReadOnlyList<PedidoDetalle> Lineas { get; }

    public DateTime Fecha { get; }
    public string Descripcion { get; }

    internal MementoPedido(Pedido pedido, DateTime fecha, string descripcion)
    {
        PedidoId = pedido.Id;
        ClienteId = pedido.ClienteId;
        ClienteNombre = pedido.ClienteNombre;
        Categoria = pedido.CategoriaCliente;
        Entrega = pedido.Entrega;
        Lineas = pedido.Lineas.Select(l => l.Clonar()).ToList();
        Fecha = fecha;
        Descripcion = descripcion;
    }
}
