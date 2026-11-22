using Microsoft.Extensions.DependencyInjection;

namespace LogiMarket.WinForms.Comun;

// Cada operacion de pantalla usa su propio ambito (un DbContext por operacion)
public sealed class EjecutorCasosUso
{
    private readonly IServiceScopeFactory _fabrica;

    public EjecutorCasosUso(IServiceScopeFactory fabrica)
    {
        _fabrica = fabrica;
    }

    public async Task<T> EjecutarAsync<TServicio, T>(Func<TServicio, Task<T>> accion) where TServicio : notnull
    {
        await using var ambito = _fabrica.CreateAsyncScope();
        return await accion(ambito.ServiceProvider.GetRequiredService<TServicio>());
    }

    public async Task<T> EjecutarAsync<TServicio, T>(Func<TServicio, T> accion) where TServicio : notnull
    {
        await using var ambito = _fabrica.CreateAsyncScope();
        return await Task.FromResult(accion(ambito.ServiceProvider.GetRequiredService<TServicio>()));
    }
}
