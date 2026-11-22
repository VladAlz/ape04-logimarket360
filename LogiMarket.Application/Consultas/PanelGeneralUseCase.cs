using LogiMarket.Application.Inventario;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.Consultas;

public sealed record PanelDto(
    IReadOnlyList<KeyValuePair<string, int>> PedidosPorEstado,
    decimal VentasCobradas,
    IReadOnlyList<StockDto> Alertas,
    int EventosPendientes,
    IReadOnlyList<KeyValuePair<string, int>> Metricas,
    Guid InstanciaMetricas);

public sealed class PanelGeneralUseCase
{
    private readonly IPedidoLectura _pedidos;
    private readonly ConsultarInventarioUseCase _inventario;
    private readonly IOutboxRepositorio _outbox;
    private readonly MetricasSesion _metricas;

    public PanelGeneralUseCase(IPedidoLectura pedidos, ConsultarInventarioUseCase inventario, IOutboxRepositorio outbox, MetricasSesion metricas)
    {
        _pedidos = pedidos;
        _inventario = inventario;
        _outbox = outbox;
        _metricas = metricas;
    }

    public async Task<PanelDto> ObtenerAsync()
    {
        var pedidos = await _pedidos.ListarAsync();
        var estados = new[] { "BORRADOR", "VALIDADO", "PAGADO", "DESPACHADO", "ENTREGADO", "CANCELADO" }
            .Select(e => new KeyValuePair<string, int>(e, pedidos.Count(p => p.Estado == e)))
            .ToList();
        var cobradas = pedidos.Where(p => p.Estado is "PAGADO" or "DESPACHADO" or "ENTREGADO").Sum(p => p.Total);
        var alertas = (await _inventario.StockAsync()).Where(s => s.Alerta != "OK").ToList();

        return new PanelDto(estados, cobradas, alertas, (await _outbox.ListarPendientesAsync()).Count,
            _metricas.Instantanea(), _metricas.Instancia);
    }
}
