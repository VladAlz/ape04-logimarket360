using LogiMarket.Domain.Catalogo;
using LogiMarket.Domain.Clientes;
using LogiMarket.Domain.Entregas;
using LogiMarket.Domain.Promociones;
using LogiMarket.Domain.Valoracion;

namespace LogiMarket.Tests.Soporte;

// Objetos de dominio sin base de datos para pruebas unitarias
public static class Fabricas
{
    public static ArmadorCatalogo Catalogo()
    {
        var productos = new[]
        {
            new Producto("P01", "Mouse USB", "Periféricos", "Mouse", 20.00m, 0.20m, 8),
            new Producto("P02", "Teclado", "Periféricos", "Teclado", 30.00m, 0.70m, 5),
            new Producto("P03", "Memoria USB", "Almacenamiento", "Memoria", 10.00m, 0.10m, 20),
            new Producto("P04", "Webcam", "Video", "Webcam", 40.00m, 0.35m, 4),
            new Producto("P05", "Alfombrilla", "Accesorios", "Alfombrilla", 5.00m, 0.10m, 30)
        };
        var oficina = new Kit("KIT_OFICINA", "Kit Oficina");
        oficina.DefinirComponentes(new[] { ("P01", 1), ("P02", 1) });
        var teletrabajo = new Kit("KIT_TELETRABAJO", "Kit Teletrabajo");
        teletrabajo.DefinirComponentes(new[] { ("KIT_OFICINA", 1), ("P03", 1) });
        return new ArmadorCatalogo(productos, new[] { oficina, teletrabajo });
    }

    public static Cliente Cliente(string nombre, CategoriaCliente categoria) =>
        new(nombre, "17" + Math.Abs(nombre.GetHashCode() % 100000000).ToString("00000000"), $"{nombre.ToLowerInvariant()}@correo.test", "0990000000", categoria, CanalAviso.Email);

    public static CalculadoraPedido Calculadora(params IPoliticaDescuento[] extra) =>
        new(new SelectorPoliticaDescuento(new IPoliticaDescuento[] { new SinDescuento(), new Vip10(), new Mayorista15() }.Concat(extra)),
            new IFabricaEntrega[] { new FabricaEntregaEstandar(), new FabricaEntregaExpres() });
}
