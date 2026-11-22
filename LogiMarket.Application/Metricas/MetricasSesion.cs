using System.Collections.Concurrent;

namespace LogiMarket.Application.Metricas;

// Singleton registrado en DI; la base de datos sigue siendo la fuente de verdad
public sealed class MetricasSesion
{
    public const string PedidosPagados = "Pedidos pagados";
    public const string CobrosRechazados = "Cobros rechazados";
    public const string Despachos = "Despachos";
    public const string Entregas = "Entregas";
    public const string Devoluciones = "Devoluciones aceptadas";
    public const string AvisosEnviados = "Avisos enviados";
    public const string AccesosDenegados = "Accesos denegados";
    public const string ComandosEjecutados = "Comandos ejecutados";

    private readonly ConcurrentDictionary<string, int> _contadores = new();

    public Guid Instancia { get; } = Guid.NewGuid();
    public DateTime Inicio { get; } = DateTime.Now;

    public MetricasSesion()
    {
        foreach (var nombre in new[] { PedidosPagados, CobrosRechazados, Despachos, Entregas, Devoluciones, AvisosEnviados, AccesosDenegados, ComandosEjecutados })
            _contadores[nombre] = 0;
    }

    // AddOrUpdate es atomico, varios hilos pueden sumar a la vez
    public void Incrementar(string nombre) => _contadores.AddOrUpdate(nombre, 1, (_, actual) => actual + 1);

    public int Valor(string nombre) => _contadores.TryGetValue(nombre, out var valor) ? valor : 0;

    public IReadOnlyList<KeyValuePair<string, int>> Instantanea() =>
        _contadores.OrderBy(c => c.Key).ToList();
}
