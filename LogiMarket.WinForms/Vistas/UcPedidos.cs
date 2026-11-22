using LogiMarket.Application.Comandos;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Seguridad;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcPedidos : VistaBase
{
    public UcPedidos(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        DefinirPedidos(dgvPedidos);
        Rejilla.Definir(dgvLineas,
            new Columna(nameof(LineaPedidoDto.Codigo), "Código", Peso: 90), new Columna(nameof(LineaPedidoDto.Cantidad), "Cant.", Peso: 40, Derecha: true),
            new Columna(nameof(LineaPedidoDto.PrecioUnitario), "Precio", "0.00", 55), new Columna(nameof(LineaPedidoDto.Subtotal), "Subtotal", "0.00", 60),
            new Columna(nameof(LineaPedidoDto.Composicion), "Composición", Peso: 110));
        Rejilla.Definir(dgvHistorial,
            new Columna(nameof(HistorialPedidoDto.Fecha), "Fecha", "dd/MM/yyyy HH:mm", 80), new Columna(nameof(HistorialPedidoDto.EstadoAnterior), "De", Peso: 60),
            new Columna(nameof(HistorialPedidoDto.EstadoNuevo), "A", Peso: 60), new Columna(nameof(HistorialPedidoDto.Usuario), "Usuario", Peso: 55),
            new Columna(nameof(HistorialPedidoDto.Detalle), "Detalle", Peso: 150));

        Permisos.Asociar(Operacion.CancelarPedido, btnCancelar);
        btnFiltrar.Click += (_, _) => Ejecutar(RefrescarAsync);
        btnLimpiarFiltro.Click += (_, _) => { txtFiltro.Clear(); Ejecutar(RefrescarAsync); };
        dgvPedidos.SelectionChanged += (_, _) => Ejecutar(MostrarDetalleAsync);
        btnCancelar.Click += (_, _) => Ejecutar(CancelarAsync);
    }

    public override string NombreVista => FormularioPrincipalMediator.Pedidos;

    public static void DefinirPedidos(DataGridView rejilla) =>
        Rejilla.Definir(rejilla,
            new Columna(nameof(PedidoResumen.Id), "Pedido", Peso: 45, Derecha: true), new Columna(nameof(PedidoResumen.Cliente), "Cliente", Peso: 100),
            new Columna(nameof(PedidoResumen.Estado), "Estado", Peso: 70), new Columna(nameof(PedidoResumen.Entrega), "Entrega", Peso: 60),
            new Columna(nameof(PedidoResumen.Unidades), "Unid.", Peso: 40, Derecha: true), new Columna(nameof(PedidoResumen.Subtotal), "Subtotal", "0.00", 60),
            new Columna(nameof(PedidoResumen.Politica), "Política", Peso: 65), new Columna(nameof(PedidoResumen.Descuento), "Descuento", "0.00", 60),
            new Columna(nameof(PedidoResumen.Tarifa), "Tarifa", "0.00", 50), new Columna(nameof(PedidoResumen.Total), "Total", "0.00", 60),
            new Columna(nameof(PedidoResumen.FechaCreacion), "Creado", "dd/MM/yyyy HH:mm", 85), new Columna(nameof(PedidoResumen.Devuelto), "Devuelto", Peso: 45));

    public override async Task RefrescarAsync()
    {
        var resultado = await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, ResultadoFiltro>(c => c.FiltrarAsync(txtFiltro.Text));
        lblResultadoFiltro.Text = resultado.Mensaje;
        Rejilla.Cargar(dgvPedidos, resultado.Pedidos, p => p.Id);
        await MostrarDetalleAsync();
    }

    private async Task MostrarDetalleAsync()
    {
        if (Rejilla.Seleccionado<PedidoResumen>(dgvPedidos) is not { } pedido)
        {
            grpDetalle.Text = "Detalle del pedido seleccionado";
            Rejilla.Cargar(dgvLineas, Array.Empty<LineaPedidoDto>());
            Rejilla.Cargar(dgvHistorial, Array.Empty<HistorialPedidoDto>());
            return;
        }

        var detalle = await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, PedidoDetalleDto?>(c => c.DetalleAsync(pedido.Id));
        grpDetalle.Text = $"Detalle del pedido {pedido.Id} ({pedido.Estado})";
        Rejilla.Cargar(dgvLineas, detalle?.Lineas ?? Array.Empty<LineaPedidoDto>());
        Rejilla.Cargar(dgvHistorial, detalle?.Historial ?? Array.Empty<HistorialPedidoDto>());
    }

    private async Task CancelarAsync()
    {
        if (Rejilla.Seleccionado<PedidoResumen>(dgvPedidos) is not { } pedido)
            return;
        if (!Confirmar($"¿Cancelar el pedido {pedido.Id} ({pedido.Estado})? Esta acción no se puede deshacer."))
            return;

        var resultado = await Ejecutor.EjecutarAsync<ServicioComandosPedido, ResultadoOperacion>(s => s.CancelarAsync(pedido.Id, txtMotivo.Text));
        MessageBox.Show(this, resultado.Mensaje, resultado.Exito ? "Pedido cancelado" : "Operación rechazada",
            MessageBoxButtons.OK, resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        if (resultado.Exito)
            await AvisarAsync(EventoVista.PedidoCancelado);
    }
}
