using System.Globalization;

namespace LogiMarket.WinForms.Comun;

public sealed record Columna(string Propiedad, string Titulo, string? Formato = null, float Peso = 100, bool Derecha = false);

internal static class Rejilla
{
    public static void Definir(DataGridView rejilla, params Columna[] columnas)
    {
        rejilla.AutoGenerateColumns = false;
        rejilla.Columns.Clear();
        foreach (var c in columnas)
        {
            var columna = new DataGridViewTextBoxColumn
            {
                DataPropertyName = c.Propiedad,
                HeaderText = c.Titulo,
                Name = $"col{c.Propiedad}",
                FillWeight = c.Peso
            };
            if (c.Formato is not null)
            {
                columna.DefaultCellStyle.Format = c.Formato;
                columna.DefaultCellStyle.FormatProvider = CultureInfo.InvariantCulture;
            }
            if (c.Derecha || c.Formato == "0.00")
                columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            rejilla.Columns.Add(columna);
        }

        // Los valores logicos se muestran como Si/No
        rejilla.CellFormatting -= FormatearLogico;
        rejilla.CellFormatting += FormatearLogico;
    }

    private static void FormatearLogico(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.Value is bool valor)
        {
            e.Value = valor ? "Sí" : "No";
            e.FormattingApplied = true;
        }
    }

    // Conserva la fila elegida despues de recargar y vuelve a avisar la seleccion
    public static void Cargar<T>(DataGridView rejilla, IEnumerable<T> datos, Func<T, object>? clave = null, Action? alSeleccionar = null)
    {
        var anterior = clave is not null && rejilla.CurrentRow?.DataBoundItem is T actual ? clave(actual) : null;
        var lista = datos.ToList();
        rejilla.DataSource = lista;

        if (anterior is not null && clave is not null)
        {
            var indice = lista.FindIndex(x => Equals(clave(x), anterior));
            if (indice >= 0 && indice < rejilla.Rows.Count)
            {
                rejilla.CurrentCell = rejilla.Rows[indice].Cells[0];
                rejilla.Rows[indice].Selected = true;
            }
        }

        alSeleccionar?.Invoke();
    }

    public static T? Seleccionado<T>(DataGridView rejilla) where T : class =>
        rejilla.CurrentRow?.DataBoundItem as T;
}
