using System.Text.Json;
using LogiMarket.Domain.Eventos;

namespace LogiMarket.Application.Eventos;

public sealed record EventoNegocio(int OutboxId, string Tipo, int PedidoId, string Resumen, decimal Monto, DateTime Fecha)
{
    private sealed record Contenido(string Resumen, decimal Monto);

    // Lo que se guarda en la tabla de Outbox dentro de la transaccion
    public static EventoOutbox CrearOutbox(string tipo, int pedidoId, string resumen, decimal monto, DateTime fecha) =>
        new(tipo, pedidoId, JsonSerializer.Serialize(new Contenido(resumen, monto)), fecha);

    public static EventoNegocio Desde(EventoOutbox evento)
    {
        var contenido = JsonSerializer.Deserialize<Contenido>(evento.Contenido) ?? new Contenido(string.Empty, 0);
        return new EventoNegocio(evento.Id, evento.Tipo, evento.PedidoId, contenido.Resumen, contenido.Monto, evento.FechaCreacion);
    }
}
