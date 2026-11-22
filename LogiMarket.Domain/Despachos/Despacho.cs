using LogiMarket.Domain.Entregas;

namespace LogiMarket.Domain.Despachos;

public class Despacho
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public string NumeroGuia { get; private set; } = string.Empty;
    public string Familia { get; private set; } = string.Empty;
    public string Etiqueta { get; private set; } = string.Empty;
    public DateTime FechaDespacho { get; private set; }
    public DateTime FechaEstimada { get; private set; }
    public DateTime? FechaEntrega { get; private set; }

    private Despacho()
    {
    }

    public Despacho(int pedidoId, GuiaEntrega guia, DateTime fechaDespacho)
    {
        PedidoId = pedidoId;
        NumeroGuia = guia.Numero;
        Familia = guia.Familia.ToString().ToUpperInvariant();
        Etiqueta = guia.Etiqueta;
        FechaDespacho = fechaDespacho;
        FechaEstimada = guia.FechaEstimada;
    }

    public void RegistrarEntrega(DateTime fecha) => FechaEntrega = fecha;
}
