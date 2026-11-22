namespace LogiMarket.Application.Comun;

public static class CategoriaAuditoria
{
    public const string Cobro = "COBRO";
    public const string Seguridad = "SEGURIDAD";
    public const string Pedido = "PEDIDO";
    public const string Inventario = "INVENTARIO";
    public const string Notificacion = "NOTIFICACION";
    public const string Comando = "COMANDO";
    public const string Evento = "EVENTO";
    public const string Sistema = "SISTEMA";

    public static readonly IReadOnlyList<string> Todas =
        new[] { Cobro, Seguridad, Pedido, Inventario, Notificacion, Comando, Evento, Sistema };
}
