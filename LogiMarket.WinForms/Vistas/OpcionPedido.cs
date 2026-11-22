using LogiMarket.Application.Comun;
using LogiMarket.Application.Pedidos;

namespace LogiMarket.WinForms.Vistas;

// Elemento para los combos de pedidos
public sealed record OpcionPedido(int Id, string Texto)
{
    public static OpcionPedido Desde(PedidoResumen p) =>
        new(p.Id, $"Pedido {p.Id} - {p.Cliente} - {p.Estado} - USD {Presentacion.Monto(p.Total)}");

    public override string ToString() => Texto;
}
