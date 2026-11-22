using LogiMarket.Application.Comandos;
using LogiMarket.Application.Comun;
using LogiMarket.Application.Consultas;
using LogiMarket.Application.Eventos;
using LogiMarket.Application.Metricas;
using LogiMarket.Application.Seguridad;
using LogiMarket.Application.Simulacion;
using LogiMarket.WinForms.Comun;
using LogiMarket.WinForms.Mediador;

namespace LogiMarket.WinForms.Vistas;

public partial class UcAuditoria : VistaBase
{
    private const string Todas = "(todas)";
    private readonly MetricasSesion _metricas;
    private readonly ISesionUsuario _sesion;

    private sealed record Fila(string Nombre, int Valor);

    // MetricasSesion llega por constructor: es la misma instancia que usa el panel
    public UcAuditoria(EjecutorCasosUso ejecutor, IMediadorVistas mediador, ControlPermisos permisos, MetricasSesion metricas,
        ISesionUsuario sesion)
        : base(ejecutor, mediador, permisos)
    {
        _metricas = metricas;
        _sesion = sesion;
        InitializeComponent();
        Rejilla.Definir(dgvAuditoria,
            new Columna(nameof(AuditoriaDto.Fecha), "Fecha", "dd/MM/yyyy HH:mm:ss", 70), new Columna(nameof(AuditoriaDto.Categoria), "Categoría", Peso: 55),
            new Columna(nameof(AuditoriaDto.Usuario), "Usuario", Peso: 45), new Columna(nameof(AuditoriaDto.Mensaje), "Mensaje", Peso: 300),
            new Columna(nameof(AuditoriaDto.Correlacion), "Correlación", Peso: 90));
        Rejilla.Definir(dgvMetricas, new Columna(nameof(Fila.Nombre), "Contador", Peso: 160), new Columna(nameof(Fila.Valor), "Valor", Derecha: true));
        Rejilla.Definir(dgvComandos,
            new Columna(nameof(RegistroComando.Fecha), "Fecha", "HH:mm:ss", 45), new Columna(nameof(RegistroComando.Usuario), "Usuario", Peso: 45),
            new Columna(nameof(RegistroComando.Comando), "Comando", Peso: 75), new Columna(nameof(RegistroComando.Entrada), "Entrada", Peso: 150),
            new Columna(nameof(RegistroComando.Exito), "Éxito", Peso: 35), new Columna(nameof(RegistroComando.Resultado), "Resultado", Peso: 170));
        Rejilla.Definir(dgvOutbox,
            new Columna(nameof(OutboxDto.Id), "Id", Peso: 30, Derecha: true), new Columna(nameof(OutboxDto.Tipo), "Tipo", Peso: 90),
            new Columna(nameof(OutboxDto.PedidoId), "Pedido", Peso: 40, Derecha: true), new Columna(nameof(OutboxDto.Creado), "Creado", "dd/MM HH:mm:ss", 65),
            new Columna(nameof(OutboxDto.Estado), "Estado", Peso: 60), new Columna(nameof(OutboxDto.Intentos), "Intentos", Peso: 40, Derecha: true),
            new Columna(nameof(OutboxDto.UltimoError), "Último error", Peso: 140));
        cmbCategoria.DataSource = new[] { Todas }.Concat(CategoriaAuditoria.Todas).ToList();

        Permisos.Asociar(Operacion.ConfigurarSimulacion, btnAplicarDemo, btnProcesarOutbox);
        btnFiltrarBitacora.Click += (_, _) => Ejecutar(() => CargarBitacoraAsync(manual: true));
        btnAplicarDemo.Click += (_, _) => Ejecutar(AplicarSimulacionAsync);
        btnProcesarOutbox.Click += (_, _) => Ejecutar(ProcesarOutboxAsync);
        btnProbarCadena.Click += (_, _) => Ejecutar(ProbarCadenaAsync);
    }

    public override string NombreVista => FormularioPrincipalMediator.Auditoria;

