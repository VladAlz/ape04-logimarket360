using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Pedidos.Estados;

namespace LogiMarket.Application.Validacion;

public sealed class ValidadorCondicionesConfirmacion : EslabonValidacion
{
    public override string Nombre => "Autorización y estado";

    protected override string? Revisar(ContextoValidacion contexto)
    {
        if (!contexto.SesionActiva || !ServicioAutorizacion.RolPuede(contexto.Rol, contexto.Operacion))
            return $"El rol {contexto.Rol.ToString().ToUpperInvariant()} no puede ejecutar {contexto.Operacion}.";

        var esperado = contexto.Operacion == Operacion.CobrarPedido ? EstadosPedido.Validado : EstadosPedido.Borrador;
        return contexto.Pedido.Estado == esperado
            ? null
            : $"El pedido está {contexto.Pedido.EstadoCodigo}; se esperaba {esperado.Codigo}.";
    }
}
