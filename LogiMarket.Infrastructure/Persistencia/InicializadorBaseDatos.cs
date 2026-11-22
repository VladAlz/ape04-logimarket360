using LogiMarket.Application.Puertos;
using LogiMarket.Domain.Catalogo;
using LogiMarket.Domain.Clientes;
using LogiMarket.Domain.Inventario;
using LogiMarket.Domain.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace LogiMarket.Infrastructure.Persistencia;

public sealed record ResumenSemilla(int Usuarios, int Productos, int Kits, int Clientes)
{
    public override string ToString() =>
        $"Semilla aplicada: {Usuarios} usuario(s), {Productos} producto(s), {Kits} kit(s), {Clientes} cliente(s) nuevos.";
}

// Migra y carga datos de control; si ya existen no los duplica
public sealed class InicializadorBaseDatos
{
    private readonly LogiMarketDbContext _contexto;
    private readonly IHasherContrasenas _hasher;

    public InicializadorBaseDatos(LogiMarketDbContext contexto, IHasherContrasenas hasher)
    {
        _contexto = contexto;
        _hasher = hasher;
    }

    public async Task<ResumenSemilla> InicializarAsync()
    {
        await _contexto.Database.MigrateAsync();
        await _contexto.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL;");

        var usuarios = await SembrarUsuariosAsync();
        var productos = await SembrarProductosAsync();
        var kits = await SembrarKitsAsync();
        var clientes = await SembrarClientesAsync();
        await _contexto.SaveChangesAsync();

        return new ResumenSemilla(usuarios, productos, kits, clientes);
    }

    // Base de prueba controlada: se borra el archivo y se vuelve a crear
    public async Task<ResumenSemilla> RestablecerAsync()
    {
        await _contexto.Database.EnsureDeletedAsync();
        return await InicializarAsync();
    }

    private async Task<int> SembrarUsuariosAsync()
    {
        var datos = new (string Usuario, string Nombre, Rol Rol, string Clave)[]
        {
            ("admin", "Administrador General", Rol.Admin, "Admin#2026"),
            ("ventas", "Vendedor de Turno", Rol.Ventas, "Ventas#2026"),
            ("bodega", "Jefe de Bodega", Rol.Bodega, "Bodega#2026"),
            ("consulta", "Usuario de Consulta", Rol.Consulta, "Consulta#2026")
        };

        var nuevos = 0;
        foreach (var d in datos)
        {
            if (await _contexto.Usuarios.AnyAsync(u => u.NombreUsuario == d.Usuario))
                continue;
            _contexto.Usuarios.Add(new Usuario(d.Usuario, d.Nombre, d.Rol, _hasher.Calcular(d.Clave)));
            nuevos++;
        }
        return nuevos;
    }

    private async Task<int> SembrarProductosAsync()
    {
        var datos = new[]
        {
            new Producto("P01", "Mouse USB", "Periféricos", "Mouse óptico USB de 1200 DPI con cable de 1.5 m, diseño ambidiestro.", 20.00m, 0.20m, 8),
            new Producto("P02", "Teclado", "Periféricos", "Teclado alámbrico USB de 104 teclas con distribución en español.", 30.00m, 0.70m, 5),
            new Producto("P03", "Memoria USB", "Almacenamiento", "Memoria flash USB 3.0 de 32 GB con tapa protectora y llavero.", 10.00m, 0.10m, 20),
            new Producto("P04", "Webcam", "Video", "Cámara web HD 1080p con micrófono integrado y clip para monitor.", 40.00m, 0.35m, 4),
            new Producto("P05", "Alfombrilla", "Accesorios", "Alfombrilla de tela con base antideslizante de 25 x 21 cm.", 5.00m, 0.10m, 30)
        };

        var nuevos = 0;
        foreach (var p in datos)
        {
            if (await _contexto.Productos.AnyAsync(x => x.Sku == p.Sku))
                continue;
            _contexto.Productos.Add(p);
            _contexto.MovimientosInventario.Add(new MovimientoInventario(p.Sku, p.Stock, p.Stock, MovimientoInventario.Inicial, "SEMILLA", DateTime.Now, "sistema"));
            nuevos++;
        }
        return nuevos;
    }

    private async Task<int> SembrarKitsAsync()
    {
        var nuevos = 0;
        if (!await _contexto.Kits.AnyAsync(k => k.Codigo == "KIT_OFICINA"))
        {
            var oficina = new Kit("KIT_OFICINA", "Kit Oficina");
            oficina.DefinirComponentes(new[] { ("P01", 1), ("P02", 1) });
            _contexto.Kits.Add(oficina);
            nuevos++;
        }

        if (!await _contexto.Kits.AnyAsync(k => k.Codigo == "KIT_TELETRABAJO"))
        {
            var teletrabajo = new Kit("KIT_TELETRABAJO", "Kit Teletrabajo");
            teletrabajo.DefinirComponentes(new[] { ("KIT_OFICINA", 1), ("P03", 1) });
            _contexto.Kits.Add(teletrabajo);
            nuevos++;
        }
        return nuevos;
    }

    private async Task<int> SembrarClientesAsync()
    {
        var datos = new[]
        {
            new Cliente("Ana Torres", "1700000001", "ana.torres@correo.test", "0990000001", CategoriaCliente.Vip, CanalAviso.Email),
            new Cliente("Luis Mena", "1700000002", "luis.mena@correo.test", "0990000002", CategoriaCliente.Normal, CanalAviso.Sms),
            new Cliente("Carmen Ruiz", "1700000003", "carmen.ruiz@correo.test", "0990000003", CategoriaCliente.Mayorista, CanalAviso.Email)
        };

        var nuevos = 0;
        foreach (var c in datos)
        {
            if (await _contexto.Clientes.AnyAsync(x => x.Identificacion == c.Identificacion))
                continue;
            _contexto.Clientes.Add(c);
            nuevos++;
        }
        return nuevos;
    }
}
