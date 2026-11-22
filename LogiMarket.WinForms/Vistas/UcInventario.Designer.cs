namespace LogiMarket.WinForms.Vistas
{
    partial class UcInventario
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
            dgvStock = new DataGridView();
            grpAjuste = new GroupBox();
            lblSku = new Label();
            cmbSku = new ComboBox();
            lblNuevoStock = new Label();
            nudNuevoStock = new NumericUpDown();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
            btnAjustar = new Button();
            lblAjuste = new Label();
            lblMovimientos = new Label();
            cmbFiltroMovimientos = new ComboBox();
            btnVerMovimientos = new Button();
            dgvMovimientos = new DataGridView();
            grpAjuste.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudNuevoStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).BeginInit();
            SuspendLayout();
            // 
            // dgvStock
            // 
            dgvStock.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgvStock.AllowUserToAddRows = false;
            dgvStock.AllowUserToDeleteRows = false;
            dgvStock.AllowUserToResizeRows = false;
            dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStock.MultiSelect = false;
            dgvStock.ReadOnly = true;
            dgvStock.RowHeadersVisible = false;
            dgvStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStock.Location = new Point(12, 12);
            dgvStock.Size = new Size(500, 420);
            dgvStock.Name = "dgvStock";
            dgvStock.TabIndex = 0;
            // 
            // grpAjuste
            // 
            grpAjuste.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            grpAjuste.Controls.Add(lblSku);
            grpAjuste.Controls.Add(cmbSku);
            grpAjuste.Controls.Add(lblNuevoStock);
            grpAjuste.Controls.Add(nudNuevoStock);
            grpAjuste.Controls.Add(lblMotivo);
            grpAjuste.Controls.Add(txtMotivo);
            grpAjuste.Controls.Add(btnAjustar);
            grpAjuste.Controls.Add(lblAjuste);
            grpAjuste.Location = new Point(12, 440);
            grpAjuste.Size = new Size(500, 188);
            grpAjuste.Name = "grpAjuste";
            grpAjuste.TabIndex = 1;
            grpAjuste.Text = "Ajuste de existencias";
            grpAjuste.TabStop = false;
            // 
            // lblSku
            // 
            lblSku.AutoSize = true;
            lblSku.Location = new Point(12, 30);
            lblSku.Name = "lblSku";
            lblSku.TabIndex = 0;
            lblSku.Text = "Producto:";
            // 
            // cmbSku
            // 
            cmbSku.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSku.FormattingEnabled = true;
            cmbSku.Location = new Point(100, 27);
            cmbSku.Size = new Size(200, 23);
            cmbSku.Name = "cmbSku";
            cmbSku.TabIndex = 1;
            // 
            // lblNuevoStock
            // 
            lblNuevoStock.AutoSize = true;
            lblNuevoStock.Location = new Point(12, 64);
            lblNuevoStock.Name = "lblNuevoStock";
            lblNuevoStock.TabIndex = 2;
            lblNuevoStock.Text = "Nuevo stock:";
            // 
            // nudNuevoStock
            // 
            nudNuevoStock.Location = new Point(100, 61);
            nudNuevoStock.Size = new Size(100, 23);
            nudNuevoStock.Name = "nudNuevoStock";
            nudNuevoStock.TabIndex = 3;
            nudNuevoStock.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // lblMotivo
            // 
            lblMotivo.AutoSize = true;
            lblMotivo.Location = new Point(12, 98);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.TabIndex = 4;
            lblMotivo.Text = "Motivo:";
            // 
            // txtMotivo
            // 
            txtMotivo.Location = new Point(100, 95);
            txtMotivo.Size = new Size(380, 23);
            txtMotivo.Name = "txtMotivo";
            txtMotivo.TabIndex = 5;
            // 
            // btnAjustar
            // 
            btnAjustar.Location = new Point(100, 130);
            btnAjustar.Size = new Size(150, 32);
            btnAjustar.Name = "btnAjustar";
            btnAjustar.TabIndex = 6;
            btnAjustar.Text = "Registrar ajuste";
            btnAjustar.UseVisualStyleBackColor = true;
            // 
            // lblAjuste
            // 
            lblAjuste.Location = new Point(258, 128);
            lblAjuste.Size = new Size(230, 50);
            lblAjuste.Name = "lblAjuste";
            lblAjuste.TabIndex = 7;
            lblAjuste.Text = "";
            // 
            // lblMovimientos
            // 
            lblMovimientos.AutoSize = true;
            lblMovimientos.Location = new Point(524, 15);
            lblMovimientos.Name = "lblMovimientos";
            lblMovimientos.TabIndex = 2;
            lblMovimientos.Text = "Movimientos de:";
            // 
            // cmbFiltroMovimientos
            // 
            cmbFiltroMovimientos.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroMovimientos.FormattingEnabled = true;
            cmbFiltroMovimientos.Location = new Point(630, 12);
            cmbFiltroMovimientos.Size = new Size(160, 23);
            cmbFiltroMovimientos.Name = "cmbFiltroMovimientos";
            cmbFiltroMovimientos.TabIndex = 3;
            // 
            // btnVerMovimientos
            // 
            btnVerMovimientos.Location = new Point(798, 11);
            btnVerMovimientos.Size = new Size(100, 25);
            btnVerMovimientos.Name = "btnVerMovimientos";
            btnVerMovimientos.TabIndex = 4;
            btnVerMovimientos.Text = "Ver";
            btnVerMovimientos.UseVisualStyleBackColor = true;
            // 
            // dgvMovimientos
            // 
            dgvMovimientos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMovimientos.AllowUserToAddRows = false;
            dgvMovimientos.AllowUserToDeleteRows = false;
            dgvMovimientos.AllowUserToResizeRows = false;
            dgvMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovimientos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMovimientos.MultiSelect = false;
            dgvMovimientos.ReadOnly = true;
            dgvMovimientos.RowHeadersVisible = false;
            dgvMovimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMovimientos.Location = new Point(524, 44);
            dgvMovimientos.Size = new Size(704, 584);
            dgvMovimientos.Name = "dgvMovimientos";
            dgvMovimientos.TabIndex = 5;
            // 
            // UcInventario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvMovimientos);
            Controls.Add(btnVerMovimientos);
            Controls.Add(cmbFiltroMovimientos);
            Controls.Add(lblMovimientos);
            Controls.Add(grpAjuste);
            Controls.Add(dgvStock);
            Name = "UcInventario";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)dgvStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudNuevoStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).EndInit();
            grpAjuste.ResumeLayout(false);
            grpAjuste.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvStock;
        private GroupBox grpAjuste;
        private Label lblSku;
        private ComboBox cmbSku;
        private Label lblNuevoStock;
        private NumericUpDown nudNuevoStock;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private Button btnAjustar;
        private Label lblAjuste;
        private Label lblMovimientos;
        private ComboBox cmbFiltroMovimientos;
        private Button btnVerMovimientos;
        private DataGridView dgvMovimientos;
    }
}
