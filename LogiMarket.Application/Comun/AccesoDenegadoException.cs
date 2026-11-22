namespace LogiMarket.Application.Comun;

public class AccesoDenegadoException : Exception
{
    public AccesoDenegadoException(string mensaje) : base(mensaje)
    {
    }
}
