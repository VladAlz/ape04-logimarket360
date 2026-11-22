using LogiMarket.Application.Pagos;

namespace LogiMarket.Application.Puertos;

public interface IPasarelaPago
{
    string Proveedor { get; }

    Task<ResultadoCobro> CobrarAsync(decimal monto, string claveIdempotencia);
}
