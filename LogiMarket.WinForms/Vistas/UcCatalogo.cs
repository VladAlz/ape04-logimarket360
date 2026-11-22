using LogiMarket.Application.Catalogo;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Seguridad;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcCatalogo : VistaBase
{
    private readonly ErrorProvider _errores = new();
    private readonly List<(string Codigo, int Cantidad)> _componentes = new();

    public UcCatalogo(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos)
        : base(ejecutor, mediador, permisos)
    {
        InitializeComponent();
        Rejilla.Definir(dgvProductos,
            new Columna(nameof(ProductoDto.Sku), "SKU", Peso: 50), new Columna(nameof(ProductoDto.Nombre), "Nombre", Peso: 110),
            new Columna(nameof(ProductoDto.Categoria), "Categoría", Peso: 90), new Columna(nameof(ProductoDto.Precio), "Precio", "0.00", 60),
            new Columna(nameof(ProductoDto.PesoKg), "Peso kg", "0.00", 55), new Columna(nameof(ProductoDto.Stock), "Stock", Peso: 50, Derecha: true));
        Rejilla.Definir(dgvKits,
            new Columna(nameof(KitDto.Codigo), "Código", Peso: 110), new Columna(nameof(KitDto.Nombre), "Nombre", Peso: 100),
            new Columna(nameof(KitDto.Composicion), "Composición", Peso: 130), new Columna(nameof(KitDto.Precio), "Precio", "0.00", 60),
            new Columna(nameof(KitDto.PesoKg), "Peso kg", "0.00", 55), new Columna(nameof(KitDto.UnidadesFisicas), "Unidades", Peso: 55, Derecha: true));

        Permisos.Asociar(Operacion.GestionarCatalogo, btnNuevoProducto, btnGuardarProducto, btnEliminarProducto,
            btnAgregarComponente, btnQuitarComponente, btnNuevoKit, btnGuardarKit);

        btnBuscar.Click += (_, _) => Ejecutar(CargarProductosAsync);
        btnLimpiarBusqueda.Click += (_, _) => { txtBuscar.Clear(); Ejecutar(CargarProductosAsync); };
        dgvProductos.SelectionChanged += (_, _) => MostrarProducto();
        btnNuevoProducto.Click += (_, _) => LimpiarProducto();
        btnGuardarProducto.Click += (_, _) => Ejecutar(GuardarProductoAsync);
        btnEliminarProducto.Click += (_, _) => Ejecutar(EliminarProductoAsync);
        dgvKits.SelectionChanged += (_, _) => Ejecutar(MostrarKitAsync);
        btnAgregarComponente.Click += (_, _) => AgregarComponente();
        btnQuitarComponente.Click += (_, _) => QuitarComponente();
        btnNuevoKit.Click += (_, _) => LimpiarKit();
        btnGuardarKit.Click += (_, _) => Ejecutar(GuardarKitAsync);
        btnDesglosar.Click += (_, _) => Ejecutar(DesglosarAsync);
    }

    public override string NombreVista => FormularioPrincipalMediator.Catalogo;

    public override async Task RefrescarAsync()
    {
        await CargarProductosAsync();
        Rejilla.Cargar(dgvKits, await Ejecutor.EjecutarAsync<ConsultarCatalogoUseCase, IReadOnlyList<KitDto>>(c => c.KitsAsync()), k => k.Codigo);

        var elementos = await Ejecutor.EjecutarAsync<ConsultarCatalogoUseCase, IReadOnlyList<ElementoVentaDto>>(c => c.ElementosVentaAsync());
        var elegido = cmbComponente.SelectedItem as ElementoVentaDto;
        cmbComponente.DataSource = elementos.ToList();
        if (elegido is not null)
            cmbComponente.SelectedItem = elementos.FirstOrDefault(e => e.Codigo == elegido.Codigo);
    }

    private async Task CargarProductosAsync() =>
        Rejilla.Cargar(dgvProductos,
            await Ejecutor.EjecutarAsync<ConsultarCatalogoUseCase, IReadOnlyList<ProductoDto>>(c => c.ProductosAsync(txtBuscar.Text)),
            p => p.Sku, MostrarProducto);

    private void MostrarProducto()
    {
        if (Rejilla.Seleccionado<ProductoDto>(dgvProductos) is not { } p)
            return;

        txtSku.Text = p.Sku;
        txtSku.ReadOnly = true;
        txtNombre.Text = p.Nombre;
        txtCategoria.Text = p.Categoria;
        txtDescripcion.Text = p.Descripcion;
        txtPrecio.Text = Presentacion.Monto(p.Precio);
        txtPeso.Text = Presentacion.Peso(p.PesoKg);
        nudStock.Value = p.Stock;
        _errores.Clear();
    }

    private void LimpiarProducto()
    {
        txtSku.ReadOnly = false;
        foreach (var caja in new[] { txtSku, txtNombre, txtCategoria, txtDescripcion, txtPrecio, txtPeso })
            caja.Clear();
        nudStock.Value = 0;
        lblMensajeProducto.Text = "Ingrese los datos del producto nuevo.";
        txtSku.Focus();
    }

    private async Task GuardarProductoAsync()
    {
        _errores.Clear();
        if (!Texto.IntentarDecimal(txtPrecio.Text, out var precio))
            _errores.SetError(txtPrecio, "Precio no válido.");
        if (!Texto.IntentarDecimal(txtPeso.Text, out var peso))
            _errores.SetError(txtPeso, "Peso no válido.");
        if (_errores.HasErrors)
            return;

        var resultado = await Ejecutor.EjecutarAsync<GestionarCatalogoUseCase, ResultadoOperacion>(g => g.GuardarProductoAsync(
            new ProductoRequest(txtSku.Text, txtNombre.Text, txtCategoria.Text, txtDescripcion.Text, precio, peso, (int)nudStock.Value)));
        Mostrar(lblMensajeProducto, resultado);
        if (resultado.Exito)
            await AvisarAsync(EventoVista.CatalogoCambiado);
    }

    private async Task EliminarProductoAsync()
    {
        if (string.IsNullOrWhiteSpace(txtSku.Text) || !Confirmar($"¿Eliminar el producto {txtSku.Text}? Esta acción no se puede deshacer."))
            return;

        var resultado = await Ejecutor.EjecutarAsync<GestionarCatalogoUseCase, ResultadoOperacion>(g => g.EliminarProductoAsync(txtSku.Text));
        Mostrar(lblMensajeProducto, resultado);
        if (resultado.Exito)
        {
            LimpiarProducto();
            await AvisarAsync(EventoVista.CatalogoCambiado);
        }
    }

    private async Task MostrarKitAsync()
    {
        if (Rejilla.Seleccionado<KitDto>(dgvKits) is not { } kit)
            return;

        txtCodigoKit.Text = kit.Codigo;
        txtCodigoKit.ReadOnly = true;
        txtNombreKit.Text = kit.Nombre;
        _componentes.Clear();
        foreach (var dato in await Ejecutor.EjecutarAsync<ConsultarCatalogoUseCase, IReadOnlyList<string>>(c => c.ComponentesDeKitAsync(kit.Codigo)))
        {
            var partes = dato.Split('|');
            _componentes.Add((partes[0], int.Parse(partes[1])));
        }
        MostrarComponentes();
    }

    private void AgregarComponente()
    {
        if (cmbComponente.SelectedItem is not ElementoVentaDto elemento)
            return;

        var cantidad = (int)nudCantidadComponente.Value;
        var indice = _componentes.FindIndex(c => c.Codigo == elemento.Codigo);
        if (indice >= 0)
            _componentes[indice] = (elemento.Codigo, _componentes[indice].Cantidad + cantidad);
        else
            _componentes.Add((elemento.Codigo, cantidad));
        MostrarComponentes();
    }

    private void QuitarComponente()
    {
        if (lstComponentes.SelectedIndex < 0)
            return;
        _componentes.RemoveAt(lstComponentes.SelectedIndex);
        MostrarComponentes();
    }

    private void LimpiarKit()
    {
        txtCodigoKit.ReadOnly = false;
        txtCodigoKit.Clear();
        txtNombreKit.Clear();
        _componentes.Clear();
        MostrarComponentes();
        lblMensajeKit.Text = "Kit nuevo.";
        txtCodigoKit.Focus();
    }

    private void MostrarComponentes()
    {
        lstComponentes.Items.Clear();
        foreach (var (codigo, cantidad) in _componentes)
            lstComponentes.Items.Add($"{codigo} x{cantidad}");
    }

    private async Task GuardarKitAsync()
    {
        var resultado = await Ejecutor.EjecutarAsync<GestionarCatalogoUseCase, ResultadoOperacion>(g =>
            g.GuardarKitAsync(new KitRequest(txtCodigoKit.Text, txtNombreKit.Text, _componentes.ToList())));
        Mostrar(lblMensajeKit, resultado);
        if (resultado.Exito)
            await AvisarAsync(EventoVista.CatalogoCambiado);
    }

    private async Task DesglosarAsync()
    {
        var codigo = Rejilla.Seleccionado<KitDto>(dgvKits)?.Codigo ?? Rejilla.Seleccionado<ProductoDto>(dgvProductos)?.Sku;
        if (codigo is null)
            return;

        var d = await Ejecutor.EjecutarAsync<ConsultarCatalogoUseCase, DesgloseDto>(c => c.DesglosarAsync(codigo));
        var lineas = new List<string> { $"{d.Codigo} - {d.Nombre}   Precio USD {Presentacion.Monto(d.Precio)}", "" };
        lineas.Add("Desglose físico por SKU (VisitanteDesgloseFisico):");
        lineas.AddRange(d.Desglose.Select(c => $"  {c.Sku,-6} {c.Nombre,-14} x{c.Cantidad,-3} {Presentacion.Peso(c.PesoKg)} kg c/u"));
        lineas.Add($"Kits visitados: {(d.KitsVisitados.Count == 0 ? "-" : string.Join(" > ", d.KitsVisitados))}");
        lineas.Add($"Peso logístico (VisitantePesoLogistico): {Presentacion.Peso(d.PesoTotalKg)} kg");
        lineas.Add("");
        lineas.Add("Recorrido del iterador de componentes:");
        lineas.AddRange(d.RecorridoIterador.Select(r => "  " + r));
        txtDesglose.Text = Texto.Lineas(lineas);
    }
}
