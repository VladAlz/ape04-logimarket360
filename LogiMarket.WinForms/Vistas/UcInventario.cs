using LogiMarket.Application.Catalogo;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Inventario;
using LogiMarket.Application.Seguridad;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcInventario : VistaBase
{
    private const string Todos = "(todos)";

    public UcInventario(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        Rejilla.Definir(dgvStock,
            new Columna(nameof(StockDto.Sku), "SKU", Peso: 50), new Columna(nameof(StockDto.Nombre), "Producto", Peso: 110),
            new Columna(nameof(StockDto.Stock), "Stock", Peso: 50, Derecha: true), new Columna(nameof(StockDto.Alerta), "Alerta", Peso: 60));
        Rejilla.Definir(dgvMovimientos,
            new Columna(nameof(MovimientoDto.Fecha), "Fecha", "dd/MM/yyyy HH:mm", 90), new Columna(nameof(MovimientoDto.Sku), "SKU", Peso: 45),
            new Columna(nameof(MovimientoDto.Cantidad), "Cantidad", "+0;-0;0", 55), new Columna(nameof(MovimientoDto.StockResultante), "Stock final", Peso: 60, Derecha: true),
            new Columna(nameof(MovimientoDto.Motivo), "Motivo", Peso: 70), new Columna(nameof(MovimientoDto.Referencia), "Referencia", Peso: 110),
            new Columna(nameof(MovimientoDto.Usuario), "Usuario", Peso: 55));

        Permisos.Asociar(Operacion.AjustarInventario, btnAjustar);
        cmbSku.SelectedIndexChanged += (_, _) =>
        {
            if (cmbSku.SelectedItem is StockDto s)
                nudNuevoStock.Value = s.Stock;
        };
        btnAjustar.Click += (_, _) => Ejecutar(AjustarAsync);
        btnVerMovimientos.Click += (_, _) => Ejecutar(CargarMovimientosAsync);
    }

    public override string NombreVista => FormularioPrincipalMediator.Inventario;

    public override async Task RefrescarAsync()
    {
        var stock = (await Ejecutor.EjecutarAsync<ConsultarInventarioUseCase, IReadOnlyList<StockDto>>(c => c.StockAsync())).ToList();
        Rejilla.Cargar(dgvStock, stock, s => s.Sku);

        var skuAjuste = (cmbSku.SelectedItem as StockDto)?.Sku;
        cmbSku.DataSource = stock;
        cmbSku.DisplayMember = nameof(StockDto.Sku);
        cmbSku.SelectedItem = stock.FirstOrDefault(s => s.Sku == skuAjuste) ?? stock.FirstOrDefault();
        if (cmbSku.SelectedItem is StockDto elegido)
            nudNuevoStock.Value = elegido.Stock;

        var filtro = cmbFiltroMovimientos.SelectedItem as string;
        cmbFiltroMovimientos.DataSource = new[] { Todos }.Concat(stock.Select(s => s.Sku)).ToList();
        cmbFiltroMovimientos.SelectedItem = filtro ?? Todos;
        await CargarMovimientosAsync();
    }

    private async Task CargarMovimientosAsync()
    {
        var sku = cmbFiltroMovimientos.SelectedItem as string;
        Rejilla.Cargar(dgvMovimientos,
            await Ejecutor.EjecutarAsync<ConsultarInventarioUseCase, IReadOnlyList<MovimientoDto>>(c => c.MovimientosAsync(sku == Todos ? null : sku)));
    }

    private async Task AjustarAsync()
    {
        if (cmbSku.SelectedItem is not StockDto s)
            return;
        if (!Confirmar($"¿Ajustar el stock de {s.Sku} de {s.Stock} a {nudNuevoStock.Value}?"))
            return;

        var resultado = await Ejecutor.EjecutarAsync<GestionarCatalogoUseCase, ResultadoOperacion>(g =>
            g.AjustarStockAsync(s.Sku, (int)nudNuevoStock.Value, txtMotivo.Text));
        Mostrar(lblAjuste, resultado);
        if (resultado.Exito)
            await AvisarAsync(EventoVista.InventarioAjustado);
    }
}
