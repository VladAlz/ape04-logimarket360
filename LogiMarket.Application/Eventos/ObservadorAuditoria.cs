using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.Eventos;

public sealed class ObservadorAuditoria : IObservadorEvento
{
    private readonly IBitacora _bitacora;

    public ObservadorAuditoria(IBitacora bitacora)
    {
        _bitacora = bitacora;
    }

    public string Nombre => "Auditoría de negocio";
    public int Orden => 2;

    public Task AlRecibirAsync(EventoNegocio evento) =>
        _bitacora.RegistrarAsync(CategoriaAuditoria.Evento,
            $"{evento.Tipo} del pedido {evento.PedidoId}: {evento.Resumen}", $"OUTBOX-{evento.OutboxId}");
}
