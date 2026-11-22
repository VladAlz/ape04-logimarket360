using LogiMarket.Domain.Clientes;

namespace LogiMarket.Application.Notificaciones;

// Factory Method: cada creador concreto decide que notificador fabricar
public abstract class CreadorNotificador
{
    public abstract CanalAviso Canal { get; }

    protected abstract INotificador CrearNotificador();

    public AvisoEmitido Notificar(string destino, string asunto, string mensaje)
    {
        var notificador = CrearNotificador();
        return notificador.Enviar(destino, asunto, mensaje);
    }
}
