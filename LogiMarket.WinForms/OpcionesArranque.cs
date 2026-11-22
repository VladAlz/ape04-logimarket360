namespace LogiMarket.WinForms;

internal sealed record OpcionesArranque(bool ReiniciarDatos, string CarpetaDatos)
{
    public const string ArgumentoReiniciar = "--reiniciar-datos";

    // Los datos quedan fuera de bin para que sobrevivan a una recompilacion
    public static OpcionesArranque Desde(string[] args)
    {
        var carpeta = args.FirstOrDefault(a => a.StartsWith("--datos="))?["--datos=".Length..]
            ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LogiMarket360");

        return new OpcionesArranque(args.Contains(ArgumentoReiniciar), carpeta);
    }
}
