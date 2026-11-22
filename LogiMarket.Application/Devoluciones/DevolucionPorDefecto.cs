using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Devoluciones;

namespace LogiMarket.Application.Devoluciones;

public sealed class DevolucionPorDefecto : ProcesoDevolucion
{
    public DevolucionPorDefecto(ServiciosDevolucion servicios) : base(servicios)
    {
    }

    public override MotivoDevolucion Motivo => MotivoDevolucion.Defecto;
    public override int PlazoDias => 30;

    protected override void InspeccionarCondicion(SolicitudDevolucion solicitud, int diasDesdeEntrega)
    {
        if (diasDesdeEntrega < 0 || diasDesdeEntrega > PlazoDias)
            throw new ReglaNegocioException($"Por defecto se acepta hasta {PlazoDias} días después de la entrega; van {diasDesdeEntrega}.");
    }
}
