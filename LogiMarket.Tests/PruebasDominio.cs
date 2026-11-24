using LogiMarket.Application.Consultas.Filtros;
using LogiMarket.Application.Pedidos;
using LogiMarket.Application.Seguridad;
using LogiMarket.Application.Validacion;
using LogiMarket.Domain.Catalogo;
using LogiMarket.Domain.Catalogo.Visitantes;
using LogiMarket.Domain.Clientes;
using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Entregas;
using LogiMarket.Domain.Pedidos;
using LogiMarket.Domain.Promociones;
using LogiMarket.Domain.Seguridad;
using LogiMarket.Tests.Soporte;

namespace LogiMarket.Tests;

// Pruebas unitarias del nucleo, sin base de datos
public class PruebasDominio
{
    private static readonly ArmadorCatalogo Catalogo = Fabricas.Catalogo();

    [Fact]
    public void R03_CuatroKitsAlcanzanElDescuentoMayorista()
    {
        var pedido = PedidoDe(CategoriaCliente.Normal, FormaEntrega.Estandar,
            ("KIT_TELETRABAJO", 4));

        var valoracion = Fabricas.Calculadora().Valorar(pedido);

        Assert.Equal(209.00m, valoracion.Total);
        Assert.Equal("MAYORISTA", valoracion.Politica);
        Assert.Equal(36.00m, valoracion.Descuento);
    }

    [Fact]
    public void R04_SumaTecladosDelKitYDeLaLineaSuelta()
    {
        var pedido = PedidoDe(CategoriaCliente.Normal, FormaEntrega.Estandar,
            ("KIT_OFICINA", 2), ("P02", 4));

        var requerido = pedido.RequerimientoStock();

        Assert.Equal(6, requerido["P02"]);
        Assert.Equal(2, requerido["P01"]);
    }

    private static Pedido PedidoDe(CategoriaCliente categoria, FormaEntrega entrega, params (string Codigo, int Cantidad)[] lineas)
    {
        var builder = new PedidoBuilder().ParaCliente(Fabricas.Cliente("Prueba", categoria)).ConEntrega(entrega);
        foreach (var (codigo, cantidad) in lineas)
            builder.Agregar(Catalogo.Obtener(codigo), cantidad);
        return builder.Construir();
    }

    [Fact(DisplayName = "P18 Composite: KIT_OFICINA = 50.00 y KIT_TELETRABAJO = 60.00")]
    public void Composite_PreciosRecursivos()
    {
        Assert.Equal(50.00m, Catalogo.Obtener("KIT_OFICINA").Precio);
        Assert.Equal(60.00m, Catalogo.Obtener("KIT_TELETRABAJO").Precio);
        Assert.True(Catalogo.Obtener("KIT_TELETRABAJO").Contiene("P02"));
    }

    [Fact(DisplayName = "P18 Composite: se rechaza un kit que se contiene directa o indirectamente")]
    public void Composite_DetectaCiclos()
    {
        var a = new KitCompuesto("KIT_A", "A");
        var b = new KitCompuesto("KIT_B", "B");
        b.Agregar(a, 1);
        var ex = Assert.Throws<ReglaNegocioException>(() => a.Agregar(b, 1));
        Assert.Contains("circular", ex.Message);

        var kitA = new Kit("KIT_A", "A");
        kitA.DefinirComponentes(new[] { ("KIT_B", 1) });
        var kitB = new Kit("KIT_B", "B");
        kitB.DefinirComponentes(new[] { ("KIT_A", 1) });
        var armador = new ArmadorCatalogo(Array.Empty<Producto>(), new[] { kitA, kitB });
        Assert.Throws<ReglaNegocioException>(() => armador.Obtener("KIT_A"));

        var directo = new Kit("KIT_C", "C");
        Assert.Throws<ReglaNegocioException>(() => directo.DefinirComponentes(new[] { ("KIT_C", 1) }));
    }

    [Fact(DisplayName = "P27 Iterator: KIT_TELETRABAJO entrega P01, P02 y P03 en orden y termina")]
    public void Iterator_RecorreKitAnidado()
    {
        var iterador = Catalogo.Obtener("KIT_TELETRABAJO").CrearIterador(2);
        var recorrido = new List<ComponenteFisico>();
        while (iterador.HaySiguiente())
            recorrido.Add(iterador.Siguiente());

        Assert.Equal(new[] { "P01", "P02", "P03" }, recorrido.Select(c => c.Sku));
        Assert.All(recorrido, c => Assert.Equal(2, c.Cantidad));
        Assert.False(iterador.HaySiguiente());
        Assert.Throws<InvalidOperationException>(() => iterador.Siguiente());
    }

