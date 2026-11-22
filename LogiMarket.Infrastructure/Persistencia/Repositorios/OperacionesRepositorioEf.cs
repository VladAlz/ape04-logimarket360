using LogiMarket.Application.Puertos;
using LogiMarket.Domain.Despachos;
using LogiMarket.Domain.Devoluciones;
using LogiMarket.Domain.Eventos;
using LogiMarket.Domain.Inventario;
using LogiMarket.Domain.Pagos;
using LogiMarket.Domain.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace LogiMarket.Infrastructure.Persistencia.Repositorios;

public sealed class PagoRepositorioEf : IPagoRepositorio
{
    private readonly LogiMarketDbContext _contexto;

    public PagoRepositorioEf(LogiMarketDbContext contexto)
    {
        _contexto = contexto;
    }

    public Task<Pago?> ObtenerPorClaveAsync(string claveIdempotencia) =>
        _contexto.Pagos.AsNoTracking().FirstOrDefaultAsync(p => p.ClaveIdempotencia == claveIdempotencia);

    public Task<Pago?> ObtenerPorPedidoAsync(int pedidoId) =>
        _contexto.Pagos.AsNoTracking().FirstOrDefaultAsync(p => p.PedidoId == pedidoId);

    public Task<int> ContarPorPedidoAsync(int pedidoId) => _contexto.Pagos.CountAsync(p => p.PedidoId == pedidoId);

    public void Agregar(Pago pago) => _contexto.Pagos.Add(pago);
}

public sealed class InventarioRepositorioEf : IInventarioRepositorio
{
    private readonly LogiMarketDbContext _contexto;

    public InventarioRepositorioEf(LogiMarketDbContext contexto)
    {
        _contexto = contexto;
    }

    public void Agregar(MovimientoInventario movimiento) => _contexto.MovimientosInventario.Add(movimiento);

    public async Task<IReadOnlyList<MovimientoInventario>> ListarAsync(string? sku = null)
    {
        var consulta = _contexto.MovimientosInventario.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(sku))
            consulta = consulta.Where(m => m.Sku == sku);
        return await consulta.OrderByDescending(m => m.Id).ToListAsync();
    }
}

public sealed class DespachoRepositorioEf : IDespachoRepositorio
{
    private readonly LogiMarketDbContext _contexto;

    public DespachoRepositorioEf(LogiMarketDbContext contexto)
    {
        _contexto = contexto;
    }

    public void Agregar(Despacho despacho) => _contexto.Despachos.Add(despacho);

    public Task<Despacho?> ObtenerPorPedidoAsync(int pedidoId) =>
        _contexto.Despachos.FirstOrDefaultAsync(d => d.PedidoId == pedidoId);

    public Task<Despacho?> ObtenerPorGuiaAsync(string numeroGuia) =>
        _contexto.Despachos.AsNoTracking().FirstOrDefaultAsync(d => d.NumeroGuia == numeroGuia);
}

public sealed class DevolucionRepositorioEf : IDevolucionRepositorio
{
    private readonly LogiMarketDbContext _contexto;

    public DevolucionRepositorioEf(LogiMarketDbContext contexto)
    {
        _contexto = contexto;
    }

    public void Agregar(Devolucion devolucion) => _contexto.Devoluciones.Add(devolucion);

    public Task<Devolucion?> ObtenerPorPedidoAsync(int pedidoId) =>
        _contexto.Devoluciones.AsNoTracking().FirstOrDefaultAsync(d => d.PedidoId == pedidoId);

    public async Task<IReadOnlyList<Devolucion>> ListarAsync() =>
        await _contexto.Devoluciones.AsNoTracking().OrderByDescending(d => d.Id).ToListAsync();
}

public sealed class OutboxRepositorioEf : IOutboxRepositorio
{
    private readonly LogiMarketDbContext _contexto;

    public OutboxRepositorioEf(LogiMarketDbContext contexto)
    {
        _contexto = contexto;
    }

    public void Agregar(EventoOutbox evento) => _contexto.EventosOutbox.Add(evento);

    public async Task<IReadOnlyList<EventoOutbox>> ListarPendientesAsync() =>
        await _contexto.EventosOutbox.Where(e => !e.Procesado).OrderBy(e => e.Id).ToListAsync();

    public async Task<IReadOnlyList<EventoOutbox>> ListarAsync() =>
        await _contexto.EventosOutbox.AsNoTracking().OrderByDescending(e => e.Id).ToListAsync();
}

public sealed class UsuarioRepositorioEf : IUsuarioRepositorio
{
    private readonly LogiMarketDbContext _contexto;

    public UsuarioRepositorioEf(LogiMarketDbContext contexto)
    {
        _contexto = contexto;
    }

    public Task<Usuario?> ObtenerPorNombreAsync(string nombreUsuario) =>
        _contexto.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
}
