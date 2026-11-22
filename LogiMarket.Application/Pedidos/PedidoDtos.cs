namespace LogiMarket.Application.Pedidos;

public sealed record PedidoResumen(
    int Id,
    string Cliente,
    string Categoria,
    string Estado,
    string Entrega,
    int Unidades,
    decimal Subtotal,
    string Politica,
    decimal Descuento,
    decimal Tarifa,
    decimal Total,
    DateTime FechaCreacion,
    bool Devuelto);

public sealed record LineaPedidoDto(string Codigo, string Nombre, string Tipo, int Cantidad, decimal PrecioUnitario, decimal Subtotal, string Composicion);

public sealed record HistorialPedidoDto(DateTime Fecha, string EstadoAnterior, string EstadoNuevo, string Usuario, string Detalle);

public sealed record PedidoDetalleDto(
    PedidoResumen Resumen,
    IReadOnlyList<LineaPedidoDto> Lineas,
    IReadOnlyList<HistorialPedidoDto> Historial,
    int PlazoDias,
    DateTime? FechaEntrega);
