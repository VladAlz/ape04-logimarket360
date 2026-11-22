using LogiMarket.Application.Metricas;

namespace LogiMarket.Application.Eventos;

public sealed class ObservadorMetricas : IObservadorEvento
{
    private readonly MetricasSesion _metricas;

    public ObservadorMetricas(MetricasSesion metricas)
    {
        _metricas = metricas;
    }

    public string Nombre => "Métricas de sesión";
    public int Orden => 3;

    public Task AlRecibirAsync(EventoNegocio evento)
    {
        var contador = evento.Tipo switch
        {
            TiposEvento.PedidoPagado => MetricasSesion.PedidosPagados,
            TiposEvento.DespachoRegistrado => MetricasSesion.Despachos,
            TiposEvento.DevolucionAceptada => MetricasSesion.Devoluciones,
            _ => null
        };

        if (contador is not null)
            _metricas.Incrementar(contador);

        return Task.CompletedTask;
    }
}
