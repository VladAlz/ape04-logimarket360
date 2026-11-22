namespace LogiMarket.Application.Puertos;

public interface IUnidadTrabajo
{
    Task GuardarCambiosAsync();

    // Confirma si la operacion termina; ante cualquier excepcion deshace todo
    Task<T> EnTransaccionAsync<T>(Func<Task<T>> operacion);
}
