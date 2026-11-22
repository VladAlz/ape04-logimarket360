namespace LogiMarket.WinForms.Vistas
{
    partial class UcReportes
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
            grpComprobantes = new GroupBox();
            lblTipo = new Label();
            cmbTipo = new ComboBox();
            lblPedidoComprobante = new Label();
            cmbPedidoComprobante = new ComboBox();
            lblMedio = new Label();
            cmbMedio = new ComboBox();
            btnEmitir = new Button();
            lblComprobante = new Label();
            grpReporte = new GroupBox();
            lblFiltroReporte = new Label();
            txtFiltroReporte = new TextBox();
            btnGenerarReporte = new Button();
            lblTotalesReporte = new Label();
            dgvReporte = new DataGridView();
            grpVistaMasiva = new GroupBox();
            lblFilas = new Label();
            nudFilas = new NumericUpDown();
            btnGenerarVista = new Button();
            lblVistaMasiva = new Label();
            dgvFilas = new DataGridView();
            grpComprobantes.SuspendLayout();
            grpReporte.SuspendLayout();
            grpVistaMasiva.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFilas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvFilas).BeginInit();
            SuspendLayout();
            // 
            // grpComprobantes
            // 
            grpComprobantes.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            grpComprobantes.Controls.Add(lblTipo);
            grpComprobantes.Controls.Add(cmbTipo);
            grpComprobantes.Controls.Add(lblPedidoComprobante);
            grpComprobantes.Controls.Add(cmbPedidoComprobante);
            grpComprobantes.Controls.Add(lblMedio);
            grpComprobantes.Controls.Add(cmbMedio);
            grpComprobantes.Controls.Add(btnEmitir);
            grpComprobantes.Controls.Add(lblComprobante);
            grpComprobantes.Location = new Point(12, 8);
            grpComprobantes.Size = new Size(600, 220);
            grpComprobantes.Name = "grpComprobantes";
            grpComprobantes.TabIndex = 0;
            grpComprobantes.Text = "Comprobantes";
            grpComprobantes.TabStop = false;
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(12, 30);
            lblTipo.Name = "lblTipo";
            lblTipo.TabIndex = 0;
            lblTipo.Text = "Comprobante:";
            // 
            // cmbTipo
            // 
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.FormattingEnabled = true;
            cmbTipo.Location = new Point(110, 27);
            cmbTipo.Size = new Size(180, 23);
            cmbTipo.Name = "cmbTipo";
            cmbTipo.TabIndex = 1;
            // 
            // lblPedidoComprobante
            // 
            lblPedidoComprobante.AutoSize = true;
            lblPedidoComprobante.Location = new Point(12, 64);
            lblPedidoComprobante.Name = "lblPedidoComprobante";
            lblPedidoComprobante.TabIndex = 2;
            lblPedidoComprobante.Text = "Pedido:";
            // 
            // cmbPedidoComprobante
            // 
            cmbPedidoComprobante.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPedidoComprobante.FormattingEnabled = true;
            cmbPedidoComprobante.Location = new Point(110, 61);
            cmbPedidoComprobante.Size = new Size(470, 23);
            cmbPedidoComprobante.Name = "cmbPedidoComprobante";
            cmbPedidoComprobante.TabIndex = 3;
            // 
            // lblMedio
            // 
            lblMedio.AutoSize = true;
            lblMedio.Location = new Point(12, 98);
            lblMedio.Name = "lblMedio";
            lblMedio.TabIndex = 4;
            lblMedio.Text = "Salida:";
            // 
            // cmbMedio
            // 
            cmbMedio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMedio.FormattingEnabled = true;
            cmbMedio.Location = new Point(110, 95);
            cmbMedio.Size = new Size(180, 23);
            cmbMedio.Name = "cmbMedio";
            cmbMedio.TabIndex = 5;
            // 
            // btnEmitir
            // 
            btnEmitir.Location = new Point(110, 130);
            btnEmitir.Size = new Size(160, 32);
            btnEmitir.Name = "btnEmitir";
            btnEmitir.TabIndex = 6;
            btnEmitir.Text = "Emitir";
            btnEmitir.UseVisualStyleBackColor = true;
            // 
            // lblComprobante
            // 
            lblComprobante.Location = new Point(12, 170);
            lblComprobante.Size = new Size(570, 42);
            lblComprobante.Name = "lblComprobante";
            lblComprobante.TabIndex = 7;
            lblComprobante.Text = "";
            // 
            // grpReporte
            // 
            grpReporte.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            grpReporte.Controls.Add(lblFiltroReporte);
            grpReporte.Controls.Add(txtFiltroReporte);
            grpReporte.Controls.Add(btnGenerarReporte);
            grpReporte.Controls.Add(lblTotalesReporte);
            grpReporte.Controls.Add(dgvReporte);
            grpReporte.Location = new Point(12, 236);
            grpReporte.Size = new Size(600, 392);
            grpReporte.Name = "grpReporte";
            grpReporte.TabIndex = 1;
            grpReporte.Text = "Reporte de pedidos";
            grpReporte.TabStop = false;
            // 
            // lblFiltroReporte
            // 
            lblFiltroReporte.AutoSize = true;
            lblFiltroReporte.Location = new Point(12, 28);
            lblFiltroReporte.Name = "lblFiltroReporte";
            lblFiltroReporte.TabIndex = 0;
            lblFiltroReporte.Text = "Filtro:";
            // 
            // txtFiltroReporte
            // 
            txtFiltroReporte.Location = new Point(60, 25);
            txtFiltroReporte.Size = new Size(330, 23);
            txtFiltroReporte.Name = "txtFiltroReporte";
            txtFiltroReporte.TabIndex = 1;
            // 
            // btnGenerarReporte
            // 
            btnGenerarReporte.Location = new Point(398, 23);
            btnGenerarReporte.Size = new Size(90, 27);
            btnGenerarReporte.Name = "btnGenerarReporte";
            btnGenerarReporte.TabIndex = 2;
            btnGenerarReporte.Text = "Generar";
            btnGenerarReporte.UseVisualStyleBackColor = true;
            // 
            // lblTotalesReporte
            // 
            lblTotalesReporte.Location = new Point(12, 56);
            lblTotalesReporte.Size = new Size(576, 20);
            lblTotalesReporte.Name = "lblTotalesReporte";
            lblTotalesReporte.TabIndex = 3;
            lblTotalesReporte.Text = "";
            // 
            // dgvReporte
            // 
            dgvReporte.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AllowUserToDeleteRows = false;
            dgvReporte.AllowUserToResizeRows = false;
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReporte.MultiSelect = false;
            dgvReporte.ReadOnly = true;
            dgvReporte.RowHeadersVisible = false;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.Location = new Point(12, 80);
            dgvReporte.Size = new Size(576, 300);
            dgvReporte.Name = "dgvReporte";
            dgvReporte.TabIndex = 4;
            // 
            // grpVistaMasiva
            // 
            grpVistaMasiva.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpVistaMasiva.Controls.Add(lblFilas);
            grpVistaMasiva.Controls.Add(nudFilas);
            grpVistaMasiva.Controls.Add(btnGenerarVista);
            grpVistaMasiva.Controls.Add(lblVistaMasiva);
            grpVistaMasiva.Controls.Add(dgvFilas);
            grpVistaMasiva.Location = new Point(624, 8);
            grpVistaMasiva.Size = new Size(604, 620);
            grpVistaMasiva.Name = "grpVistaMasiva";
            grpVistaMasiva.TabIndex = 2;
            grpVistaMasiva.Text = "Vista masiva del catálogo";
            grpVistaMasiva.TabStop = false;
            // 
            // lblFilas
            // 
            lblFilas.AutoSize = true;
            lblFilas.Location = new Point(12, 30);
            lblFilas.Name = "lblFilas";
            lblFilas.TabIndex = 0;
            lblFilas.Text = "Filas:";
            // 
            // nudFilas
            // 
            nudFilas.Location = new Point(60, 27);
            nudFilas.Size = new Size(90, 23);
            nudFilas.Name = "nudFilas";
            nudFilas.TabIndex = 1;
            nudFilas.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            nudFilas.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            nudFilas.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // btnGenerarVista
            // 
            btnGenerarVista.Location = new Point(160, 25);
            btnGenerarVista.Size = new Size(160, 27);
            btnGenerarVista.Name = "btnGenerarVista";
            btnGenerarVista.TabIndex = 2;
            btnGenerarVista.Text = "Generar vista";
            btnGenerarVista.UseVisualStyleBackColor = true;
            // 
            // lblVistaMasiva
            // 
            lblVistaMasiva.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblVistaMasiva.Location = new Point(12, 60);
            lblVistaMasiva.Size = new Size(580, 100);
            lblVistaMasiva.Name = "lblVistaMasiva";
            lblVistaMasiva.TabIndex = 3;
            lblVistaMasiva.Text = "";
            // 
            // dgvFilas
            // 
            dgvFilas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvFilas.AllowUserToAddRows = false;
            dgvFilas.AllowUserToDeleteRows = false;
            dgvFilas.AllowUserToResizeRows = false;
            dgvFilas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFilas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFilas.MultiSelect = false;
            dgvFilas.ReadOnly = true;
            dgvFilas.RowHeadersVisible = false;
            dgvFilas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFilas.Location = new Point(12, 166);
            dgvFilas.Size = new Size(580, 442);
            dgvFilas.Name = "dgvFilas";
            dgvFilas.TabIndex = 4;
            // 
            // UcReportes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(grpVistaMasiva);
            Controls.Add(grpReporte);
            Controls.Add(grpComprobantes);
            Name = "UcReportes";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFilas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvFilas).EndInit();
            grpVistaMasiva.ResumeLayout(false);
            grpVistaMasiva.PerformLayout();
            grpReporte.ResumeLayout(false);
            grpReporte.PerformLayout();
            grpComprobantes.ResumeLayout(false);
            grpComprobantes.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpComprobantes;
        private Label lblTipo;
        private ComboBox cmbTipo;
        private Label lblPedidoComprobante;
        private ComboBox cmbPedidoComprobante;
        private Label lblMedio;
        private ComboBox cmbMedio;
        private Button btnEmitir;
        private Label lblComprobante;
        private GroupBox grpReporte;
        private Label lblFiltroReporte;
        private TextBox txtFiltroReporte;
        private Button btnGenerarReporte;
        private Label lblTotalesReporte;
        private DataGridView dgvReporte;
        private GroupBox grpVistaMasiva;
        private Label lblFilas;
        private NumericUpDown nudFilas;
        private Button btnGenerarVista;
        private Label lblVistaMasiva;
        private DataGridView dgvFilas;
    }
}
