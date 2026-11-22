using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Auditoria;
using LogiMarket.Domain.Catalogo;
using LogiMarket.Domain.Eventos;
using LogiMarket.Domain.Inventario;
using LogiMarket.Domain.Pagos;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Seguridad;
using LogiMarket.Infrastructure.Persistencia;
using LogiMarket.Tests.Soporte;
using Microsoft.Extensions.DependencyInjection;

namespace LogiMarket.Tests;

// Casos numericos de la guia sobre una base recien preparada
public class PruebasCasosOficiales
{
    [Fact(DisplayName = "P01 Base nueva: migraciones, 5 productos, 2 kits, 4 usuarios y semilla sin duplicados")]
    public async Task P01_SemillaIdempotente()
    {
        await using var e = await EntornoPrueba.CrearAsync();

        var segunda = await e.Usar(sp => sp.GetRequiredService<InicializadorBaseDatos>().InicializarAsync());

        Assert.Equal(new ResumenSemilla(0, 0, 0, 0), segunda);
        Assert.Equal(5, await e.ContarAsync<Producto>());
        Assert.Equal(2, await e.ContarAsync<Kit>());
        Assert.Equal(4, await e.ContarAsync<Usuario>());
        Assert.Equal(new[] { 8, 5, 20, 4, 30 }, (await e.ProductosAsync()).Select(p => p.Stock));
    }

