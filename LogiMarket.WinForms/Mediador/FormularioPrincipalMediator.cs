namespace LogiMarket.WinForms.Mediador;

// Mediator: las pantallas no se llaman entre si; avisan aqui y el mediador decide a quien refrescar
public sealed class FormularioPrincipalMediator : IMediadorVistas
{
    public const string Panel = "Panel";
    public const string Catalogo = "Catálogo";
    public const string Clientes = "Clientes";
    public const string Carrito = "Carrito";
    public const string Pedidos = "Pedidos";
    public const string Cobro = "Cobro";
    public const string Despacho = "Despacho";
    public const string Devoluciones = "Devoluciones";
    public const string Inventario = "Inventario";
    public const string Reportes = "Reportes";
    public const string Auditoria = "Auditoría";

    private static readonly Dictionary<EventoVista, string[]> Destinos = new()
    {
        [EventoVista.CatalogoCambiado] = new[] { Panel, Catalogo, Carrito, Inventario, Reportes },
        [EventoVista.ClientesCambiados] = new[] { Clientes, Carrito },
        [EventoVista.BorradorCambiado] = new[] { Panel, Pedidos, Carrito },
        [EventoVista.PedidoValidado] = new[] { Panel, Pedidos, Carrito, Cobro },
        [EventoVista.PedidoConfirmado] = new[] { Panel, Catalogo, Pedidos, Cobro, Despacho, Inventario, Reportes, Auditoria },
        [EventoVista.PedidoCancelado] = new[] { Panel, Pedidos, Carrito, Cobro },
        [EventoVista.PedidoDespachado] = new[] { Panel, Pedidos, Despacho, Auditoria },
        [EventoVista.PedidoEntregado] = new[] { Panel, Pedidos, Despacho, Devoluciones },
        [EventoVista.DevolucionRegistrada] = new[] { Panel, Catalogo, Pedidos, Devoluciones, Inventario, Reportes, Auditoria },
        [EventoVista.InventarioAjustado] = new[] { Panel, Catalogo, Inventario },
        [EventoVista.SimulacionCambiada] = new[] { Cobro, Auditoria },
        [EventoVista.OutboxProcesado] = new[] { Panel, Auditoria }
    };

    private readonly List<IVistaColaboradora> _vistas = new();

    public event Action<string>? Notificado;

    public void Registrar(IVistaColaboradora vista)
    {
        if (!_vistas.Contains(vista))
            _vistas.Add(vista);
    }

    public void Reiniciar() => _vistas.Clear();

    public async Task NotificarAsync(IVistaColaboradora? emisor, EventoVista evento)
    {
        var destinos = evento == EventoVista.SesionIniciada
            ? _vistas.ToList()
            : _vistas.Where(v => Destinos[evento].Contains(v.NombreVista)).ToList();

        var actualizadas = new List<string>();
        foreach (var vista in destinos)
        {
            // El emisor tambien se refresca para mostrar el resultado guardado
            await vista.RefrescarAsync();
            actualizadas.Add(vista.NombreVista);
        }

        Notificado?.Invoke($"Mediador: {evento} desde {emisor?.NombreVista ?? "Sistema"} → actualizadas: {string.Join(", ", actualizadas)} ({DateTime.Now:HH:mm:ss})");
    }
}
