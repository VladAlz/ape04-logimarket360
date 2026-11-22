using System.Globalization;

namespace LogiMarket.WinForms.Comun;

internal static class Texto
{
    // Acepta 20.5 o 20,5 sin depender de la configuracion regional
    public static bool IntentarDecimal(string texto, out decimal valor) =>
        decimal.TryParse(texto.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out valor);

    // Muestra un valor con otro texto sin cambiar el elemento enlazado
    public static void Mostrar<T>(ComboBox combo, Func<T, string> texto) =>
        combo.Format += (_, e) => { if (e.ListItem is T valor) e.Value = texto(valor); };

    public static string Lineas(IEnumerable<string> lineas) => string.Join(Environment.NewLine, lineas);
}
