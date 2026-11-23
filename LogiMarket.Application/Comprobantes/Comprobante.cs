using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.Comprobantes;

// Bridge: la abstraccion arma el contenido, el implementador decide la salida
public abstract class Comprobante
{
    private readonly ISalidaDocumento _salida;

    protected Comprobante(ISalidaDocumento salida)
    {
        _salida = salida;
    }

    public SalidaEmitida Emitir() => _salida.Emitir(Componer());

    protected abstract DocumentoTexto Componer();

    protected static string Linea(string etiqueta, string valor) => $"{etiqueta,-25}{valor}";
}
