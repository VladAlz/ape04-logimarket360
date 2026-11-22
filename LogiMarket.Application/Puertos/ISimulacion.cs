namespace LogiMarket.Application.Puertos;

// Opciones de la pantalla de demostracion; no tocan las reglas de negocio
public interface ISimulacion
{
    IReadOnlyList<string> Proveedores { get; }

    string ProveedorPago { get; set; }

    bool FallarCobro { get; set; }

    bool FallarNotificacion { get; set; }
}
