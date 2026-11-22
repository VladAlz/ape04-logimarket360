using LogiMarket.Domain.Devoluciones;

namespace LogiMarket.Application.Devoluciones;

public sealed class SelectorDevolucion
{
    private readonly IEnumerable<ProcesoDevolucion> _procesos;

    public SelectorDevolucion(IEnumerable<ProcesoDevolucion> procesos)
    {
        _procesos = procesos;
    }

    public ProcesoDevolucion Para(MotivoDevolucion motivo) => _procesos.Single(p => p.Motivo == motivo);
}
