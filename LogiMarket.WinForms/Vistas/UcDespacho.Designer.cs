namespace LogiMarket.WinForms.Vistas
{
    partial class UcDespacho
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
            dgvPedidos = new DataGridView();
            grpAcciones = new GroupBox();
            btnDespachar = new Button();
            lblFechaEntrega = new Label();
            dtpEntrega = new DateTimePicker();
            btnEntregar = new Button();
            lblAccion = new Label();
            grpSeguimiento = new GroupBox();
            lblGuia = new Label();
            txtGuia = new TextBox();
            btnConsultarGuia = new Button();
            lblSeguimiento = new Label();
            txtResultado = new TextBox();
            grpAcciones.SuspendLayout();
            grpSeguimiento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).BeginInit();
            SuspendLayout();
            // 
            // dgvPedidos
            // 
            dgvPedidos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.AllowUserToDeleteRows = false;
            dgvPedidos.AllowUserToResizeRows = false;
            dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPedidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPedidos.MultiSelect = false;
            dgvPedidos.ReadOnly = true;
            dgvPedidos.RowHeadersVisible = false;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.Location = new Point(12, 12);
            dgvPedidos.Size = new Size(760, 400);
            dgvPedidos.Name = "dgvPedidos";
            dgvPedidos.TabIndex = 0;
            // 
            // grpAcciones
            // 
            grpAcciones.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpAcciones.Controls.Add(btnDespachar);
            grpAcciones.Controls.Add(lblFechaEntrega);
            grpAcciones.Controls.Add(dtpEntrega);
            grpAcciones.Controls.Add(btnEntregar);
            grpAcciones.Controls.Add(lblAccion);
            grpAcciones.Location = new Point(784, 8);
            grpAcciones.Size = new Size(444, 250);
            grpAcciones.Name = "grpAcciones";
            grpAcciones.TabIndex = 1;
            grpAcciones.Text = "Despacho y entrega";
            grpAcciones.TabStop = false;
            // 
            // btnDespachar
            // 
            btnDespachar.Location = new Point(12, 28);
            btnDespachar.Size = new Size(200, 32);
            btnDespachar.Name = "btnDespachar";
            btnDespachar.TabIndex = 0;
            btnDespachar.Text = "Despachar seleccionado";
            btnDespachar.UseVisualStyleBackColor = true;
            // 
            // lblFechaEntrega
            // 
            lblFechaEntrega.AutoSize = true;
            lblFechaEntrega.Location = new Point(12, 78);
            lblFechaEntrega.Name = "lblFechaEntrega";
            lblFechaEntrega.TabIndex = 1;
            lblFechaEntrega.Text = "Fecha de entrega:";
            // 
            // dtpEntrega
            // 
            dtpEntrega.Format = DateTimePickerFormat.Short;
            dtpEntrega.Location = new Point(130, 74);
            dtpEntrega.Size = new Size(150, 23);
            dtpEntrega.Name = "dtpEntrega";
            dtpEntrega.TabIndex = 2;
            // 
            // btnEntregar
            // 
            btnEntregar.Location = new Point(12, 108);
            btnEntregar.Size = new Size(200, 32);
            btnEntregar.Name = "btnEntregar";
            btnEntregar.TabIndex = 3;
            btnEntregar.Text = "Registrar entrega";
            btnEntregar.UseVisualStyleBackColor = true;
            // 
            // lblAccion
            // 
            lblAccion.Location = new Point(12, 150);
            lblAccion.Size = new Size(420, 90);
            lblAccion.Name = "lblAccion";
            lblAccion.TabIndex = 4;
            lblAccion.Text = "";
            // 
            // grpSeguimiento
            // 
            grpSeguimiento.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpSeguimiento.Controls.Add(lblGuia);
            grpSeguimiento.Controls.Add(txtGuia);
            grpSeguimiento.Controls.Add(btnConsultarGuia);
            grpSeguimiento.Controls.Add(lblSeguimiento);
            grpSeguimiento.Location = new Point(784, 264);
            grpSeguimiento.Size = new Size(444, 148);
            grpSeguimiento.Name = "grpSeguimiento";
            grpSeguimiento.TabIndex = 2;
            grpSeguimiento.Text = "Seguimiento por guía";
            grpSeguimiento.TabStop = false;
            // 
            // lblGuia
            // 
            lblGuia.AutoSize = true;
            lblGuia.Location = new Point(12, 30);
            lblGuia.Name = "lblGuia";
            lblGuia.TabIndex = 0;
            lblGuia.Text = "Guía:";
            // 
            // txtGuia
            // 
            txtGuia.Location = new Point(60, 27);
            txtGuia.Size = new Size(250, 23);
            txtGuia.Name = "txtGuia";
            txtGuia.TabIndex = 1;
            txtGuia.CharacterCasing = CharacterCasing.Upper;
            // 
            // btnConsultarGuia
            // 
            btnConsultarGuia.Location = new Point(318, 25);
            btnConsultarGuia.Size = new Size(110, 27);
            btnConsultarGuia.Name = "btnConsultarGuia";
            btnConsultarGuia.TabIndex = 2;
            btnConsultarGuia.Text = "Consultar";
            btnConsultarGuia.UseVisualStyleBackColor = true;
            // 
            // lblSeguimiento
            // 
            lblSeguimiento.Location = new Point(12, 60);
            lblSeguimiento.Size = new Size(420, 80);
            lblSeguimiento.Name = "lblSeguimiento";
            lblSeguimiento.TabIndex = 3;
            lblSeguimiento.Text = "";
            // 
            // txtResultado
            // 
            txtResultado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtResultado.Multiline = true;
            txtResultado.ReadOnly = true;
            txtResultado.ScrollBars = ScrollBars.Vertical;
            txtResultado.Font = new Font("Consolas", 9F);
            txtResultado.Location = new Point(12, 420);
            txtResultado.Size = new Size(1216, 208);
            txtResultado.Name = "txtResultado";
            txtResultado.TabIndex = 3;
            // 
            // UcDespacho
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtResultado);
            Controls.Add(grpSeguimiento);
            Controls.Add(grpAcciones);
            Controls.Add(dgvPedidos);
            Name = "UcDespacho";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).EndInit();
            grpSeguimiento.ResumeLayout(false);
            grpSeguimiento.PerformLayout();
            grpAcciones.ResumeLayout(false);
            grpAcciones.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvPedidos;
        private GroupBox grpAcciones;
        private Button btnDespachar;
        private Label lblFechaEntrega;
        private DateTimePicker dtpEntrega;
        private Button btnEntregar;
        private Label lblAccion;
        private GroupBox grpSeguimiento;
        private Label lblGuia;
        private TextBox txtGuia;
        private Button btnConsultarGuia;
        private Label lblSeguimiento;
        private TextBox txtResultado;
    }
}
