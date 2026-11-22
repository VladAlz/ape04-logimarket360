using LogiMarket.Application.Puertos;

namespace LogiMarket.Infrastructure.Simulacion;

public sealed class ConfiguracionSimulacion : ISimulacion
{
    public const string Legado = "LEGADO";
    public const string NovaPay = "NOVAPAY";

    public IReadOnlyList<string> Proveedores { get; } = new[] { Legado, NovaPay };

    public string ProveedorPago { get; set; } = Legado;

    public bool FallarCobro { get; set; }

    public bool FallarNotificacion { get; set; }
}
