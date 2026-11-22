using LogiMarket.Infrastructure;
using LogiMarket.Infrastructure.Comun;
using LogiMarket.Infrastructure.Persistencia;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;
using LogiMarket.WinForms.Vistas;
using Microsoft.Extensions.DependencyInjection;

namespace LogiMarket.WinForms;

// Composition root: unico lugar de WinForms que conoce Infrastructure
internal static class ConfiguracionDependencias
{
    public static ServiceProvider Crear(OpcionesArranque arranque)
    {
        Directory.CreateDirectory(arranque.CarpetaDatos);
        var servicios = new ServiceCollection();
        servicios.AgregarLogiMarket(new OpcionesLogiMarket(Path.Combine(arranque.CarpetaDatos, "logimarket.db"), arranque.CarpetaDatos));

        servicios.AddSingleton<IMediadorVistas, FormularioPrincipalMediator>();
        servicios.AddSingleton<ControlPermisos>();
        servicios.AddSingleton<EjecutorCasosUso>();

        servicios.AddTransient<FrmAcceso>();
        servicios.AddTransient<FrmPrincipal>();
        servicios.AddTransient<UcPanel>();
        servicios.AddTransient<UcCatalogo>();
        servicios.AddTransient<UcClientes>();
        servicios.AddTransient<UcCarrito>();
        servicios.AddTransient<UcPedidos>();
        servicios.AddTransient<UcCobro>();
        servicios.AddTransient<UcDespacho>();
        servicios.AddTransient<UcDevoluciones>();
        servicios.AddTransient<UcInventario>();
        servicios.AddTransient<UcReportes>();
        servicios.AddTransient<UcAuditoria>();

        return servicios.BuildServiceProvider();
    }

    public static string PrepararBaseDatos(ServiceProvider proveedor, OpcionesArranque arranque)
    {
        using var ambito = proveedor.CreateScope();
        var inicializador = ambito.ServiceProvider.GetRequiredService<InicializadorBaseDatos>();
        var resumen = Task.Run(() => arranque.ReiniciarDatos ? inicializador.RestablecerAsync() : inicializador.InicializarAsync())
            .GetAwaiter().GetResult();
        return resumen.ToString();
    }
}
