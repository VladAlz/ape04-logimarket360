using LogiMarket.Application.Comun;
using LogiMarket.Application.Pagos;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Infrastructure.Pagos;

// Segundo proveedor simulado; ya cumple el contrato y no necesita adaptador
public sealed class PasarelaNovaPaySimulada : IPasarelaPago
{
    private readonly ISimulacion _simulacion;
    private readonly IBitacora _bitacora;

    public PasarelaNovaPaySimulada(ISimulacion simulacion, IBitacora bitacora)
    {
        _simulacion = simulacion;
        _bitacora = bitacora;
    }

    public string Proveedor => "NOVAPAY";

    public async Task<ResultadoCobro> CobrarAsync(decimal monto, string claveIdempotencia)
    {
        // Mismo comportamiento que el legado ante un monto invalido (LSP)
        if (monto <= 0)
            return ResultadoCobro.Rechazo(Proveedor, "MONTO INVALIDO");

        await Task.Delay(50);
        var aprobado = !_simulacion.FallarCobro;
        var referencia = $"NVP-{Math.Abs(HashCode.Combine(claveIdempotencia, monto)) % 1_000_000:000000}";

        await _bitacora.RegistrarAsync(CategoriaAuditoria.Cobro,
            $"PASARELA {Proveedor}: autorización de {Presentacion.Monto(monto)} {(aprobado ? $"aceptada {referencia}" : "rechazada (fondos insuficientes)")}.",
            claveIdempotencia);

        return aprobado
            ? ResultadoCobro.Aprobacion(referencia, Proveedor)
            : ResultadoCobro.Rechazo(Proveedor, "fondos insuficientes");
    }
}
