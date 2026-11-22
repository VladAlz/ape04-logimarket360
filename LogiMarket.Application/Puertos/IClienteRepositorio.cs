using LogiMarket.Domain.Clientes;

namespace LogiMarket.Application.Puertos;

public interface IClienteRepositorio
{
    Task<IReadOnlyList<Cliente>> ListarAsync(string? filtro = null);

    Task<Cliente?> ObtenerAsync(int id);

    Task<bool> IdentificacionEnUsoAsync(string identificacion, int excluirId);

    void Agregar(Cliente cliente);
}
