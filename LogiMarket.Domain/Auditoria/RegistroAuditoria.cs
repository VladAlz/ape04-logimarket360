namespace LogiMarket.Domain.Auditoria;

public class RegistroAuditoria
{
    public int Id { get; private set; }
    public DateTime Fecha { get; private set; }
    public string Categoria { get; private set; } = string.Empty;
    public string Usuario { get; private set; } = string.Empty;
    public string Mensaje { get; private set; } = string.Empty;
    public string Correlacion { get; private set; } = string.Empty;

    private RegistroAuditoria()
    {
    }

    public RegistroAuditoria(DateTime fecha, string categoria, string usuario, string mensaje, string correlacion)
    {
        Fecha = fecha;
        Categoria = categoria;
        Usuario = usuario;
        Mensaje = mensaje;
        Correlacion = correlacion;
    }
}
