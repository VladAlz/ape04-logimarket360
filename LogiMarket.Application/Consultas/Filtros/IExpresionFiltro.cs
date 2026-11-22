using LogiMarket.Application.Pedidos;

namespace LogiMarket.Application.Consultas.Filtros;

// Interpreter: cada nodo del arbol sabe evaluarse sobre un pedido
public interface IExpresionFiltro
{
    bool Evaluar(PedidoResumen pedido);

    string Describir();
}
