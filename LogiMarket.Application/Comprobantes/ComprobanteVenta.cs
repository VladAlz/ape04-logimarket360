using LogiMarket.Application.Comun;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.Comprobantes;

public sealed class ComprobanteVenta : Comprobante
{
    private readonly PedidoDetalleDto _pedido;
    private readonly string _referenciaPago;
    private readonly DateTime _fecha;

    public ComprobanteVenta(ISalidaDocumento salida, PedidoDetalleDto pedido, string referenciaPago, DateTime fecha)
        : base(salida)
    {
        _pedido = pedido;
        _referenciaPago = referenciaPago;
        _fecha = fecha;
    }

    protected override DocumentoTexto Componer()
    {
        var r = _pedido.Resumen;
        var lineas = new List<string>
        {
            "LOGIMARKET 360 - COMPROBANTE INTERNO DE VENTA",
            "Documento de demostración, sin validez tributaria",
            new string('-', 60),
            Linea("Pedido:", r.Id.ToString()),
            Linea("Cliente:", $"{r.Cliente} ({r.Categoria})"),
            Linea("Fecha emisión:", _fecha.ToString("dd/MM/yyyy HH:mm")),
            Linea("Referencia de pago:", _referenciaPago),
            Linea("Entrega:", r.Entrega),
            new string('-', 60)
        };

        lineas.AddRange(_pedido.Lineas.Select(l =>
            $"{l.Codigo,-16}{l.Cantidad,4} x {Presentacion.Monto(l.PrecioUnitario),8} = {Presentacion.Monto(l.Subtotal),9}  {l.Composicion}"));

        lineas.AddRange(new[]
        {
            new string('-', 60),
            Linea("Subtotal:", Presentacion.Monto(r.Subtotal)),
            Linea($"Descuento {r.Politica}:", Presentacion.Monto(r.Descuento)),
            Linea("Tarifa de envío:", Presentacion.Monto(r.Tarifa)),
            Linea("TOTAL USD:", Presentacion.Monto(r.Total))
        });

        return new DocumentoTexto($"Comprobante de venta {r.Id}", $"venta_pedido_{r.Id:0000}.txt", lineas);
    }
}
