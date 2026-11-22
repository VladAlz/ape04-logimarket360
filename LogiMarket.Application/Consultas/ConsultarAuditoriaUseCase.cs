using LogiMarket.Application.Comandos;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;

namespace LogiMarket.Application.Consultas;

public sealed record AuditoriaDto(DateTime Fecha, string Categoria, string Usuario, string Mensaje, string Correlacion);

public sealed record OutboxDto(int Id, string Tipo, int PedidoId, DateTime Creado, string Estado, int Intentos, string UltimoError);

public sealed class ConsultarAuditoriaUseCase
{
    private readonly IBitacora _bitacora;
    private readonly IOutboxRepositorio _outbox;
    private readonly HistorialComandos _comandos;
    private readonly ServicioAutorizacion _autorizacion;

    public ConsultarAuditoriaUseCase(IBitacora bitacora, IOutboxRepositorio outbox, HistorialComandos comandos, ServicioAutorizacion autorizacion)
    {
        _bitacora = bitacora;
        _outbox = outbox;
        _comandos = comandos;
        _autorizacion = autorizacion;
    }

    public async Task<IReadOnlyList<AuditoriaDto>> RegistrosAsync(string? categoria = null)
    {
        await _autorizacion.ExigirAsync(Operacion.VerAuditoria);
        return (await _bitacora.ListarAsync(categoria))
            .Select(r => new AuditoriaDto(r.Fecha, r.Categoria, r.Usuario, r.Mensaje, r.Correlacion))
            .ToList();
    }

    public async Task<IReadOnlyList<OutboxDto>> OutboxAsync()
    {
        await _autorizacion.ExigirAsync(Operacion.VerAuditoria);
        return (await _outbox.ListarAsync())
            .Select(e => new OutboxDto(e.Id, e.Tipo, e.PedidoId, e.FechaCreacion, e.Procesado ? "PROCESADO" : "PENDIENTE", e.Intentos, e.UltimoError))
            .ToList();
    }

    public IReadOnlyList<RegistroComando> Comandos() => _comandos.Listar();

    public IReadOnlyList<string> Categorias => CategoriaAuditoria.Todas;
}
