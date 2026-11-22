namespace LogiMarket.Application.Confirmacion;

public sealed record SolicitudConfirmacion(int PedidoId, string ClaveIdempotencia);

public sealed record ResultadoConfirmacion(
    bool Exito,
    bool Repetido,
    string Mensaje,
    int PedidoId,
    string Estado,
    decimal Total,
    string ReferenciaPago,
    IReadOnlyList<string> Trazas);
