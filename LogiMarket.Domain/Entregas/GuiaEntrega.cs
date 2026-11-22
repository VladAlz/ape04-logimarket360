using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Domain.Entregas;

public sealed record GuiaEntrega(string Numero, FormaEntrega Familia, string Etiqueta, DateTime FechaEstimada);
