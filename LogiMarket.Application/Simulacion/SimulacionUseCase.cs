using LogiMarket.Application.Comun;
using LogiMarket.Application.Eventos;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;

namespace LogiMarket.Application.Simulacion;

public sealed record EstadoSimulacion(IReadOnlyList<string> Proveedores, string ProveedorPago, bool FallarCobro, bool FallarNotificacion);

public sealed record ResultadoPruebaCobro(bool Aprobado, string Mensaje, IReadOnlyList<string> Trazas);

// Pantalla de demostracion: cambia colaboradores simulados, no reglas
public sealed class SimulacionUseCase
{
    private readonly ISimulacion _simulacion;
    private readonly ProcesadorOutbox _procesador;
    private readonly IPasarelaPago _pasarela;
    private readonly ServicioAutorizacion _autorizacion;
    private readonly IBitacora _bitacora;

    public SimulacionUseCase(ISimulacion simulacion, ProcesadorOutbox procesador, IPasarelaPago pasarela,
        ServicioAutorizacion autorizacion, IBitacora bitacora)
    {
        _simulacion = simulacion;
        _procesador = procesador;
        _pasarela = pasarela;
        _autorizacion = autorizacion;
        _bitacora = bitacora;
    }

    public EstadoSimulacion Estado() =>
        new(_simulacion.Proveedores, _simulacion.ProveedorPago, _simulacion.FallarCobro, _simulacion.FallarNotificacion);

    public async Task<ResultadoOperacion> AplicarAsync(string proveedor, bool fallarCobro, bool fallarNotificacion)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.ConfigurarSimulacion);
            if (!_simulacion.Proveedores.Contains(proveedor))
                return ResultadoOperacion.Fallo($"Proveedor desconocido: {proveedor}.");

            _simulacion.ProveedorPago = proveedor;
            _simulacion.FallarCobro = fallarCobro;
            _simulacion.FallarNotificacion = fallarNotificacion;

            var texto = $"Simulación: proveedor {proveedor}, fallo de cobro {(fallarCobro ? "ACTIVO" : "no")}, fallo de notificación {(fallarNotificacion ? "ACTIVO" : "no")}.";
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Sistema, texto);
            return ResultadoOperacion.Correcto(texto);
        }
        catch (AccesoDenegadoException ex)
        {
            return ResultadoOperacion.Fallo(ex.Message);
        }
    }

    public async Task<ResumenOutbox> ProcesarOutboxAsync()
    {
        await _autorizacion.ExigirAsync(Operacion.ConfigurarSimulacion);
        return await _procesador.ProcesarPendientesAsync();
    }

    // Llama a la cadena Proxy -> Decorator -> Adapter sin tocar pedidos ni pagos
    public async Task<ResultadoPruebaCobro> ProbarCadenaPagoAsync(decimal monto)
    {
        var inicio = _bitacora.RegistrosDelAmbito.Count;
        var clave = $"PRUEBA-{Guid.NewGuid():N}"[..20];
        var resultado = await _pasarela.CobrarAsync(monto, clave);
        var trazas = _bitacora.RegistrosDelAmbito.Skip(inicio).Select(r => $"[{r.Categoria}] {r.Mensaje}").ToList();
        return new ResultadoPruebaCobro(resultado.Aprobado, resultado.Mensaje, trazas);
    }
}
