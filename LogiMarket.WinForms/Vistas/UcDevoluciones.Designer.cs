namespace LogiMarket.WinForms.Vistas
{
    partial class UcDevoluciones
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
            grpSolicitud = new GroupBox();
            lblPedido = new Label();
            cmbPedido = new ComboBox();
            lblMotivo = new Label();
            cmbMotivo = new ComboBox();
            lblFecha = new Label();
            dtpSolicitud = new DateTimePicker();
            chkSinUso = new CheckBox();
            lblEstimadoTitulo = new Label();
            lblEstimado = new Label();
            lblEntrega = new Label();
            btnProcesar = new Button();
            grpResultado = new GroupBox();
            txtResultado = new TextBox();
            dgvDevoluciones = new DataGridView();
            grpSolicitud.SuspendLayout();
            grpResultado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDevoluciones).BeginInit();
            SuspendLayout();
            // 
            // grpSolicitud
            // 
            grpSolicitud.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            grpSolicitud.Controls.Add(lblPedido);
            grpSolicitud.Controls.Add(cmbPedido);
            grpSolicitud.Controls.Add(lblMotivo);
            grpSolicitud.Controls.Add(cmbMotivo);
            grpSolicitud.Controls.Add(lblFecha);
            grpSolicitud.Controls.Add(dtpSolicitud);
            grpSolicitud.Controls.Add(chkSinUso);
            grpSolicitud.Controls.Add(lblEstimadoTitulo);
            grpSolicitud.Controls.Add(lblEstimado);
            grpSolicitud.Controls.Add(lblEntrega);
            grpSolicitud.Controls.Add(btnProcesar);
            grpSolicitud.Location = new Point(12, 8);
            grpSolicitud.Size = new Size(600, 330);
            grpSolicitud.Name = "grpSolicitud";
            grpSolicitud.TabIndex = 0;
            grpSolicitud.Text = "Solicitud de devolución total";
            grpSolicitud.TabStop = false;
            // 
            // lblPedido
            // 
            lblPedido.AutoSize = true;
            lblPedido.Location = new Point(12, 30);
            lblPedido.Name = "lblPedido";
            lblPedido.TabIndex = 0;
            lblPedido.Text = "Pedido ENTREGADO:";
            // 
            // cmbPedido
            // 
            cmbPedido.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPedido.FormattingEnabled = true;
            cmbPedido.Location = new Point(140, 27);
            cmbPedido.Size = new Size(440, 23);
            cmbPedido.Name = "cmbPedido";
            cmbPedido.TabIndex = 1;
            // 
            // lblMotivo
            // 
            lblMotivo.AutoSize = true;
            lblMotivo.Location = new Point(12, 64);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.TabIndex = 2;
            lblMotivo.Text = "Motivo:";
            // 
            // cmbMotivo
            // 
            cmbMotivo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMotivo.FormattingEnabled = true;
            cmbMotivo.Location = new Point(140, 61);
            cmbMotivo.Size = new Size(200, 23);
            cmbMotivo.Name = "cmbMotivo";
            cmbMotivo.TabIndex = 3;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(12, 98);
            lblFecha.Name = "lblFecha";
            lblFecha.TabIndex = 4;
            lblFecha.Text = "Fecha de solicitud:";
            // 
            // dtpSolicitud
            // 
            dtpSolicitud.Format = DateTimePickerFormat.Short;
            dtpSolicitud.Location = new Point(140, 95);
            dtpSolicitud.Size = new Size(150, 23);
            dtpSolicitud.Name = "dtpSolicitud";
            dtpSolicitud.TabIndex = 5;
            // 
            // chkSinUso
            // 
            chkSinUso.AutoSize = true;
            chkSinUso.Location = new Point(140, 128);
            chkSinUso.Name = "chkSinUso";
            chkSinUso.TabIndex = 6;
            chkSinUso.Text = "Producto sin uso";
            chkSinUso.TabStop = true;
            // 
            // lblEstimadoTitulo
            // 
            lblEstimadoTitulo.AutoSize = true;
            lblEstimadoTitulo.Location = new Point(12, 162);
            lblEstimadoTitulo.Name = "lblEstimadoTitulo";
            lblEstimadoTitulo.TabIndex = 7;
            lblEstimadoTitulo.Text = "Reembolso estimado:";
            // 
            // lblEstimado
            // 
            lblEstimado.Location = new Point(160, 162);
            lblEstimado.Size = new Size(410, 20);
            lblEstimado.Name = "lblEstimado";
            lblEstimado.TabIndex = 8;
            lblEstimado.Text = "-";
            // 
            // lblEntrega
            // 
            lblEntrega.Location = new Point(12, 190);
            lblEntrega.Size = new Size(568, 20);
            lblEntrega.Name = "lblEntrega";
            lblEntrega.TabIndex = 9;
            lblEntrega.Text = "";
            // 
            // btnProcesar
            // 
            btnProcesar.Location = new Point(140, 222);
            btnProcesar.Size = new Size(200, 34);
            btnProcesar.Name = "btnProcesar";
            btnProcesar.TabIndex = 10;
            btnProcesar.Text = "Procesar devolución";
            btnProcesar.UseVisualStyleBackColor = true;
            // 
            // grpResultado
            // 
            grpResultado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpResultado.Controls.Add(txtResultado);
            grpResultado.Location = new Point(624, 8);
            grpResultado.Size = new Size(604, 330);
            grpResultado.Name = "grpResultado";
            grpResultado.TabIndex = 1;
            grpResultado.Text = "Resultado";
            grpResultado.TabStop = false;
            // 
            // txtResultado
            // 
            txtResultado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtResultado.Multiline = true;
            txtResultado.ReadOnly = true;
            txtResultado.ScrollBars = ScrollBars.Vertical;
            txtResultado.Font = new Font("Consolas", 9F);
            txtResultado.Location = new Point(12, 22);
            txtResultado.Size = new Size(580, 296);
            txtResultado.Name = "txtResultado";
            txtResultado.TabIndex = 0;
            // 
            // dgvDevoluciones
            // 
            dgvDevoluciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDevoluciones.AllowUserToAddRows = false;
            dgvDevoluciones.AllowUserToDeleteRows = false;
            dgvDevoluciones.AllowUserToResizeRows = false;
            dgvDevoluciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDevoluciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDevoluciones.MultiSelect = false;
            dgvDevoluciones.ReadOnly = true;
            dgvDevoluciones.RowHeadersVisible = false;
            dgvDevoluciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDevoluciones.Location = new Point(12, 346);
            dgvDevoluciones.Size = new Size(1216, 282);
            dgvDevoluciones.Name = "dgvDevoluciones";
            dgvDevoluciones.TabIndex = 2;
            // 
            // UcDevoluciones
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvDevoluciones);
            Controls.Add(grpResultado);
            Controls.Add(grpSolicitud);
            Name = "UcDevoluciones";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)dgvDevoluciones).EndInit();
            grpResultado.ResumeLayout(false);
            grpResultado.PerformLayout();
            grpSolicitud.ResumeLayout(false);
            grpSolicitud.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpSolicitud;
        private Label lblPedido;
        private ComboBox cmbPedido;
        private Label lblMotivo;
        private ComboBox cmbMotivo;
        private Label lblFecha;
        private DateTimePicker dtpSolicitud;
        private CheckBox chkSinUso;
        private Label lblEstimadoTitulo;
        private Label lblEstimado;
        private Label lblEntrega;
        private Button btnProcesar;
        private GroupBox grpResultado;
        private TextBox txtResultado;
        private DataGridView dgvDevoluciones;
    }
}
