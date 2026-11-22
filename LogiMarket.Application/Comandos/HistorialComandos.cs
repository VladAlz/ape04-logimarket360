namespace LogiMarket.Application.Comandos;

public sealed record RegistroComando(DateTime Fecha, string Usuario, string Comando, string Entrada, bool Exito, string Resultado);

// Vive toda la sesion para ver los comandos desde cualquier pantalla
public sealed class HistorialComandos
{
    private readonly List<RegistroComando> _registros = new();
    private readonly object _candado = new();

    public void Agregar(RegistroComando registro)
    {
        lock (_candado)
            _registros.Add(registro);
    }

    public IReadOnlyList<RegistroComando> Listar()
    {
        lock (_candado)
            return _registros.OrderByDescending(r => r.Fecha).ToList();
    }
}
