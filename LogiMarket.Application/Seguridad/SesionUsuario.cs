using LogiMarket.Domain.Seguridad;

namespace LogiMarket.Application.Seguridad;

public sealed class SesionUsuario : ISesionUsuario
{
    public bool Activa { get; private set; }
    public string NombreUsuario { get; private set; } = "-";
    public string NombreCompleto { get; private set; } = "-";

    // Sin sesion se trata como el rol con menos permisos
    public Rol Rol { get; private set; } = Rol.Consulta;

    public void Iniciar(Usuario usuario)
    {
        NombreUsuario = usuario.NombreUsuario;
        NombreCompleto = usuario.NombreCompleto;
        Rol = usuario.Rol;
        Activa = true;
    }

    public void Cerrar()
    {
        Activa = false;
        NombreUsuario = "-";
        NombreCompleto = "-";
        Rol = Rol.Consulta;
    }
}
