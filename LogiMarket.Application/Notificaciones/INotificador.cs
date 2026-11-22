namespace LogiMarket.Application.Notificaciones;

public sealed record AvisoEmitido(string Canal, string Destino, string Mensaje, string Registro);

public interface INotificador
{
    string Canal { get; }

    AvisoEmitido Enviar(string destino, string asunto, string mensaje);
}
