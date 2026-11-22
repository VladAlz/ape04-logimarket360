using LogiMarket.Application.Comun;
using LogiMarket.Application.Consultas;
using LogiMarket.Application.Inventario;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcPanel : VistaBase
{
    private sealed record Fila(string Nombre, int Valor);

    public UcPanel(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        Rejilla.Definir(dgvEstados, new Columna(nameof(Fila.Nombre), "Estado"), new Columna(nameof(Fila.Valor), "Pedidos", Derecha: true));
        Rejilla.Definir(dgvAlertas, new Columna(nameof(StockDto.Sku), "SKU", Peso: 60), new Columna(nameof(StockDto.Nombre), "Producto"),
            new Columna(nameof(StockDto.Stock), "Stock", Peso: 50, Derecha: true), new Columna(nameof(StockDto.Alerta), "Alerta", Peso: 60));
        Rejilla.Definir(dgvMetricas, new Columna(nameof(Fila.Nombre), "Contador", Peso: 160), new Columna(nameof(Fila.Valor), "Valor", Derecha: true));
        btnActualizar.Click += (_, _) => Ejecutar(RefrescarAsync);
    }

    public override string NombreVista => FormularioPrincipalMediator.Panel;

    public override async Task RefrescarAsync()
    {
        var panel = await Ejecutor.EjecutarAsync<PanelGeneralUseCase, PanelDto>(p => p.ObtenerAsync());

        Rejilla.Cargar(dgvEstados, panel.PedidosPorEstado.Select(e => new Fila(e.Key, e.Value)));
        Rejilla.Cargar(dgvAlertas, panel.Alertas);
        Rejilla.Cargar(dgvMetricas, panel.Metricas.Select(m => new Fila(m.Key, m.Value)));
        lblVentas.Text = Presentacion.Monto(panel.VentasCobradas);
        lblOutbox.Text = panel.EventosPendientes.ToString();
        lblInstancia.Text = $"Instancia de MetricasSesion:{Environment.NewLine}{panel.InstanciaMetricas}";
    }
}
