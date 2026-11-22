using System.Reflection;
using LogiMarket.Application.Catalogo;
using LogiMarket.Application.Comandos;
using LogiMarket.Application.Confirmacion;
using LogiMarket.Application.Devoluciones;
using LogiMarket.Application.Eventos;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Pagos;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Devoluciones;
using LogiMarket.Domain.Eventos;
using LogiMarket.Domain.Inventario;
using LogiMarket.Domain.Pagos;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Seguridad;
using LogiMarket.Infrastructure.Pagos;
using LogiMarket.Infrastructure.Pagos.Legado;
using LogiMarket.Infrastructure.Simulacion;
using LogiMarket.Tests.Soporte;
using Microsoft.Extensions.DependencyInjection;

namespace LogiMarket.Tests;

public class PruebasIntegridad
{
    [Fact(DisplayName = "P35 Devolucion de Ana por defecto: reembolso 54.00 una sola vez")]
    public async Task P35_DevolucionUnica()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        var id = await e.CrearEntregadoAsync("Ana Torres", e.Reloj.Ahora, ("KIT_TELETRABAJO", 1));
        e.Sesion(Rol.Ventas);
        var solicitud = new SolicitudDevolucion(id, MotivoDevolucion.Defecto, e.Reloj.Ahora.AddDays(5), false);

        var primera = await e.Usar(sp => sp.GetRequiredService<ServicioComandosPedido>().DevolverAsync(solicitud));
        var segunda = await e.Usar(sp => sp.GetRequiredService<ServicioComandosPedido>().DevolverAsync(solicitud));

