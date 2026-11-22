using LogiMarket.Application.Comun;

namespace LogiMarket.Application.Comandos;

public interface IComandoPedido
{
    string Nombre { get; }

    string Entrada { get; }

    Task<ResultadoOperacion> EjecutarAsync();
}
