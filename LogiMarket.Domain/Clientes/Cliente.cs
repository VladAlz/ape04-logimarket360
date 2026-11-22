using LogiMarket.Domain.Comun;

namespace LogiMarket.Domain.Clientes;

public class Cliente
{
    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Identificacion { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Telefono { get; private set; } = string.Empty;
    public CategoriaCliente Categoria { get; private set; }
    public CanalAviso Canal { get; private set; }
    public bool Activo { get; private set; }

    private Cliente()
    {
    }

    public Cliente(string nombre, string identificacion, string email, string telefono,
        CategoriaCliente categoria, CanalAviso canal)
    {
        Actualizar(nombre, identificacion, email, telefono, categoria, canal, true);
    }

    public void Actualizar(string nombre, string identificacion, string email, string telefono,
        CategoriaCliente categoria, CanalAviso canal, bool activo)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ReglaNegocioException("El nombre del cliente es obligatorio.");
        if (string.IsNullOrWhiteSpace(identificacion))
            throw new ReglaNegocioException("La identificación del cliente es obligatoria.");
        if (canal == CanalAviso.Email && !email.Contains('@'))
            throw new ReglaNegocioException("Para avisos por EMAIL se requiere un correo válido.");
        if (canal == CanalAviso.Sms && string.IsNullOrWhiteSpace(telefono))
            throw new ReglaNegocioException("Para avisos por SMS se requiere un teléfono.");

        Nombre = nombre.Trim();
        Identificacion = identificacion.Trim();
        Email = email.Trim();
        Telefono = telefono.Trim();
        Categoria = categoria;
        Canal = canal;
        Activo = activo;
    }

    public string Destino => Canal == CanalAviso.Email ? Email : Telefono;
}
