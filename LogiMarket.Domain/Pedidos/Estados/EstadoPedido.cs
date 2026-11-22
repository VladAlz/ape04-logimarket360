using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Pedidos.Estados;

// State: cada estado abre solo sus transiciones, lo demas se rechaza
public abstract class EstadoPedido
{
    public abstract string Codigo { get; }

    public virtual bool PermiteEdicion => false;

    public virtual EstadoPedido Validar() => throw NoPermitido("validar");
    public virtual EstadoPedido Pagar() => throw NoPermitido("cobrar");
    public virtual EstadoPedido Despachar() => throw NoPermitido("despachar");
    public virtual EstadoPedido Entregar() => throw NoPermitido("entregar");
    public virtual EstadoPedido Cancelar() => throw NoPermitido("cancelar");

    protected ReglaNegocioException NoPermitido(string accion) =>
        new($"No se puede {accion} un pedido en estado {Codigo}.");

    public override string ToString() => Codigo;
}
