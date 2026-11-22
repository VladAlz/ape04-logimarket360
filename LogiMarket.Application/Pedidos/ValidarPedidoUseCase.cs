using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Application.Validacion;
using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Valoracion;

namespace LogiMarket.Application.Pedidos;

public sealed class ValidarPedidoUseCase
{
    private readonly IPedidoEscritura _pedidos;
    private readonly CadenaValidacionPedido _cadena;
    private readonly CalculadoraPedido _calculadora;
    private readonly HistorialEdiciones _historial;
    private readonly IUnidadTrabajo _unidad;
    private readonly ServicioAutorizacion _autorizacion;
    private readonly ISesionUsuario _sesion;
    private readonly IReloj _reloj;
    private readonly IBitacora _bitacora;

    public ValidarPedidoUseCase(IPedidoEscritura pedidos, CadenaValidacionPedido cadena, CalculadoraPedido calculadora,
        HistorialEdiciones historial, IUnidadTrabajo unidad, ServicioAutorizacion autorizacion, ISesionUsuario sesion,
        IReloj reloj, IBitacora bitacora)
    {
        _pedidos = pedidos;
        _cadena = cadena;
        _calculadora = calculadora;
        _historial = historial;
        _unidad = unidad;
        _autorizacion = autorizacion;
        _sesion = sesion;
        _reloj = reloj;
        _bitacora = bitacora;
    }

    public async Task<ResultadoOperacion> EjecutarAsync(int pedidoId)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.ValidarPedido);
            var pedido = await _pedidos.ObtenerAsync(pedidoId)
                ?? throw new ReglaNegocioException($"El pedido {pedidoId} no existe.");

            var validacion = await _cadena.ValidarAsync(pedido, Operacion.ValidarPedido);
            if (!validacion.Aprobado)
                return ResultadoOperacion.Fallo(
                    $"Validación detenida en «{validacion.Eslabon}»: {validacion.Mensaje}{Environment.NewLine}Recorrido: {validacion.RecorridoTexto}");

            var valoracion = _calculadora.Valorar(pedido);
            pedido.Validar(valoracion, _reloj.Ahora, _sesion.NombreUsuario);
            await _unidad.GuardarCambiosAsync();

            // Ya no se edita, el historial de deshacer deja de tener sentido
            _historial.Limpiar(pedidoId);
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Pedido,
                $"Pedido {pedidoId} VALIDADO. Total {Presentacion.Monto(pedido.Total)} ({pedido.Politica}).");

            return ResultadoOperacion.Correcto(
                $"Pedido {pedidoId} validado.{Environment.NewLine}" +
                $"Subtotal {Presentacion.Monto(valoracion.Subtotal)} - descuento {Presentacion.Monto(valoracion.Descuento)} ({valoracion.Politica}) " +
                $"+ tarifa {Presentacion.Monto(valoracion.Tarifa)} = {Presentacion.Monto(valoracion.Total)}{Environment.NewLine}" +
                $"Recorrido: {validacion.RecorridoTexto}");
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException)
        {
            return ResultadoOperacion.Fallo(ex.Message);
        }
    }
}
