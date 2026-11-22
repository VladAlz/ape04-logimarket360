using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.Eventos;

public sealed record ResumenOutbox(int Procesados, int Fallidos, IReadOnlyList<string> Detalle);

// Toma los eventos pendientes y los publica despues de confirmar la transaccion
public sealed class ProcesadorOutbox
{
    private readonly IOutboxRepositorio _outbox;
    private readonly PublicadorEventos _publicador;
    private readonly IUnidadTrabajo _unidad;
    private readonly IReloj _reloj;
    private readonly IBitacora _bitacora;

    public ProcesadorOutbox(IOutboxRepositorio outbox, PublicadorEventos publicador, IUnidadTrabajo unidad, IReloj reloj, IBitacora bitacora)
    {
        _outbox = outbox;
        _publicador = publicador;
        _unidad = unidad;
        _reloj = reloj;
        _bitacora = bitacora;
    }

    public async Task<ResumenOutbox> ProcesarPendientesAsync()
    {
        var detalle = new List<string>();
        int procesados = 0, fallidos = 0;

        foreach (var pendiente in await _outbox.ListarPendientesAsync())
        {
            try
            {
                await _publicador.PublicarAsync(EventoNegocio.Desde(pendiente));
                pendiente.MarcarProcesado(_reloj.Ahora);
                procesados++;
                detalle.Add($"Evento {pendiente.Id} {pendiente.Tipo} procesado (intento {pendiente.Intentos}).");
            }
            catch (Exception ex)
            {
                // Queda pendiente para un nuevo intento
                pendiente.RegistrarFallo(ex.Message);
                fallidos++;
                detalle.Add($"Evento {pendiente.Id} {pendiente.Tipo} sigue pendiente: {ex.Message}");
                await _bitacora.RegistrarAsync(CategoriaAuditoria.Evento,
                    $"Fallo al procesar evento {pendiente.Id} {pendiente.Tipo}: {ex.Message}");
            }

            await _unidad.GuardarCambiosAsync();
        }

        return new ResumenOutbox(procesados, fallidos, detalle);
    }
}
