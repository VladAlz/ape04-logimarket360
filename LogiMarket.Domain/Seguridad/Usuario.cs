using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Seguridad;

public class Usuario
{
    public int Id { get; private set; }
    public string NombreUsuario { get; private set; } = string.Empty;
    public string NombreCompleto { get; private set; } = string.Empty;
    public Rol Rol { get; private set; }
    public string HashContrasena { get; private set; } = string.Empty;
    public bool Activo { get; private set; }

    private Usuario()
    {
    }

    public Usuario(string nombreUsuario, string nombreCompleto, Rol rol, string hashContrasena)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            throw new ReglaNegocioException("El usuario es obligatorio.");
        if (string.IsNullOrWhiteSpace(hashContrasena))
            throw new ReglaNegocioException("La contraseña debe guardarse como hash.");

        NombreUsuario = nombreUsuario.Trim().ToLowerInvariant();
        NombreCompleto = nombreCompleto.Trim();
        Rol = rol;
        HashContrasena = hashContrasena;
        Activo = true;
    }
}
