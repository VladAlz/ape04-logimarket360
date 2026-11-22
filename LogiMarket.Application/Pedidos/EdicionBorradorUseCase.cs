using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Valoracion;

namespace LogiMarket.Application.Pedidos;

public sealed record ResultadoEdicion(bool Exito, string Mensaje, int PedidoId, int Deshacer, int Rehacer);

public sealed class EdicionBorradorUseCase
{
    private readonly IPedidoEscritura _pedidos;
    private readonly IClienteRepositorio _clientes;
    private readonly ICatalogoLectura _catalogo;
    private readonly CalculadoraPedido _calculadora;
    private readonly HistorialEdiciones _historial;
    private readonly IUnidadTrabajo _unidad;
    private readonly ServicioAutorizacion _autorizacion;
    private readonly ISesionUsuario _sesion;
    private readonly IReloj _reloj;

    public EdicionBorradorUseCase(IPedidoEscritura pedidos, IClienteRepositorio clientes, ICatalogoLectura catalogo,
        CalculadoraPedido calculadora, HistorialEdiciones historial, IUnidadTrabajo unidad,
        ServicioAutorizacion autorizacion, ISesionUsuario sesion, IReloj reloj)
    {
        _pedidos = pedidos;
        _clientes = clientes;
        _catalogo = catalogo;
        _calculadora = calculadora;
        _historial = historial;
        _unidad = unidad;
        _autorizacion = autorizacion;
        _sesion = sesion;
        _reloj = reloj;
    }

