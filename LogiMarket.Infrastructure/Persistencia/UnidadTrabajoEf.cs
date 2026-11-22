using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;
using LogiMarket.Infrastructure.Bitacora;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LogiMarket.Infrastructure.Persistencia;

public sealed class UnidadTrabajoEf : IUnidadTrabajo
{
    private readonly LogiMarketDbContext _contexto;
    private readonly BitacoraEf _bitacora;

    public UnidadTrabajoEf(LogiMarketDbContext contexto, BitacoraEf bitacora)
    {
        _contexto = contexto;
        _bitacora = bitacora;
    }

    public async Task GuardarCambiosAsync()
    {
        try
        {
            await _contexto.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            Descartar();
            throw new OperacionRechazadaException("Otro proceso modificó los mismos datos antes de guardar (control de concurrencia).");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqliteException sql)
        {
            Descartar();
            throw new OperacionRechazadaException(sql.SqliteExtendedErrorCode == 2067 || sql.SqliteExtendedErrorCode == 1555
                ? "Se intentó registrar un dato duplicado (restricción de unicidad)."
                : $"La base de datos rechazó el cambio: {sql.Message}");
        }
    }

    public async Task<T> EnTransaccionAsync<T>(Func<Task<T>> operacion)
    {
        if (_contexto.Database.CurrentTransaction is not null)
            return await operacion();

        await using var transaccion = await _contexto.Database.BeginTransactionAsync();
        try
        {
            var resultado = await operacion();
            await transaccion.CommitAsync();
            return resultado;
        }
        catch
        {
            await transaccion.RollbackAsync();
            Descartar();
            throw;
        }
        finally
        {
            // Lo registrado durante la transaccion se escribe al terminar
            await _bitacora.VaciarPendientesAsync();
        }
    }

    // Lo que quedo en memoria tras un fallo no debe guardarse despues
    private void Descartar() => _contexto.ChangeTracker.Clear();
}
