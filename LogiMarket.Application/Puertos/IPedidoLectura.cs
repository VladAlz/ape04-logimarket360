using LogiMarket.Application.Pedidos;

namespace LogiMarket.Application.Puertos;

public interface IPedidoLectura
{
    Task<IReadOnlyList<PedidoResumen>> ListarAsync();

    Task<PedidoDetalleDto?> ObtenerDetalleAsync(int id);

    Task<bool> CodigoUsadoEnPedidosAsync(string codigo);
}
