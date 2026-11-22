using LogiMarket.Application.Comun;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Puertos;
using LogiMarket.Domain.Seguridad;

namespace LogiMarket.Application.Seguridad;

// La regla vive aqui; ocultar botones en pantalla es solo comodidad
public sealed class ServicioAutorizacion
{
    private static readonly Dictionary<Operacion, Rol[]> Permisos = new()
    {
        [Operacion.Consultar] = new[] { Rol.Admin, Rol.Ventas, Rol.Bodega, Rol.Consulta },
        [Operacion.GestionarCatalogo] = new[] { Rol.Admin },
        [Operacion.AjustarInventario] = new[] { Rol.Admin, Rol.Bodega },
        [Operacion.GestionarClientes] = new[] { Rol.Admin, Rol.Ventas },
        [Operacion.EditarPedido] = new[] { Rol.Admin, Rol.Ventas },
        [Operacion.ValidarPedido] = new[] { Rol.Admin, Rol.Ventas },
        [Operacion.CobrarPedido] = new[] { Rol.Admin, Rol.Ventas },
        [Operacion.CancelarPedido] = new[] { Rol.Admin, Rol.Ventas },
        [Operacion.DespacharPedido] = new[] { Rol.Admin, Rol.Bodega },
        [Operacion.EntregarPedido] = new[] { Rol.Admin, Rol.Bodega },
        [Operacion.ProcesarDevolucion] = new[] { Rol.Admin, Rol.Ventas },
        [Operacion.EmitirComprobante] = new[] { Rol.Admin, Rol.Ventas, Rol.Bodega },
        [Operacion.VerAuditoria] = new[] { Rol.Admin, Rol.Consulta },
        [Operacion.ConfigurarSimulacion] = new[] { Rol.Admin }
    };

    private readonly ISesionUsuario _sesion;
    private readonly IBitacora _bitacora;
    private readonly MetricasSesion _metricas;

    public ServicioAutorizacion(ISesionUsuario sesion, IBitacora bitacora, MetricasSesion metricas)
    {
        _sesion = sesion;
        _bitacora = bitacora;
        _metricas = metricas;
    }

    public static bool RolPuede(Rol rol, Operacion operacion) => Permisos[operacion].Contains(rol);

    public bool Puede(Operacion operacion) => _sesion.Activa && RolPuede(_sesion.Rol, operacion);

    public async Task ExigirAsync(Operacion operacion)
    {
        if (Puede(operacion))
            return;

        _metricas.Incrementar(MetricasSesion.AccesosDenegados);
        await _bitacora.RegistrarAsync(CategoriaAuditoria.Seguridad,
            $"Acceso denegado: {_sesion.NombreUsuario} ({_sesion.Rol.ToString().ToUpperInvariant()}) intentó {operacion}.");

        throw new AccesoDenegadoException(
            $"El rol {_sesion.Rol.ToString().ToUpperInvariant()} no tiene permiso para {Describir(operacion)}.");
    }

    private static string Describir(Operacion operacion) => operacion switch
    {
        Operacion.CobrarPedido => "cobrar pedidos",
        Operacion.DespacharPedido => "despachar pedidos",
        Operacion.EntregarPedido => "registrar entregas",
        Operacion.ProcesarDevolucion => "procesar devoluciones",
        Operacion.GestionarCatalogo => "modificar el catálogo",
        Operacion.GestionarClientes => "modificar clientes",
        Operacion.EditarPedido => "editar borradores",
        _ => operacion.ToString()
    };
}
