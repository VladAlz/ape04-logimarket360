using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Domain.Entregas;

public sealed class FabricaEntregaEstandar : IFabricaEntrega
{
    public FormaEntrega Forma => FormaEntrega.Estandar;

    public ITarifaEntrega CrearTarifa() => new TarifaEstandar();

    public IPlazoEntrega CrearPlazo() => new PlazoEstandar();

    public IGeneradorGuia CrearGuia() => new GuiaEstandar();

    private sealed class TarifaEstandar : ITarifaEntrega
    {
        public FormaEntrega Familia => FormaEntrega.Estandar;

        public decimal Calcular() => 5.00m;
    }

    private sealed class PlazoEstandar : IPlazoEntrega
    {
        public FormaEntrega Familia => FormaEntrega.Estandar;
        public int Dias => 3;

        public DateTime Estimar(DateTime desde) => desde.Date.AddDays(Dias);
    }

    private sealed class GuiaEstandar : IGeneradorGuia
    {
        public FormaEntrega Familia => FormaEntrega.Estandar;

        public GuiaEntrega Generar(int pedidoId, string cliente, DateTime fecha, IPlazoEntrega plazo)
        {
            var estimada = plazo.Estimar(fecha);
            return new GuiaEntrega($"STD-{fecha:yyyyMMdd}-{pedidoId:0000}", Familia,
                $"ENVÍO ESTÁNDAR | Pedido {pedidoId} | {cliente} | Llega {estimada:dd/MM/yyyy}", estimada);
        }
    }
}
