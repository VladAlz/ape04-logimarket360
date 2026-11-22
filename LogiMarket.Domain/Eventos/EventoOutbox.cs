namespace LogiMarket.Domain.Eventos;

// Se guarda en la misma transaccion del cambio y se procesa despues
public class EventoOutbox
{
    public int Id { get; private set; }
    public string Tipo { get; private set; } = string.Empty;
    public int PedidoId { get; private set; }
    public string Contenido { get; private set; } = string.Empty;
    public DateTime FechaCreacion { get; private set; }
    public bool Procesado { get; private set; }
    public DateTime? FechaProcesado { get; private set; }
    public int Intentos { get; private set; }
    public string UltimoError { get; private set; } = string.Empty;

    private EventoOutbox()
    {
    }

    public EventoOutbox(string tipo, int pedidoId, string contenido, DateTime fecha)
    {
        Tipo = tipo;
        PedidoId = pedidoId;
        Contenido = contenido;
        FechaCreacion = fecha;
    }

    public void MarcarProcesado(DateTime fecha)
    {
        Intentos++;
        Procesado = true;
        FechaProcesado = fecha;
        UltimoError = string.Empty;
    }

    public void RegistrarFallo(string error)
    {
        Intentos++;
        UltimoError = error.Length > 300 ? error[..300] : error;
    }
}
