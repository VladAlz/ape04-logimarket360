namespace LogiMarket.WinForms.Vistas
{
    partial class UcAuditoria
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabAuditoria = new TabControl();
            tabBitacora = new TabPage();
            tabMetricas = new TabPage();
            tabDemo = new TabPage();
            lblCategoria = new Label();
            cmbCategoria = new ComboBox();
            btnFiltrarBitacora = new Button();
            dgvAuditoria = new DataGridView();
            grpMetricasAuditoria = new GroupBox();
            dgvMetricas = new DataGridView();
            lblInstancia = new Label();
            grpComandos = new GroupBox();
            dgvComandos = new DataGridView();
            grpSimulacion = new GroupBox();
            lblProveedor = new Label();
            cmbProveedor = new ComboBox();
            chkFalloCobro = new CheckBox();
            chkFalloOutbox = new CheckBox();
            btnAplicarDemo = new Button();
            btnProcesarOutbox = new Button();
            lblDemo = new Label();
            grpPruebaPago = new GroupBox();
            lblMonto = new Label();
            nudMonto = new NumericUpDown();
            btnProbarCadena = new Button();
            txtPrueba = new TextBox();
            grpOutbox = new GroupBox();
            dgvOutbox = new DataGridView();
            tabAuditoria.SuspendLayout();
            tabBitacora.SuspendLayout();
            tabMetricas.SuspendLayout();
            tabDemo.SuspendLayout();
            grpMetricasAuditoria.SuspendLayout();
            grpComandos.SuspendLayout();
            grpSimulacion.SuspendLayout();
            grpPruebaPago.SuspendLayout();
            grpOutbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMetricas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvComandos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMonto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvOutbox).BeginInit();
            SuspendLayout();
            // 
            // tabAuditoria
            // 
            tabAuditoria.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabAuditoria.Controls.Add(tabBitacora);
            tabAuditoria.Controls.Add(tabMetricas);
            tabAuditoria.Controls.Add(tabDemo);
            tabAuditoria.Location = new Point(12, 8);
            tabAuditoria.Size = new Size(1216, 620);
            tabAuditoria.Name = "tabAuditoria";
            tabAuditoria.TabIndex = 0;
            // 
            // tabBitacora
            // 
            tabBitacora.Controls.Add(lblCategoria);
            tabBitacora.Controls.Add(cmbCategoria);
            tabBitacora.Controls.Add(btnFiltrarBitacora);
            tabBitacora.Controls.Add(dgvAuditoria);
            tabBitacora.Location = new Point(4, 24);
            tabBitacora.Size = new Size(1208, 592);
            tabBitacora.Name = "tabBitacora";
            tabBitacora.TabIndex = 0;
            tabBitacora.Text = "Bitácora";
            tabBitacora.Padding = new Padding(3);
            tabBitacora.UseVisualStyleBackColor = true;
            // 
            // tabMetricas
            // 
            tabMetricas.Controls.Add(grpMetricasAuditoria);
            tabMetricas.Controls.Add(grpComandos);
            tabMetricas.Location = new Point(4, 24);
            tabMetricas.Size = new Size(1208, 592);
            tabMetricas.Name = "tabMetricas";
            tabMetricas.TabIndex = 1;
            tabMetricas.Text = "Métricas y comandos";
            tabMetricas.Padding = new Padding(3);
            tabMetricas.UseVisualStyleBackColor = true;
            // 
            // tabDemo
            // 
            tabDemo.Controls.Add(grpSimulacion);
            tabDemo.Controls.Add(grpPruebaPago);
            tabDemo.Controls.Add(grpOutbox);
            tabDemo.Location = new Point(4, 24);
            tabDemo.Size = new Size(1208, 592);
            tabDemo.Name = "tabDemo";
            tabDemo.TabIndex = 2;
            tabDemo.Text = "Outbox y demostración";
            tabDemo.Padding = new Padding(3);
            tabDemo.UseVisualStyleBackColor = true;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(12, 15);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.TabIndex = 0;
            lblCategoria.Text = "Categoría:";
            // 
            // cmbCategoria
            // 
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Location = new Point(85, 12);
            cmbCategoria.Size = new Size(180, 23);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.TabIndex = 1;
            // 
            // btnFiltrarBitacora
            // 
            btnFiltrarBitacora.Location = new Point(273, 11);
            btnFiltrarBitacora.Size = new Size(90, 25);
            btnFiltrarBitacora.Name = "btnFiltrarBitacora";
            btnFiltrarBitacora.TabIndex = 2;
            btnFiltrarBitacora.Text = "Mostrar";
            btnFiltrarBitacora.UseVisualStyleBackColor = true;
            // 
            // dgvAuditoria
            // 
            dgvAuditoria.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAuditoria.AllowUserToAddRows = false;
            dgvAuditoria.AllowUserToDeleteRows = false;
            dgvAuditoria.AllowUserToResizeRows = false;
            dgvAuditoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAuditoria.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAuditoria.MultiSelect = false;
            dgvAuditoria.ReadOnly = true;
            dgvAuditoria.RowHeadersVisible = false;
            dgvAuditoria.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuditoria.Location = new Point(12, 44);
            dgvAuditoria.Size = new Size(1180, 536);
            dgvAuditoria.Name = "dgvAuditoria";
            dgvAuditoria.TabIndex = 3;
            // 
            // grpMetricasAuditoria
            // 
            grpMetricasAuditoria.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            grpMetricasAuditoria.Controls.Add(dgvMetricas);
            grpMetricasAuditoria.Controls.Add(lblInstancia);
            grpMetricasAuditoria.Location = new Point(12, 8);
            grpMetricasAuditoria.Size = new Size(420, 572);
            grpMetricasAuditoria.Name = "grpMetricasAuditoria";
            grpMetricasAuditoria.TabIndex = 0;
            grpMetricasAuditoria.Text = "Métricas de la sesión";
            grpMetricasAuditoria.TabStop = false;
            // 
            // dgvMetricas
            // 
            dgvMetricas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMetricas.AllowUserToAddRows = false;
            dgvMetricas.AllowUserToDeleteRows = false;
            dgvMetricas.AllowUserToResizeRows = false;
            dgvMetricas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMetricas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMetricas.MultiSelect = false;
            dgvMetricas.ReadOnly = true;
            dgvMetricas.RowHeadersVisible = false;
            dgvMetricas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMetricas.Location = new Point(12, 22);
            dgvMetricas.Size = new Size(396, 478);
            dgvMetricas.Name = "dgvMetricas";
            dgvMetricas.TabIndex = 0;
            // 
            // lblInstancia
            // 
            lblInstancia.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblInstancia.Location = new Point(12, 508);
            lblInstancia.Size = new Size(396, 54);
            lblInstancia.Name = "lblInstancia";
            lblInstancia.TabIndex = 1;
            lblInstancia.Text = "";
            // 
            // grpComandos
            // 
            grpComandos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpComandos.Controls.Add(dgvComandos);
            grpComandos.Location = new Point(444, 8);
            grpComandos.Size = new Size(748, 572);
            grpComandos.Name = "grpComandos";
            grpComandos.TabIndex = 1;
            grpComandos.Text = "Comandos ejecutados";
            grpComandos.TabStop = false;
            // 
            // dgvComandos
            // 
            dgvComandos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvComandos.AllowUserToAddRows = false;
            dgvComandos.AllowUserToDeleteRows = false;
            dgvComandos.AllowUserToResizeRows = false;
            dgvComandos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvComandos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvComandos.MultiSelect = false;
            dgvComandos.ReadOnly = true;
            dgvComandos.RowHeadersVisible = false;
            dgvComandos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvComandos.Location = new Point(12, 22);
            dgvComandos.Size = new Size(724, 538);
            dgvComandos.Name = "dgvComandos";
            dgvComandos.TabIndex = 0;
            // 
            // grpSimulacion
            // 
            grpSimulacion.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            grpSimulacion.Controls.Add(lblProveedor);
            grpSimulacion.Controls.Add(cmbProveedor);
            grpSimulacion.Controls.Add(chkFalloCobro);
            grpSimulacion.Controls.Add(chkFalloOutbox);
            grpSimulacion.Controls.Add(btnAplicarDemo);
            grpSimulacion.Controls.Add(btnProcesarOutbox);
            grpSimulacion.Controls.Add(lblDemo);
            grpSimulacion.Location = new Point(12, 8);
            grpSimulacion.Size = new Size(440, 250);
            grpSimulacion.Name = "grpSimulacion";
            grpSimulacion.TabIndex = 0;
            grpSimulacion.Text = "Simulación controlada";
            grpSimulacion.TabStop = false;
            // 
            // lblProveedor
            // 
            lblProveedor.AutoSize = true;
            lblProveedor.Location = new Point(12, 30);
            lblProveedor.Name = "lblProveedor";
            lblProveedor.TabIndex = 0;
            lblProveedor.Text = "Proveedor de pago:";
            // 
            // cmbProveedor
            // 
            cmbProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProveedor.FormattingEnabled = true;
            cmbProveedor.Location = new Point(140, 27);
            cmbProveedor.Size = new Size(150, 23);
            cmbProveedor.Name = "cmbProveedor";
            cmbProveedor.TabIndex = 1;
            // 
            // chkFalloCobro
            // 
            chkFalloCobro.AutoSize = true;
            chkFalloCobro.Location = new Point(140, 62);
            chkFalloCobro.Name = "chkFalloCobro";
            chkFalloCobro.TabIndex = 2;
            chkFalloCobro.Text = "Forzar rechazo de cobro";
            chkFalloCobro.TabStop = true;
            // 
            // chkFalloOutbox
            // 
            chkFalloOutbox.AutoSize = true;
            chkFalloOutbox.Location = new Point(140, 90);
            chkFalloOutbox.Name = "chkFalloOutbox";
            chkFalloOutbox.TabIndex = 3;
            chkFalloOutbox.Text = "Forzar fallo del notificador";
            chkFalloOutbox.TabStop = true;
            // 
            // btnAplicarDemo
            // 
            btnAplicarDemo.Location = new Point(140, 122);
            btnAplicarDemo.Size = new Size(150, 30);
            btnAplicarDemo.Name = "btnAplicarDemo";
            btnAplicarDemo.TabIndex = 4;
            btnAplicarDemo.Text = "Aplicar";
            btnAplicarDemo.UseVisualStyleBackColor = true;
            // 
            // btnProcesarOutbox
            // 
            btnProcesarOutbox.Location = new Point(140, 158);
            btnProcesarOutbox.Size = new Size(200, 30);
            btnProcesarOutbox.Name = "btnProcesarOutbox";
            btnProcesarOutbox.TabIndex = 5;
            btnProcesarOutbox.Text = "Reprocesar Outbox";
            btnProcesarOutbox.UseVisualStyleBackColor = true;
            // 
            // lblDemo
            // 
            lblDemo.Location = new Point(12, 196);
            lblDemo.Size = new Size(416, 48);
            lblDemo.Name = "lblDemo";
            lblDemo.TabIndex = 6;
            lblDemo.Text = "";
            // 
            // grpPruebaPago
            // 
            grpPruebaPago.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            grpPruebaPago.Controls.Add(lblMonto);
            grpPruebaPago.Controls.Add(nudMonto);
            grpPruebaPago.Controls.Add(btnProbarCadena);
            grpPruebaPago.Controls.Add(txtPrueba);
            grpPruebaPago.Location = new Point(12, 266);
            grpPruebaPago.Size = new Size(440, 314);
            grpPruebaPago.Name = "grpPruebaPago";
            grpPruebaPago.TabIndex = 1;
            grpPruebaPago.Text = "Prueba directa de la cadena de pago";
            grpPruebaPago.TabStop = false;
            // 
            // lblMonto
            // 
            lblMonto.AutoSize = true;
            lblMonto.Location = new Point(12, 30);
            lblMonto.Name = "lblMonto";
            lblMonto.TabIndex = 0;
            lblMonto.Text = "Monto:";
            // 
            // nudMonto
            // 
            nudMonto.Location = new Point(70, 27);
            nudMonto.Size = new Size(100, 23);
            nudMonto.Name = "nudMonto";
            nudMonto.TabIndex = 1;
            nudMonto.DecimalPlaces = 2;
            nudMonto.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudMonto.Value = new decimal(new int[] { 59, 0, 0, 0 });
            // 
            // btnProbarCadena
            // 
            btnProbarCadena.Location = new Point(180, 25);
            btnProbarCadena.Size = new Size(240, 27);
            btnProbarCadena.Name = "btnProbarCadena";
            btnProbarCadena.TabIndex = 2;
            btnProbarCadena.Text = "Cobrar con el rol actual";
            btnProbarCadena.UseVisualStyleBackColor = true;
            // 
            // txtPrueba
            // 
            txtPrueba.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtPrueba.Multiline = true;
            txtPrueba.ReadOnly = true;
            txtPrueba.ScrollBars = ScrollBars.Vertical;
            txtPrueba.Font = new Font("Consolas", 9F);
            txtPrueba.Location = new Point(12, 60);
            txtPrueba.Size = new Size(416, 242);
            txtPrueba.Name = "txtPrueba";
            txtPrueba.TabIndex = 3;
            // 
            // grpOutbox
            // 
            grpOutbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpOutbox.Controls.Add(dgvOutbox);
            grpOutbox.Location = new Point(464, 8);
            grpOutbox.Size = new Size(728, 572);
            grpOutbox.Name = "grpOutbox";
            grpOutbox.TabIndex = 2;
            grpOutbox.Text = "Eventos Outbox";
            grpOutbox.TabStop = false;
            // 
            // dgvOutbox
            // 
            dgvOutbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOutbox.AllowUserToAddRows = false;
            dgvOutbox.AllowUserToDeleteRows = false;
            dgvOutbox.AllowUserToResizeRows = false;
            dgvOutbox.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOutbox.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOutbox.MultiSelect = false;
            dgvOutbox.ReadOnly = true;
            dgvOutbox.RowHeadersVisible = false;
            dgvOutbox.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOutbox.Location = new Point(12, 22);
            dgvOutbox.Size = new Size(704, 538);
            dgvOutbox.Name = "dgvOutbox";
            dgvOutbox.TabIndex = 0;
            // 
            // UcAuditoria
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabAuditoria);
            Name = "UcAuditoria";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMetricas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvComandos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMonto).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvOutbox).EndInit();
            grpOutbox.ResumeLayout(false);
            grpOutbox.PerformLayout();
            grpPruebaPago.ResumeLayout(false);
            grpPruebaPago.PerformLayout();
            grpSimulacion.ResumeLayout(false);
            grpSimulacion.PerformLayout();
            grpComandos.ResumeLayout(false);
            grpComandos.PerformLayout();
            grpMetricasAuditoria.ResumeLayout(false);
            grpMetricasAuditoria.PerformLayout();
            tabDemo.ResumeLayout(false);
            tabDemo.PerformLayout();
            tabMetricas.ResumeLayout(false);
            tabMetricas.PerformLayout();
            tabBitacora.ResumeLayout(false);
            tabBitacora.PerformLayout();
            tabAuditoria.ResumeLayout(false);
            tabAuditoria.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabAuditoria;
        private TabPage tabBitacora;
        private TabPage tabMetricas;
        private TabPage tabDemo;
        private Label lblCategoria;
        private ComboBox cmbCategoria;
        private Button btnFiltrarBitacora;
        private DataGridView dgvAuditoria;
        private GroupBox grpMetricasAuditoria;
        private DataGridView dgvMetricas;
        private Label lblInstancia;
        private GroupBox grpComandos;
        private DataGridView dgvComandos;
        private GroupBox grpSimulacion;
        private Label lblProveedor;
        private ComboBox cmbProveedor;
        private CheckBox chkFalloCobro;
        private CheckBox chkFalloOutbox;
        private Button btnAplicarDemo;
        private Button btnProcesarOutbox;
        private Label lblDemo;
        private GroupBox grpPruebaPago;
        private Label lblMonto;
        private NumericUpDown nudMonto;
        private Button btnProbarCadena;
        private TextBox txtPrueba;
        private GroupBox grpOutbox;
        private DataGridView dgvOutbox;
    }
}
