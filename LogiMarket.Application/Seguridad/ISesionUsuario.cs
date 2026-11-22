using LogiMarket.Domain.Seguridad;

namespace LogiMarket.Application.Seguridad;

public interface ISesionUsuario
{
    bool Activa { get; }
    string NombreUsuario { get; }
    string NombreCompleto { get; }
    Rol Rol { get; }

    void Iniciar(Usuario usuario);

    void Cerrar();
}