    public async Task<ResultadoEdicion> CrearAsync(int clienteId, FormaEntrega entrega, string codigo, int cantidad)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.EditarPedido);
            var cliente = await _clientes.ObtenerAsync(clienteId)
                ?? throw new ReglaNegocioException("Seleccione un cliente registrado.");
            var armador = await _catalogo.CrearArmadorAsync();

            var builder = new PedidoBuilder()
                .ParaCliente(cliente)
                .ConEntrega(entrega)
                .RegistradoPor(_sesion.NombreUsuario, _reloj.Ahora);
            if (!string.IsNullOrWhiteSpace(codigo))
                builder.Agregar(armador.Obtener(codigo), cantidad);

            var pedido = builder.Construir();
            pedido.AplicarValoracion(_calculadora.Valorar(pedido));
            _pedidos.Agregar(pedido);
            await _unidad.GuardarCambiosAsync();

            return Correcto(pedido, $"Borrador {pedido.Id} creado para {pedido.ClienteNombre}.");
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException)
        {
            return new ResultadoEdicion(false, ex.Message, 0, 0, 0);
        }
    }

    public Task<ResultadoEdicion> AgregarAsync(int pedidoId, string codigo, int cantidad) =>
        EditarAsync(pedidoId, $"Agregar {codigo} x{cantidad}", async p =>
        {
            var armador = await _catalogo.CrearArmadorAsync();
            p.AgregarElemento(armador.Obtener(codigo), cantidad);
        });

    public Task<ResultadoEdicion> CambiarCantidadAsync(int pedidoId, string codigo, int cantidad) =>
        EditarAsync(pedidoId, $"Cantidad de {codigo} a {cantidad}", p =>
        {
            p.CambiarCantidad(codigo, cantidad);
            return Task.CompletedTask;
        });

    public Task<ResultadoEdicion> QuitarAsync(int pedidoId, string codigo) =>
        EditarAsync(pedidoId, $"Quitar {codigo}", p =>
        {
            p.QuitarLinea(codigo);
            return Task.CompletedTask;
        });

    public Task<ResultadoEdicion> CambiarEntregaAsync(int pedidoId, FormaEntrega entrega) =>
        EditarAsync(pedidoId, $"Entrega {PedidoMapper.Entrega(entrega)}", p =>
        {
            p.CambiarEntrega(entrega);
            return Task.CompletedTask;
        });

    public Task<ResultadoEdicion> CambiarClienteAsync(int pedidoId, int clienteId) =>
        EditarAsync(pedidoId, "Cambio de cliente", async p =>
        {
            var cliente = await _clientes.ObtenerAsync(clienteId)
                ?? throw new ReglaNegocioException("Seleccione un cliente registrado.");
            p.CambiarCliente(cliente);
        });

    public Task<ResultadoEdicion> DeshacerAsync(int pedidoId) =>
        RestaurarAsync(pedidoId, deshacer: true);

    public Task<ResultadoEdicion> RehacerAsync(int pedidoId) =>
        RestaurarAsync(pedidoId, deshacer: false);

    // Prototype: la copia queda como borrador nuevo, la plantilla no cambia
    public async Task<ResultadoEdicion> DuplicarAsync(int pedidoId)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.EditarPedido);
            var plantilla = await Cargar(pedidoId);
            var copia = plantilla.Clonar();
            copia.AnotarOrigen(_reloj.Ahora, _sesion.NombreUsuario, $"Copia del borrador {plantilla.Id}.");
            copia.AplicarValoracion(_calculadora.Valorar(copia));
            _pedidos.Agregar(copia);
            await _unidad.GuardarCambiosAsync();

            return Correcto(copia, $"Borrador {copia.Id} creado como copia del {plantilla.Id}.");
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException)
        {
            return Fallo(pedidoId, ex.Message);
        }
    }

    public (int Deshacer, int Rehacer) Conteo(int pedidoId) => _historial.Conteo(pedidoId);

    private async Task<ResultadoEdicion> EditarAsync(int pedidoId, string descripcion, Func<Pedido, Task> cambio)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.EditarPedido);
            var pedido = await Cargar(pedidoId);
            var antes = pedido.CrearMemento(_reloj.Ahora, descripcion);

            await cambio(pedido);
            pedido.AplicarValoracion(_calculadora.Valorar(pedido));
            await _unidad.GuardarCambiosAsync();

            // Solo se guarda el estado previo si el cambio se aplico
            _historial.Registrar(pedidoId, antes);
            return Correcto(pedido, $"{descripcion}: listo.");
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException)
        {
            return Fallo(pedidoId, ex.Message);
        }
    }

    private async Task<ResultadoEdicion> RestaurarAsync(int pedidoId, bool deshacer)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.EditarPedido);
            var pedido = await Cargar(pedidoId);
            var actual = pedido.CrearMemento(_reloj.Ahora, "Estado actual");
            var memento = deshacer ? _historial.Deshacer(pedidoId, actual) : _historial.Rehacer(pedidoId, actual);

            if (memento is null)
                return Fallo(pedidoId, deshacer ? "No hay cambios para deshacer." : "No hay cambios para rehacer.");

            pedido.Restaurar(memento);
            pedido.AplicarValoracion(_calculadora.Valorar(pedido));
            await _unidad.GuardarCambiosAsync();

            return Correcto(pedido, deshacer ? $"Se deshizo: {memento.Descripcion}." : "Cambio rehecho.");
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException)
        {
            return Fallo(pedidoId, ex.Message);
        }
    }

    private async Task<Pedido> Cargar(int pedidoId) =>
        await _pedidos.ObtenerAsync(pedidoId) ?? throw new ReglaNegocioException($"El pedido {pedidoId} no existe.");

    private ResultadoEdicion Correcto(Pedido pedido, string mensaje)
    {
        var (deshacer, rehacer) = _historial.Conteo(pedido.Id);
        return new ResultadoEdicion(true, mensaje, pedido.Id, deshacer, rehacer);
    }

    private ResultadoEdicion Fallo(int pedidoId, string mensaje)
    {
        var (deshacer, rehacer) = _historial.Conteo(pedidoId);
        return new ResultadoEdicion(false, mensaje, pedidoId, deshacer, rehacer);
    }
}
