using LogiMarket.Application.Comandos;
using LogiMarket.Application.Comprobantes;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Eventos;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Pagos;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Simulacion;
using LogiMarket.Application.VistaCatalogo;
using LogiMarket.Domain.Auditoria;
using LogiMarket.Domain.Devoluciones;
using LogiMarket.Domain.Pagos;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Seguridad;
using LogiMarket.Infrastructure.Pagos;
using LogiMarket.Infrastructure.Pagos.Legado;
using LogiMarket.Infrastructure.Simulacion;
using LogiMarket.Tests.Soporte;
using Microsoft.Extensions.DependencyInjection;

namespace LogiMarket.Tests;

// Patrones observados en ejecucion real con base SQLite
public class PruebasPatrones
{
    [Fact(DisplayName = "P11 Factory Method: EMAIL para Ana y SMS para Luis desde los eventos")]
    public async Task P11_FactoryMethod()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        await e.ConfirmarAsync(await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("P01", 1)));
        await e.ConfirmarAsync(await e.CrearValidadoAsync("Luis Mena", FormaEntrega.Expres, ("P05", 1)));

        var email = File.ReadAllLines(Path.Combine(e.Carpeta, "avisos", "email.log"));
        var sms = File.ReadAllLines(Path.Combine(e.Carpeta, "avisos", "sms.log"));
        Assert.Single(email);
        Assert.Contains("ana.torres@correo.test", email[0]);
        Assert.Single(sms);
        Assert.Contains("0990000002", sms[0]);

        var avisos = (await e.ListarAsync<RegistroAuditoria>()).Where(r => r.Categoria == CategoriaAuditoria.Notificacion).ToList();
        Assert.Contains(avisos, r => r.Mensaje.StartsWith("EMAIL a ana.torres"));
        Assert.Contains(avisos, r => r.Mensaje.StartsWith("SMS a 0990000002"));
    }

    [Fact(DisplayName = "P15 Singleton: dos consumidores reciben la misma instancia y los contadores son seguros")]
    public async Task P15_Singleton()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        using var vistaPanel = e.Proveedor.CreateScope();
        using var vistaAuditoria = e.Proveedor.CreateScope();

        var a = vistaPanel.ServiceProvider.GetRequiredService<MetricasSesion>();
        var b = vistaAuditoria.ServiceProvider.GetRequiredService<MetricasSesion>();
        Parallel.For(0, 1000, _ => a.Incrementar(MetricasSesion.ComandosEjecutados));

        Assert.Same(a, b);
        Assert.Equal(a.Instancia, b.Instancia);
        Assert.Equal(1000, b.Valor(MetricasSesion.ComandosEjecutados));
    }

    [Fact(DisplayName = "P16 Adapter: proveedor legado y NovaPay cobran con el mismo contrato")]
    public async Task P16_Adapter()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var legado = e.Servicio<PasarelaLegadaSimulada>();

        var conLegado = await e.ConfirmarAsync(await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("P01", 1)));
        e.Servicio<ISimulacion>().ProveedorPago = ConfiguracionSimulacion.NovaPay;
        var conNovaPay = await e.ConfirmarAsync(await e.CrearValidadoAsync("Luis Mena", FormaEntrega.Estandar, ("P05", 1)));

        Assert.True(conLegado.Exito);
        Assert.StartsWith("LEG-", conLegado.ReferenciaPago);
        Assert.True(conNovaPay.Exito);
        Assert.StartsWith("NVP-", conNovaPay.ReferenciaPago);
        Assert.Equal(1, legado.Invocaciones);
        Assert.Contains(conLegado.Trazas, t => t.Contains("CobrarCentavos(2300)"));
    }

    [Fact(DisplayName = "P17 Bridge: venta y devolucion por vista previa y archivo TXT")]
    public async Task P17_Bridge()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        var id = await e.CrearEntregadoAsync("Ana Torres", e.Reloj.Ahora, ("KIT_TELETRABAJO", 1));
        e.Sesion(Rol.Ventas);
        await e.Usar(sp => sp.GetRequiredService<ServicioComandosPedido>()
            .DevolverAsync(new Application.Devoluciones.SolicitudDevolucion(id, MotivoDevolucion.Defecto, e.Reloj.Ahora.AddDays(2), true)));

        var resultados = new List<ResultadoComprobante>();
        foreach (var tipo in new[] { TipoComprobante.Venta, TipoComprobante.Devolucion })
        foreach (var medio in new[] { MediosSalida.VistaPrevia, MediosSalida.ArchivoTxt })
            resultados.Add(await e.Usar(sp => sp.GetRequiredService<GenerarComprobanteUseCase>().EjecutarAsync(tipo, id, medio)));

        Assert.All(resultados, r => Assert.True(r.Exito, r.Mensaje));
        Assert.True(File.Exists(Path.Combine(e.Carpeta, "comprobantes", $"venta_pedido_{id:0000}.txt")));
        Assert.Contains("REEMBOLSO USD:", File.ReadAllText(Path.Combine(e.Carpeta, "comprobantes", $"devolucion_pedido_{id:0000}.txt")));
        Assert.Contains("TOTAL USD:", resultados[0].Salida!.Contenido);
    }

    [Fact(DisplayName = "P19 Decorator: INICIO -> PASARELA -> FIN en cobro aprobado y rechazado")]
    public async Task P19_Decorator()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);

        var aprobado = await e.ConfirmarAsync(await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("P01", 1)));
        e.Servicio<ISimulacion>().FallarCobro = true;
        var rechazado = await e.ConfirmarAsync(await e.CrearValidadoAsync("Luis Mena", FormaEntrega.Estandar, ("P05", 1)));

        string[] Cobro(IReadOnlyList<string> trazas) => trazas.Where(t => t.StartsWith("[COBRO]")).ToArray();
        Assert.Collection(Cobro(aprobado.Trazas),
            t => Assert.Contains("INICIO cobro 23.00", t),
            t => Assert.Contains("PASARELA LEGADO", t),
            t => Assert.Contains("FIN cobro APROBADO", t));
        Assert.Collection(Cobro(rechazado.Trazas),
            t => Assert.Contains("INICIO cobro", t),
            t => Assert.Contains("MANTENIMIENTO", t),
            t => Assert.Contains("FIN cobro RECHAZADO", t));
    }

    private sealed class PasarelaQueFalla : IPasarelaPago
    {
        public string Proveedor => "ROTO";
        public Task<ResultadoCobro> CobrarAsync(decimal monto, string claveIdempotencia) =>
            throw new TimeoutException("el proveedor no respondió");
    }

    [Fact(DisplayName = "P20 Facade: si un colaborador falla no queda persistencia parcial")]
    public async Task P20_Facade()
    {
        await using var e = await EntornoPrueba.CrearAsync(s => s.AddScoped<IPasarelaPago, PasarelaQueFalla>());
        e.Sesion(Rol.Ventas);
        var id = await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1));

        var resultado = await e.ConfirmarAsync(id);

        Assert.False(resultado.Exito);
        Assert.Contains("Error de la pasarela", resultado.Mensaje);
        Assert.Equal("VALIDADO", (await e.PedidoAsync(id))!.Resumen.Estado);
        Assert.Equal((8, 5, 20), (await e.StockAsync("P01"), await e.StockAsync("P02"), await e.StockAsync("P03")));
        Assert.Equal(0, await e.ContarAsync<Pago>());
    }

    [Fact(DisplayName = "P21 Flyweight: 1000 filas comparten 5 metadatos (ReferenceEquals) y usan menos memoria")]
    public async Task P21_Flyweight()
    {
        await using var e = await EntornoPrueba.CrearAsync();

        var resultado = await e.Usar(sp => sp.GetRequiredService<GenerarVistaMasivaUseCase>().EjecutarAsync(1000));

        Assert.Equal(1000, resultado.Filas);
        Assert.Equal(5, resultado.MetadatosCompartidos);
        Assert.True(resultado.MismaReferencia);
        Assert.Same(resultado.Renglones[0].Metadato, resultado.Renglones[5].Metadato);
        Assert.True(resultado.BytesConFlyweight < resultado.BytesSinFlyweight,
            $"con {resultado.BytesConFlyweight} bytes, sin {resultado.BytesSinFlyweight} bytes");
    }

    [Theory(DisplayName = "P22 Proxy: CONSULTA y BODEGA no llegan a la pasarela ni dejan LOG de cobro")]
    [InlineData(Rol.Consulta)]
    [InlineData(Rol.Bodega)]
    public async Task P22_Proxy(Rol rol)
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(rol);
        var legado = e.Servicio<PasarelaLegadaSimulada>();

        var prueba = await e.Usar(sp => sp.GetRequiredService<SimulacionUseCase>().ProbarCadenaPagoAsync(59.00m));

        Assert.False(prueba.Aprobado);
        Assert.Contains("denegado", prueba.Mensaje);
        Assert.Equal(0, legado.Invocaciones);
        Assert.DoesNotContain(prueba.Trazas, t => t.StartsWith("[COBRO]"));
        Assert.Contains(prueba.Trazas, t => t.StartsWith("[SEGURIDAD] Proxy de pago"));

        e.Sesion(Rol.Ventas);
        var autorizado = await e.Usar(sp => sp.GetRequiredService<SimulacionUseCase>().ProbarCadenaPagoAsync(59.00m));
        Assert.True(autorizado.Aprobado);
        Assert.Equal(1, legado.Invocaciones);
    }

    [Fact(DisplayName = "P24 Command: el invocador registra comando, entrada y resultado")]
    public async Task P24_Command()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var id = await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("P01", 1));

        await e.ConfirmarAsync(id, "CLAVE-CMD");
        await e.Usar(sp => sp.GetRequiredService<ServicioComandosPedido>().CancelarAsync(id, "ya pagado"));

        var historial = e.Servicio<HistorialComandos>().Listar();
        Assert.Contains(historial, r => r.Comando == "ConfirmarPedido" && r.Entrada.Contains("clave=CLAVE-CMD") && r.Exito);
        Assert.Contains(historial, r => r.Comando == "CancelarPedido" && !r.Exito);
        Assert.Equal(2, e.Servicio<MetricasSesion>().Valor(MetricasSesion.ComandosEjecutados));
    }

    [Fact(DisplayName = "P29 Memento en casos de uso: deshacer/rehacer y bloqueo al validar")]
    public async Task P29_MementoCasoUso()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var id = await e.CrearBorradorAsync("Ana Torres", FormaEntrega.Estandar, ("P01", 1));

        await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().AgregarAsync(id, "P02", 1));
        await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().CambiarCantidadAsync(id, "P01", 3));
        await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().AgregarAsync(id, "KIT_OFICINA", 1));
        await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().DeshacerAsync(id));
        await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().DeshacerAsync(id));
        var lineas = (await e.PedidoAsync(id))!.Lineas.Select(l => (l.Codigo, l.Cantidad)).ToArray();
        await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().RehacerAsync(id));
        var rehecho = (await e.PedidoAsync(id))!;

        Assert.Equal(new[] { ("P01", 1), ("P02", 1) }, lineas);
        Assert.Equal(new[] { ("P01", 3), ("P02", 1) }, rehecho.Lineas.Select(l => (l.Codigo, l.Cantidad)));
        Assert.Equal(90.00m, rehecho.Resumen.Subtotal);

        await e.Usar(sp => sp.GetRequiredService<ValidarPedidoUseCase>().EjecutarAsync(id));
        var bloqueado = await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().DeshacerAsync(id));
        Assert.False(bloqueado.Exito);
        Assert.Equal((0, 0), e.Servicio<HistorialEdiciones>().Conteo(id));
    }

    [Fact(DisplayName = "P14 Prototype en casos de uso: duplicar borrador y editar la copia")]
    public async Task P14_PrototypeCasoUso()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var plantilla = await e.CrearBorradorAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1));

        var copia = await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().DuplicarAsync(plantilla));
        await e.Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().CambiarCantidadAsync(copia.PedidoId, "KIT_TELETRABAJO", 3));

        Assert.NotEqual(plantilla, copia.PedidoId);
        Assert.Equal(1, (await e.PedidoAsync(plantilla))!.Lineas[0].Cantidad);
        Assert.Equal(3, (await e.PedidoAsync(copia.PedidoId))!.Lineas[0].Cantidad);
        Assert.Equal(180.00m, (await e.PedidoAsync(copia.PedidoId))!.Resumen.Subtotal);
    }

    [Fact(DisplayName = "P30 Observer: PedidoPagado actualiza notificacion, auditoria y metrica una sola vez")]
    public async Task P30_Observer()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);

        await e.ConfirmarAsync(await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1)));
        var otraVez = await e.Usar(sp => sp.GetRequiredService<ProcesadorOutbox>().ProcesarPendientesAsync());

        var suscriptores = await e.Usar(sp => Task.FromResult(sp.GetRequiredService<PublicadorEventos>().Suscriptores));
        Assert.Equal(3, suscriptores.Count);
        Assert.Single(File.ReadAllLines(Path.Combine(e.Carpeta, "avisos", "email.log")));
        Assert.Single(await e.ListarAsync<RegistroAuditoria>(), r => r.Categoria == CategoriaAuditoria.Evento && r.Mensaje.StartsWith("PedidoPagado"));
        Assert.Equal(1, e.Servicio<MetricasSesion>().Valor(MetricasSesion.PedidosPagados));
        Assert.Equal(0, otraVez.Procesados);
    }

    [Fact(DisplayName = "P33 Template Method: defecto dia 20 aceptado y arrepentimiento dia 10 rechazado")]
    public async Task P33_TemplateMethod()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        var entrega = e.Reloj.Ahora;
        var ana = await e.CrearEntregadoAsync("Ana Torres", entrega, ("KIT_TELETRABAJO", 1));
        var luis = await e.CrearEntregadoAsync("Luis Mena", entrega, ("P04", 1));
        e.Sesion(Rol.Ventas);
        var stockP04 = await e.StockAsync("P04");

        var defecto = await e.Usar(sp => sp.GetRequiredService<ServicioComandosPedido>()
            .DevolverAsync(new Application.Devoluciones.SolicitudDevolucion(ana, MotivoDevolucion.Defecto, entrega.AddDays(20), false)));
        var arrepentimiento = await e.Usar(sp => sp.GetRequiredService<ServicioComandosPedido>()
            .DevolverAsync(new Application.Devoluciones.SolicitudDevolucion(luis, MotivoDevolucion.Arrepentimiento, entrega.AddDays(10), true)));

        Assert.True(defecto.Exito, defecto.Mensaje);
        Assert.Equal(5, defecto.Pasos.Count);
        Assert.False(arrepentimiento.Exito);
        Assert.Contains("7 días", arrepentimiento.Mensaje);
        Assert.Equal(stockP04, await e.StockAsync("P04"));
        Assert.Equal(1, await e.ContarAsync<Devolucion>());
    }
}
