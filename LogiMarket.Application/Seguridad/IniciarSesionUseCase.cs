using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.Seguridad;

public sealed class IniciarSesionUseCase
{
    private readonly IUsuarioRepositorio _usuarios;
    private readonly IHasherContrasenas _hasher;
    private readonly ISesionUsuario _sesion;
    private readonly IBitacora _bitacora;

    public IniciarSesionUseCase(IUsuarioRepositorio usuarios, IHasherContrasenas hasher, ISesionUsuario sesion, IBitacora bitacora)
    {
        _usuarios = usuarios;
        _hasher = hasher;
        _sesion = sesion;
        _bitacora = bitacora;
    }

    public async Task<ResultadoOperacion> EjecutarAsync(string nombreUsuario, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrEmpty(contrasena))
            return ResultadoOperacion.Fallo("Ingrese usuario y contraseña.");

        var usuario = await _usuarios.ObtenerPorNombreAsync(nombreUsuario.Trim().ToLowerInvariant());

        // Mismo mensaje para usuario inexistente o clave errada
        if (usuario is null || !usuario.Activo || !_hasher.Verificar(contrasena, usuario.HashContrasena))
        {
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Seguridad, $"Intento de acceso fallido para '{nombreUsuario.Trim()}'.");
            return ResultadoOperacion.Fallo("Usuario o contraseña incorrectos.");
        }

        _sesion.Iniciar(usuario);
        await _bitacora.RegistrarAsync(CategoriaAuditoria.Seguridad, $"Inicio de sesión de {usuario.NombreUsuario} ({usuario.Rol.ToString().ToUpperInvariant()}).");
        return ResultadoOperacion.Correcto($"Bienvenido, {usuario.NombreCompleto}.");
    }

    public async Task CerrarAsync()
    {
        if (!_sesion.Activa)
            return;

        await _bitacora.RegistrarAsync(CategoriaAuditoria.Seguridad, $"Cierre de sesión de {_sesion.NombreUsuario}.");
        _sesion.Cerrar();
    }
}