    public override async Task RefrescarAsync()
    {
        Rejilla.Cargar(dgvMetricas, _metricas.Instantanea().Select(m => new Fila(m.Key, m.Value)));
        lblInstancia.Text = $"Instancia de MetricasSesion:{Environment.NewLine}{_metricas.Instancia}";
        Rejilla.Cargar(dgvComandos, await Ejecutor.EjecutarAsync<ConsultarAuditoriaUseCase, IReadOnlyList<RegistroComando>>(c => c.Comandos()));

        var simulacion = await Ejecutor.EjecutarAsync<SimulacionUseCase, EstadoSimulacion>(s => s.Estado());
        cmbProveedor.DataSource = simulacion.Proveedores.ToList();
        cmbProveedor.SelectedItem = simulacion.ProveedorPago;
        chkFalloCobro.Checked = simulacion.FallarCobro;
        chkFalloOutbox.Checked = simulacion.FallarNotificacion;

        await CargarBitacoraAsync();
    }

    private async Task CargarBitacoraAsync(bool manual = false)
    {
        var categoria = cmbCategoria.SelectedItem as string;

        // El refresco automatico no intenta leer si el rol no tiene permiso
        if (!ServicioAutorizacion.RolPuede(_sesion.Rol, Operacion.VerAuditoria) && !manual)
        {
            tabBitacora.Text = "Bitácora (sin permiso)";
            return;
        }

        try
        {
            tabBitacora.Text = "Bitácora";
            Rejilla.Cargar(dgvAuditoria, await Ejecutor.EjecutarAsync<ConsultarAuditoriaUseCase, IReadOnlyList<AuditoriaDto>>(
                c => c.RegistrosAsync(categoria == Todas ? null : categoria)));
            Rejilla.Cargar(dgvOutbox, await Ejecutor.EjecutarAsync<ConsultarAuditoriaUseCase, IReadOnlyList<OutboxDto>>(c => c.OutboxAsync()), o => o.Id);
        }
        catch (AccesoDenegadoException ex)
        {
            Rejilla.Cargar(dgvAuditoria, Array.Empty<AuditoriaDto>());
            Rejilla.Cargar(dgvOutbox, Array.Empty<OutboxDto>());
            tabBitacora.Text = "Bitácora (sin permiso)";
            lblDemo.Text = ex.Message;
        }
    }

    private async Task AplicarSimulacionAsync()
    {
        var resultado = await Ejecutor.EjecutarAsync<SimulacionUseCase, ResultadoOperacion>(s =>
            s.AplicarAsync((string)cmbProveedor.SelectedItem!, chkFalloCobro.Checked, chkFalloOutbox.Checked));
        Mostrar(lblDemo, resultado);
        if (resultado.Exito)
            await AvisarAsync(EventoVista.SimulacionCambiada);
    }

    private async Task ProcesarOutboxAsync()
    {
        var resumen = await Ejecutor.EjecutarAsync<SimulacionUseCase, ResumenOutbox>(s => s.ProcesarOutboxAsync());
        lblDemo.Text = $"Outbox: {resumen.Procesados} procesado(s), {resumen.Fallidos} pendiente(s).";
        txtPrueba.Text = Texto.Lineas(resumen.Detalle);
        await AvisarAsync(EventoVista.OutboxProcesado);
    }

    private async Task ProbarCadenaAsync()
    {
        var monto = nudMonto.Value;
        var prueba = await Ejecutor.EjecutarAsync<SimulacionUseCase, ResultadoPruebaCobro>(s => s.ProbarCadenaPagoAsync(monto));
        txtPrueba.Text = $"{(prueba.Aprobado ? "APROBADO" : "NO APROBADO")}: {prueba.Mensaje}{Environment.NewLine}{Environment.NewLine}Registros generados:{Environment.NewLine}"
            + Texto.Lineas(prueba.Trazas.Select((t, i) => $"{i + 1}. {t}"))
            + (prueba.Trazas.Any(t => t.StartsWith("[COBRO]")) ? string.Empty : $"{Environment.NewLine}(ningún registro de COBRO: la pasarela no fue invocada)");
        await CargarBitacoraAsync();
    }
}
