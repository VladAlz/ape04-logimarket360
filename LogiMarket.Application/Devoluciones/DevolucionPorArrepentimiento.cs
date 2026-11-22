using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Devoluciones;

namespace LogiMarket.Application.Devoluciones;

public sealed class DevolucionPorArrepentimiento : ProcesoDevolucion
{
    public DevolucionPorArrepentimiento(ServiciosDevolucion servicios) : base(servicios)
    {
    }

    public override MotivoDevolucion Motivo => MotivoDevolucion.Arrepentimiento;
    public override int PlazoDias => 7;

    protected override void InspeccionarCondicion(SolicitudDevolucion solicitud, int diasDesdeEntrega)
    {
        if (diasDesdeEntrega < 0 || diasDesdeEntrega > PlazoDias)
            throw new ReglaNegocioException($"Por arrepentimiento se acepta hasta {PlazoDias} días después de la entrega; van {diasDesdeEntrega}.");
        if (!solicitud.ProductoSinUso)
            throw new ReglaNegocioException("Por arrepentimiento el producto debe estar sin uso.");
    }
}
