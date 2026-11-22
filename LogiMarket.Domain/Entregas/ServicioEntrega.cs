using LogiMarket.Domain.Comun;
using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Domain.Entregas;

// Agrupa los productos de una sola fabrica; si se mezclan familias se rechaza
public sealed class ServicioEntrega
{
    public FormaEntrega Forma { get; }
    public ITarifaEntrega Tarifa { get; }
    public IPlazoEntrega Plazo { get; }
    public IGeneradorGuia Guia { get; }

    public ServicioEntrega(IFabricaEntrega fabrica)
    {
        Forma = fabrica.Forma;
        Tarifa = fabrica.CrearTarifa();
        Plazo = fabrica.CrearPlazo();
        Guia = fabrica.CrearGuia();

        if (Tarifa.Familia != Forma || Plazo.Familia != Forma || Guia.Familia != Forma)
            throw new ReglaNegocioException($"La fábrica {Forma} produjo objetos de otra familia.");
    }

    public static ServicioEntrega Para(FormaEntrega forma, IEnumerable<IFabricaEntrega> fabricas) =>
        new(fabricas.FirstOrDefault(f => f.Forma == forma)
            ?? throw new ReglaNegocioException($"No hay fábrica de entrega para {forma}."));
}
