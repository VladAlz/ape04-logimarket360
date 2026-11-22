using LogiMarket.Application.Clientes;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Clientes;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcClientes : VistaBase
{
    private int _clienteId;

    public UcClientes(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        Rejilla.Definir(dgvClientes,
            new Columna(nameof(ClienteDto.Id), "Id", Peso: 30, Derecha: true), new Columna(nameof(ClienteDto.Nombre), "Nombre", Peso: 110),
            new Columna(nameof(ClienteDto.Identificacion), "Identificación", Peso: 80), new Columna(nameof(ClienteDto.Email), "Email", Peso: 130),
            new Columna(nameof(ClienteDto.Telefono), "Teléfono", Peso: 70), new Columna(nameof(ClienteDto.Categoria), "Categoría", Peso: 70),
            new Columna(nameof(ClienteDto.Canal), "Aviso", Peso: 50), new Columna(nameof(ClienteDto.Activo), "Activo", Peso: 40));
        Texto.Mostrar<CategoriaCliente>(cmbCategoria, c => c.ToString().ToUpperInvariant());
        Texto.Mostrar<CanalAviso>(cmbCanal, c => c.ToString().ToUpperInvariant());
        cmbCategoria.DataSource = Enum.GetValues<CategoriaCliente>();
        cmbCanal.DataSource = Enum.GetValues<CanalAviso>();
        chkActivo.Checked = true;

        Permisos.Asociar(Operacion.GestionarClientes, btnNuevoCliente, btnGuardarCliente);
        btnBuscar.Click += (_, _) => Ejecutar(RefrescarAsync);
        btnLimpiar.Click += (_, _) => { txtBuscar.Clear(); Ejecutar(RefrescarAsync); };
        dgvClientes.SelectionChanged += (_, _) => MostrarCliente();
        btnNuevoCliente.Click += (_, _) => Limpiar();
        btnGuardarCliente.Click += (_, _) => Ejecutar(GuardarAsync);
    }

    public override string NombreVista => FormularioPrincipalMediator.Clientes;

    public override async Task RefrescarAsync() =>
        Rejilla.Cargar(dgvClientes,
            await Ejecutor.EjecutarAsync<GestionarClientesUseCase, IReadOnlyList<ClienteDto>>(g => g.ListarAsync(txtBuscar.Text)),
            c => c.Id, MostrarCliente);

    private void MostrarCliente()
    {
        if (Rejilla.Seleccionado<ClienteDto>(dgvClientes) is not { } c)
            return;

        _clienteId = c.Id;
        lblId.Text = $"Cliente Id {c.Id}";
        txtNombre.Text = c.Nombre;
        txtIdentificacion.Text = c.Identificacion;
        txtEmail.Text = c.Email;
        txtTelefono.Text = c.Telefono;
        cmbCategoria.SelectedItem = Enum.Parse<CategoriaCliente>(c.Categoria, true);
        cmbCanal.SelectedItem = Enum.Parse<CanalAviso>(c.Canal, true);
        chkActivo.Checked = c.Activo;
    }

    private void Limpiar()
    {
        _clienteId = 0;
        lblId.Text = "Cliente nuevo";
        foreach (var caja in new[] { txtNombre, txtIdentificacion, txtEmail, txtTelefono })
            caja.Clear();
        cmbCategoria.SelectedIndex = 0;
        cmbCanal.SelectedIndex = 0;
        chkActivo.Checked = true;
        txtNombre.Focus();
    }

    private async Task GuardarAsync()
    {
        var resultado = await Ejecutor.EjecutarAsync<GestionarClientesUseCase, ResultadoOperacion>(g => g.GuardarAsync(new ClienteRequest(
            _clienteId, txtNombre.Text, txtIdentificacion.Text, txtEmail.Text, txtTelefono.Text,
            (CategoriaCliente)cmbCategoria.SelectedItem!, (CanalAviso)cmbCanal.SelectedItem!, chkActivo.Checked)));
        Mostrar(lblMensaje, resultado);
        if (resultado.Exito)
            await AvisarAsync(EventoVista.ClientesCambiados);
    }
}
