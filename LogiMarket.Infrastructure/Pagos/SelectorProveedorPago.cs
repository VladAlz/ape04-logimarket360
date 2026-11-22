using LogiMarket.Application.Pagos;
using LogiMarket.Application.Puertos;
using LogiMarket.Infrastructure.Simulacion;

namespace LogiMarket.Infrastructure.Pagos;

// Elige el proveedor activo; ambos cumplen el mismo contrato (LSP)
public sealed class SelectorProveedorPago : IPasarelaPago
{
    private readonly PasarelaLegadaAdapter _legado;
    private readonly PasarelaNovaPaySimulada _novaPay;
    private readonly ISimulacion _simulacion;

    public SelectorProveedorPago(PasarelaLegadaAdapter legado, PasarelaNovaPaySimulada novaPay, ISimulacion simulacion)
    {
        _legado = legado;
        _novaPay = novaPay;
        _simulacion = simulacion;
    }

    private IPasarelaPago Activo => _simulacion.ProveedorPago == ConfiguracionSimulacion.NovaPay ? _novaPay : _legado;

    public string Proveedor => Activo.Proveedor;

    public Task<ResultadoCobro> CobrarAsync(decimal monto, string claveIdempotencia) =>
        Activo.CobrarAsync(monto, claveIdempotencia);
}