        Assert.True(primera.Exito, primera.Mensaje);
        Assert.Equal(54.00m, primera.Reembolso);
        Assert.False(segunda.Exito);
        Assert.Equal((8, 5, 20), (await e.StockAsync("P01"), await e.StockAsync("P02"), await e.StockAsync("P03")));
        Assert.Equal(1, await e.ContarAsync<Devolucion>());
        Assert.Equal(3, (await e.ListarAsync<MovimientoInventario>()).Count(m => m.Motivo == MovimientoInventario.Devolucion));
    }

    [Fact(DisplayName = "P36 Dos cobros concurrentes con stock 1: solo uno se aprueba y no hay sobreventa")]
    public async Task P36_Concurrencia()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Admin);
        await e.Usar(sp => sp.GetRequiredService<GestionarCatalogoUseCase>().AjustarStockAsync("P04", 1, "PRUEBA CONCURRENCIA"));
        var primero = await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("P04", 1));
        var segundo = await e.CrearValidadoAsync("Luis Mena", FormaEntrega.Estandar, ("P04", 1));

        var resultados = await Task.WhenAll(
            Task.Run(() => e.ConfirmarAsync(primero)),
            Task.Run(() => e.ConfirmarAsync(segundo)));

        Assert.Equal(1, resultados.Count(r => r.Exito));
        Assert.Equal(0, await e.StockAsync("P04"));
        Assert.Equal(1, await e.ContarAsync<Pago>());
    }

    [Fact(DisplayName = "P37 Outbox: si falla el notificador el evento queda pendiente y luego se procesa una vez")]
    public async Task P37_OutboxRecuperable()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var simulacion = e.Servicio<ISimulacion>();
        simulacion.FallarNotificacion = true;

        var pago = await e.ConfirmarAsync(await e.CrearValidadoAsync("Ana Torres", FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1)));
        var pendiente = (await e.ListarAsync<EventoOutbox>()).Single();

        simulacion.FallarNotificacion = false;
        var reintento = await e.Usar(sp => sp.GetRequiredService<ProcesadorOutbox>().ProcesarPendientesAsync());
        var otraVez = await e.Usar(sp => sp.GetRequiredService<ProcesadorOutbox>().ProcesarPendientesAsync());
        var final = (await e.ListarAsync<EventoOutbox>()).Single();

        Assert.True(pago.Exito);
        Assert.False(pendiente.Procesado);
        Assert.Contains("no disponible", pendiente.UltimoError);
        Assert.Equal(1, reintento.Procesados);
        Assert.Equal(0, otraVez.Procesados);
        Assert.True(final.Procesado);
        Assert.Equal(2, final.Intentos);
        Assert.Single(File.ReadAllLines(Path.Combine(e.Carpeta, "avisos", "email.log")));
        Assert.Equal(1, await e.ContarAsync<Pago>());
        Assert.Equal(1, e.Servicio<MetricasSesion>().Valor(MetricasSesion.PedidosPagados));
    }

    [Fact(DisplayName = "P38 LSP: los dos proveedores responden igual ante monto valido e invalido")]
    public async Task P38_SustitucionDeProveedores()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        await e.Usar(async sp =>
        {
            var proveedores = new IPasarelaPago[] { sp.GetRequiredService<PasarelaLegadaAdapter>(), sp.GetRequiredService<PasarelaNovaPaySimulada>() };
            foreach (var proveedor in proveedores)
            {
                var valido = await proveedor.CobrarAsync(59.00m, $"LSP-{proveedor.Proveedor}");
                var invalido = await proveedor.CobrarAsync(0m, $"LSP0-{proveedor.Proveedor}");
                Assert.True(valido.Aprobado);
                Assert.False(string.IsNullOrEmpty(valido.Referencia));
                Assert.False(invalido.Aprobado);
                Assert.False(invalido.Denegado);
            }
            return true;
        });
    }

    [Fact(DisplayName = "P38 DIP/ISP: Domain no referencia proyectos ni EF; Application solo a Domain; fachada depende de abstracciones")]
    public void P38_DependenciasDeCompilacion()
    {
        static string[] Referencias(Assembly a) => a.GetReferencedAssemblies().Select(r => r.Name!).ToArray();

        var dominio = Referencias(typeof(Pedido).Assembly);
        var aplicacion = Referencias(typeof(ConfirmacionPedidoFacade).Assembly);

        Assert.DoesNotContain(dominio, n => n.StartsWith("LogiMarket.") || n.Contains("EntityFramework"));
        Assert.Contains("LogiMarket.Domain", aplicacion);
        Assert.DoesNotContain(aplicacion, n => n is "LogiMarket.Infrastructure" or "LogiMarket.WinForms" || n.Contains("EntityFramework") || n.Contains("Sqlite"));

        var parametros = typeof(ConfirmacionPedidoFacade).GetConstructors().Single().GetParameters();
        Assert.All(parametros, p => Assert.NotEqual("LogiMarket.Infrastructure", p.ParameterType.Assembly.GetName().Name));
        Assert.Single(typeof(IPasarelaPago).GetMethods(), m => !m.IsSpecialName);
        Assert.Empty(typeof(ICatalogoLectura).GetMethods().Select(m => m.Name).Intersect(typeof(ICatalogoEscritura).GetMethods().Select(m => m.Name)));
    }

    [Fact(DisplayName = "Seguridad: contraseñas con hash y acceso con credenciales")]
    public async Task Seguridad_HashYAcceso()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        var usuarios = await e.ListarAsync<Usuario>();

        Assert.All(usuarios, u => Assert.StartsWith("PBKDF2$", u.HashContrasena));
        Assert.DoesNotContain(usuarios, u => u.HashContrasena.Contains("#2026"));

        var errado = await e.Usar(sp => sp.GetRequiredService<IniciarSesionUseCase>().EjecutarAsync("ventas", "clave-mala"));
        var correcto = await e.Usar(sp => sp.GetRequiredService<IniciarSesionUseCase>().EjecutarAsync("ventas", "Ventas#2026"));

        Assert.False(errado.Exito);
        Assert.True(correcto.Exito);
        Assert.Equal(Rol.Ventas, e.Servicio<ISesionUsuario>().Rol);
    }

    [Fact(DisplayName = "Autorizacion en Application: CONSULTA no puede validar aunque llame directo al caso de uso")]
    public async Task Autorizacion_SinDependerDeBotones()
    {
        await using var e = await EntornoPrueba.CrearAsync();
        e.Sesion(Rol.Ventas);
        var id = await e.CrearBorradorAsync("Ana Torres", FormaEntrega.Estandar, ("P01", 1));
        e.Sesion(Rol.Consulta);

        var resultado = await e.Usar(sp => sp.GetRequiredService<Application.Pedidos.ValidarPedidoUseCase>().EjecutarAsync(id));

        Assert.False(resultado.Exito);
        Assert.Contains("CONSULTA", resultado.Mensaje);
        Assert.Equal("BORRADOR", (await e.PedidoAsync(id))!.Resumen.Estado);
        Assert.Equal(1, e.Servicio<MetricasSesion>().Valor(MetricasSesion.AccesosDenegados));
    }
}
