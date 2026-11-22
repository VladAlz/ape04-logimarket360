using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Seguridad;

namespace LogiMarket.WinForms.Comun;

// Solo comodidad visual; la regla real se valida en Application
public sealed class ControlPermisos
{
    private readonly List<(Control Control, Operacion Operacion)> _asociados = new();

    public void Asociar(Operacion operacion, params Control[] controles)
    {
        foreach (var control in controles)
            _asociados.Add((control, operacion));
    }

    public void Aplicar(Rol rol, bool habilitarTodo)
    {
        foreach (var (control, operacion) in _asociados.Where(a => !a.Control.IsDisposed))
            control.Enabled = habilitarTodo || ServicioAutorizacion.RolPuede(rol, operacion);
    }

    public void Reiniciar() => _asociados.Clear();
}
