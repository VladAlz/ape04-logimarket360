using LogiMarket.Application.Comprobantes;

namespace LogiMarket.Application.Puertos;

// Implementador del Bridge: a donde va el documento
public interface ISalidaDocumento
{
    string Medio { get; }

    SalidaEmitida Emitir(DocumentoTexto documento);
}