    [Fact(DisplayName = "P02 Ana VIP: KIT_TELETRABAJO x1 ESTANDAR = 59.00 y stock 7/4/19")]
    public async Task P02_Ana()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);

        var id = await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1));
        var resultado = await e.ConfirmarAsync(id);

        Assert.True(resultado.Exito, resultado.Mensaje);
        var pedido = (await e.PedidoAsync(id))!.Resumen;
        Assert.Equal((60.00m, 6.00m, 5.00m, 59.00m, "PAGADO"), (pedido.Subtotal, pedido.Descuento, pedido.Tarifa, pedido.Total, pedido.Estado));
        Assert.Equal((7, 4, 19), (await e.StockAsync("P01"), await e.StockAsync("P02"), await e.StockAsync("P03")));
        Assert.Equal(1, await e.ContarAsync<Pago>());
    }

    [Fact(DisplayName = "P03 Luis NORMAL: P04 x2 + P05 x2 EXPRES = 102.00")]
    public async Task P03_Luis()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);

        var id = await e.CrearValidadoAsync("Luis Mena", FormaEntrega.Expres, ("P04", 2), ("P05", 2));
        var resultado = await e.ConfirmarAsync(id);

        Assert.True(resultado.Exito, resultado.Mensaje);
        Assert.Equal(102.00m, resultado.Total);
        Assert.Equal((2, 28), (await e.StockAsync("P04"), await e.StockAsync("P05")));
        Assert.Equal(1, await e.ContarAsync<Pago>());
    }

    [Fact(DisplayName = "P04 Carmen: P03 x10 ESTANDAR = 90.00 con descuento 15.00")]
    public async Task P04_Carmen()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);

        var id = await e.CrearValidadoAsync("Carmen Ruiz", FormaEntrega.Estandar, ("P03", 10));
        var resultado = await e.ConfirmarAsync(id);

        var pedido = (await e.PedidoAsync(id))!.Resumen;
        Assert.True(resultado.Exito, resultado.Mensaje);
        Assert.Equal((15.00m, "MAYORISTA", 90.00m), (pedido.Descuento, pedido.Politica, pedido.Total));
        Assert.Equal(10, await e.StockAsync("P03"));
    }

    [Fact(DisplayName = "P05 KIT_TELETRABAJO x6: rechazo por P02 sin tocar stock, pago ni eventos")]
    public async Task P05_StockInsuficiente()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);

        var id = await e.CrearBorradorAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 6));
        var validacion = await e.Usar(sp => sp.GetRequiredService<ValidarPedidoUseCase>().EjecutarAsync(id));
        var confirmacion = await e.ConfirmarAsync(id);

        Assert.False(validacion.Exito);
        Assert.Contains("Stock insuficiente de P02: se requieren 6 y hay 5", validacion.Mensaje);
        Assert.False(confirmacion.Exito);
        Assert.Equal("BORRADOR", (await e.PedidoAsync(id))!.Resumen.Estado);
        Assert.Equal((8, 5, 20), (await e.StockAsync("P01"), await e.StockAsync("P02"), await e.StockAsync("P03")));
        Assert.Equal(0, await e.ContarAsync<Pago>());
        Assert.Equal(0, await e.ContarAsync<EventoOutbox>());
    }

    [Fact(DisplayName = "P06 Cobro rechazado: inventario y estado intactos, sin pago ni Outbox de exito")]
    public async Task P06_FalloDeCobro()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var id = await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1));
        e.Servicio<ISimulacion>().FallarCobro = true;

        var resultado = await e.ConfirmarAsync(id);

        Assert.False(resultado.Exito);
        Assert.Contains("rechazado", resultado.Mensaje);
        Assert.Equal("VALIDADO", (await e.PedidoAsync(id))!.Resumen.Estado);
        Assert.Equal((8, 5, 20), (await e.StockAsync("P01"), await e.StockAsync("P02"), await e.StockAsync("P03")));
        Assert.Equal(0, await e.ContarAsync<Pago>());
        Assert.Equal(0, await e.ContarAsync<EventoOutbox>());
        Assert.Equal(5, await e.ContarAsync<MovimientoInventario>());
    }

    [Fact(DisplayName = "P07 Idempotencia: la misma clave devuelve el resultado original sin cobro doble")]
    public async Task P07_Idempotencia()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var id = await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1));

        var primera = await e.ConfirmarAsync(id, "CLAVE-FIJA-ANA");
        var segunda = await e.ConfirmarAsync(id, "CLAVE-FIJA-ANA");
        var otraClave = await e.ConfirmarAsync(id, "OTRA-CLAVE");

        Assert.True(primera.Exito);
        Assert.True(segunda.Repetido);
        Assert.Equal(primera.ReferenciaPago, segunda.ReferenciaPago);
        Assert.False(otraClave.Exito);
        Assert.Equal(1, await e.ContarAsync<Pago>());
        var ventas = (await e.ListarAsync<MovimientoInventario>()).Where(m => m.Motivo == MovimientoInventario.Venta).ToList();
        Assert.Equal(3, ventas.Count);
        Assert.Equal(7, await e.StockAsync("P01"));
    }

    [Fact(DisplayName = "P08 Pedido PAGADO: no se cancela ni se edita y el historial queda intacto")]
    public async Task P08_PagadoInmutable()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var id = await e.CrearValidadoAsync("Luis Mena", FormaEntrega.Expres, ("P04", 2), ("P05", 2));
        await e.ConfirmarAsync(id);
        var antes = await e.PedidoAsync(id);

        var cancelar = await e.Usar(sp => sp.GetRequiredService<Application.Comandos.ServicioComandosPedido>().CancelarAsync(id, "prueba"));
        var editar = await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().CambiarCantidadAsync(id, "P04", 1));
        var despues = await e.PedidoAsync(id);

        Assert.False(cancelar.Exito);
        Assert.False(editar.Exito);
        Assert.Equal(antes!.Resumen, despues!.Resumen);
        Assert.Equal(antes.Historial.Count, despues.Historial.Count);
    }

    [Fact(DisplayName = "P09 BODEGA despacha y entrega; despachar sin pago se rechaza sin cambios")]
    public async Task P09_DespachoYEntrega()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var sinPago = await e.CrearValidadoAsync("Luis Mena", FormaEntrega.Expres, ("P05", 1));
        var pagado = await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1));
        await e.ConfirmarAsync(pagado);

        e.Sesion(Rol.Bodega);
        var rechazo = await e.Usar(sp => sp.GetRequiredService<Application.Comandos.ServicioComandosPedido>().DespacharAsync(sinPago));
        var despacho = await e.Usar(sp => sp.GetRequiredService<Application.Comandos.ServicioComandosPedido>().DespacharAsync(pagado));
        var entrega = await e.Usar(sp => sp.GetRequiredService<EntregarPedidoUseCase>().EjecutarAsync(pagado, e.Reloj.Ahora.AddDays(3)));

        Assert.False(rechazo.Exito);
        Assert.Equal("VALIDADO", (await e.PedidoAsync(sinPago))!.Resumen.Estado);
        Assert.True(despacho.Exito, despacho.Mensaje);
        Assert.Contains("STD-", despacho.Mensaje);
        Assert.True(entrega.Exito, entrega.Mensaje);
        Assert.Equal("ENTREGADO", (await e.PedidoAsync(pagado))!.Resumen.Estado);
    }

    [Fact(DisplayName = "P10 Cerrar y reabrir: pedido, stock, pago y auditoria persisten en SQLite")]
    public async Task P10_PersistenciaTrasReinicio()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var id = await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1));
        await e.ConfirmarAsync(id);
        var auditoriaAntes = await e.ContarAsync<RegistroAuditoria>();

        await e.ReiniciarAplicacionAsync();

        Assert.False(e.Servicio<ISesionUsuario>().Activa);
        Assert.Equal("PAGADO", (await e.PedidoAsync(id))!.Resumen.Estado);
        Assert.Equal(7, await e.StockAsync("P01"));
        Assert.Equal(1, await e.ContarAsync<Pago>());
        Assert.Equal(auditoriaAntes, await e.ContarAsync<RegistroAuditoria>());
        Assert.Equal(5, await e.ContarAsync<Producto>());
    }
}
