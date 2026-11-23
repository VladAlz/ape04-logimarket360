using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Comun;

namespace LogiMarket.Application.Comprobantes;

public sealed record ResultadoComprobante(bool Exito, string Mensaje, SalidaEmitida? Salida);

public sealed class GenerarComprobanteUseCase
{
    private readonly IPedidoLectura _pedidos;
    private readonly IPagoRepositorio _pagos;
    private readonly IDevolucionRepositorio _devoluciones;
    private readonly IEnumerable<ISalidaDocumento> _salidas;
    private readonly ServicioAutorizacion _autorizacion;
    private readonly IReloj _reloj;

    public GenerarComprobanteUseCase(IPedidoLectura pedidos, IPagoRepositorio pagos, IDevolucionRepositorio devoluciones,
        IEnumerable<ISalidaDocumento> salidas, ServicioAutorizacion autorizacion, IReloj reloj)
    {
        _pedidos = pedidos;
        _pagos = pagos;
        _devoluciones = devoluciones;
        _salidas = salidas;
        _autorizacion = autorizacion;
        _reloj = reloj;
    }

    public IReadOnlyList<string> Medios => _salidas.Select(s => s.Medio).ToList();

    public async Task<ResultadoComprobante> EjecutarAsync(TipoComprobante tipo, int pedidoId, string medio)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.EmitirComprobante);
            var salida = _salidas.SingleOrDefault(s => s.Medio == medio)
                ?? throw new ReglaNegocioException($"Medio de salida no disponible: {medio}.");
            var pedido = await _pedidos.ObtenerDetalleAsync(pedidoId)
                ?? throw new ReglaNegocioException($"El pedido {pedidoId} no existe.");

            // Cualquier comprobante con cualquier salida, sin clases por combinacion
            Comprobante comprobante = tipo switch
            {
                TipoComprobante.Venta => new ComprobanteVenta(salida, pedido,
                    (await _pagos.ObtenerPorPedidoAsync(pedidoId))?.Referencia
                        ?? throw new ReglaNegocioException($"El pedido {pedidoId} no tiene un pago aprobado."),
                    _reloj.Ahora),
                _ => new ComprobanteDevolucion(salida, pedido,
                    await _devoluciones.ObtenerPorPedidoAsync(pedidoId)
                        ?? throw new ReglaNegocioException($"El pedido {pedidoId} no tiene devolución registrada."))
            };

            var emitida = comprobante.Emitir();
            return new ResultadoComprobante(true, $"Comprobante de {tipo.Nombre().ToLowerInvariant()} emitido en {emitida.Medio}: {emitida.Destino}", emitida);
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException)
        {
            return new ResultadoComprobante(false, ex.Message, null);
        }
    }
}
