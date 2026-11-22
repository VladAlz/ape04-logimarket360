namespace LogiMarket.Application.Validacion;

public sealed class ValidadorClienteActivo : EslabonValidacion
{
    public override string Nombre => "Cliente activo";

    protected override string? Revisar(ContextoValidacion contexto)
    {
        if (contexto.Cliente is null)
            return "El pedido no tiene un cliente registrado.";

        return contexto.Cliente.Activo ? null : $"El cliente {contexto.Cliente.Nombre} está inactivo.";
    }
}