    [Fact(DisplayName = "P34 Visitor: desglose P01/P02/P03 y peso total 1.00 kg")]
    public void Visitor_DesgloseYPeso()
    {
        var kit = Catalogo.Obtener("KIT_TELETRABAJO");
        var desglose = new VisitanteDesgloseFisico();
        kit.Aceptar(desglose, 1);
        var peso = new VisitantePesoLogistico();
        kit.Aceptar(peso, 1);

        Assert.Equal(new[] { "P01", "P02", "P03" }, desglose.Lineas.Select(l => l.Sku));
        Assert.Equal(new[] { "KIT_TELETRABAJO x1", "KIT_OFICINA x1" }, desglose.KitsVisitados);
        Assert.Equal(1.00m, peso.TotalKg);
        Assert.Equal(2, peso.NivelesKit);
    }

    [Fact(DisplayName = "P32 Strategy: Ana 6.00, Carmen 15.00 y sin descuento 0.00")]
    public void Strategy_DescuentosOficiales()
    {
        var calculadora = Fabricas.Calculadora();

        var ana = calculadora.Valorar(PedidoDe(CategoriaCliente.Vip, FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1)));
        var luis = calculadora.Valorar(PedidoDe(CategoriaCliente.Normal, FormaEntrega.Expres, ("P04", 2), ("P05", 2)));
        var carmen = calculadora.Valorar(PedidoDe(CategoriaCliente.Mayorista, FormaEntrega.Estandar, ("P03", 10)));

