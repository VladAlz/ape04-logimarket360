using LogiMarket.Application.Comun;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;

namespace LogiMarket.Application.Comandos;

// Invocador: ejecuta cualquier comando y deja registro de entrada y resultado
public sealed class InvocadorComandos
{
    private readonly HistorialComandos _historial;
    private readonly IBitacora _bitacora;
    private readonly ISesionUsuario _sesion;
    private readonly IReloj _reloj;
    private readonly MetricasSesion _metricas;

    public InvocadorComandos(HistorialComandos historial, IBitacora bitacora, ISesionUsuario sesion, IReloj reloj, MetricasSesion metricas)
    {
        _historial = historial;
        _bitacora = bitacora;
        _sesion = sesion;
        _reloj = reloj;
        _metricas = metricas;
    }

    public async Task<ResultadoOperacion> EjecutarAsync(IComandoPedido comando)
    {
        var resultado = await comando.EjecutarAsync();
        var primeraLinea = resultado.Mensaje.Split(Environment.NewLine)[0];

        _historial.Agregar(new RegistroComando(_reloj.Ahora, _sesion.NombreUsuario, comando.Nombre, comando.Entrada, resultado.Exito, primeraLinea));
        _metricas.Incrementar(MetricasSesion.ComandosEjecutados);
        await _bitacora.RegistrarAsync(CategoriaAuditoria.Comando,
            $"{comando.Nombre}({comando.Entrada}) -> {(resultado.Exito ? "OK" : "RECHAZO")}: {primeraLinea}");

        return resultado;
    }
}
