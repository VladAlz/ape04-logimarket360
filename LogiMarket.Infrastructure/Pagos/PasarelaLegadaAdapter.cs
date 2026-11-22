using LogiMarket.Application.Comun;
using LogiMarket.Application.Pagos;
using LogiMarket.Application.Puertos;
using LogiMarket.Infrastructure.Pagos.Legado;

namespace LogiMarket.Infrastructure.Pagos;

// Adapter: traduce decimal a centavos y la respuesta de texto al contrato IPasarelaPago
public sealed class PasarelaLegadaAdapter : IPasarelaPago
{
    private readonly PasarelaLegadaSimulada _legado;
    private readonly IBitacora _bitacora;

    public PasarelaLegadaAdapter(PasarelaLegadaSimulada legado, IBitacora bitacora)
    {
        _legado = legado;
        _bitacora = bitacora;
    }

    public string Proveedor => "LEGADO";

    public async Task<ResultadoCobro> CobrarAsync(decimal monto, string claveIdempotencia)
    {
        var centavos = (long)Math.Round(monto * 100, MidpointRounding.AwayFromZero);
        var respuesta = _legado.CobrarCentavos(centavos, claveIdempotencia);

        await _bitacora.RegistrarAsync(CategoriaAuditoria.Cobro,
            $"PASARELA {Proveedor}: CobrarCentavos({centavos}) respondió \"{respuesta}\".", claveIdempotencia);

        var partes = respuesta.Split('|', 2);
        return partes[0] == "OK"
            ? ResultadoCobro.Aprobacion(partes[1], Proveedor)
            : ResultadoCobro.Rechazo(Proveedor, partes.Length > 1 ? partes[1] : respuesta);
    }
}
