using LogiMarket.Application.Comun;
using LogiMarket.Application.Puertos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Domain.Clientes;
using LogiMarket.Domain.Comun;

namespace LogiMarket.Application.Clientes;

public sealed record ClienteDto(int Id, string Nombre, string Identificacion, string Email, string Telefono, string Categoria, string Canal, bool Activo)
{
    public override string ToString() => $"{Nombre} ({Categoria})";
}

public sealed record ClienteRequest(int Id, string Nombre, string Identificacion, string Email, string Telefono,
    CategoriaCliente Categoria, CanalAviso Canal, bool Activo);

public sealed class GestionarClientesUseCase
{
    private readonly IClienteRepositorio _clientes;
    private readonly IUnidadTrabajo _unidad;
    private readonly ServicioAutorizacion _autorizacion;
    private readonly IBitacora _bitacora;

    public GestionarClientesUseCase(IClienteRepositorio clientes, IUnidadTrabajo unidad, ServicioAutorizacion autorizacion, IBitacora bitacora)
    {
        _clientes = clientes;
        _unidad = unidad;
        _autorizacion = autorizacion;
        _bitacora = bitacora;
    }

    public async Task<IReadOnlyList<ClienteDto>> ListarAsync(string? filtro = null) =>
        (await _clientes.ListarAsync(filtro)).Select(Mapear).ToList();

    public async Task<ResultadoOperacion> GuardarAsync(ClienteRequest r)
    {
        try
        {
            await _autorizacion.ExigirAsync(Operacion.GestionarClientes);
            if (await _clientes.IdentificacionEnUsoAsync(r.Identificacion.Trim(), r.Id))
                throw new ReglaNegocioException($"Ya existe un cliente con identificación {r.Identificacion.Trim()}.");

            Cliente cliente;
            if (r.Id == 0)
            {
                cliente = new Cliente(r.Nombre, r.Identificacion, r.Email, r.Telefono, r.Categoria, r.Canal);
                if (!r.Activo)
                    cliente.Actualizar(r.Nombre, r.Identificacion, r.Email, r.Telefono, r.Categoria, r.Canal, false);
                _clientes.Agregar(cliente);
            }
            else
            {
                cliente = await _clientes.ObtenerAsync(r.Id) ?? throw new ReglaNegocioException($"El cliente {r.Id} no existe.");
                cliente.Actualizar(r.Nombre, r.Identificacion, r.Email, r.Telefono, r.Categoria, r.Canal, r.Activo);
            }

            await _unidad.GuardarCambiosAsync();
            await _bitacora.RegistrarAsync(CategoriaAuditoria.Sistema, $"Cliente {cliente.Id} {cliente.Nombre} guardado ({cliente.Categoria.ToString().ToUpperInvariant()}, aviso {cliente.Canal.ToString().ToUpperInvariant()}).");
            return ResultadoOperacion.Correcto($"Cliente {cliente.Nombre} guardado con Id {cliente.Id}.");
        }
        catch (Exception ex) when (ex is ReglaNegocioException or AccesoDenegadoException)
        {
            return ResultadoOperacion.Fallo(ex.Message);
        }
    }

    private static ClienteDto Mapear(Cliente c) => new(c.Id, c.Nombre, c.Identificacion, c.Email, c.Telefono,
        c.Categoria.ToString().ToUpperInvariant(), c.Canal.ToString().ToUpperInvariant(), c.Activo);
}
