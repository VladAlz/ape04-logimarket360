namespace LogiMarket.WinForms;

public partial class FrmVistaPrevia : Form
{
    public FrmVistaPrevia(string titulo, string contenido)
    {
        InitializeComponent();
        Text = $"Vista previa - {titulo}";
        txtContenido.Text = contenido;
        txtContenido.Select(0, 0);
        btnCerrar.Click += (_, _) => Close();
        CancelButton = btnCerrar;
    }
}
