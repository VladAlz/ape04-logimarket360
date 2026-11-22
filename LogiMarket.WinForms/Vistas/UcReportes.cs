using LogiMarket.Application.Comprobantes;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Application.VistaCatalogo;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcReportes : VistaBase
{
    public UcReportes(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        Rejilla.Definir(dgvReporte,
            new Columna(nameof(PedidoResumen.Id), "Pedido", Peso: 45, Derecha: true), new Columna(nameof(PedidoResumen.Cliente), "Cliente", Peso: 100),
            new Columna(nameof(PedidoResumen.Estado), "Estado", Peso: 75), new Columna(nameof(PedidoResumen.Politica), "Política", Peso: 70),
            new Columna(nameof(PedidoResumen.Total), "Total", "0.00", 60));
        Rejilla.Definir(dgvFilas,
            new Columna(nameof(FilaCatalogoVisual.Numero), "Fila", Peso: 40, Derecha: true), new Columna(nameof(FilaCatalogoVisual.Sku), "SKU", Peso: 40),
            new Columna(nameof(FilaCatalogoVisual.Nombre), "Nombre", Peso: 80), new Columna(nameof(FilaCatalogoVisual.Categoria), "Categoría", Peso: 70),
            new Columna(nameof(FilaCatalogoVisual.Stock), "Stock", Peso: 40, Derecha: true), new Columna(nameof(FilaCatalogoVisual.Cantidad), "Cant.", Peso: 40, Derecha: true),
            new Columna(nameof(FilaCatalogoVisual.PrecioVenta), "Precio", "0.00", 50));
        cmbTipo.Format += (_, e) => e.Value = e.Value is TipoComprobante t ? t.Nombre() : e.Value;
        cmbTipo.DataSource = Enum.GetValues<TipoComprobante>();
        cmbMedio.DataSource = new[] { MediosSalida.VistaPrevia, MediosSalida.ArchivoTxt };

        Permisos.Asociar(Operacion.EmitirComprobante, btnEmitir);
        cmbTipo.SelectedIndexChanged += (_, _) => Ejecutar(CargarPedidosAsync);
        btnEmitir.Click += (_, _) => Ejecutar(EmitirAsync);
        btnGenerarReporte.Click += (_, _) => Ejecutar(GenerarReporteAsync);
        btnGenerarVista.Click += (_, _) => Ejecutar(GenerarVistaAsync);
    }

    public override string NombreVista => FormularioPrincipalMediator.Reportes;

    public override async Task RefrescarAsync()
    {
        await CargarPedidosAsync();
        await GenerarReporteAsync();
    }

    // Venta: pedidos ya cobrados; devolucion: pedidos devueltos
    private async Task CargarPedidosAsync()
    {
        var anterior = (cmbPedidoComprobante.SelectedItem as OpcionPedido)?.Id;
        var devolucion = cmbTipo.SelectedItem is TipoComprobante.Devolucion;
        var pedidos = (await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, IReadOnlyList<PedidoResumen>>(c => c.ListarAsync()))
            .Where(p => devolucion ? p.Devuelto : p.Estado is "PAGADO" or "DESPACHADO" or "ENTREGADO")
            .Select(OpcionPedido.Desde).ToList();
        cmbPedidoComprobante.DataSource = pedidos;
        cmbPedidoComprobante.SelectedItem = pedidos.FirstOrDefault(p => p.Id == anterior) ?? pedidos.FirstOrDefault();
    }

    private async Task EmitirAsync()
    {
        if (cmbPedidoComprobante.SelectedItem is not OpcionPedido pedido)
            return;

        var tipo = (TipoComprobante)cmbTipo.SelectedItem!;
        var medio = (string)cmbMedio.SelectedItem!;
        var resultado = await Ejecutor.EjecutarAsync<GenerarComprobanteUseCase, ResultadoComprobante>(g => g.EjecutarAsync(tipo, pedido.Id, medio));
        lblComprobante.Text = resultado.Mensaje;

        if (resultado.Exito && resultado.Salida!.Medio == MediosSalida.VistaPrevia)
        {
            using var vista = new FrmVistaPrevia($"{tipo.Nombre()} pedido {pedido.Id}", resultado.Salida.Contenido);
            vista.ShowDialog(this);
        }
    }

    private async Task GenerarReporteAsync()
    {
        var resultado = await Ejecutor.EjecutarAsync<ConsultarPedidosUseCase, ResultadoFiltro>(c => c.FiltrarAsync(txtFiltroReporte.Text));
        Rejilla.Cargar(dgvReporte, resultado.Pedidos);
        if (!resultado.Exito)
        {
            lblTotalesReporte.Text = resultado.Mensaje;
            return;
        }

        lblTotalesReporte.Text = $"{resultado.Pedidos.Count} pedido(s) · total USD {Presentacion.Monto(resultado.Pedidos.Sum(p => p.Total))}";
    }

    private async Task GenerarVistaAsync()
    {
        var filas = (int)nudFilas.Value;
        var r = await Ejecutor.EjecutarAsync<GenerarVistaMasivaUseCase, ResultadoVistaMasiva>(g => g.EjecutarAsync(filas));
        Rejilla.Cargar(dgvFilas, r.Renglones);

        var ahorro = r.BytesSinFlyweight == 0 ? 0 : 100.0 * (r.BytesSinFlyweight - r.BytesConFlyweight) / r.BytesSinFlyweight;
        lblVistaMasiva.Text =
            $"Filas generadas: {r.Filas} · metadatos compartidos: {r.MetadatosCompartidos}{Environment.NewLine}" +
            $"ReferenceEquals(fila 1, misma SKU más abajo) = {r.MismaReferencia}{Environment.NewLine}" +
            $"Memoria asignada con Flyweight: {r.BytesConFlyweight:N0} bytes{Environment.NewLine}" +
            $"Memoria asignada sin Flyweight: {r.BytesSinFlyweight:N0} bytes (ahorro {ahorro:0.0} %)";
    }
}
