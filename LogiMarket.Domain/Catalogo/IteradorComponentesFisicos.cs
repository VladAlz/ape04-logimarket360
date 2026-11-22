namespace LogiMarket.Domain.Catalogo;

// Recorre en profundidad con una pila propia; no entrega la lista interna del kit
internal sealed class IteradorComponentesFisicos : IIteradorComponentes
{
    private readonly Stack<(IElementoCatalogo Elemento, int Cantidad)> _pendientes = new();
    private ComponenteFisico? _siguiente;

    public IteradorComponentesFisicos(IElementoCatalogo raiz, int cantidad)
    {
        _pendientes.Push((raiz, cantidad));
        Avanzar();
    }

    public bool HaySiguiente() => _siguiente is not null;

    public ComponenteFisico Siguiente()
    {
        var actual = _siguiente ?? throw new InvalidOperationException("No hay más componentes.");
        Avanzar();
        return actual;
    }

    private void Avanzar()
    {
        _siguiente = null;
        while (_pendientes.Count > 0)
        {
            var (elemento, cantidad) = _pendientes.Pop();

            if (elemento is KitCompuesto kit)
            {
                // Se apilan al reves para respetar el orden en que se declararon
                for (var i = kit.Hijos.Count - 1; i >= 0; i--)
                    _pendientes.Push((kit.Hijos[i].Elemento, cantidad * kit.Hijos[i].Cantidad));
                continue;
            }

            _siguiente = new ComponenteFisico(elemento.Codigo, elemento.Nombre, cantidad, elemento.Precio, elemento.PesoKg);
            return;
        }
    }
}
