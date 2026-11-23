namespace LogiMarket.Application.Comprobantes;

public sealed record DocumentoTexto(string Titulo, string NombreArchivo, IReadOnlyList<string> Lineas)
{
    public string Texto => string.Join(Environment.NewLine, Lineas);
}

public sealed record SalidaEmitida(string Medio, string Destino, string Contenido);

public static class MediosSalida
{
    public const string VistaPrevia = "VISTA PREVIA";
    public const string ArchivoTxt = "ARCHIVO TXT";
}

public enum TipoComprobante
{
    Venta,
    Devolucion
}

public static class TiposComprobante
{
    public static string Nombre(this TipoComprobante tipo) => tipo == TipoComprobante.Venta ? "Venta" : "Devolución";
}
