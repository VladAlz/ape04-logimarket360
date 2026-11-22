using LogiMarket.Application.Comandos;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Confirmacion;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Application.Simulacion;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcCobro : VistaBase
{
    // Una clave por intento de confirmacion de cada pedido
    private readonly Dictionary<int, string> _claves = new();
    private (int PedidoId, string Clave)? _ultimoIntento;

    public UcCobro(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        Permisos.Asociar(Operacion.CobrarPedido, btnConfirmar, btnRepetir, btnNuevaClave);

        cmbPedido.SelectedIndexChanged += (_, _) => Ejecutar(MostrarPedidoAsync);
        btnNuevaClave.Click += (_, _) => NuevaClave();
        btnConfirmar.Click += (_, _) => Ejecutar(() => ConfirmarAsync(repetir: false));
        btnRepetir.Click += (_, _) => Ejecutar(() => ConfirmarAsync(repetir: true));
    }

    public override string NombreVista => FormularioPrincipalMediator.Cobro;

    private int? PedidoElegido => (cmbPedido.SelectedItem as OpcionPedido)?.Id;

    public override async Task RefrescarAsync()
    {
        var anterior = PedidoElegido;
        var validados = (await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, IReadOnlyList<PedidoResumen>>(c => c.ListarAsync()))
            .Where(p => p.Estado == "VALIDADO").Select(OpcionPedido.Desde).ToList();
        cmbPedido.DataSource = validados;
        cmbPedido.SelectedItem = validados.FirstOrDefault(v => v.Id == anterior) ?? validados.FirstOrDefault();

        var simulacion = await Ejecutor.EjecutarAsync<SimulacionUseCase, EstadoSimulacion>(s => s.Estado());
        lblProveedor.Text = simulacion.ProveedorPago + (simulacion.FallarCobro ? " (rechazo forzado)" : string.Empty);
        await MostrarPedidoAsync();
    }

    private async Task MostrarPedidoAsync()
    {
        if (PedidoElegido is not { } id)
        {
            foreach (var etiqueta in new[] { lblCliente, lblSubtotal, lblDescuento, lblTarifa, lblTotal })
                etiqueta.Text = "-";
            txtClave.Clear();
            return;
        }

        var detalle = await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, PedidoDetalleDto?>(c => c.DetalleAsync(id));
        if (detalle is null)
            return;

        var r = detalle.Resumen;
        lblCliente.Text = $"{r.Cliente} ({r.Categoria}) · {r.Entrega}";
        lblSubtotal.Text = Presentacion.Monto(r.Subtotal);
        lblDescuento.Text = $"{Presentacion.Monto(r.Descuento)} ({r.Politica})";
        lblTarifa.Text = Presentacion.Monto(r.Tarifa);
        lblTotal.Text = Presentacion.Monto(r.Total);
        if (!_claves.ContainsKey(id))
            _claves[id] = CrearClave(id);
        txtClave.Text = _claves[id];
    }

    private void NuevaClave()
    {
        if (PedidoElegido is not { } id)
            return;
        _claves[id] = CrearClave(id);
        txtClave.Text = _claves[id];
    }

    private static string CrearClave(int pedidoId) => $"CONF-{pedidoId}-{Guid.NewGuid().ToString("N")[..12].ToUpperInvariant()}";

    private async Task ConfirmarAsync(bool repetir)
    {
        // Repetir reenvia el ultimo intento aunque el pedido ya no este en la lista
        int id;
        string clave;
        if (repetir)
        {
            if (_ultimoIntento is not { } ultimo)
                return;
            (id, clave) = ultimo;
        }
        else
        {
            if (PedidoElegido is not { } elegido || string.IsNullOrWhiteSpace(txtClave.Text))
                return;
            (id, clave) = (elegido, txtClave.Text);
        }

        if (!repetir && !Confirmar($"¿Confirmar y cobrar el pedido {id} por USD {lblTotal.Text}?"))
            return;

        var resultado = await Ejecutor.EjecutarAsync<ServicioComandosPedido, ResultadoConfirmacion>(s => s.ConfirmarAsync(new SolicitudConfirmacion(id, clave)));
        _ultimoIntento = (id, clave);

        lblResultadoCobro.Text = (resultado.Exito ? (resultado.Repetido ? "REPETIDO (sin nuevo cobro). " : "APROBADO. ") : "RECHAZADO. ")
            + resultado.Mensaje + Environment.NewLine + $"Estado del pedido: {resultado.Estado}";
        txtTrazas.Text = $"Pedido {id} · clave {clave}{Environment.NewLine}" + Texto.Lineas(resultado.Trazas.Select((t, i) => $"{i + 1,2}. {t}"));

        if (resultado.Exito && !resultado.Repetido)
            await AvisarAsync(EventoVista.PedidoConfirmado);
    }
}
