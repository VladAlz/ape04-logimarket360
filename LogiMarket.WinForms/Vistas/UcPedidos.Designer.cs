namespace LogiMarket.WinForms.Vistas
{
    partial class UcPedidos
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
            lblFiltro = new Label();
            txtFiltro = new TextBox();
            btnFiltrar = new Button();
            btnLimpiarFiltro = new Button();
            lblResultadoFiltro = new Label();
            dgvPedidos = new DataGridView();
            grpDetalle = new GroupBox();
            dgvLineas = new DataGridView();
            dgvHistorial = new DataGridView();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
            btnCancelar = new Button();
            grpDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLineas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
            SuspendLayout();
            // 
            // lblFiltro
            // 
            lblFiltro.AutoSize = true;
            lblFiltro.Location = new Point(12, 15);
            lblFiltro.Name = "lblFiltro";
            lblFiltro.TabIndex = 0;
            lblFiltro.Text = "Filtro:";
            // 
            // txtFiltro
            // 
            txtFiltro.Location = new Point(60, 12);
            txtFiltro.Size = new Size(400, 23);
            txtFiltro.Name = "txtFiltro";
            txtFiltro.TabIndex = 1;
            // 
            // btnFiltrar
            // 
            btnFiltrar.Location = new Point(468, 11);
            btnFiltrar.Size = new Size(90, 25);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.TabIndex = 2;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiarFiltro
            // 
            btnLimpiarFiltro.Location = new Point(564, 11);
            btnLimpiarFiltro.Size = new Size(90, 25);
            btnLimpiarFiltro.Name = "btnLimpiarFiltro";
            btnLimpiarFiltro.TabIndex = 3;
            btnLimpiarFiltro.Text = "Limpiar";
            btnLimpiarFiltro.UseVisualStyleBackColor = true;
            // 
            // lblResultadoFiltro
            // 
            lblResultadoFiltro.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblResultadoFiltro.Location = new Point(662, 15);
            lblResultadoFiltro.Size = new Size(566, 20);
            lblResultadoFiltro.Name = "lblResultadoFiltro";
            lblResultadoFiltro.TabIndex = 4;
            lblResultadoFiltro.Text = "";
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
            dgvPedidos.Location = new Point(12, 44);
            dgvPedidos.Size = new Size(1216, 300);
            dgvPedidos.Name = "dgvPedidos";
            dgvPedidos.TabIndex = 5;
            // 
            // grpDetalle
            // 
            grpDetalle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpDetalle.Controls.Add(dgvLineas);
            grpDetalle.Controls.Add(dgvHistorial);
            grpDetalle.Controls.Add(lblMotivo);
            grpDetalle.Controls.Add(txtMotivo);
            grpDetalle.Controls.Add(btnCancelar);
            grpDetalle.Location = new Point(12, 352);
            grpDetalle.Size = new Size(1216, 276);
            grpDetalle.Name = "grpDetalle";
            grpDetalle.TabIndex = 6;
            grpDetalle.Text = "Detalle del pedido seleccionado";
            grpDetalle.TabStop = false;
            // 
            // dgvLineas
            // 
            dgvLineas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgvLineas.AllowUserToAddRows = false;
            dgvLineas.AllowUserToDeleteRows = false;
            dgvLineas.AllowUserToResizeRows = false;
            dgvLineas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLineas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLineas.MultiSelect = false;
            dgvLineas.ReadOnly = true;
            dgvLineas.RowHeadersVisible = false;
            dgvLineas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLineas.Location = new Point(12, 22);
            dgvLineas.Size = new Size(600, 206);
            dgvLineas.Name = "dgvLineas";
            dgvLineas.TabIndex = 0;
            // 
            // dgvHistorial
            // 
            dgvHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            dgvHistorial.AllowUserToResizeRows = false;
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorial.MultiSelect = false;
            dgvHistorial.ReadOnly = true;
            dgvHistorial.RowHeadersVisible = false;
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.Location = new Point(622, 22);
            dgvHistorial.Size = new Size(582, 206);
            dgvHistorial.Name = "dgvHistorial";
            dgvHistorial.TabIndex = 1;
            // 
            // lblMotivo
            // 
            lblMotivo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblMotivo.AutoSize = true;
            lblMotivo.Location = new Point(12, 240);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.TabIndex = 2;
            lblMotivo.Text = "Motivo:";
            // 
            // txtMotivo
            // 
            txtMotivo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtMotivo.Location = new Point(70, 237);
            txtMotivo.Size = new Size(300, 23);
            txtMotivo.Name = "txtMotivo";
            txtMotivo.TabIndex = 3;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelar.Location = new Point(380, 235);
            btnCancelar.Size = new Size(160, 28);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar pedido";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // UcPedidos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(grpDetalle);
            Controls.Add(dgvPedidos);
            Controls.Add(lblResultadoFiltro);
            Controls.Add(btnLimpiarFiltro);
            Controls.Add(btnFiltrar);
            Controls.Add(txtFiltro);
            Controls.Add(lblFiltro);
            Name = "UcPedidos";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLineas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
            grpDetalle.ResumeLayout(false);
            grpDetalle.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFiltro;
        private TextBox txtFiltro;
        private Button btnFiltrar;
        private Button btnLimpiarFiltro;
        private Label lblResultadoFiltro;
        private DataGridView dgvPedidos;
        private GroupBox grpDetalle;
        private DataGridView dgvLineas;
        private DataGridView dgvHistorial;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private Button btnCancelar;
    }
}
