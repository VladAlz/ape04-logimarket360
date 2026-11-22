namespace LogiMarket.Domain.Catalogo.Visitantes;

public sealed class VisitantePesoLogistico : IVisitanteCatalogo
{
    public decimal TotalKg { get; private set; }
    public int NivelesKit { get; private set; }

    public void VisitarProducto(ProductoSimple producto, int cantidad) =>
        TotalKg += producto.PesoKg * cantidad;

    public void VisitarKit(KitCompuesto kit, int cantidad) => NivelesKit++;
}
