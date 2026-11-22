using LogiMarket.Application.Consultas.Filtros;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.Pedidos;

public sealed record SeguimientoDto(string NumeroGuia, int PedidoId, string Estado, string Familia, string Etiqueta,
    DateTime FechaDespacho, DateTime FechaEstimada, DateTime? FechaEntrega);

public sealed record ResultadoFiltro(bool Exito, string Mensaje, IReadOnlyList<PedidoResumen> Pedidos);

public sealed record DevolucionDto(int PedidoId, string Motivo, DateTime FechaSolicitud, int DiasDesdeEntrega, decimal Reembolso, string Referencia, string Usuario);

// Lado de consulta (CQRS): solo lee, nunca modifica
public sealed class ConsultarPedidosUseCase
{
    private readonly IPedidoLectura _lectura;
    private readonly IDespachoRepositorio _despachos;
    private readonly IDevolucionRepositorio _devoluciones;

    public ConsultarPedidosUseCase(IPedidoLectura lectura, IDespachoRepositorio despachos, IDevolucionRepositorio devoluciones)
    {
        _lectura = lectura;
        _despachos = despachos;
        _devoluciones = devoluciones;
    }

    public async Task<IReadOnlyList<DevolucionDto>> DevolucionesAsync() =>
        (await _devoluciones.ListarAsync())
            .Select(d => new DevolucionDto(d.PedidoId, d.Motivo.ToString().ToUpperInvariant(), d.FechaSolicitud, d.DiasDesdeEntrega,
                d.Reembolso, d.ReferenciaReembolso, d.Usuario))
            .ToList();

    public Task<IReadOnlyList<PedidoResumen>> ListarAsync() => _lectura.ListarAsync();

    public Task<PedidoDetalleDto?> DetalleAsync(int pedidoId) => _lectura.ObtenerDetalleAsync(pedidoId);

    public async Task<ResultadoFiltro> FiltrarAsync(string expresion)
    {
        var pedidos = await _lectura.ListarAsync();
        if (string.IsNullOrWhiteSpace(expresion))
            return new ResultadoFiltro(true, $"{pedidos.Count} pedido(s).", pedidos);

        try
        {
            var arbol = new AnalizadorFiltro(expresion).Analizar();
            var filtrados = pedidos.Where(arbol.Evaluar).ToList();
            return new ResultadoFiltro(true, $"{filtrados.Count} pedido(s) cumplen {arbol.Describir()}.", filtrados);
        }
        catch (ErrorFiltroException ex)
        {
            return new ResultadoFiltro(false, ex.Message, Array.Empty<PedidoResumen>());
        }
    }

    public async Task<SeguimientoDto?> SeguimientoAsync(string numeroGuia)
    {
        var despacho = await _despachos.ObtenerPorGuiaAsync(numeroGuia.Trim().ToUpperInvariant());
        if (despacho is null)
            return null;

        var pedido = await _lectura.ObtenerDetalleAsync(despacho.PedidoId);
        return new SeguimientoDto(despacho.NumeroGuia, despacho.PedidoId, pedido?.Resumen.Estado ?? "-",
            despacho.Familia, despacho.Etiqueta, despacho.FechaDespacho, despacho.FechaEstimada, despacho.FechaEntrega);
    }
}
