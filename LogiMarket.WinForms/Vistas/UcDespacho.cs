using LogiMarket.Application.Comandos;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Seguridad;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcDespacho : VistaBase
{
    public UcDespacho(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        // Rejilla reducida: el despacho solo necesita estado, entrega y total
        Rejilla.Definir(dgvPedidos,
            new Columna(nameof(PedidoResumen.Id), "Pedido", Peso: 50, Derecha: true), new Columna(nameof(PedidoResumen.Cliente), "Cliente", Peso: 100),
            new Columna(nameof(PedidoResumen.Estado), "Estado", Peso: 80), new Columna(nameof(PedidoResumen.Entrega), "Entrega", Peso: 70),
            new Columna(nameof(PedidoResumen.Unidades), "Unid.", Peso: 45, Derecha: true), new Columna(nameof(PedidoResumen.Total), "Total", "0.00", 60),
            new Columna(nameof(PedidoResumen.FechaCreacion), "Creado", "dd/MM/yyyy HH:mm", 100), new Columna(nameof(PedidoResumen.Devuelto), "Devuelto", Peso: 55));
        Permisos.Asociar(Operacion.DespacharPedido, btnDespachar);
        Permisos.Asociar(Operacion.EntregarPedido, btnEntregar);

        btnDespachar.Click += (_, _) => Ejecutar(DespacharAsync);
        btnEntregar.Click += (_, _) => Ejecutar(EntregarAsync);
        btnConsultarGuia.Click += (_, _) => Ejecutar(ConsultarGuiaAsync);
    }

    public override string NombreVista => FormularioPrincipalMediator.Despacho;

    // Muestra lo que bodega puede mover y tambien los validados, para probar el rechazo
    public override async Task RefrescarAsync() =>
        Rejilla.Cargar(dgvPedidos,
            (await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, IReadOnlyList<PedidoResumen>>(c => c.ListarAsync()))
                .Where(p => p.Estado is "VALIDADO" or "PAGADO" or "DESPACHADO"),
            p => p.Id);

    private async Task DespacharAsync()
    {
        if (Rejilla.Seleccionado<PedidoResumen>(dgvPedidos) is not { } pedido)
            return;
        if (!Confirmar($"¿Despachar el pedido {pedido.Id} con entrega {pedido.Entrega}?"))
            return;

        var resultado = await Ejecutor.EjecutarAsync<ServicioComandosPedido, ResultadoOperacion>(s => s.DespacharAsync(pedido.Id));
        Registrar(resultado);
        if (resultado.Exito)
        {
            txtGuia.Text = resultado.Mensaje.Split(Environment.NewLine)[1].Split(' ')[1];
            await AvisarAsync(EventoVista.PedidoDespachado);
        }
    }

    private async Task EntregarAsync()
    {
        if (Rejilla.Seleccionado<PedidoResumen>(dgvPedidos) is not { } pedido)
            return;
        if (!Confirmar($"¿Registrar la entrega del pedido {pedido.Id} con fecha {dtpEntrega.Value:dd/MM/yyyy}?"))
            return;

        var fecha = dtpEntrega.Value.Date.Add(DateTime.Now.TimeOfDay);
        var resultado = await Ejecutor.EjecutarAsync<EntregarPedidoUseCase, ResultadoOperacion>(u => u.EjecutarAsync(pedido.Id, fecha));
        Registrar(resultado);
        if (resultado.Exito)
            await AvisarAsync(EventoVista.PedidoEntregado);
    }

    private async Task ConsultarGuiaAsync()
    {
        var seguimiento = await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, SeguimientoDto?>(c => c.SeguimientoAsync(txtGuia.Text));
        lblSeguimiento.Text = seguimiento is null
            ? "No existe una guía con ese número."
            : $"Pedido {seguimiento.PedidoId} · {seguimiento.Estado} · {seguimiento.Familia}{Environment.NewLine}" +
              $"Despachado {seguimiento.FechaDespacho:dd/MM/yyyy} · estimado {seguimiento.FechaEstimada:dd/MM/yyyy}{Environment.NewLine}" +
              (seguimiento.FechaEntrega is { } f ? $"Entregado el {f:dd/MM/yyyy}" : "En tránsito");
    }

    private void Registrar(ResultadoOperacion resultado)
    {
        Mostrar(lblAccion, resultado);
        txtResultado.AppendText($"[{DateTime.Now:HH:mm:ss}] {(resultado.Exito ? "OK" : "RECHAZADO")}: {resultado.Mensaje.Replace(Environment.NewLine, " | ")}{Environment.NewLine}");
    }
}
