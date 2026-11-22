using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.VistaCatalogo;

public sealed record ResultadoVistaMasiva(
    int Filas,
    int MetadatosCompartidos,
    bool MismaReferencia,
    long BytesConFlyweight,
    long BytesSinFlyweight,
    IReadOnlyList<FilaCatalogoVisual> Renglones);

public sealed class GenerarVistaMasivaUseCase
{
    private readonly ICatalogoLectura _catalogo;
    private readonly CatalogoVisualFlyweightFactory _fabrica;

    public GenerarVistaMasivaUseCase(ICatalogoLectura catalogo, CatalogoVisualFlyweightFactory fabrica)
    {
        _catalogo = catalogo;
        _fabrica = fabrica;
    }

    public async Task<ResultadoVistaMasiva> EjecutarAsync(int filas = 1000)
    {
        var productos = await _catalogo.ListarProductosAsync();
        if (productos.Count == 0)
            return new ResultadoVistaMasiva(0, 0, false, 0, 0, Array.Empty<FilaCatalogoVisual>());

        _fabrica.Reiniciar();

        // Mismas filas, mismos datos externos; solo cambia si se comparte el metadato
        var antes = GC.GetAllocatedBytesForCurrentThread();
        var conFlyweight = new List<FilaCatalogoVisual>(filas);
        for (var i = 0; i < filas; i++)
        {
            var p = productos[i % productos.Count];
            conFlyweight.Add(new FilaCatalogoVisual(i + 1, _fabrica.Obtener(p), p.Stock, i % 7 + 1, p.Precio));
        }
        var bytesCon = GC.GetAllocatedBytesForCurrentThread() - antes;

        antes = GC.GetAllocatedBytesForCurrentThread();
        var sinFlyweight = new List<FilaCatalogoCompleta>(filas);
        for (var i = 0; i < filas; i++)
        {
            var p = productos[i % productos.Count];
            // Copia de los textos como haria una fila que carga sus propios datos
            sinFlyweight.Add(new FilaCatalogoCompleta(new string(p.Sku), new string(p.Nombre), new string(p.Categoria),
                new string(p.Descripcion), p.Stock, i % 7 + 1, p.Precio));
        }
        var bytesSin = GC.GetAllocatedBytesForCurrentThread() - antes;
        GC.KeepAlive(sinFlyweight);

        var primera = conFlyweight[0].Metadato;
        var mismaSku = conFlyweight.First(f => f.Numero > 1 && f.Sku == primera.Sku).Metadato;

        return new ResultadoVistaMasiva(filas, _fabrica.ObjetosCompartidos, ReferenceEquals(primera, mismaSku),
            bytesCon, bytesSin, conFlyweight);
    }
}
