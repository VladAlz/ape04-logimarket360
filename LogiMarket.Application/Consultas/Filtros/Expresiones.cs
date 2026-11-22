using System.Globalization;
using LogiMarket.Application.Pedidos;

namespace LogiMarket.Application.Consultas.Filtros;

public sealed class ExpresionEstado : IExpresionFiltro
{
    private readonly string _estado;

    public ExpresionEstado(string estado)
    {
        _estado = estado;
    }

    public bool Evaluar(PedidoResumen pedido) => pedido.Estado == _estado;

    public string Describir() => $"ESTADO = {_estado}";
}

public sealed class ExpresionTotal : IExpresionFiltro
{
    private readonly string _operador;
    private readonly decimal _valor;

    public ExpresionTotal(string operador, decimal valor)
    {
        _operador = operador;
        _valor = valor;
    }

    public bool Evaluar(PedidoResumen pedido) => _operador switch
    {
        "=" => pedido.Total == _valor,
        ">=" => pedido.Total >= _valor,
        "<=" => pedido.Total <= _valor,
        ">" => pedido.Total > _valor,
        "<" => pedido.Total < _valor,
        _ => false
    };

    public string Describir() => $"TOTAL {_operador} {_valor.ToString("0.00", CultureInfo.InvariantCulture)}";
}

public sealed class ExpresionCliente : IExpresionFiltro
{
    private readonly string _texto;

    public ExpresionCliente(string texto)
    {
        _texto = texto;
    }

    public bool Evaluar(PedidoResumen pedido) =>
        string.Equals(pedido.Cliente, _texto, StringComparison.CurrentCultureIgnoreCase);

    public string Describir() => $"CLIENTE = '{_texto}'";
}

public sealed class ExpresionY : IExpresionFiltro
{
    private readonly IExpresionFiltro _izquierda;
    private readonly IExpresionFiltro _derecha;

    public ExpresionY(IExpresionFiltro izquierda, IExpresionFiltro derecha)
    {
        _izquierda = izquierda;
        _derecha = derecha;
    }

    public bool Evaluar(PedidoResumen pedido) => _izquierda.Evaluar(pedido) && _derecha.Evaluar(pedido);

    public string Describir() => $"({_izquierda.Describir()} AND {_derecha.Describir()})";
}

public sealed class ExpresionO : IExpresionFiltro
{
    private readonly IExpresionFiltro _izquierda;
    private readonly IExpresionFiltro _derecha;

    public ExpresionO(IExpresionFiltro izquierda, IExpresionFiltro derecha)
    {
        _izquierda = izquierda;
        _derecha = derecha;
    }

    public bool Evaluar(PedidoResumen pedido) => _izquierda.Evaluar(pedido) || _derecha.Evaluar(pedido);

    public string Describir() => $"({_izquierda.Describir()} OR {_derecha.Describir()})";
}
