using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Clientes;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Seguridad;

namespace LogiMarket.Application.Validacion;

public sealed record ContextoValidacion(
    Pedido Pedido,
    Cliente? Cliente,
    IReadOnlyDictionary<string, int> StockDisponible,
    Operacion Operacion,
    bool SesionActiva,
    Rol Rol);

public sealed record ResultadoValidacion(bool Aprobado, string Eslabon, string Mensaje, IReadOnlyList<string> Recorrido)
{
    public string RecorridoTexto => string.Join(" → ", Recorrido);
}
