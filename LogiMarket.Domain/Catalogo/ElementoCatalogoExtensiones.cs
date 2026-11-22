namespace LogiMarket.Domain.Catalogo;

public static class ElementoCatalogoExtensiones
{
    // Suma por SKU final; un mismo SKU puede aparecer en varios niveles
    public static IReadOnlyList<ComponenteFisico> ExpandirSku(this IElementoCatalogo elemento, int cantidad)
    {
        var acumulado = new List<ComponenteFisico>();
        var iterador = elemento.CrearIterador(cantidad);

        while (iterador.HaySiguiente())
        {
            var componente = iterador.Siguiente();
            var indice = acumulado.FindIndex(c => c.Sku == componente.Sku);
            if (indice < 0)
                acumulado.Add(componente);
            else
                acumulado[indice] = acumulado[indice] with { Cantidad = acumulado[indice].Cantidad + componente.Cantidad };
        }

        return acumulado;
    }
}
