using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Application.Pedidos;

public static class PedidoMapper
{
    public static PedidoResumen Resumen(Pedido p) => new(
        p.Id, p.ClienteNombre, p.CategoriaCliente.ToString().ToUpperInvariant(), p.EstadoCodigo,
        Entrega(p.Entrega), p.UnidadesFisicas, p.Subtotal, p.Politica, p.Descuento, p.Tarifa, p.Total,
        p.FechaCreacion, p.Devuelto);

    public static PedidoDetalleDto Detalle(Pedido p) => new(
        Resumen(p),
        p.Lineas.Select(l => new LineaPedidoDto(
            l.Codigo, l.Nombre, l.EsKit ? "KIT" : "PRODUCTO", l.Cantidad, l.PrecioUnitario, l.Subtotal,
            string.Join(" + ", l.Componentes.Select(c => $"{c.Sku}x{c.CantidadPorUnidad}")))).ToList(),
        p.Historial.OrderBy(h => h.Fecha).ThenBy(h => h.Id)
            .Select(h => new HistorialPedidoDto(h.Fecha, h.EstadoAnterior, h.EstadoNuevo, h.Usuario, h.Detalle)).ToList(),
        p.PlazoDias,
        p.FechaEntrega);

    public static string Entrega(FormaEntrega forma) => forma == FormaEntrega.Expres ? "EXPRÉS" : "ESTÁNDAR";
}
