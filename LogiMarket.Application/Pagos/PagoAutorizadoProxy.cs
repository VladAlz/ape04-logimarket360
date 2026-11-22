using LogiMarket.Application.Comun;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Seguridad;

namespace LogiMarket.Application.Pagos;

// Proxy de proteccion: un rol sin permiso no llega al decorador ni al proveedor
public sealed class PagoAutorizadoProxy : IPasarelaPago
{
    private readonly IPasarelaPago _real;
    private readonly ISesionUsuario _sesion;
    private readonly IBitacora _bitacora;
    private readonly MetricasSesion _metricas;

    public PagoAutorizadoProxy(IPasarelaPago real, ISesionUsuario sesion, IBitacora bitacora, MetricasSesion metricas)
    {
        _real = real;
        _sesion = sesion;
        _bitacora = bitacora;
        _metricas = metricas;
    }

    public string Proveedor => _real.Proveedor;

    public async Task<ResultadoCobro> CobrarAsync(decimal monto, string claveIdempotencia)
    {
        if (!_sesion.Activa || (_sesion.Rol != Rol.Ventas && _sesion.Rol != Rol.Admin))
        {
            _metricas.Incrementar(MetricasSesion.AccesosDenegados);
            // Va a la auditoria de seguridad, no al LOG de cobro
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Seguridad,
                $"Proxy de pago: {_sesion.NombreUsuario} ({_sesion.Rol.ToString().ToUpperInvariant()}) intentó cobrar {Presentacion.Monto(monto)}. Denegado.",
                claveIdempotencia);

            return ResultadoCobro.AccesoDenegado(
                $"Cobro denegado: el rol {_sesion.Rol.ToString().ToUpperInvariant()} no puede cobrar.");
        }

        return await _real.CobrarAsync(monto, claveIdempotencia);
    }
}
