using LogiMarket.Domain.Clientes;

namespace LogiMarket.Domain.Promociones;

public sealed class Vip10 : IPoliticaDescuento
{
    public string Nombre => "VIP";
    public int Prioridad => 10;
    public decimal Tasa => 0.10m;

    public bool Aplica(ContextoDescuento contexto) => contexto.Categoria == CategoriaCliente.Vip;
}
