using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.Inventario;

public sealed record StockDto(string Sku, string Nombre, int Stock, string Alerta);

public sealed record MovimientoDto(DateTime Fecha, string Sku, int Cantidad, int StockResultante, string Motivo, string Referencia, string Usuario);

public sealed class ConsultarInventarioUseCase
{
    public const int StockMinimo = 3;

    private readonly ICatalogoLectura _catalogo;
    private readonly IInventarioRepositorio _inventario;

    public ConsultarInventarioUseCase(ICatalogoLectura catalogo, IInventarioRepositorio inventario)
    {
        _catalogo = catalogo;
        _inventario = inventario;
    }

    public async Task<IReadOnlyList<StockDto>> StockAsync() =>
        (await _catalogo.ListarProductosAsync())
            .Select(p => new StockDto(p.Sku, p.Nombre, p.Stock, p.Stock == 0 ? "AGOTADO" : p.Stock <= StockMinimo ? "BAJO" : "OK"))
            .ToList();

    public async Task<IReadOnlyList<MovimientoDto>> MovimientosAsync(string? sku = null) =>
        (await _inventario.ListarAsync(sku))
            .Select(m => new MovimientoDto(m.Fecha, m.Sku, m.Cantidad, m.StockResultante, m.Motivo, m.Referencia, m.Usuario))
            .ToList();
}
