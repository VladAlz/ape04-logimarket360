using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Application.Validacion;

public sealed class CadenaValidacionPedido
{
    private readonly IClienteRepositorio _clientes;
    private readonly ICatalogoLectura _catalogo;
    private readonly ISesionUsuario _sesion;
    private readonly EslabonValidacion _inicio;

    public CadenaValidacionPedido(IClienteRepositorio clientes, ICatalogoLectura catalogo, ISesionUsuario sesion)
    {
        _clientes = clientes;
        _catalogo = catalogo;
        _sesion = sesion;
        _inicio = Construir();
    }

    // El orden de la guia: cliente -> lineas -> stock -> autorizacion
    public static EslabonValidacion Construir()
    {
        var inicio = new ValidadorClienteActivo();
        inicio.Enlazar(new ValidadorLineas())
            .Enlazar(new ValidadorStock())
            .Enlazar(new ValidadorCondicionesConfirmacion());
        return inicio;
    }

    public async Task<ResultadoValidacion> ValidarAsync(Pedido pedido, Operacion operacion)
    {
        var cliente = await _clientes.ObtenerAsync(pedido.ClienteId);
        var stock = (await _catalogo.ListarProductosAsync()).ToDictionary(p => p.Sku, p => p.Stock);
        var contexto = new ContextoValidacion(pedido, cliente, stock, operacion, _sesion.Activa, _sesion.Rol);
        return _inicio.Validar(contexto, new List<string>());
    }
}
