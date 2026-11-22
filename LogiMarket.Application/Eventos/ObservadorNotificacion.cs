using LogiMarket.Application.Comun;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Notificaciones;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Application.Eventos;

public sealed class ObservadorNotificacion : IObservadorEvento
{
    private readonly IPedidoEscritura _pedidos;
    private readonly IClienteRepositorio _clientes;
    private readonly IEnumerable<CreadorNotificador> _creadores;
    private readonly IBitacora _bitacora;
    private readonly MetricasSesion _metricas;

    public ObservadorNotificacion(IPedidoEscritura pedidos, IClienteRepositorio clientes,
        IEnumerable<CreadorNotificador> creadores, IBitacora bitacora, MetricasSesion metricas)
    {
        _pedidos = pedidos;
        _clientes = clientes;
        _creadores = creadores;
        _bitacora = bitacora;
        _metricas = metricas;
    }

    public string Nombre => "Notificación al cliente";
    public int Orden => 1;

    public async Task AlRecibirAsync(EventoNegocio evento)
    {
        var pedido = await _pedidos.ObtenerAsync(evento.PedidoId)
            ?? throw new InvalidOperationException($"Pedido {evento.PedidoId} no encontrado.");
        var cliente = await _clientes.ObtenerAsync(pedido.ClienteId)
            ?? throw new InvalidOperationException($"Cliente {pedido.ClienteId} no encontrado.");

        // El canal lo decide el cliente, no el formulario
        var creador = _creadores.Single(c => c.Canal == cliente.Canal);
        var aviso = creador.Notificar(cliente.Destino, Asunto(evento.Tipo), evento.Resumen);

        _metricas.Incrementar(MetricasSesion.AvisosEnviados);
        await _bitacora.RegistrarAsync(CategoriaAuditoria.Notificacion,
            $"{aviso.Canal} a {aviso.Destino}: {aviso.Mensaje} [{aviso.Registro}]", $"OUTBOX-{evento.OutboxId}");
    }

    private static string Asunto(string tipo) => tipo switch
    {
        TiposEvento.PedidoPagado => "Pago recibido",
        TiposEvento.DespachoRegistrado => "Pedido despachado",
        TiposEvento.DevolucionAceptada => "Devolución aceptada",
        _ => "Aviso LogiMarket"
    };
}
