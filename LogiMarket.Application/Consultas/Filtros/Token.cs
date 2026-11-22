namespace LogiMarket.Application.Consultas.Filtros;

public enum TipoToken
{
    Campo,
    Operador,
    Valor,
    And,
    Or,
    AbreParentesis,
    CierraParentesis,
    Fin
}

public sealed record Token(TipoToken Tipo, string Texto, int Posicion);
