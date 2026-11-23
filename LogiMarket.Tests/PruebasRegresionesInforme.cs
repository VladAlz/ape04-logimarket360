using System.Globalization;
using LogiMarket.Application.Comandos;
using LogiMarket.Application.Pedidos;
using LogiMarket.Domain.Clientes;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Seguridad;
using LogiMarket.Tests.Soporte;
using Microsoft.Extensions.DependencyInjection;

namespace LogiMarket.Tests;

public class PruebasRegresionesInforme
{
    [Fact]
    public void R01_HistorialConservaPuntoDecimal()
    {
        var culturaOriginal = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("es-ES");
            var pedido = new PedidoBuilder()
                .ParaCliente(Fabricas.Cliente("Ana", CategoriaCliente.Vip))
                .ConEntrega(FormaEntrega.Estandar)
                .Agregar(Fabricas.Catalogo().Obtener("P01"), 1)
                .Construir();

            pedido.Validar(Fabricas.Calculadora().Valorar(pedido),
                new DateTime(2026, 11, 2), "ventas");

            var entrada = Assert.Single(pedido.Historial,
                h => h.EstadoNuevo == "VALIDADO");
            Assert.Contains("Total 23.00 ", entrada.Detalle);
        }
        finally
        {
            CultureInfo.CurrentCulture = culturaOriginal;
        }
    }

    [Fact]
    public async Task R02_RechazaEntregaAnteriorYConservaEstado()
    {
        await using var entorno = await EntornoPrueba.CrearAsync();
        entorno.Sesion(Rol.Ventas);
        var id = await entorno.CrearValidadoAsync(
            "Ana Torres", FormaEntrega.Estandar, ("P01", 1));
        var pago = await entorno.ConfirmarAsync(id);
        Assert.True(pago.Exito, pago.Mensaje);

        entorno.Sesion(Rol.Bodega);
        var despacho = await entorno.Usar(sp =>
            sp.GetRequiredService<ServicioComandosPedido>().DespacharAsync(id));
        Assert.True(despacho.Exito, despacho.Mensaje);

        var entrega = await entorno.Usar(sp =>
            sp.GetRequiredService<EntregarPedidoUseCase>()
                .EjecutarAsync(id, entorno.Reloj.Ahora.AddDays(-1)));

        Assert.False(entrega.Exito);
        Assert.Contains("anterior al despacho", entrega.Mensaje);
        var pedido = await entorno.PedidoAsync(id);
        Assert.NotNull(pedido);
        Assert.Equal("DESPACHADO", pedido.Resumen.Estado);
        Assert.Null(pedido.FechaEntrega);
    }
}
