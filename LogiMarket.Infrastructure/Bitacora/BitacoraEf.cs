using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Auditoria;
using LogiMarket.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace LogiMarket.Infrastructure.Bitacora;

// Escribe con su propio contexto para no mezclarse con los cambios del caso de uso.
// Si hay una transaccion abierta, espera a que termine y luego escribe.
public sealed class BitacoraEf : IBitacora
{
    private readonly LogiMarketDbContext _principal;
    private readonly IDbContextFactory<LogiMarketDbContext> _fabrica;
    private readonly ISesionUsuario _sesion;
    private readonly IReloj _reloj;
    private readonly List<RegistroAuditoria> _ambito = new();
    private readonly List<RegistroAuditoria> _pendientes = new();

    public BitacoraEf(LogiMarketDbContext principal, IDbContextFactory<LogiMarketDbContext> fabrica, ISesionUsuario sesion, IReloj reloj)
    {
        _principal = principal;
        _fabrica = fabrica;
        _sesion = sesion;
        _reloj = reloj;
    }

    public IReadOnlyList<RegistroAuditoria> RegistrosDelAmbito => _ambito;

    public async Task RegistrarAsync(string categoria, string mensaje, string correlacion = "")
    {
        var registro = new RegistroAuditoria(_reloj.Ahora, categoria, _sesion.NombreUsuario,
            mensaje.Length > 500 ? mensaje[..500] : mensaje, correlacion);
        _ambito.Add(registro);

        if (_principal.Database.CurrentTransaction is not null)
        {
            _pendientes.Add(registro);
            return;
        }

        await EscribirAsync(new[] { registro });
    }

    public async Task VaciarPendientesAsync()
    {
        if (_pendientes.Count == 0)
            return;

        var lote = _pendientes.ToList();
        _pendientes.Clear();
        await EscribirAsync(lote);
    }

    public async Task<IReadOnlyList<RegistroAuditoria>> ListarAsync(string? categoria = null)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();
        var consulta = contexto.Auditoria.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(categoria))
            consulta = consulta.Where(r => r.Categoria == categoria);
        return await consulta.OrderByDescending(r => r.Id).Take(500).ToListAsync();
    }

    private async Task EscribirAsync(IEnumerable<RegistroAuditoria> registros)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();
        contexto.Auditoria.AddRange(registros);
        await contexto.SaveChangesAsync();
    }
}
