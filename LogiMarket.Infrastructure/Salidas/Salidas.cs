using LogiMarket.Application.Comprobantes;
using LogiMarket.Application.Puertos;
using LogiMarket.Infrastructure.Comun;

namespace LogiMarket.Infrastructure.Salidas;

// Implementador del Bridge que guarda el documento para mostrarlo en pantalla
public sealed class SalidaVistaPrevia : ISalidaDocumento
{
    private readonly List<DocumentoTexto> _emitidos = new();

    public string Medio => MediosSalida.VistaPrevia;

    public int DocumentosEmitidos => _emitidos.Count;

    public SalidaEmitida Emitir(DocumentoTexto documento)
    {
        _emitidos.Add(documento);
        return new SalidaEmitida(Medio, $"Ventana de vista previa ({documento.Titulo})", documento.Texto);
    }
}

public sealed class SalidaArchivoTxt : ISalidaDocumento
{
    private readonly string _carpeta;

    public SalidaArchivoTxt(OpcionesLogiMarket opciones)
    {
        _carpeta = Path.Combine(opciones.CarpetaSalidas, "comprobantes");
    }

    public string Medio => MediosSalida.ArchivoTxt;

    public SalidaEmitida Emitir(DocumentoTexto documento)
    {
        Directory.CreateDirectory(_carpeta);
        var ruta = Path.Combine(_carpeta, documento.NombreArchivo);
        File.WriteAllText(ruta, documento.Texto + Environment.NewLine);
        return new SalidaEmitida(Medio, ruta, documento.Texto);
    }
}
