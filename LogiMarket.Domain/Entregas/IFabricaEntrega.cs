using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Domain.Entregas;

public interface ITarifaEntrega
{
    FormaEntrega Familia { get; }

    decimal Calcular();
}

public interface IPlazoEntrega
{
    FormaEntrega Familia { get; }

    int Dias { get; }

    DateTime Estimar(DateTime desde);
}

public interface IGeneradorGuia
{
    FormaEntrega Familia { get; }

    GuiaEntrega Generar(int pedidoId, string cliente, DateTime fecha, IPlazoEntrega plazo);
}

// Abstract Factory: una fabrica entrega los tres productos de la misma familia
public interface IFabricaEntrega
{
    FormaEntrega Forma { get; }

    ITarifaEntrega CrearTarifa();

    IPlazoEntrega CrearPlazo();

    IGeneradorGuia CrearGuia();
}
