namespace LogiMarket.Domain.Catalogo.Visitantes;

public sealed class VisitanteDesgloseFisico : IVisitanteCatalogo
{
    private readonly List<ComponenteFisico> _lineas = new();
    private readonly List<string> _kitsVisitados = new();

    public IReadOnlyList<ComponenteFisico> Lineas => _lineas;
    public IReadOnlyList<string> KitsVisitados => _kitsVisitados;

    public void VisitarProducto(ProductoSimple producto, int cantidad)
    {
        var indice = _lineas.FindIndex(l => l.Sku == producto.Codigo);
        if (indice < 0)
            _lineas.Add(new ComponenteFisico(producto.Codigo, producto.Nombre, cantidad, producto.Precio, producto.PesoKg));
        else
            _lineas[indice] = _lineas[indice] with { Cantidad = _lineas[indice].Cantidad + cantidad };
    }

    public void VisitarKit(KitCompuesto kit, int cantidad) =>
        _kitsVisitados.Add($"{kit.Codigo} x{cantidad}");
}
