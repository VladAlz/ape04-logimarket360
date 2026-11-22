using Microsoft.Extensions.DependencyInjection;

namespace LogiMarket.WinForms;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        // Formato local con punto decimal, igual que los montos del dominio
        var cultura = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.GetCultureInfo("es-EC").Clone();
        cultura.NumberFormat.NumberDecimalSeparator = ".";
        cultura.NumberFormat.NumberGroupSeparator = ",";
        cultura.NumberFormat.PercentDecimalSeparator = ".";
        cultura.NumberFormat.PercentGroupSeparator = ",";
        cultura.NumberFormat.CurrencyDecimalSeparator = ".";
        cultura.NumberFormat.CurrencyGroupSeparator = ",";
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultura;
        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultura;
        System.Globalization.CultureInfo.CurrentCulture = cultura;
        System.Globalization.CultureInfo.CurrentUICulture = cultura;

        var arranque = OpcionesArranque.Desde(args);
        using var proveedor = ConfiguracionDependencias.Crear(arranque);
        var semilla = ConfiguracionDependencias.PrepararBaseDatos(proveedor, arranque);

        // Al cerrar sesion se vuelve al acceso; al cerrar la ventana se sale
        while (true)
        {
            using var acceso = proveedor.GetRequiredService<FrmAcceso>();
            if (acceso.ShowDialog() != DialogResult.OK)
                break;

            // Las vistas de una sesion anterior ya no reciben avisos
            proveedor.GetRequiredService<Mediador.IMediadorVistas>().Reiniciar();
            proveedor.GetRequiredService<Comun.ControlPermisos>().Reiniciar();

            using var principal = proveedor.GetRequiredService<FrmPrincipal>();
            principal.MostrarEstado(semilla);
            System.Windows.Forms.Application.Run(principal);

            if (!principal.SesionCerrada)
                break;
        }
    }
}
