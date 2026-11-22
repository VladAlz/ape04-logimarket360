using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Domain.Entregas;

public sealed class FabricaEntregaExpres : IFabricaEntrega
{
    public FormaEntrega Forma => FormaEntrega.Expres;

    public ITarifaEntrega CrearTarifa() => new TarifaExpres();

    public IPlazoEntrega CrearPlazo() => new PlazoExpres();

    public IGeneradorGuia CrearGuia() => new GuiaExpres();

    private sealed class TarifaExpres : ITarifaEntrega
    {
        public FormaEntrega Familia => FormaEntrega.Expres;

        public decimal Calcular() => 12.00m;
    }

    private sealed class PlazoExpres : IPlazoEntrega
    {
        public FormaEntrega Familia => FormaEntrega.Expres;
        public int Dias => 1;

        public DateTime Estimar(DateTime desde) => desde.Date.AddDays(Dias);
    }

    private sealed class GuiaExpres : IGeneradorGuia
    {
        public FormaEntrega Familia => FormaEntrega.Expres;

        public GuiaEntrega Generar(int pedidoId, string cliente, DateTime fecha, IPlazoEntrega plazo)
        {
            var estimada = plazo.Estimar(fecha);
            return new GuiaEntrega($"EXP-{fecha:yyyyMMdd}-{pedidoId:0000}", Familia,
                $"ENVÍO EXPRÉS PRIORITARIO | Pedido {pedidoId} | {cliente} | Llega {estimada:dd/MM/yyyy}", estimada);
        }
    }
}
