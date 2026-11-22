namespace LogiMarket.Domain.Catalogo.Visitantes;

public interface IVisitanteCatalogo
{
    void VisitarProducto(ProductoSimple producto, int cantidad);

    void VisitarKit(KitCompuesto kit, int cantidad);
}
