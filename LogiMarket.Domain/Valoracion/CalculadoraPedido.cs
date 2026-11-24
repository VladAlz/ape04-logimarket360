using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Entregas;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Promociones;

namespace LogiMarket.Domain.Valoracion;

// subtotal - descuento + tarifa; no conoce las politicas concretas
public sealed class CalculadoraPedido
{
    private readonly SelectorPoliticaDescuento _selector;
    private readonly IReadOnlyList<IFabricaEntrega> _fabricas;

    public CalculadoraPedido(SelectorPoliticaDescuento selector, IEnumerable<IFabricaEntrega> fabricas)
    {
        _selector = selector;
        _fabricas = fabricas.ToList();
    }

    public ValoracionPedido Valorar(Pedido pedido)
    {
        var subtotal = Dinero.Redondear(pedido.Lineas.Sum(l => l.Subtotal));
        var unidades = 0;
        foreach (var linea in pedido.Lineas)
        {
            if (linea.EsKit)
            {
                unidades += linea.Cantidad;
                continue;
            }

            foreach (var componente in linea.Componentes)
            {
                unidades += componente.CantidadPorUnidad * linea.Cantidad;
            }
        }
        var politica = _selector.Elegir(new ContextoDescuento(pedido.CategoriaCliente, unidades, subtotal));
        var descuento = Dinero.Redondear(subtotal * politica.Tasa);
        var entrega = ServicioEntrega.Para(pedido.Entrega, _fabricas);

        return new ValoracionPedido(subtotal, politica.Nombre, politica.Tasa, descuento,
            entrega.Tarifa.Calcular(), entrega.Plazo.Dias, unidades);
    }
}
