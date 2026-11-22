using LogiMarket.Application.Comandos;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Devoluciones;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Devoluciones;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcDevoluciones : VistaBase
{
    private PedidoDetalleDto? _detalle;

    public UcDevoluciones(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        Rejilla.Definir(dgvDevoluciones,
            new Columna(nameof(DevolucionDto.PedidoId), "Pedido", Peso: 50, Derecha: true), new Columna(nameof(DevolucionDto.Motivo), "Motivo", Peso: 80),
            new Columna(nameof(DevolucionDto.FechaSolicitud), "Solicitud", "dd/MM/yyyy", 70), new Columna(nameof(DevolucionDto.DiasDesdeEntrega), "Días", Peso: 40, Derecha: true),
            new Columna(nameof(DevolucionDto.Reembolso), "Reembolso", "0.00", 60), new Columna(nameof(DevolucionDto.Referencia), "Referencia", Peso: 110),
            new Columna(nameof(DevolucionDto.Usuario), "Usuario", Peso: 60));
        cmbMotivo.DataSource = Enum.GetValues<MotivoDevolucion>();

        Permisos.Asociar(Operacion.ProcesarDevolucion, btnProcesar);
        cmbPedido.SelectedIndexChanged += (_, _) => Ejecutar(MostrarPedidoAsync);
        dtpSolicitud.ValueChanged += (_, _) => MostrarPlazo();
        btnProcesar.Click += (_, _) => Ejecutar(ProcesarAsync);
    }

    public override string NombreVista => FormularioPrincipalMediator.Devoluciones;

    public override async Task RefrescarAsync()
    {
        var anterior = (cmbPedido.SelectedItem as OpcionPedido)?.Id;
        var entregados = (await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, IReadOnlyList<PedidoResumen>>(c => c.ListarAsync()))
            .Where(p => p.Estado == "ENTREGADO" && !p.Devuelto).Select(OpcionPedido.Desde).ToList();
        cmbPedido.DataSource = entregados;
        cmbPedido.SelectedItem = entregados.FirstOrDefault(e => e.Id == anterior) ?? entregados.FirstOrDefault();
        Rejilla.Cargar(dgvDevoluciones, await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, IReadOnlyList<DevolucionDto>>(c => c.DevolucionesAsync()));
        await MostrarPedidoAsync();
    }

    private async Task MostrarPedidoAsync()
    {
        _detalle = cmbPedido.SelectedItem is OpcionPedido opcion
            ? await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, PedidoDetalleDto?>(c => c.DetalleAsync(opcion.Id))
            : null;

        lblEstimado.Text = _detalle is null
            ? "-"
            : $"USD {Presentacion.Monto(_detalle.Resumen.Subtotal - _detalle.Resumen.Descuento)} (pagado {Presentacion.Monto(_detalle.Resumen.Total)}, tarifa no reembolsable)";
        MostrarPlazo();
    }

    private void MostrarPlazo()
    {
        if (_detalle?.FechaEntrega is not { } entrega)
        {
            lblEntrega.Text = string.Empty;
            return;
        }

        var dias = (dtpSolicitud.Value.Date - entrega.Date).Days;
        lblEntrega.Text = $"Entregado el {entrega:dd/MM/yyyy}; la solicitud cae en el día {dias} después de la entrega.";
    }

    private async Task ProcesarAsync()
    {
        if (cmbPedido.SelectedItem is not OpcionPedido opcion)
            return;
        var motivo = (MotivoDevolucion)cmbMotivo.SelectedItem!;
        if (!Confirmar($"¿Procesar la devolución total del pedido {opcion.Id} por {motivo.ToString().ToUpperInvariant()}?"))
            return;

        var solicitud = new SolicitudDevolucion(opcion.Id, motivo, dtpSolicitud.Value.Date.Add(DateTime.Now.TimeOfDay), chkSinUso.Checked);
        var resultado = await Ejecutor.EjecutarAsync<ServicioComandosPedido, ResultadoDevolucion>(s => s.DevolverAsync(solicitud));

        txtResultado.Text = $"{(resultado.Exito ? "ACEPTADA" : "RECHAZADA")} · pedido {opcion.Id} · motivo {motivo.ToString().ToUpperInvariant()}{Environment.NewLine}"
            + resultado.Mensaje + Environment.NewLine + Environment.NewLine + "Pasos de la plantilla:" + Environment.NewLine
            + Texto.Lineas(resultado.Pasos.Select(p => "  " + p));

        if (resultado.Exito)
            await AvisarAsync(EventoVista.DevolucionRegistrada);
    }
}
