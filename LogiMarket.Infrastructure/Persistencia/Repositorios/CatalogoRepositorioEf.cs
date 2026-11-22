using LogiMarket.Application.Puertos;
using LogiMarket.Domain.Catalogo;
using Microsoft.EntityFrameworkCore;

namespace LogiMarket.Infrastructure.Persistencia.Repositorios;

public sealed class CatalogoRepositorioEf : ICatalogoLectura, ICatalogoEscritura
{
    private readonly LogiMarketDbContext _contexto;

    public CatalogoRepositorioEf(LogiMarketDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<Producto>> ListarProductosAsync(string? filtro = null)
    {
        var consulta = _contexto.Productos.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(filtro))
        {
            // Parametro de EF, nunca texto concatenado al SQL
            var patron = $"%{filtro.Trim()}%";
            consulta = consulta.Where(p => EF.Functions.Like(p.Sku, patron) || EF.Functions.Like(p.Nombre, patron)
                || EF.Functions.Like(p.Categoria, patron));
        }

        return await consulta.OrderBy(p => p.Sku).ToListAsync();
    }

    public async Task<IReadOnlyList<Kit>> ListarKitsAsync() =>
        await _contexto.Kits.AsNoTracking().Include(k => k.Componentes).OrderBy(k => k.Codigo).ToListAsync();

    public async Task<ArmadorCatalogo> CrearArmadorAsync() =>
        new(await ListarProductosAsync(), await ListarKitsAsync());

    public async Task<bool> CodigoEnUsoAsync(string codigo) =>
        await _contexto.Productos.AnyAsync(p => p.Sku == codigo) || await _contexto.Kits.AnyAsync(k => k.Codigo == codigo);

    public Task<Producto?> ObtenerProductoAsync(string sku) =>
        _contexto.Productos.FirstOrDefaultAsync(p => p.Sku == sku);

    public Task<Kit?> ObtenerKitAsync(string codigo) =>
        _contexto.Kits.Include(k => k.Componentes).FirstOrDefaultAsync(k => k.Codigo == codigo);

    public async Task<IReadOnlyList<Producto>> ObtenerParaStockAsync(IEnumerable<string> skus)
    {
        var lista = skus.ToList();
        var productos = await _contexto.Productos.Where(p => lista.Contains(p.Sku)).ToListAsync();

        // Si ya estaban en memoria se recargan para leer el stock vigente
        foreach (var producto in productos)
            await _contexto.Entry(producto).ReloadAsync();

        return productos;
    }

    public void AgregarProducto(Producto producto) => _contexto.Productos.Add(producto);

    public void EliminarProducto(Producto producto) => _contexto.Productos.Remove(producto);

    public void AgregarKit(Kit kit) => _contexto.Kits.Add(kit);
}
