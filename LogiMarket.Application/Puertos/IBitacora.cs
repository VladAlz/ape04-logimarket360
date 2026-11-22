using LogiMarket.Domain.Auditoria;

namespace LogiMarket.Application.Puertos;

public interface IBitacora
{
    Task RegistrarAsync(string categoria, string mensaje, string correlacion = "");

    Task<IReadOnlyList<RegistroAuditoria>> ListarAsync(string? categoria = null);

    // Lo registrado durante la operacion actual, para mostrar la secuencia
    IReadOnlyList<RegistroAuditoria> RegistrosDelAmbito { get; }
}
