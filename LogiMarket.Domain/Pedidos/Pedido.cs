using System.Globalization;
using LogiMarket.Domain.Catalogo;
using LogiMarket.Domain.Clientes;
using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Pedidos.Estados;
using LogiMarket.Domain.Valoracion;

namespace LogiMarket.Domain.Pedidos;

public class Pedido : IPrototipo<Pedido>
{
    private readonly List<PedidoDetalle> _lineas = new();
    private readonly List<PedidoHistorial> _historial = new();

    public int Id { get; private set; }
    public int ClienteId { get; private set; }
    public string ClienteNombre { get; private set; } = string.Empty;
    public CategoriaCliente CategoriaCliente { get; private set; }
    public FormaEntrega Entrega { get; private set; }
    public string EstadoCodigo { get; private set; } = EstadosPedido.Borrador.Codigo;
    public decimal Subtotal { get; private set; }
    public string Politica { get; private set; } = string.Empty;
    public decimal Descuento { get; private set; }
    public decimal Tarifa { get; private set; }
    public decimal Total { get; private set; }
    public int PlazoDias { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaPago { get; private set; }
    public DateTime? FechaEntrega { get; private set; }
    public bool Devuelto { get; private set; }
    public int Version { get; private set; }

    public IReadOnlyList<PedidoDetalle> Lineas => _lineas;
    public IReadOnlyList<PedidoHistorial> Historial => _historial;

    public EstadoPedido Estado => EstadosPedido.Desde(EstadoCodigo);
    public int UnidadesFisicas => _lineas.Sum(l => l.UnidadesFisicas);

    // Importe de productos sin la tarifa, base del reembolso
    public decimal ImporteProductos => Subtotal - Descuento;

    private Pedido()
    {
    }

    internal Pedido(Cliente cliente, FormaEntrega entrega, IEnumerable<PedidoDetalle> lineas, DateTime fecha, string usuario)
    {
        AsignarCliente(cliente);
        Entrega = entrega;
        _lineas.AddRange(lineas);
        FechaCreacion = fecha;
        _historial.Add(new PedidoHistorial("-", EstadoCodigo, fecha, usuario, "Borrador creado."));
    }

    private Pedido(Pedido plantilla)
    {
        ClienteId = plantilla.ClienteId;
        ClienteNombre = plantilla.ClienteNombre;
        CategoriaCliente = plantilla.CategoriaCliente;
        Entrega = plantilla.Entrega;
        FechaCreacion = plantilla.FechaCreacion;
        _lineas.AddRange(plantilla._lineas.Select(l => l.Clonar()));
    }

    public void AgregarElemento(IElementoCatalogo elemento, int cantidad)
    {
        ExigirEdicion();
        var existente = _lineas.FirstOrDefault(l => l.Codigo == elemento.Codigo);
        if (existente is null)
            _lineas.Add(new PedidoDetalle(elemento, cantidad));
        else
            existente.CambiarCantidad(existente.Cantidad + cantidad);
    }

    public void CambiarCantidad(string codigo, int cantidad)
    {
        ExigirEdicion();
        BuscarLinea(codigo).CambiarCantidad(cantidad);
    }

    public void QuitarLinea(string codigo)
    {
        ExigirEdicion();
        _lineas.Remove(BuscarLinea(codigo));
    }

    public void CambiarEntrega(FormaEntrega entrega)
    {
        ExigirEdicion();
        Entrega = entrega;
    }

    public void CambiarCliente(Cliente cliente)
    {
        ExigirEdicion();
        AsignarCliente(cliente);
    }

    public void AplicarValoracion(ValoracionPedido valoracion)
    {
        ExigirEdicion();
        Subtotal = valoracion.Subtotal;
        Politica = valoracion.Politica;
        Descuento = valoracion.Descuento;
        Tarifa = valoracion.Tarifa;
        Total = valoracion.Total;
        PlazoDias = valoracion.PlazoDias;
    }

    public void Validar(ValoracionPedido valoracion, DateTime fecha, string usuario)
    {
        if (_lineas.Count == 0)
            throw new ReglaNegocioException("El pedido no tiene líneas.");

        AplicarValoracion(valoracion);
        Transicion(Estado.Validar(), fecha, usuario,
            $"Total {Total.ToString("0.00", CultureInfo.InvariantCulture)} con política {Politica}.");
    }

    public void MarcarPagado(DateTime fecha, string usuario, string referenciaPago)
    {
        Transicion(Estado.Pagar(), fecha, usuario, $"Cobro aprobado {referenciaPago}.");
        FechaPago = fecha;
    }

    public void Despachar(DateTime fecha, string usuario, string numeroGuia) =>
        Transicion(Estado.Despachar(), fecha, usuario, $"Guía {numeroGuia}.");

    public void Entregar(DateTime fecha, string usuario)
    {
        Transicion(Estado.Entregar(), fecha, usuario, "Entrega confirmada.");
        FechaEntrega = fecha;
    }

    public void Cancelar(DateTime fecha, string usuario, string motivo) =>
        Transicion(Estado.Cancelar(), fecha, usuario, string.IsNullOrWhiteSpace(motivo) ? "Cancelado." : motivo);

    public void RegistrarDevolucion(DateTime fecha, string usuario, string detalle)
    {
        if (Estado != EstadosPedido.Entregado)
            throw new ReglaNegocioException($"Solo se devuelve un pedido ENTREGADO; el pedido {Id} está {EstadoCodigo}.");
        if (Devuelto)
            throw new ReglaNegocioException($"El pedido {Id} ya tiene una devolución registrada.");

        Devuelto = true;
        Version++;
        _historial.Add(new PedidoHistorial(EstadoCodigo, EstadoCodigo, fecha, usuario, detalle));
    }

    // Requerimiento por SKU final segun las instantaneas de cada linea
    public IReadOnlyDictionary<string, int> RequerimientoStock()
    {
        var cantidades = new Dictionary<string, int>();
        foreach (var linea in _lineas)
        {
            foreach (var componente in linea.Componentes)
            {
                var sku = componente.Sku;
                var cantidad = componente.CantidadPorUnidad * linea.Cantidad;

                if (cantidades.TryGetValue(sku, out var anterior))
                    cantidades[sku] = Math.Max(anterior, cantidad);
                else
                    cantidades.Add(sku, cantidad);
            }
        }

        return cantidades;
    }

    public MementoPedido CrearMemento(DateTime fecha, string descripcion)
    {
        ExigirEdicion();
        return new MementoPedido(this, fecha, descripcion);
    }

    public void Restaurar(MementoPedido memento)
    {
        ExigirEdicion();
        if (memento.PedidoId != Id)
            throw new ReglaNegocioException("El estado guardado pertenece a otro pedido.");

        ClienteId = memento.ClienteId;
        ClienteNombre = memento.ClienteNombre;
        CategoriaCliente = memento.Categoria;
        Entrega = memento.Entrega;
        _lineas.Clear();
        // Se clona otra vez para que el memento siga sirviendo en un rehacer
        _lineas.AddRange(memento.Lineas.Select(l => l.Clonar()));
    }

    // Prototype: copia profunda sin Id, sin pago ni historial
    public Pedido Clonar()
    {
        ExigirEdicion();
        return new Pedido(this);
    }

    public void AnotarOrigen(DateTime fecha, string usuario, string detalle)
    {
        if (Id != 0 || _historial.Count > 0)
            throw new ReglaNegocioException("El origen solo se anota en una copia nueva.");

        FechaCreacion = fecha;
        _historial.Add(new PedidoHistorial("-", EstadoCodigo, fecha, usuario, detalle));
    }

    private void Transicion(EstadoPedido nuevo, DateTime fecha, string usuario, string detalle)
    {
        var anterior = EstadoCodigo;
        EstadoCodigo = nuevo.Codigo;
        Version++;
        _historial.Add(new PedidoHistorial(anterior, nuevo.Codigo, fecha, usuario, detalle));
    }

    private void AsignarCliente(Cliente cliente)
    {
        if (!cliente.Activo)
            throw new ReglaNegocioException($"El cliente {cliente.Nombre} está inactivo.");

        ClienteId = cliente.Id;
        ClienteNombre = cliente.Nombre;
        CategoriaCliente = cliente.Categoria;
    }

    private PedidoDetalle BuscarLinea(string codigo) =>
        _lineas.FirstOrDefault(l => l.Codigo == codigo)
        ?? throw new ReglaNegocioException($"El pedido no tiene una línea {codigo}.");

    private void ExigirEdicion()
    {
        if (!Estado.PermiteEdicion)
            throw new ReglaNegocioException($"El pedido {Id} está {EstadoCodigo} y ya no se puede editar.");
    }
}
