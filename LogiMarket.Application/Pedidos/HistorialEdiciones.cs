using LogiMarket.Domain.Pedidos;

namespace LogiMarket.Application.Pedidos;

// Cuidador del Memento: guarda hasta cinco estados por borrador
public sealed class HistorialEdiciones
{
    public const int Capacidad = 5;

    private sealed class Pilas
    {
        public LinkedList<MementoPedido> Deshacer { get; } = new();
        public Stack<MementoPedido> Rehacer { get; } = new();
    }

    private readonly Dictionary<int, Pilas> _porPedido = new();
    private readonly object _candado = new();

    public void Registrar(int pedidoId, MementoPedido antes)
    {
        lock (_candado)
        {
            var pilas = Obtener(pedidoId);
            Apilar(pilas.Deshacer, antes);
            // Un cambio nuevo invalida lo que se podia rehacer
            pilas.Rehacer.Clear();
        }
    }

    public MementoPedido? Deshacer(int pedidoId, MementoPedido actual)
    {
        lock (_candado)
        {
            var pilas = Obtener(pedidoId);
            if (pilas.Deshacer.Count == 0)
                return null;

            var anterior = pilas.Deshacer.Last!.Value;
            pilas.Deshacer.RemoveLast();
            pilas.Rehacer.Push(actual);
            return anterior;
        }
    }

    public MementoPedido? Rehacer(int pedidoId, MementoPedido actual)
    {
        lock (_candado)
        {
            var pilas = Obtener(pedidoId);
            if (pilas.Rehacer.Count == 0)
                return null;

            Apilar(pilas.Deshacer, actual);
            return pilas.Rehacer.Pop();
        }
    }

    public void Limpiar(int pedidoId)
    {
        lock (_candado)
            _porPedido.Remove(pedidoId);
    }

    public (int Deshacer, int Rehacer) Conteo(int pedidoId)
    {
        lock (_candado)
            return _porPedido.TryGetValue(pedidoId, out var p) ? (p.Deshacer.Count, p.Rehacer.Count) : (0, 0);
    }

    private Pilas Obtener(int pedidoId)
    {
        if (!_porPedido.TryGetValue(pedidoId, out var pilas))
            _porPedido[pedidoId] = pilas = new Pilas();
        return pilas;
    }

    private static void Apilar(LinkedList<MementoPedido> lista, MementoPedido memento)
    {
        lista.AddLast(memento);
        if (lista.Count > Capacidad)
            lista.RemoveFirst();
    }
}
