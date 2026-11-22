using LogiMarket.Application.Puertos;

namespace LogiMarket.Tests.Soporte;

public sealed class RelojPrueba : IReloj
{
    public RelojPrueba(DateTime inicio)
    {
        Ahora = inicio;
    }

    public DateTime Ahora { get; set; }
}
