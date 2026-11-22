using LogiMarket.Application.Comun;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

// Base comun de las pantallas; el constructor vacio es solo para el diseñador
public class VistaBase : UserControl, IVistaColaboradora
{
    public VistaBase()
    {
    }

    protected VistaBase(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
    {
        Ejecutor = ejecutor;
        Mediador = mediador;
        Permisos = permisos;
        mediador.Registrar(this);
    }

    protected EjecutorCasosUso Ejecutor { get; } = null!;
    protected IMediadorVistas Mediador { get; } = null!;
    protected ControlPermisos Permisos { get; } = null!;

    public virtual string NombreVista => Name;

    public virtual Task RefrescarAsync() => Task.CompletedTask;

    protected Task AvisarAsync(EventoVista evento) => Mediador.NotificarAsync(this, evento);

    // Los manejadores de botones pasan por aqui para no dejar excepciones sueltas
    protected async void Ejecutar(Func<Task> accion)
    {
        try
        {
            UseWaitCursor = true;
            await accion();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "LogiMarket 360", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    protected bool Confirmar(string pregunta) =>
        MessageBox.Show(this, pregunta, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

    protected static void Mostrar(Control destino, ResultadoOperacion resultado) =>
        destino.Text = (resultado.Exito ? "OK. " : "RECHAZADO. ") + resultado.Mensaje;
}
