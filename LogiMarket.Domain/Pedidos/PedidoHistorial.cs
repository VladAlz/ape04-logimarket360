namespace LogiMarket.Domain.Pedidos;

public class PedidoHistorial
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public string EstadoAnterior { get; private set; } = string.Empty;
    public string EstadoNuevo { get; private set; } = string.Empty;
    public DateTime Fecha { get; private set; }
    public string Usuario { get; private set; } = string.Empty;
    public string Detalle { get; private set; } = string.Empty;

    private PedidoHistorial()
    {
    }

    internal PedidoHistorial(string estadoAnterior, string estadoNuevo, DateTime fecha, string usuario, string detalle)
    {
        EstadoAnterior = estadoAnterior;
        EstadoNuevo = estadoNuevo;
        Fecha = fecha;
        Usuario = usuario;
        Detalle = detalle;
    }
}
