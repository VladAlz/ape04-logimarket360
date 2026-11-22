using LogiMarket.Domain.Devoluciones;

namespace LogiMarket.Application.Devoluciones;

public sealed record SolicitudDevolucion(int PedidoId, MotivoDevolucion Motivo, DateTime FechaSolicitud, bool ProductoSinUso);

public sealed record ResultadoDevolucion(bool Exito, string Mensaje, int PedidoId, decimal Reembolso, IReadOnlyList<string> Pasos);
