namespace LogiMarket.Infrastructure.Pagos.Legado;

// Codigo heredado: trabaja en centavos y responde texto "ESTADO|dato". No se modifica.
public sealed class PasarelaLegadaSimulada
{
    private readonly Func<bool> _enMantenimiento;
    private int _secuencia = 1000;

    public PasarelaLegadaSimulada(Func<bool> enMantenimiento)
    {
        _enMantenimiento = enMantenimiento;
    }

    public int Invocaciones { get; private set; }

    public string CobrarCentavos(long centavos, string referencia)
    {
        Invocaciones++;

        if (_enMantenimiento())
            return "ERR|SERVICIO EN MANTENIMIENTO";
        if (centavos <= 0)
            return "ERR|MONTO INVALIDO";
        if (string.IsNullOrWhiteSpace(referencia))
            return "ERR|SIN REFERENCIA";

        var numero = Interlocked.Increment(ref _secuencia);
        return $"OK|LEG-{DateTime.Now:yyMMddHHmmss}-{numero}";
    }
}
