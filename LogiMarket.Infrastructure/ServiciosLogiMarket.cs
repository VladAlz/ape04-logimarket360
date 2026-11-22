using LogiMarket.Application.Catalogo;
using LogiMarket.Application.Clientes;
using LogiMarket.Application.Comandos;
using LogiMarket.Application.Comprobantes;
using LogiMarket.Application.Confirmacion;
using LogiMarket.Application.Consultas;
using LogiMarket.Application.Devoluciones;
using LogiMarket.Application.Eventos;
using LogiMarket.Application.Inventario;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Notificaciones;
using LogiMarket.Application.Pagos;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Application.Simulacion;
using LogiMarket.Application.Validacion;
using LogiMarket.Application.VistaCatalogo;
using LogiMarket.Domain.Entregas;
using LogiMarket.Domain.Promociones;
using LogiMarket.Domain.Valoracion;
using LogiMarket.Infrastructure.Bitacora;
using LogiMarket.Infrastructure.Comun;
using LogiMarket.Infrastructure.Notificaciones;
using LogiMarket.Infrastructure.Pagos;
using LogiMarket.Infrastructure.Pagos.Legado;
using LogiMarket.Infrastructure.Persistencia;
using LogiMarket.Infrastructure.Persistencia.Repositorios;
using LogiMarket.Infrastructure.Salidas;
using LogiMarket.Infrastructure.Seguridad;
using LogiMarket.Infrastructure.Simulacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LogiMarket.Infrastructure;

// Registro comun para WinForms y para las pruebas; aqui se nombran las clases concretas
public static class ServiciosLogiMarket
{
    public static IServiceCollection AgregarLogiMarket(this IServiceCollection servicios, OpcionesLogiMarket opciones)
    {
        servicios.AddSingleton(opciones);
        servicios.AddDbContextFactory<LogiMarketDbContext>(o => o.UseSqlite(opciones.CadenaConexion));

        AgregarCompartidos(servicios);
        AgregarDominio(servicios);
        AgregarPersistencia(servicios);
        AgregarCadenaPago(servicios);
        AgregarCasosUso(servicios);
        return servicios;
    }

    // Una sola instancia durante toda la sesion
    private static void AgregarCompartidos(IServiceCollection s)
    {
        s.AddSingleton<ISesionUsuario, SesionUsuario>();
        s.AddSingleton<MetricasSesion>();
        s.AddSingleton<HistorialEdiciones>();
        s.AddSingleton<HistorialComandos>();
        s.AddSingleton<CatalogoVisualFlyweightFactory>();
        s.AddSingleton<ISimulacion, ConfiguracionSimulacion>();
        s.AddSingleton<IReloj, RelojSistema>();
        s.AddSingleton<IHasherContrasenas, HasherPbkdf2>();
        s.AddSingleton<ISalidaDocumento, SalidaVistaPrevia>();
        s.AddSingleton<ISalidaDocumento, SalidaArchivoTxt>();
        s.AddSingleton<CreadorNotificador, CreadorNotificadorEmail>();
        s.AddSingleton<CreadorNotificador, CreadorNotificadorSms>();
    }

    private static void AgregarDominio(IServiceCollection s)
    {
        s.AddSingleton<IPoliticaDescuento, SinDescuento>();
        s.AddSingleton<IPoliticaDescuento, Vip10>();
        s.AddSingleton<IPoliticaDescuento, Mayorista15>();
        s.AddSingleton<SelectorPoliticaDescuento>();
        s.AddSingleton<IFabricaEntrega, FabricaEntregaEstandar>();
        s.AddSingleton<IFabricaEntrega, FabricaEntregaExpres>();
        s.AddSingleton<CalculadoraPedido>();
    }

