using LogiMarket.Domain.Catalogo;
using LogiMarket.Domain.Clientes;
using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Pedidos;

// Builder: el pedido solo existe si tiene cliente y al menos una linea
public sealed class PedidoBuilder
{
    private Cliente? _cliente;
    private FormaEntrega _entrega = FormaEntrega.Estandar;
    private readonly List<(IElementoCatalogo Elemento, int Cantidad)> _lineas = new();
    private DateTime _fecha = DateTime.Now;
    private string _usuario = "sistema";

    public PedidoBuilder ParaCliente(Cliente cliente)
    {
        _cliente = cliente;
        return this;
    }

    public PedidoBuilder ConEntrega(FormaEntrega entrega)
    {
        _entrega = entrega;
        return this;
    }

    public PedidoBuilder Agregar(IElementoCatalogo elemento, int cantidad)
    {
        _lineas.Add((elemento, cantidad));
        return this;
    }

    public PedidoBuilder RegistradoPor(string usuario, DateTime fecha)
    {
        _usuario = usuario;
        _fecha = fecha;
        return this;
    }

    public Pedido Construir()
    {
        if (_cliente is null)
            throw new ReglaNegocioException("No se puede crear un pedido sin cliente.");
        if (_lineas.Count == 0)
            throw new ReglaNegocioException("No se puede crear un pedido sin líneas.");

        // Lineas repetidas se juntan en una sola
        var lineas = _lineas
            .GroupBy(l => l.Elemento.Codigo)
            .Select(g => new PedidoDetalle(g.First().Elemento, g.Sum(x => x.Cantidad)))
            .ToList();

        return new Pedido(_cliente, _entrega, lineas, _fecha, _usuario);
    }
}
