namespace LogiMarket.Application.Validacion;

public sealed class ValidadorLineas : EslabonValidacion
{
    public override string Nombre => "Líneas y cantidades";

    protected override string? Revisar(ContextoValidacion contexto)
    {
        var lineas = contexto.Pedido.Lineas;
        if (lineas.Count == 0)
            return "El pedido no tiene líneas.";

        var invalida = lineas.FirstOrDefault(l => l.Cantidad <= 0 || l.Componentes.Count == 0);
        return invalida is null ? null : $"La línea {invalida.Codigo} no tiene una cantidad válida.";
    }
}
