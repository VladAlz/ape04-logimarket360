using LogiMarket.Application.Comun;
using LogiMarket.Application.Devoluciones;

namespace LogiMarket.Application.Comandos;

public sealed class ProcesarDevolucionCommand : IComandoPedido
{
    private readonly SelectorDevolucion _receptor;
    private readonly SolicitudDevolucion _solicitud;

    public ProcesarDevolucionCommand(SelectorDevolucion receptor, SolicitudDevolucion solicitud)
    {
        _receptor = receptor;
        _solicitud = solicitud;
    }

    public string Nombre => "ProcesarDevolucion";
    public string Entrada =>
        $"pedido={_solicitud.PedidoId}; motivo={_solicitud.Motivo.ToString().ToUpperInvariant()}; fecha={_solicitud.FechaSolicitud:dd/MM/yyyy}; sinUso={(_solicitud.ProductoSinUso ? "SI" : "NO")}";

    public ResultadoDevolucion? Resultado { get; private set; }

    public async Task<ResultadoOperacion> EjecutarAsync()
    {
        Resultado = await _receptor.Para(_solicitud.Motivo).ProcesarAsync(_solicitud);
        return new ResultadoOperacion(Resultado.Exito, Resultado.Mensaje);
    }
}
