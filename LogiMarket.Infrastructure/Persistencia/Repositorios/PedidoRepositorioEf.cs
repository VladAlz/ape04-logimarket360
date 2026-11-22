using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Puertos;
using LogiMarket.Domain.Pedidos;
using Microsoft.EntityFrameworkCore;

namespace LogiMarket.Infrastructure.Persistencia.Repositorios;

// Escritura devuelve el agregado completo; lectura devuelve DTO sin seguimiento
public sealed class PedidoRepositorioEf : IPedidoEscritura, IPedidoLectura
{
    private readonly LogiMarketDbContext _contexto;

    public PedidoRepositorioEf(LogiMarketDbContext contexto)
    {
        _contexto = contexto;
    }

    public Task<Pedido?> ObtenerAsync(int id) =>
        _contexto.Pedidos
            .Include(p => p.Lineas).ThenInclude(l => l.Componentes)
            .Include(p => p.Historial)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id);

    public void Agregar(Pedido pedido) => _contexto.Pedidos.Add(pedido);

    public async Task<IReadOnlyList<PedidoResumen>> ListarAsync() =>
        (await _contexto.Pedidos.AsNoTracking()
            .Include(p => p.Lineas).ThenInclude(l => l.Componentes)
            .AsSplitQuery()
            .OrderByDescending(p => p.Id)
            .ToListAsync())
        .Select(PedidoMapper.Resumen)
        .ToList();

    public async Task<PedidoDetalleDto?> ObtenerDetalleAsync(int id)
    {
        var pedido = await _contexto.Pedidos.AsNoTracking()
            .Include(p => p.Lineas).ThenInclude(l => l.Componentes)
            .Include(p => p.Historial)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id);

        return pedido is null ? null : PedidoMapper.Detalle(pedido);
    }

    public async Task<bool> CodigoUsadoEnPedidosAsync(string codigo) =>
        await _contexto.PedidoDetalles.AnyAsync(l => l.Codigo == codigo)
        || await _contexto.Set<PedidoDetalleComponente>().AnyAsync(c => c.Sku == codigo);
}
