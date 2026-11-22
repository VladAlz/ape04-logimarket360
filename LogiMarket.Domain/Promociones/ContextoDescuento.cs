using LogiMarket.Domain.Clientes;

namespace LogiMarket.Domain.Promociones;

public sealed record ContextoDescuento(CategoriaCliente Categoria, int UnidadesFisicas, decimal Subtotal);
