using LogiMarket.Application.Comun;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Puertos;
using LogiMarket.Domain.Devoluciones;

namespace LogiMarket.Application.Comprobantes;

public sealed class ComprobanteDevolucion : Comprobante
{
    private readonly PedidoDetalleDto _pedido;
    private readonly Devolucion _devolucion;

    public ComprobanteDevolucion(ISalidaDocumento salida, PedidoDetalleDto pedido, Devolucion devolucion)
        : base(salida)
    {
        _pedido = pedido;
        _devolucion = devolucion;
    }

    protected override DocumentoTexto Componer()
    {
        var r = _pedido.Resumen;
        var lineas = new List<string>
        {
            "LOGIMARKET 360 - COMPROBANTE INTERNO DE DEVOLUCIÓN",
            "Documento de demostración, sin validez tributaria",
            new string('-', 60),
            Linea("Pedido:", r.Id.ToString()),
            Linea("Cliente:", r.Cliente),
            Linea("Motivo:", _devolucion.Motivo.ToString().ToUpperInvariant()),
            Linea("Fecha solicitud:", _devolucion.FechaSolicitud.ToString("dd/MM/yyyy")),
            Linea("Días desde entrega:", _devolucion.DiasDesdeEntrega.ToString()),
            Linea("Referencia reembolso:", _devolucion.ReferenciaReembolso),
            new string('-', 60)
        };

        lineas.AddRange(_pedido.Lineas.Select(l => $"{l.Codigo,-16}{l.Cantidad,4}  {l.Composicion}"));
        lineas.AddRange(new[]
        {
            new string('-', 60),
            Linea("Total pagado:", Presentacion.Monto(r.Total)),
            Linea("Tarifa no reembolsable:", Presentacion.Monto(r.Tarifa)),
            Linea("REEMBOLSO USD:", Presentacion.Monto(_devolucion.Reembolso))
        });

        return new DocumentoTexto($"Comprobante de devolución {r.Id}", $"devolucion_pedido_{r.Id:0000}.txt", lineas);
    }
}
