namespace LogiMarket.Domain.Comun;

public interface IPrototipo<out T>
{
    T Clonar();
}
