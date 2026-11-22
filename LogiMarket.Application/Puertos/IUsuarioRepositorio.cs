using LogiMarket.Domain.Seguridad;

namespace LogiMarket.Application.Puertos;

public interface IUsuarioRepositorio
{
    Task<Usuario?> ObtenerPorNombreAsync(string nombreUsuario);
}
