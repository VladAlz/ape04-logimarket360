using LogiMarket.Application.Puertos;
using LogiMarket.Domain.Clientes;
using Microsoft.EntityFrameworkCore;

namespace LogiMarket.Infrastructure.Persistencia.Repositorios;

public sealed class ClienteRepositorioEf : IClienteRepositorio
{
    private readonly LogiMarketDbContext _contexto;

    public ClienteRepositorioEf(LogiMarketDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<Cliente>> ListarAsync(string? filtro = null)
    {
        var consulta = _contexto.Clientes.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var patron = $"%{filtro.Trim()}%";
            consulta = consulta.Where(c => EF.Functions.Like(c.Nombre, patron) || EF.Functions.Like(c.Identificacion, patron));
        }

        return await consulta.OrderBy(c => c.Nombre).ToListAsync();
    }

    public Task<Cliente?> ObtenerAsync(int id) => _contexto.Clientes.FirstOrDefaultAsync(c => c.Id == id);

    public Task<bool> IdentificacionEnUsoAsync(string identificacion, int excluirId) =>
        _contexto.Clientes.AnyAsync(c => c.Identificacion == identificacion && c.Id != excluirId);

    public void Agregar(Cliente cliente) => _contexto.Clientes.Add(cliente);
}
