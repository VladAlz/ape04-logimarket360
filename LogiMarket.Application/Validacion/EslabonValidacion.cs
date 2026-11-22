namespace LogiMarket.Application.Validacion;

// Chain of Responsibility: cada eslabon revisa una cosa y pasa al siguiente
public abstract class EslabonValidacion
{
    private EslabonValidacion? _siguiente;

    public abstract string Nombre { get; }

    public EslabonValidacion Enlazar(EslabonValidacion siguiente)
    {
        _siguiente = siguiente;
        return siguiente;
    }

    public ResultadoValidacion Validar(ContextoValidacion contexto, List<string> recorrido)
    {
        recorrido.Add(Nombre);

        var error = Revisar(contexto);
        if (error is not null)
            return new ResultadoValidacion(false, Nombre, error, recorrido);

        return _siguiente is null
            ? new ResultadoValidacion(true, Nombre, "Validación completa.", recorrido)
            : _siguiente.Validar(contexto, recorrido);
    }

    // null si este eslabon no encontro problema
    protected abstract string? Revisar(ContextoValidacion contexto);
}
