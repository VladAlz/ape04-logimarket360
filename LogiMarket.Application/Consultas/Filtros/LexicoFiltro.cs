namespace LogiMarket.Application.Consultas.Filtros;

// Parte el texto en tokens; no ejecuta nada
public static class LexicoFiltro
{
    private static readonly string[] Campos = { "ESTADO", "TOTAL", "CLIENTE" };

    public static IReadOnlyList<Token> Tokenizar(string texto)
    {
        var tokens = new List<Token>();
        var i = 0;

        while (i < texto.Length)
        {
            var c = texto[i];
            if (char.IsWhiteSpace(c))
            {
                i++;
                continue;
            }

            if (c == '(' || c == ')')
            {
                tokens.Add(new Token(c == '(' ? TipoToken.AbreParentesis : TipoToken.CierraParentesis, c.ToString(), i));
                i++;
                continue;
            }

            if (c is '=' or '>' or '<')
            {
                var operador = i + 1 < texto.Length && texto[i + 1] == '=' && c != '=' ? texto.Substring(i, 2) : c.ToString();
                tokens.Add(new Token(TipoToken.Operador, operador, i));
                i += operador.Length;
                continue;
            }

            if (c is '\'' or '"')
            {
                var cierre = texto.IndexOf(c, i + 1);
                if (cierre < 0)
                    throw new ErrorFiltroException("falta cerrar las comillas.", i);
                tokens.Add(new Token(TipoToken.Valor, texto.Substring(i + 1, cierre - i - 1), i));
                i = cierre + 1;
                continue;
            }

            var inicio = i;
            while (i < texto.Length && !char.IsWhiteSpace(texto[i]) && "()=<>'\"".IndexOf(texto[i]) < 0)
                i++;

            if (i == inicio)
                throw new ErrorFiltroException($"carácter no permitido '{c}'.", i);

            var palabra = texto[inicio..i];
            var mayus = palabra.ToUpperInvariant();
            var tipo = mayus switch
            {
                "AND" => TipoToken.And,
                "OR" => TipoToken.Or,
                _ when Campos.Contains(mayus) => TipoToken.Campo,
                _ => TipoToken.Valor
            };
            tokens.Add(new Token(tipo, tipo == TipoToken.Valor ? palabra : mayus, inicio));
        }

        tokens.Add(new Token(TipoToken.Fin, string.Empty, texto.Length));
        return tokens;
    }
}
