namespace LogiMarket.Domain.Catalogo;

public interface IIteradorComponentes
{
    bool HaySiguiente();

    ComponenteFisico Siguiente();
}
