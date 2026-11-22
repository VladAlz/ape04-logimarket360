using System.Globalization;
using LogiMarket.Domain.Pedidos.Estados;

namespace LogiMarket.Application.Consultas.Filtros;

// Gramatica acotada, AND tiene mas precedencia que OR:
//   expresion   := termino (OR termino)*
//   termino     := factor (AND factor)*
//   factor      := '(' expresion ')' | comparacion
//   comparacion := CAMPO OPERADOR VALOR
public sealed class AnalizadorFiltro
{
    private readonly IReadOnlyList<Token> _tokens;
    private int _actual;

    public AnalizadorFiltro(string texto)
    {
        _tokens = LexicoFiltro.Tokenizar(texto);
    }

    public IExpresionFiltro Analizar()
    {
        var expresion = Expresion();
        if (Ver.Tipo != TipoToken.Fin)
            throw new ErrorFiltroException($"sobra '{Ver.Texto}'.", Ver.Posicion);
        return expresion;
    }

    private Token Ver => _tokens[_actual];

    private Token Consumir() => _tokens[_actual++];

    private IExpresionFiltro Expresion()
    {
        var izquierda = Termino();
        while (Ver.Tipo == TipoToken.Or)
        {
            Consumir();
            izquierda = new ExpresionO(izquierda, Termino());
        }
        return izquierda;
    }

    private IExpresionFiltro Termino()
    {
        var izquierda = Factor();
        while (Ver.Tipo == TipoToken.And)
        {
            Consumir();
            izquierda = new ExpresionY(izquierda, Factor());
        }
        return izquierda;
    }

    private IExpresionFiltro Factor()
    {
        if (Ver.Tipo == TipoToken.AbreParentesis)
        {
            var abre = Consumir();
            var interna = Expresion();
            if (Ver.Tipo != TipoToken.CierraParentesis)
                throw new ErrorFiltroException("falta ')' para el paréntesis abierto.", abre.Posicion);
            Consumir();
            return interna;
        }

        return Comparacion();
    }

    private IExpresionFiltro Comparacion()
    {
        var campo = Consumir();
        if (campo.Tipo != TipoToken.Campo)
            throw new ErrorFiltroException(campo.Tipo == TipoToken.Fin
                ? "falta una condición al final del filtro."
                : $"se esperaba ESTADO, TOTAL o CLIENTE y llegó '{campo.Texto}'.", campo.Posicion);

        var operador = Consumir();
        if (operador.Tipo != TipoToken.Operador)
            throw new ErrorFiltroException($"se esperaba un operador después de {campo.Texto}.", operador.Posicion);

        var valor = Consumir();
        if (valor.Tipo != TipoToken.Valor)
            throw new ErrorFiltroException($"falta el valor para {campo.Texto}.", valor.Posicion);

        switch (campo.Texto)
        {
            case "ESTADO":
                if (operador.Texto != "=")
                    throw new ErrorFiltroException("ESTADO solo admite '='.", operador.Posicion);
                var estado = valor.Texto.ToUpperInvariant();
                if (!EstadosPedido.Codigos.Contains(estado))
                    throw new ErrorFiltroException($"estado '{valor.Texto}' no existe.", valor.Posicion);
                return new ExpresionEstado(estado);

            case "TOTAL":
                if (!decimal.TryParse(valor.Texto, NumberStyles.Number, CultureInfo.InvariantCulture, out var numero))
                    throw new ErrorFiltroException($"'{valor.Texto}' no es un número (use punto decimal).", valor.Posicion);
                return new ExpresionTotal(operador.Texto, numero);

            default:
                if (operador.Texto != "=")
                    throw new ErrorFiltroException("CLIENTE solo admite '='.", operador.Posicion);
                return new ExpresionCliente(valor.Texto);
        }
    }
}