    private static void AgregarPersistencia(IServiceCollection s)
    {
        s.AddScoped<BitacoraEf>();
        s.AddScoped<IBitacora>(sp => sp.GetRequiredService<BitacoraEf>());
        s.AddScoped<IUnidadTrabajo, UnidadTrabajoEf>();
        s.AddScoped<InicializadorBaseDatos>();

        s.AddScoped<CatalogoRepositorioEf>();
        s.AddScoped<ICatalogoLectura>(sp => sp.GetRequiredService<CatalogoRepositorioEf>());
        s.AddScoped<ICatalogoEscritura>(sp => sp.GetRequiredService<CatalogoRepositorioEf>());
        s.AddScoped<PedidoRepositorioEf>();
        s.AddScoped<IPedidoEscritura>(sp => sp.GetRequiredService<PedidoRepositorioEf>());
        s.AddScoped<IPedidoLectura>(sp => sp.GetRequiredService<PedidoRepositorioEf>());
        s.AddScoped<IClienteRepositorio, ClienteRepositorioEf>();
        s.AddScoped<IPagoRepositorio, PagoRepositorioEf>();
        s.AddScoped<IInventarioRepositorio, InventarioRepositorioEf>();
        s.AddScoped<IDespachoRepositorio, DespachoRepositorioEf>();
        s.AddScoped<IDevolucionRepositorio, DevolucionRepositorioEf>();
        s.AddScoped<IOutboxRepositorio, OutboxRepositorioEf>();
        s.AddScoped<IUsuarioRepositorio, UsuarioRepositorioEf>();
    }

    // Orden de la guia: Proxy -> Decorator -> Adapter (proveedor elegido)
    private static void AgregarCadenaPago(IServiceCollection s)
    {
        s.AddSingleton(sp => new PasarelaLegadaSimulada(() => sp.GetRequiredService<ISimulacion>().FallarCobro));
        s.AddScoped<PasarelaLegadaAdapter>();
        s.AddScoped<PasarelaNovaPaySimulada>();
        s.AddScoped<SelectorProveedorPago>();
        s.AddScoped<IPasarelaPago>(sp =>
        {
            var bitacora = sp.GetRequiredService<IBitacora>();
            var decorador = new AuditoriaPagoDecorator(sp.GetRequiredService<SelectorProveedorPago>(), bitacora);
            return new PagoAutorizadoProxy(decorador, sp.GetRequiredService<ISesionUsuario>(), bitacora,
                sp.GetRequiredService<MetricasSesion>());
        });
    }

    private static void AgregarCasosUso(IServiceCollection s)
    {
        s.AddScoped<ServicioAutorizacion>();
        s.AddScoped<IniciarSesionUseCase>();
        s.AddScoped<CadenaValidacionPedido>();

        s.AddScoped<IObservadorEvento, ObservadorNotificacion>();
        s.AddScoped<IObservadorEvento, ObservadorAuditoria>();
        s.AddScoped<IObservadorEvento, ObservadorMetricas>();
        s.AddScoped<PublicadorEventos>();
        s.AddScoped<ProcesadorOutbox>();

        s.AddScoped<ConsultarCatalogoUseCase>();
        s.AddScoped<GestionarCatalogoUseCase>();
        s.AddScoped<GestionarClientesUseCase>();
        s.AddScoped<EdicionBorradorUseCase>();
        s.AddScoped<ValidarPedidoUseCase>();
        s.AddScoped<CancelarPedidoUseCase>();
        s.AddScoped<DespacharPedidoUseCase>();
        s.AddScoped<EntregarPedidoUseCase>();
        s.AddScoped<ConsultarPedidosUseCase>();
        s.AddScoped<ConfirmacionPedidoFacade>();

        s.AddScoped<ServiciosDevolucion>();
        s.AddScoped<ProcesoDevolucion, DevolucionPorDefecto>();
        s.AddScoped<ProcesoDevolucion, DevolucionPorArrepentimiento>();
        s.AddScoped<SelectorDevolucion>();

        s.AddScoped<InvocadorComandos>();
        s.AddScoped<ServicioComandosPedido>();

        s.AddScoped<GenerarComprobanteUseCase>();
        s.AddScoped<GenerarVistaMasivaUseCase>();
        s.AddScoped<ConsultarInventarioUseCase>();
        s.AddScoped<PanelGeneralUseCase>();
        s.AddScoped<ConsultarAuditoriaUseCase>();
        s.AddScoped<SimulacionUseCase>();
    }
}