        Assert.Equal((6.00m, "VIP", 59.00m), (ana.Descuento, ana.Politica, ana.Total));
        Assert.Equal((0.00m, "NORMAL", 102.00m), (luis.Descuento, luis.Politica, luis.Total));
        Assert.Equal((15.00m, "MAYORISTA", 90.00m), (carmen.Descuento, carmen.Politica, carmen.Total));
    }

    [Fact(DisplayName = "P32 Strategy: MAYORISTA gana a VIP y no se acumulan")]
    public void Strategy_PrioridadSinAcumular()
    {
        var valoracion = Fabricas.Calculadora().Valorar(PedidoDe(CategoriaCliente.Vip, FormaEntrega.Estandar, ("P03", 10)));
        Assert.Equal("MAYORISTA", valoracion.Politica);
        Assert.Equal(15.00m, valoracion.Descuento);
    }

    private sealed class PromocionAniversario : IPoliticaDescuento
    {
        public string Nombre => "ANIVERSARIO";
        public int Prioridad => 30;
        public decimal Tasa => 0.20m;
        public bool Aplica(ContextoDescuento contexto) => contexto.Subtotal >= 100m;
    }

    [Fact(DisplayName = "P32 Strategy: una politica nueva se agrega sin editar la calculadora")]
    public void Strategy_NuevaPolitica()
    {
        var valoracion = Fabricas.Calculadora(new PromocionAniversario())
            .Valorar(PedidoDe(CategoriaCliente.Normal, FormaEntrega.Estandar, ("P04", 3)));
        Assert.Equal("ANIVERSARIO", valoracion.Politica);
        Assert.Equal(24.00m, valoracion.Descuento);
    }

    [Fact(DisplayName = "P12 Abstract Factory: ESTANDAR 5.00/3 dias y EXPRES 12.00/1 dia con guias distinguibles")]
    public void AbstractFactory_Familias()
    {
        var fecha = new DateTime(2026, 11, 2);
        var estandar = new ServicioEntrega(new FabricaEntregaEstandar());
        var expres = new ServicioEntrega(new FabricaEntregaExpres());

        Assert.Equal((5.00m, 3), (estandar.Tarifa.Calcular(), estandar.Plazo.Dias));
        Assert.Equal((12.00m, 1), (expres.Tarifa.Calcular(), expres.Plazo.Dias));

        var guiaEstandar = estandar.Guia.Generar(7, "Ana", fecha, estandar.Plazo);
        var guiaExpres = expres.Guia.Generar(7, "Ana", fecha, expres.Plazo);
        Assert.StartsWith("STD-", guiaEstandar.Numero);
        Assert.StartsWith("EXP-", guiaExpres.Numero);
        Assert.Equal(fecha.AddDays(3), guiaEstandar.FechaEstimada);
        Assert.Equal(fecha.AddDays(1), guiaExpres.FechaEstimada);
    }

    [Fact(DisplayName = "P13 Builder: bloquea pedido sin cliente o sin lineas y arma varios renglones")]
    public void Builder_PedidoValido()
    {
        Assert.Throws<ReglaNegocioException>(() => new PedidoBuilder().Agregar(Catalogo.Obtener("P01"), 1).Construir());
        Assert.Throws<ReglaNegocioException>(() => new PedidoBuilder().ParaCliente(Fabricas.Cliente("Ana", CategoriaCliente.Vip)).Construir());

        var pedido = PedidoDe(CategoriaCliente.Normal, FormaEntrega.Expres, ("P04", 2), ("P05", 2), ("KIT_OFICINA", 1));
        Assert.Equal(3, pedido.Lineas.Count);
        Assert.Equal("BORRADOR", pedido.EstadoCodigo);
        Assert.Equal(new[] { "P01", "P02" }, pedido.Lineas.Single(l => l.Codigo == "KIT_OFICINA").Componentes.Select(c => c.Sku));
    }

    [Fact(DisplayName = "P14 Prototype: la copia es profunda y la plantilla no cambia")]
    public void Prototype_CopiaProfunda()
    {
        var plantilla = PedidoDe(CategoriaCliente.Vip, FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1));
        var copia = plantilla.Clonar();
        var otra = plantilla.Clonar();

        copia.CambiarCantidad("KIT_TELETRABAJO", 4);

        Assert.Equal(1, plantilla.Lineas[0].Cantidad);
        Assert.Equal(1, otra.Lineas[0].Cantidad);
        Assert.Equal(4, copia.Lineas[0].Cantidad);
        Assert.NotSame(plantilla.Lineas, copia.Lineas);
        Assert.NotSame(plantilla.Lineas[0], copia.Lineas[0]);
        Assert.NotSame(plantilla.Lineas[0].Componentes, copia.Lineas[0].Componentes);
        Assert.Equal(0, copia.Id);
        Assert.Null(copia.FechaPago);
    }

    [Fact(DisplayName = "P31 State: BORRADOR -> VALIDADO -> PAGADO -> DESPACHADO -> ENTREGADO")]
    public void State_CicloValido()
    {
        var pedido = PedidoDe(CategoriaCliente.Vip, FormaEntrega.Estandar, ("KIT_TELETRABAJO", 1));
        var hoy = new DateTime(2026, 11, 2);

        pedido.Validar(Fabricas.Calculadora().Valorar(pedido), hoy, "ventas");
        pedido.MarcarPagado(hoy, "ventas", "REF-1");
        pedido.Despachar(hoy, "bodega", "STD-1");
        pedido.Entregar(hoy.AddDays(3), "bodega");

        Assert.Equal("ENTREGADO", pedido.EstadoCodigo);
        Assert.Equal(new[] { "BORRADOR", "VALIDADO", "PAGADO", "DESPACHADO", "ENTREGADO" }, pedido.Historial.Select(h => h.EstadoNuevo));
    }

    [Fact(DisplayName = "P31 State: rechaza PAGADO -> CANCELADO, estados repetidos y despacho sin pago")]
    public void State_TransicionesInvalidas()
    {
        var pedido = PedidoDe(CategoriaCliente.Normal, FormaEntrega.Estandar, ("P01", 1));
        var hoy = new DateTime(2026, 11, 2);
        pedido.Validar(Fabricas.Calculadora().Valorar(pedido), hoy, "ventas");

        Assert.Throws<ReglaNegocioException>(() => pedido.Despachar(hoy, "bodega", "X"));
        Assert.Throws<ReglaNegocioException>(() => pedido.Validar(Fabricas.Calculadora().Valorar(pedido), hoy, "ventas"));

        pedido.MarcarPagado(hoy, "ventas", "REF");
        Assert.Throws<ReglaNegocioException>(() => pedido.Cancelar(hoy, "ventas", "x"));
        Assert.Throws<ReglaNegocioException>(() => pedido.MarcarPagado(hoy, "ventas", "REF2"));
        Assert.Throws<ReglaNegocioException>(() => pedido.AgregarElemento(Catalogo.Obtener("P05"), 1));
        Assert.Equal("PAGADO", pedido.EstadoCodigo);
    }

    [Fact(DisplayName = "P29 Memento: tres cambios, dos deshacer y un rehacer restauran valores exactos")]
    public void Memento_DeshacerRehacer()
    {
        var historial = new HistorialEdiciones();
        var pedido = PedidoDe(CategoriaCliente.Vip, FormaEntrega.Estandar, ("P01", 1));
        var hoy = DateTime.Now;

        historial.Registrar(pedido.Id, pedido.CrearMemento(hoy, "1"));
        pedido.AgregarElemento(Catalogo.Obtener("P02"), 1);
        historial.Registrar(pedido.Id, pedido.CrearMemento(hoy, "2"));
        pedido.CambiarCantidad("P01", 3);
        historial.Registrar(pedido.Id, pedido.CrearMemento(hoy, "3"));
        pedido.AgregarElemento(Catalogo.Obtener("KIT_OFICINA"), 1);

        pedido.Restaurar(historial.Deshacer(pedido.Id, pedido.CrearMemento(hoy, "actual"))!);
        pedido.Restaurar(historial.Deshacer(pedido.Id, pedido.CrearMemento(hoy, "actual"))!);
        Assert.Equal(new[] { ("P01", 1), ("P02", 1) }, pedido.Lineas.Select(l => (l.Codigo, l.Cantidad)));

        pedido.Restaurar(historial.Rehacer(pedido.Id, pedido.CrearMemento(hoy, "actual"))!);
        Assert.Equal(new[] { ("P01", 3), ("P02", 1) }, pedido.Lineas.Select(l => (l.Codigo, l.Cantidad)));
    }

    [Fact(DisplayName = "P23 Chain: el primer error detiene la cadena; al corregirlo aparece el siguiente")]
    public void Chain_OrdenDeEslabones()
    {
        var cadena = CadenaValidacionPedido.Construir();
        var pedido = PedidoDe(CategoriaCliente.Vip, FormaEntrega.Estandar, ("KIT_TELETRABAJO", 6));
        var stock = new Dictionary<string, int> { ["P01"] = 8, ["P02"] = 5, ["P03"] = 20 };

        var sinCliente = cadena.Validar(new ContextoValidacion(pedido, null, stock, Operacion.ValidarPedido, true, Rol.Ventas), new List<string>());
        Assert.False(sinCliente.Aprobado);
        Assert.Equal("Cliente activo", sinCliente.Eslabon);
        Assert.Single(sinCliente.Recorrido);

        var cliente = Fabricas.Cliente("Ana", CategoriaCliente.Vip);
        var sinStock = cadena.Validar(new ContextoValidacion(pedido, cliente, stock, Operacion.ValidarPedido, true, Rol.Ventas), new List<string>());
        Assert.Equal("Reglas de stock", sinStock.Eslabon);
        Assert.Contains("P02", sinStock.Mensaje);
        Assert.Equal(3, sinStock.Recorrido.Count);

        var consulta = cadena.Validar(new ContextoValidacion(PedidoDe(CategoriaCliente.Vip, FormaEntrega.Estandar, ("P01", 1)), cliente, stock,
            Operacion.ValidarPedido, true, Rol.Consulta), new List<string>());
        Assert.Equal("Autorización y estado", consulta.Eslabon);
    }

    private static readonly PedidoResumen[] Pedidos =
    {
        new(1, "Ana Torres", "VIP", "PAGADO", "ESTÁNDAR", 3, 60, "VIP", 6, 5, 59, DateTime.Today, false),
        new(2, "Luis Mena", "NORMAL", "PAGADO", "EXPRÉS", 4, 90, "NORMAL", 0, 12, 102, DateTime.Today, false),
        new(3, "Carmen Ruiz", "MAYORISTA", "VALIDADO", "ESTÁNDAR", 10, 100, "MAYORISTA", 15, 5, 90, DateTime.Today, false),
        new(4, "Ana Torres", "VIP", "BORRADOR", "ESTÁNDAR", 1, 20, "VIP", 2, 5, 23, DateTime.Today, false)
    };

    [Fact(DisplayName = "P25 Interpreter: ESTADO = PAGADO AND TOTAL >= 50")]
    public void Interpreter_FiltroOficial()
    {
        var filtro = new AnalizadorFiltro("ESTADO = PAGADO AND TOTAL >= 50").Analizar();
        Assert.Equal(new[] { 1, 2 }, Pedidos.Where(filtro.Evaluar).Select(p => p.Id));

        var cliente = new AnalizadorFiltro("CLIENTE = 'Ana Torres' AND TOTAL < 50").Analizar();
        Assert.Equal(new[] { 4 }, Pedidos.Where(cliente.Evaluar).Select(p => p.Id));
    }

    [Fact(DisplayName = "P26 Interpreter: parentesis, precedencia AND sobre OR y errores de sintaxis")]
    public void Interpreter_PrecedenciaYErrores()
    {
        var sinParentesis = new AnalizadorFiltro("ESTADO = BORRADOR OR ESTADO = PAGADO AND TOTAL >= 100").Analizar();
        Assert.Equal(new[] { 2, 4 }, Pedidos.Where(sinParentesis.Evaluar).Select(p => p.Id));

        var conParentesis = new AnalizadorFiltro("(ESTADO = BORRADOR OR ESTADO = PAGADO) AND TOTAL >= 100").Analizar();
        Assert.Equal(new[] { 2 }, Pedidos.Where(conParentesis.Evaluar).Select(p => p.Id));

        Assert.Throws<ErrorFiltroException>(() => new AnalizadorFiltro("ESTADO = PAGADO AND").Analizar());
        Assert.Throws<ErrorFiltroException>(() => new AnalizadorFiltro("(ESTADO = PAGADO").Analizar());
        Assert.Throws<ErrorFiltroException>(() => new AnalizadorFiltro("TOTAL >= cincuenta").Analizar());
        Assert.Throws<ErrorFiltroException>(() => new AnalizadorFiltro("ESTADO = PAGADO; DROP TABLE Pedido").Analizar());
    }
}
