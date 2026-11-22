using LogiMarket.Application.Comun;
using LogiMarket.Application.Pagos;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Infrastructure.Pagos;

// Decorator: deja INICIO y FIN/ERROR alrededor del cobro sin cambiar su resultado
public sealed class AuditoriaPagoDecorator : IPasarelaPago
{
    private readonly IPasarelaPago _interno;
    private readonly IBitacora _bitacora;

    public AuditoriaPagoDecorator(IPasarelaPago interno, IBitacora bitacora)
    {
        _interno = interno;
        _bitacora = bitacora;
    }

    public string Proveedor => _interno.Proveedor;

    public async Task<ResultadoCobro> CobrarAsync(decimal monto, string claveIdempotencia)
    {
        var correlacion = $"COB-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
        await _bitacora.RegistrarAsync(CategoriaAuditoria.Cobro,
            $"INICIO cobro {Presentacion.Monto(monto)} con {_interno.Proveedor} ({correlacion}).", claveIdempotencia);

        try
        {
            var resultado = await _interno.CobrarAsync(monto, claveIdempotencia);
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Cobro,
                $"FIN cobro {(resultado.Aprobado ? "APROBADO " + resultado.Referencia : "RECHAZADO")} ({correlacion}).", claveIdempotencia);
            return resultado;
        }
        catch (Exception ex)
        {
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Cobro, $"ERROR cobro: {ex.Message} ({correlacion}).", claveIdempotencia);
            throw;
        }
    }
}
