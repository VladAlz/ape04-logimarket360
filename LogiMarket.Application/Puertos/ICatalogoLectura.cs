using LogiMarket.Domain.Catalogo;

namespace LogiMarket.Application.Puertos;

public interface ICatalogoLectura
{
    Task<IReadOnlyList<Producto>> ListarProductosAsync(string? filtro = null);

    Task<IReadOnlyList<Kit>> ListarKitsAsync();

    Task<ArmadorCatalogo> CrearArmadorAsync();

    Task<bool> CodigoEnUsoAsync(string codigo);
}
