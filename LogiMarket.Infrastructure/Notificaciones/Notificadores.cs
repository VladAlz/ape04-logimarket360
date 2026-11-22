using LogiMarket.Application.Notificaciones;
using LogiMarket.Application.Puertos;
using LogiMarket.Domain.Clientes;
using LogiMarket.Infrastructure.Comun;

namespace LogiMarket.Infrastructure.Notificaciones;

// Avisos simulados: se escriben en un archivo de buzon, no salen del equipo
public abstract class NotificadorArchivo : INotificador
{
    private readonly ISimulacion _simulacion;
    private readonly string _archivo;

    protected NotificadorArchivo(ISimulacion simulacion, OpcionesLogiMarket opciones, string nombreArchivo)
    {
        _simulacion = simulacion;
        var carpeta = Path.Combine(opciones.CarpetaSalidas, "avisos");
        Directory.CreateDirectory(carpeta);
        _archivo = Path.Combine(carpeta, nombreArchivo);
    }

    public abstract string Canal { get; }

    public AvisoEmitido Enviar(string destino, string asunto, string mensaje)
    {
        if (_simulacion.FallarNotificacion)
            throw new InvalidOperationException($"Servidor de {Canal} no disponible (fallo simulado).");

        var texto = Formatear(destino, asunto, mensaje);
        File.AppendAllText(_archivo, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {texto}{Environment.NewLine}");
        return new AvisoEmitido(Canal, destino, texto, Path.GetFileName(_archivo));
    }

    protected abstract string Formatear(string destino, string asunto, string mensaje);
}

public sealed class NotificadorEmail : NotificadorArchivo
{
    public NotificadorEmail(ISimulacion simulacion, OpcionesLogiMarket opciones) : base(simulacion, opciones, "email.log")
    {
    }

    public override string Canal => "EMAIL";

    protected override string Formatear(string destino, string asunto, string mensaje) =>
        $"Para: {destino} | Asunto: {asunto} | {mensaje}";
}

public sealed class NotificadorSms : NotificadorArchivo
{
    public NotificadorSms(ISimulacion simulacion, OpcionesLogiMarket opciones) : base(simulacion, opciones, "sms.log")
    {
    }

    public override string Canal => "SMS";

    // Un SMS no lleva asunto y se corta a 160 caracteres
    protected override string Formatear(string destino, string asunto, string mensaje)
    {
        var texto = $"LogiMarket: {mensaje}";
        return $"Tel: {destino} | {(texto.Length > 160 ? texto[..160] : texto)}";
    }
}

public sealed class CreadorNotificadorEmail : CreadorNotificador
{
    private readonly ISimulacion _simulacion;
    private readonly OpcionesLogiMarket _opciones;

    public CreadorNotificadorEmail(ISimulacion simulacion, OpcionesLogiMarket opciones)
    {
        _simulacion = simulacion;
        _opciones = opciones;
    }

    public override CanalAviso Canal => CanalAviso.Email;

    protected override INotificador CrearNotificador() => new NotificadorEmail(_simulacion, _opciones);
}

public sealed class CreadorNotificadorSms : CreadorNotificador
{
    private readonly ISimulacion _simulacion;
    private readonly OpcionesLogiMarket _opciones;

    public CreadorNotificadorSms(ISimulacion simulacion, OpcionesLogiMarket opciones)
    {
        _simulacion = simulacion;
        _opciones = opciones;
    }

    public override CanalAviso Canal => CanalAviso.Sms;

    protected override INotificador CrearNotificador() => new NotificadorSms(_simulacion, _opciones);
}
