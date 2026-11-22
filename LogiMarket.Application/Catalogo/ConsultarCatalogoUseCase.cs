using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;
using LogiMarket.Domain.Catalogo;
using LogiMarket.Domain.Catalogo.Visitantes;
using LogiMarket.Domain.Comun;

namespace LogiMarket.Application.Catalogo;

public sealed class ConsultarCatalogoUseCase
{
    private readonly ICatalogoLectura _catalogo;

    public ConsultarCatalogoUseCase(ICatalogoLectura catalogo)
    {
        _catalogo = catalogo;
    }

    public async Task<IReadOnlyList<ProductoDto>> ProductosAsync(string? filtro = null) =>
        (await _catalogo.ListarProductosAsync(filtro))
            .Select(p => new ProductoDto(p.Sku, p.Nombre, p.Categoria, p.Descripcion, p.Precio, p.PesoKg, p.Stock))
            .ToList();

    public async Task<IReadOnlyList<KitDto>> KitsAsync()
    {
        var armador = await _catalogo.CrearArmadorAsync();
        var kits = await _catalogo.ListarKitsAsync();

        return kits.Select(k =>
        {
            var elemento = armador.Obtener(k.Codigo);
            var composicion = string.Join(" + ", k.Componentes.Select(c => c.Cantidad == 1 ? c.CodigoComponente : $"{c.CodigoComponente}x{c.Cantidad}"));
            return new KitDto(k.Codigo, k.Nombre, elemento.Precio, elemento.PesoKg, composicion, elemento.ExpandirSku(1).Sum(c => c.Cantidad));
        }).ToList();
    }

    public async Task<IReadOnlyList<string>> ComponentesDeKitAsync(string codigo)
    {
        var kit = (await _catalogo.ListarKitsAsync()).FirstOrDefault(k => k.Codigo == codigo);
        return kit?.Componentes.Select(c => $"{c.CodigoComponente}|{c.Cantidad}").ToList() ?? new List<string>();
    }

    public async Task<IReadOnlyList<ElementoVentaDto>> ElementosVentaAsync()
    {
        var armador = await _catalogo.CrearArmadorAsync();
        return armador.Todos()
            .Select(e => new ElementoVentaDto(e.Codigo, $"{e.Codigo} - {e.Nombre} (USD {Presentacion.Monto(e.Precio)})", e.Precio))
            .ToList();
    }

    // Visitor para desglose y peso; Iterator para mostrar el recorrido
    public async Task<DesgloseDto> DesglosarAsync(string codigo, int cantidad = 1)
    {
        var armador = await _catalogo.CrearArmadorAsync();
        var elemento = armador.Obtener(codigo);

        var desglose = new VisitanteDesgloseFisico();
        elemento.Aceptar(desglose, cantidad);
        var peso = new VisitantePesoLogistico();
        elemento.Aceptar(peso, cantidad);

        var recorrido = new List<string>();
        var iterador = elemento.CrearIterador(cantidad);
        while (iterador.HaySiguiente())
        {
            var c = iterador.Siguiente();
            recorrido.Add($"{recorrido.Count + 1}. {c.Sku} x{c.Cantidad}");
        }

        return new DesgloseDto(elemento.Codigo, elemento.Nombre, Dinero.Redondear(elemento.Precio * cantidad),
            desglose.Lineas, peso.TotalKg, desglose.KitsVisitados, recorrido);
    }
}
