using LogiMarket.Application.Seguridad;
using LogiMarket.WinForms.Comun;

namespace LogiMarket.WinForms;

public partial class FrmAcceso : Form
{
    private readonly EjecutorCasosUso _ejecutor;

    public FrmAcceso(EjecutorCasosUso ejecutor)
    {
        _ejecutor = ejecutor;
        InitializeComponent();
        btnIngresar.Click += (_, _) => Ingresar();
        btnSalir.Click += (_, _) => Close();
    }

    private async void Ingresar()
    {
        btnIngresar.Enabled = false;
        try
        {
            var resultado = await _ejecutor.EjecutarAsync<IniciarSesionUseCase, Application.Comun.ResultadoOperacion>(
                u => u.EjecutarAsync(txtUsuario.Text, txtContrasena.Text));

            if (resultado.Exito)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            lblMensaje.Text = resultado.Mensaje;
            txtContrasena.Clear();
            txtContrasena.Focus();
        }
        finally
        {
            btnIngresar.Enabled = true;
        }
    }
}
