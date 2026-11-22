namespace LogiMarket.Domain.Devoluciones;

public class Devolucion
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public MotivoDevolucion Motivo { get; private set; }
    public DateTime FechaSolicitud { get; private set; }
    public int DiasDesdeEntrega { get; private set; }
    public decimal Reembolso { get; private set; }
    public string ReferenciaReembolso { get; private set; } = string.Empty;
    public string Usuario { get; private set; } = string.Empty;

    private Devolucion()
    {
    }

    public Devolucion(int pedidoId, MotivoDevolucion motivo, DateTime fechaSolicitud, int diasDesdeEntrega,
        decimal reembolso, string referenciaReembolso, string usuario)
    {
        PedidoId = pedidoId;
        Motivo = motivo;
        FechaSolicitud = fechaSolicitud;
        DiasDesdeEntrega = diasDesdeEntrega;
        Reembolso = reembolso;
        ReferenciaReembolso = referenciaReembolso;
        Usuario = usuario;
    }
}
