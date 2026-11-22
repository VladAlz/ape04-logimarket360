using LogiMarket.Application.Catalogo;
using LogiMarket.Application.Clientes;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Pedidos;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcCarrito : VistaBase
{
    private int _pedidoId;

    public UcCarrito(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        Rejilla.Definir(dgvLineas,
            new Columna(nameof(LineaPedidoDto.Codigo), "Código", Peso: 90), new Columna(nameof(LineaPedidoDto.Nombre), "Nombre", Peso: 100),
            new Columna(nameof(LineaPedidoDto.Tipo), "Tipo", Peso: 60), new Columna(nameof(LineaPedidoDto.Cantidad), "Cantidad", Peso: 55, Derecha: true),
            new Columna(nameof(LineaPedidoDto.PrecioUnitario), "Precio unit.", "0.00", 65), new Columna(nameof(LineaPedidoDto.Subtotal), "Subtotal", "0.00", 65),
            new Columna(nameof(LineaPedidoDto.Composicion), "Composición congelada", Peso: 120));
        Texto.Mostrar<FormaEntrega>(cmbEntrega, PedidoMapper.Entrega);
        cmbEntrega.DataSource = Enum.GetValues<FormaEntrega>();

        Permisos.Asociar(Operacion.EditarPedido, btnNuevoBorrador, btnDuplicar, btnAgregar, btnCambiarCantidad, btnQuitar, btnDeshacer, btnRehacer);
        Permisos.Asociar(Operacion.ValidarPedido, btnValidar);

        cmbBorrador.SelectionChangeCommitted += (_, _) =>
        {
            _pedidoId = (cmbBorrador.SelectedItem as OpcionPedido)?.Id ?? 0;
            Ejecutar(MostrarBorradorAsync);
        };
        btnNuevoBorrador.Click += (_, _) => NuevoBorrador();
        btnAgregar.Click += (_, _) => Ejecutar(AgregarAsync);
        btnCambiarCantidad.Click += (_, _) => Ejecutar(() => EditarLineaAsync(quitar: false));
        btnQuitar.Click += (_, _) => Ejecutar(() => EditarLineaAsync(quitar: true));
        cmbCliente.SelectionChangeCommitted += (_, _) => Ejecutar(CambiarClienteAsync);
        cmbEntrega.SelectionChangeCommitted += (_, _) => Ejecutar(CambiarEntregaAsync);
        btnDeshacer.Click += (_, _) => Ejecutar(() => AplicarAsync(e => e.DeshacerAsync(_pedidoId)));
        btnRehacer.Click += (_, _) => Ejecutar(() => AplicarAsync(e => e.RehacerAsync(_pedidoId)));
        btnDuplicar.Click += (_, _) => Ejecutar(DuplicarAsync);
        btnValidar.Click += (_, _) => Ejecutar(ValidarAsync);
        dgvLineas.SelectionChanged += (_, _) =>
        {
            if (Rejilla.Seleccionado<LineaPedidoDto>(dgvLineas) is { } linea)
                nudCantidad.Value = Math.Min(nudCantidad.Maximum, linea.Cantidad);
        };
    }

    public override string NombreVista => FormularioPrincipalMediator.Carrito;

    public override async Task RefrescarAsync()
    {
        var clientes = (await Ejecutor.EjecutarAsync<GestionarClientesUseCase, IReadOnlyList<ClienteDto>>(g => g.ListarAsync()))
            .Where(c => c.Activo).ToList();
        var elegido = (cmbCliente.SelectedItem as ClienteDto)?.Id;
        cmbCliente.DataSource = clientes;
        if (elegido is not null)
            cmbCliente.SelectedItem = clientes.FirstOrDefault(c => c.Id == elegido);

        var elemento = (cmbElemento.SelectedItem as ElementoVentaDto)?.Codigo;
        var elementos = (await Ejecutor.EjecutarAsync<ConsultarCatalogoUseCase, IReadOnlyList<ElementoVentaDto>>(c => c.ElementosVentaAsync())).ToList();
        cmbElemento.DataSource = elementos;
        if (elemento is not null)
            cmbElemento.SelectedItem = elementos.FirstOrDefault(e => e.Codigo == elemento);

        await CargarBorradoresAsync();
        await MostrarBorradorAsync();
    }

    private async Task CargarBorradoresAsync()
    {
        var borradores = (await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, IReadOnlyList<PedidoResumen>>(c => c.ListarAsync()))
            .Where(p => p.Estado == "BORRADOR").Select(OpcionPedido.Desde).ToList();
        cmbBorrador.DataSource = borradores;
        cmbBorrador.SelectedItem = borradores.FirstOrDefault(b => b.Id == _pedidoId);
        if (cmbBorrador.SelectedItem is null)
            cmbBorrador.SelectedIndex = -1;
    }

    private void NuevoBorrador()
    {
        _pedidoId = 0;
        cmbBorrador.SelectedIndex = -1;
        Rejilla.Cargar(dgvLineas, Array.Empty<LineaPedidoDto>());
        foreach (var etiqueta in new[] { lblSubtotal, lblPolitica, lblDescuento, lblTarifa, lblTotal, lblUnidades })
            etiqueta.Text = "-";
        lblEstadoBorrador.Text = "Nuevo: elija cliente, entrega y la primera línea, luego presione Agregar.";
        lblHistorial.Text = "Deshacer: 0 | Rehacer: 0";
    }

    private async Task MostrarBorradorAsync()
    {
        if (_pedidoId == 0)
        {
            NuevoBorrador();
            return;
        }

        var detalle = await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, PedidoDetalleDto?>(c => c.DetalleAsync(_pedidoId));
        if (detalle is null)
        {
            NuevoBorrador();
            return;
        }

        var r = detalle.Resumen;
        Rejilla.Cargar(dgvLineas, detalle.Lineas, l => l.Codigo);
        lblSubtotal.Text = Presentacion.Monto(r.Subtotal);
        lblPolitica.Text = r.Politica;
        lblDescuento.Text = Presentacion.Monto(r.Descuento);
        lblTarifa.Text = Presentacion.Monto(r.Tarifa);
        lblTotal.Text = Presentacion.Monto(r.Total);
        lblUnidades.Text = r.Unidades.ToString();
        lblEstadoBorrador.Text = $"Pedido {r.Id} · {r.Estado} · {r.Cliente} ({r.Categoria}) · entrega {r.Entrega}";

        if (cmbCliente.DataSource is List<ClienteDto> clientes)
            cmbCliente.SelectedItem = clientes.FirstOrDefault(c => c.Nombre == r.Cliente);
        cmbEntrega.SelectedItem = r.Entrega == "EXPRÉS" ? FormaEntrega.Expres : FormaEntrega.Estandar;

        var (deshacer, rehacer) = await Ejecutor.EjecutarAsync<EdicionBorradorUseCase, (int, int)>(e => e.Conteo(_pedidoId));
        lblHistorial.Text = $"Deshacer: {deshacer} | Rehacer: {rehacer}";
    }

    private async Task AgregarAsync()
    {
        if (cmbElemento.SelectedItem is not ElementoVentaDto elemento)
            return;

        var cantidad = (int)nudCantidad.Value;
        if (_pedidoId == 0)
        {
            if (cmbCliente.SelectedItem is not ClienteDto cliente)
            {
                Registrar("Seleccione un cliente.");
                return;
            }
            var entrega = (FormaEntrega)cmbEntrega.SelectedItem!;
            await AplicarAsync(e => e.CrearAsync(cliente.Id, entrega, elemento.Codigo, cantidad));
            return;
        }

        await AplicarAsync(e => e.AgregarAsync(_pedidoId, elemento.Codigo, cantidad));
    }

    private async Task EditarLineaAsync(bool quitar)
    {
        if (_pedidoId == 0 || Rejilla.Seleccionado<LineaPedidoDto>(dgvLineas) is not { } linea)
            return;

        var cantidad = (int)nudCantidad.Value;
        await AplicarAsync(e => quitar ? e.QuitarAsync(_pedidoId, linea.Codigo) : e.CambiarCantidadAsync(_pedidoId, linea.Codigo, cantidad));
    }

    private async Task CambiarClienteAsync()
    {
        if (_pedidoId != 0 && cmbCliente.SelectedItem is ClienteDto cliente)
            await AplicarAsync(e => e.CambiarClienteAsync(_pedidoId, cliente.Id));
    }

    private async Task CambiarEntregaAsync()
    {
        if (_pedidoId != 0)
            await AplicarAsync(e => e.CambiarEntregaAsync(_pedidoId, (FormaEntrega)cmbEntrega.SelectedItem!));
    }

    private async Task DuplicarAsync()
    {
        if (_pedidoId != 0)
            await AplicarAsync(e => e.DuplicarAsync(_pedidoId));
    }

    private async Task AplicarAsync(Func<EdicionBorradorUseCase, Task<ResultadoEdicion>> accion)
    {
        var resultado = await Ejecutor.EjecutarAsync(accion);
        Registrar((resultado.Exito ? "OK. " : "RECHAZADO. ") + resultado.Mensaje);
        if (!resultado.Exito)
            return;

        _pedidoId = resultado.PedidoId;
        await AvisarAsync(EventoVista.BorradorCambiado);
    }

    private async Task ValidarAsync()
    {
        if (_pedidoId == 0)
            return;

        var resultado = await Ejecutor.EjecutarAsync<ValidarPedidoUseCase, ResultadoOperacion>(v => v.EjecutarAsync(_pedidoId));
        Registrar((resultado.Exito ? "OK. " : "RECHAZADO. ") + resultado.Mensaje);
        if (resultado.Exito)
        {
            _pedidoId = 0;
            await AvisarAsync(EventoVista.PedidoValidado);
        }
    }

    private void Registrar(string mensaje)
    {
        txtMensajes.AppendText($"[{DateTime.Now:HH:mm:ss}] {mensaje.Replace(Environment.NewLine, Environment.NewLine + "           ")}{Environment.NewLine}");
    }
}
