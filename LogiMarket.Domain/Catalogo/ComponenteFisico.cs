namespace LogiMarket.Domain.Catalogo;

public sealed record ComponenteFisico(string Sku, string Nombre, int Cantidad, decimal PrecioUnitario, decimal PesoKg);
