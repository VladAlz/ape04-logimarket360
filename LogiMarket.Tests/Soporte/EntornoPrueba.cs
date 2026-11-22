using LogiMarket.Application.Catalogo;
using LogiMarket.Application.Comandos;
using LogiMarket.Application.Confirmacion;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Seguridad;
using LogiMarket.Infrastructure;
using LogiMarket.Infrastructure.Comun;
using LogiMarket.Infrastructure.Persistencia;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LogiMarket.Tests.Soporte;

// Cada prueba trabaja con su propia base SQLite en una carpeta temporal
public sealed class EntornoPrueba : IAsyncDisposable
{
    private readonly Action<IServiceCollection>? _ajustes;

    private EntornoPrueba(Action<IServiceCollection>? ajustes)
    {
        _ajustes = ajustes;
        Carpeta = Path.Combine(Path.GetTempPath(), "logimarket_pruebas", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Carpeta);
        Opciones = new OpcionesLogiMarket(Path.Combine(Carpeta, "logimarket_prueba.db"), Carpeta);
        Proveedor = Construir();
    }

    public string Carpeta { get; }
    public OpcionesLogiMarket Opciones { get; }
    public RelojPrueba Reloj { get; } = new(new DateTime(2026, 11, 2, 10, 0, 0));
    public ServiceProvider Proveedor { get; private set; }

    public static async Task<EntornoPrueba> CrearAsync(Action<IServiceCollection>? ajustes = null)
    {
        var entorno = new EntornoPrueba(ajustes);
        await entorno.Usar(sp => sp.GetRequiredService<InicializadorBaseDatos>().InicializarAsync());
        return entorno;
    }

    // Simula cerrar y volver a abrir la aplicacion sobre el mismo archivo
    public async Task ReiniciarAplicacionAsync()
    {
        await Proveedor.DisposeAsync();
        SqliteConnection.ClearAllPools();
        Proveedor = Construir();
        await Usar(sp => sp.GetRequiredService<InicializadorBaseDatos>().InicializarAsync());
    }

    public async Task<T> Usar<T>(Func<IServiceProvider, Task<T>> accion)
    {
        await using var ambito = Proveedor.CreateAsyncScope();
        return await accion(ambito.ServiceProvider);
    }

    public T Servicio<T>() where T : notnull => Proveedor.GetRequiredService<T>();

    public void Sesion(Rol rol) =>
        Servicio<ISesionUsuario>().Iniciar(new Usuario(rol.ToString().ToLowerInvariant(), $"Prueba {rol}", rol, "hash-de-prueba"));

    public Task<int> ClienteIdAsync(string nombre) =>
        Usar(async sp => (await sp.GetRequiredService<IClienteRepositorio>().ListarAsync(nombre)).Single().Id);

    public Task<int> StockAsync(string sku) =>
        Usar(async sp => (await sp.GetRequiredService<ICatalogoLectura>().ListarProductosAsync()).Single(p => p.Sku == sku).Stock);

    public async Task<int> CrearBorradorAsync(string cliente, FormaEntrega entrega, params (string Codigo, int Cantidad)[] lineas)
    {
        var clienteId = await ClienteIdAsync(cliente);
        var resultado = await Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>()
            .CrearAsync(clienteId, entrega, lineas[0].Codigo, lineas[0].Cantidad));
        Assert.True(resultado.Exito, resultado.Mensaje);

        foreach (var (codigo, cantidad) in lineas.Skip(1))
        {
            var r = await Usar(sp => sp.GetRequiredService<EdicionBorradorUseCase>().AgregarAsync(resultado.PedidoId, codigo, cantidad));
            Assert.True(r.Exito, r.Mensaje);
        }

        return resultado.PedidoId;
    }

    public async Task<int> CrearValidadoAsync(string cliente, FormaEntrega entrega, params (string Codigo, int Cantidad)[] lineas)
    {
        var id = await CrearBorradorAsync(cliente, entrega, lineas);
        var validacion = await Usar(sp => sp.GetRequiredService<ValidarPedidoUseCase>().EjecutarAsync(id));
        Assert.True(validacion.Exito, validacion.Mensaje);
        return id;
    }

    public Task<ResultadoConfirmacion> ConfirmarAsync(int pedidoId, string? clave = null) =>
        Usar(sp => sp.GetRequiredService<ServicioComandosPedido>()
            .ConfirmarAsync(new SolicitudConfirmacion(pedidoId, clave ?? $"CLAVE-{pedidoId}-{Guid.NewGuid():N}")));

    public Task<PedidoDetalleDto?> PedidoAsync(int pedidoId) =>
        Usar(sp => sp.GetRequiredService<ConsultarPedidosUseCase>().DetalleAsync(pedidoId));

    public Task<int> ContarAsync<T>() where T : class =>
        Usar(sp => sp.GetRequiredService<LogiMarketDbContext>().Set<T>().CountAsync());

    public Task<List<T>> ListarAsync<T>() where T : class =>
        Usar(sp => sp.GetRequiredService<LogiMarketDbContext>().Set<T>().AsNoTracking().ToListAsync());

    public Task<IReadOnlyList<ProductoDto>> ProductosAsync() =>
        Usar(sp => sp.GetRequiredService<ConsultarCatalogoUseCase>().ProductosAsync());

    // Pagado, despachado y entregado en la fecha indicada
    public async Task<int> CrearEntregadoAsync(string cliente, DateTime fechaEntrega, params (string Codigo, int Cantidad)[] lineas)
    {
        Sesion(Rol.Ventas);
        var id = await CrearValidadoAsync(cliente, FormaEntrega.Estandar, lineas);
        var pago = await ConfirmarAsync(id);
        Assert.True(pago.Exito, pago.Mensaje);

        Sesion(Rol.Bodega);
        var despacho = await Usar(sp => sp.GetRequiredService<ServicioComandosPedido>().DespacharAsync(id));
        Assert.True(despacho.Exito, despacho.Mensaje);
        var entrega = await Usar(sp => sp.GetRequiredService<EntregarPedidoUseCase>().EjecutarAsync(id, fechaEntrega));
        Assert.True(entrega.Exito, entrega.Mensaje);
        return id;
    }

    public async ValueTask DisposeAsync()
    {
        await Proveedor.DisposeAsync();
        SqliteConnection.ClearAllPools();
        try
        {
            Directory.Delete(Carpeta, true);
        }
        catch (IOException)
        {
            // Si el sistema aun retiene el archivo, la carpeta temporal se limpia despues
        }
    }

    private ServiceProvider Construir()
    {
        var servicios = new ServiceCollection();
        servicios.AgregarLogiMarket(Opciones);
        servicios.AddSingleton<IReloj>(Reloj);
        _ajustes?.Invoke(servicios);
        return servicios.BuildServiceProvider();
    }
}
