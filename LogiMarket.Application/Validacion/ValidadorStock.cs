namespace LogiMarket.Application.Validacion;

public sealed class ValidadorStock : EslabonValidacion
{
    public override string Nombre => "Reglas de stock";

    protected override string? Revisar(ContextoValidacion contexto)
    {
        foreach (var (sku, requerido) in contexto.Pedido.RequerimientoStock().OrderBy(r => r.Key))
        {
            var disponible = contexto.StockDisponible.TryGetValue(sku, out var stock) ? stock : 0;
            if (requerido > disponible)
                return $"Stock insuficiente de {sku}: se requieren {requerido} y hay {disponible}.";
        }

        return null;
    }
}
