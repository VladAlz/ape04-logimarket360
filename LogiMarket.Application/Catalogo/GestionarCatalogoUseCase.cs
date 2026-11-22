using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Application.VistaCatalogo;
using LogiMarket.Domain.Catalogo;
using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Inventario;

namespace LogiMarket.Application.Catalogo;

public sealed class GestionarCatalogoUseCase
{
    private readonly ICatalogoLectura _lectura;
    private readonly ICatalogoEscritura _escritura;
    private readonly IPedidoLectura _pedidos;
    private readonly IInventarioRepositorio _inventario;
    private readonly IUnidadTrabajo _unidad;
    private readonly ServicioAutorizacion _autorizacion;
    private readonly CatalogoVisualFlyweightFactory _flyweight;
    private readonly ISesionUsuario _sesion;
    private readonly IReloj _reloj;
    private readonly IBitacora _bitacora;

    public GestionarCatalogoUseCase(ICatalogoLectura lectura, ICatalogoEscritura escritura, IPedidoLectura pedidos,
        IInventarioRepositorio inventario, IUnidadTrabajo unidad, ServicioAutorizacion autorizacion,
        CatalogoVisualFlyweightFactory flyweight, ISesionUsuario sesion, IReloj reloj, IBitacora bitacora)
    {
        _lectura = lectura;
        _escritura = escritura;
        _pedidos = pedidos;
        _inventario = inventario;
        _unidad = unidad;
        _autorizacion = autorizacion;
        _flyweight = flyweight;
        _sesion = sesion;
        _reloj = reloj;
        _bitacora = bitacora;
    }

    public Task<ResultadoOperacion> GuardarProductoAsync(ProductoRequest r) => EjecutarAsync(Operacion.GestionarCatalogo, async () =>
    {
        var sku = r.Sku.Trim().ToUpperInvariant();
        var producto = await _escritura.ObtenerProductoAsync(sku);

        if (producto is null)
        {
            if (await _lectura.CodigoEnUsoAsync(sku))
                throw new ReglaNegocioException($"El código {sku} ya está en uso.");

            producto = new Producto(sku, r.Nombre, r.Categoria, r.Descripcion, r.Precio, r.PesoKg, r.Stock);
            _escritura.AgregarProducto(producto);
            _inventario.Agregar(new MovimientoInventario(sku, r.Stock, r.Stock, MovimientoInventario.Inicial, "ALTA", _reloj.Ahora, _sesion.NombreUsuario));
            await _unidad.GuardarCambiosAsync();
            _flyweight.Reiniciar();
            return $"Producto {sku} creado.";
        }

        producto.Actualizar(r.Nombre, r.Categoria, r.Descripcion, r.Precio, r.PesoKg);
        var diferencia = producto.Stock != r.Stock ? producto.AjustarStock(r.Stock) : 0;
        if (diferencia != 0)
            _inventario.Agregar(new MovimientoInventario(sku, diferencia, producto.Stock, MovimientoInventario.Ajuste, "EDICION CATALOGO", _reloj.Ahora, _sesion.NombreUsuario));

        await _unidad.GuardarCambiosAsync();
        _flyweight.Reiniciar();
        return $"Producto {sku} actualizado.";
    });

    public Task<ResultadoOperacion> EliminarProductoAsync(string sku) => EjecutarAsync(Operacion.GestionarCatalogo, async () =>
    {
        var producto = await _escritura.ObtenerProductoAsync(sku)
            ?? throw new ReglaNegocioException($"El producto {sku} no existe.");

        if ((await _lectura.ListarKitsAsync()).Any(k => k.Componentes.Any(c => c.CodigoComponente == sku)))
            throw new ReglaNegocioException($"{sku} forma parte de un kit; quítelo del kit antes de eliminarlo.");
        if (await _pedidos.CodigoUsadoEnPedidosAsync(sku))
            throw new ReglaNegocioException($"{sku} ya aparece en pedidos registrados y no se puede eliminar.");

        _escritura.EliminarProducto(producto);
        await _unidad.GuardarCambiosAsync();
        _flyweight.Reiniciar();
        return $"Producto {sku} eliminado.";
    });

    public Task<ResultadoOperacion> AjustarStockAsync(string sku, int nuevoStock, string motivo) => EjecutarAsync(Operacion.AjustarInventario, async () =>
    {
        var producto = await _escritura.ObtenerProductoAsync(sku)
            ?? throw new ReglaNegocioException($"El producto {sku} no existe.");

        var diferencia = producto.AjustarStock(nuevoStock);
        if (diferencia == 0)
            return $"El stock de {sku} ya era {nuevoStock}.";

        _inventario.Agregar(new MovimientoInventario(sku, diferencia, producto.Stock, MovimientoInventario.Ajuste,
            string.IsNullOrWhiteSpace(motivo) ? "AJUSTE MANUAL" : motivo.Trim(), _reloj.Ahora, _sesion.NombreUsuario));
        await _unidad.GuardarCambiosAsync();
        return $"Stock de {sku} ajustado a {nuevoStock} ({(diferencia > 0 ? "+" : "")}{diferencia}).";
    });

    public Task<ResultadoOperacion> GuardarKitAsync(KitRequest r) => EjecutarAsync(Operacion.GestionarCatalogo, async () =>
    {
        var codigo = r.Codigo.Trim().ToUpperInvariant();
        var existente = await _escritura.ObtenerKitAsync(codigo);

        if (existente is null && await _lectura.CodigoEnUsoAsync(codigo))
            throw new ReglaNegocioException($"El código {codigo} ya está en uso.");

        // Se prueba el kit nuevo con el arbol completo antes de tocar la base
        var candidato = new Kit(codigo, r.Nombre);
        candidato.DefinirComponentes(r.Componentes);
        var kits = (await _lectura.ListarKitsAsync()).Where(k => k.Codigo != codigo).Append(candidato).ToList();
        var armador = new ArmadorCatalogo(await _lectura.ListarProductosAsync(), kits);
        var elemento = armador.Obtener(codigo);

        if (existente is null)
            _escritura.AgregarKit(candidato);
        else
        {
            existente.Renombrar(r.Nombre);
            existente.DefinirComponentes(r.Componentes);
        }

        await _unidad.GuardarCambiosAsync();
        return $"Kit {codigo} guardado. Precio calculado USD {Presentacion.Monto(elemento.Precio)}.";
    });

    private async Task<ResultadoOperacion> EjecutarAsync(Operacion operacion, Func<Task<string>> accion)
    {
        try
        {
            await _autorizacion.ExigirAsync(operacion);
            var mensaje = await accion();
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Inventario, mensaje);
            return ResultadoOperacion.Correcto(mensaje);
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException or OperacionRechazadaException)
        {
            return ResultadoOperacion.Fallo(ex.Message);
        }
    }
}
