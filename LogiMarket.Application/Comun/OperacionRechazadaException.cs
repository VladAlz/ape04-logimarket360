namespace LogiMarket.Application.Comun;

// Se lanza dentro de una transaccion para deshacerla sin dejar cambios
public class OperacionRechazadaException : Exception
{
    public OperacionRechazadaException(string mensaje) : base(mensaje)
    {
    }
}
