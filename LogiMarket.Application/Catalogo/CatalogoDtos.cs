using LogiMarket.Domain.Catalogo;

namespace LogiMarket.Application.Catalogo;

public sealed record ProductoDto(string Sku, string Nombre, string Categoria, string Descripcion, decimal Precio, decimal PesoKg, int Stock);

public sealed record KitDto(string Codigo, string Nombre, decimal Precio, decimal PesoKg, string Composicion, int UnidadesFisicas);

public sealed record ElementoVentaDto(string Codigo, string Descripcion, decimal Precio)
{
    public override string ToString() => Descripcion;
}

public sealed record DesgloseDto(
    string Codigo,
    string Nombre,
    decimal Precio,
    IReadOnlyList<ComponenteFisico> Desglose,
    decimal PesoTotalKg,
    IReadOnlyList<string> KitsVisitados,
    IReadOnlyList<string> RecorridoIterador);

public sealed record ProductoRequest(string Sku, string Nombre, string Categoria, string Descripcion, decimal Precio, decimal PesoKg, int Stock);

public sealed record KitRequest(string Codigo, string Nombre, IReadOnlyList<(string Codigo, int Cantidad)> Componentes);
