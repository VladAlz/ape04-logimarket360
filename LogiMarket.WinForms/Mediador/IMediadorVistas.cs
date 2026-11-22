namespace LogiMarket.WinForms.Mediador;

public enum EventoVista
{
    SesionIniciada,
    CatalogoCambiado,
    ClientesCambiados,
    BorradorCambiado,
    PedidoValidado,
    PedidoConfirmado,
    PedidoCancelado,
    PedidoDespachado,
    PedidoEntregado,
    DevolucionRegistrada,
    InventarioAjustado,
    SimulacionCambiada,
    OutboxProcesado
}

public interface IVistaColaboradora
{
    string NombreVista { get; }

    Task RefrescarAsync();
}

public interface IMediadorVistas
{
    event Action<string>? Notificado;

    void Registrar(IVistaColaboradora vista);

    void Reiniciar();

    Task NotificarAsync(IVistaColaboradora? emisor, EventoVista evento);
}
