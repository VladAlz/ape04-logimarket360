using LogiMarket.Application.Seguridad;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;
using LogiMarket.WinForms.Vistas;

namespace LogiMarket.WinForms;

public partial class FrmPrincipal : Form
{
    private readonly ISesionUsuario _sesion;
    private readonly IMediadorVistas _mediador;
    private readonly ControlPermisos _permisos;
    private readonly EjecutorCasosUso _ejecutor;

    public FrmPrincipal(ISesionUsuario sesion, IMediadorVistas mediador, ControlPermisos permisos, EjecutorCasosUso ejecutor,
        UcPanel panel, UcCatalogo catalogo, UcClientes clientes, UcCarrito carrito, UcPedidos pedidos, UcCobro cobro,
        UcDespacho despacho, UcDevoluciones devoluciones, UcInventario inventario, UcReportes reportes, UcAuditoria auditoria)
    {
        _sesion = sesion;
        _mediador = mediador;
        _permisos = permisos;
        _ejecutor = ejecutor;
        InitializeComponent();

        Agregar(tabPanel, panel);
        Agregar(tabCatalogo, catalogo);
        Agregar(tabClientes, clientes);
        Agregar(tabCarrito, carrito);
        Agregar(tabPedidos, pedidos);
        Agregar(tabCobro, cobro);
        Agregar(tabDespacho, despacho);
        Agregar(tabDevoluciones, devoluciones);
        Agregar(tabInventario, inventario);
        Agregar(tabReportes, reportes);
        Agregar(tabAuditoria, auditoria);

        _mediador.Notificado += MostrarEstado;
        lblUsuario.Text = $"Usuario: {_sesion.NombreCompleto} ({_sesion.NombreUsuario})   |   Rol: {_sesion.Rol.ToString().ToUpperInvariant()}";
        chkProbarPermisos.CheckedChanged += (_, _) => AplicarPermisos();
        btnCerrarSesion.Click += (_, _) => CerrarSesion();
        Shown += async (_, _) =>
        {
            AplicarPermisos();
            await _mediador.NotificarAsync(null, EventoVista.SesionIniciada);
        };
        FormClosed += (_, _) => _mediador.Notificado -= MostrarEstado;
    }

    public bool SesionCerrada { get; private set; }

    public void MostrarEstado(string texto) => lblEstado.Text = "  " + texto;

    private static void Agregar(TabPage pagina, UserControl vista)
    {
        vista.Dock = DockStyle.Fill;
        pagina.Controls.Add(vista);
    }

    private void AplicarPermisos() => _permisos.Aplicar(_sesion.Rol, chkProbarPermisos.Checked);

    private async void CerrarSesion()
    {
        if (MessageBox.Show(this, "¿Cerrar la sesión actual?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        await _ejecutor.EjecutarAsync<IniciarSesionUseCase, bool>(async u =>
        {
            await u.CerrarAsync();
            return true;
        });
        SesionCerrada = true;
        Close();
    }
}
