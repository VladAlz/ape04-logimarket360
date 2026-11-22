namespace LogiMarket.WinForms.Vistas
{
    partial class UcPanel
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
            grpEstados = new GroupBox();
            dgvEstados = new DataGridView();
            lblVentasTitulo = new Label();
            lblVentas = new Label();
            lblOutboxTitulo = new Label();
            lblOutbox = new Label();
            btnActualizar = new Button();
            grpAlertas = new GroupBox();
            dgvAlertas = new DataGridView();
            grpMetricas = new GroupBox();
            dgvMetricas = new DataGridView();
            lblInstancia = new Label();
            grpEstados.SuspendLayout();
            grpAlertas.SuspendLayout();
            grpMetricas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEstados).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvAlertas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMetricas).BeginInit();
            SuspendLayout();
            // 
            // grpEstados
            // 
            grpEstados.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            grpEstados.Controls.Add(dgvEstados);
            grpEstados.Location = new Point(12, 8);
            grpEstados.Size = new Size(380, 280);
            grpEstados.Name = "grpEstados";
            grpEstados.TabIndex = 0;
            grpEstados.Text = "Pedidos por estado";
            grpEstados.TabStop = false;
            // 
            // dgvEstados
            // 
            dgvEstados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEstados.AllowUserToAddRows = false;
            dgvEstados.AllowUserToDeleteRows = false;
            dgvEstados.AllowUserToResizeRows = false;
            dgvEstados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEstados.MultiSelect = false;
            dgvEstados.ReadOnly = true;
            dgvEstados.RowHeadersVisible = false;
            dgvEstados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstados.Location = new Point(12, 22);
            dgvEstados.Size = new Size(356, 246);
            dgvEstados.Name = "dgvEstados";
            dgvEstados.TabIndex = 0;
            // 
            // lblVentasTitulo
            // 
            lblVentasTitulo.AutoSize = true;
            lblVentasTitulo.Location = new Point(14, 302);
            lblVentasTitulo.Name = "lblVentasTitulo";
            lblVentasTitulo.TabIndex = 1;
            lblVentasTitulo.Text = "Ventas cobradas (USD):";
            // 
            // lblVentas
            // 
            lblVentas.Location = new Point(230, 300);
            lblVentas.Size = new Size(160, 20);
            lblVentas.Name = "lblVentas";
            lblVentas.TabIndex = 2;
            lblVentas.Text = "-";
            lblVentas.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblOutboxTitulo
            // 
            lblOutboxTitulo.AutoSize = true;
            lblOutboxTitulo.Location = new Point(14, 330);
            lblOutboxTitulo.Name = "lblOutboxTitulo";
            lblOutboxTitulo.TabIndex = 3;
            lblOutboxTitulo.Text = "Eventos Outbox pendientes:";
            // 
            // lblOutbox
            // 
            lblOutbox.Location = new Point(230, 328);
            lblOutbox.Size = new Size(160, 20);
            lblOutbox.Name = "lblOutbox";
            lblOutbox.TabIndex = 4;
            lblOutbox.Text = "-";
            lblOutbox.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(14, 364);
            btnActualizar.Size = new Size(120, 30);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.TabIndex = 5;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // grpAlertas
            // 
            grpAlertas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpAlertas.Controls.Add(dgvAlertas);
            grpAlertas.Location = new Point(404, 8);
            grpAlertas.Size = new Size(400, 620);
            grpAlertas.Name = "grpAlertas";
            grpAlertas.TabIndex = 6;
            grpAlertas.Text = "Alertas de existencias";
            grpAlertas.TabStop = false;
            // 
            // dgvAlertas
            // 
            dgvAlertas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAlertas.AllowUserToAddRows = false;
            dgvAlertas.AllowUserToDeleteRows = false;
            dgvAlertas.AllowUserToResizeRows = false;
            dgvAlertas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAlertas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAlertas.MultiSelect = false;
            dgvAlertas.ReadOnly = true;
            dgvAlertas.RowHeadersVisible = false;
            dgvAlertas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlertas.Location = new Point(12, 22);
            dgvAlertas.Size = new Size(376, 586);
            dgvAlertas.Name = "dgvAlertas";
            dgvAlertas.TabIndex = 0;
            // 
            // grpMetricas
            // 
            grpMetricas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            grpMetricas.Controls.Add(dgvMetricas);
            grpMetricas.Controls.Add(lblInstancia);
            grpMetricas.Location = new Point(816, 8);
            grpMetricas.Size = new Size(412, 620);
            grpMetricas.Name = "grpMetricas";
            grpMetricas.TabIndex = 7;
            grpMetricas.Text = "Métricas de la sesión";
            grpMetricas.TabStop = false;
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
            dgvMetricas.Size = new Size(388, 520);
            dgvMetricas.Name = "dgvMetricas";
            dgvMetricas.TabIndex = 0;
            // 
            // lblInstancia
            // 
            lblInstancia.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblInstancia.Location = new Point(12, 552);
            lblInstancia.Size = new Size(388, 56);
            lblInstancia.Name = "lblInstancia";
            lblInstancia.TabIndex = 1;
            lblInstancia.Text = "";
            // 
            // UcPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(grpMetricas);
            Controls.Add(grpAlertas);
            Controls.Add(btnActualizar);
            Controls.Add(lblOutbox);
            Controls.Add(lblOutboxTitulo);
            Controls.Add(lblVentas);
            Controls.Add(lblVentasTitulo);
            Controls.Add(grpEstados);
            Name = "UcPanel";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)dgvEstados).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvAlertas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMetricas).EndInit();
            grpMetricas.ResumeLayout(false);
            grpMetricas.PerformLayout();
            grpAlertas.ResumeLayout(false);
            grpAlertas.PerformLayout();
            grpEstados.ResumeLayout(false);
            grpEstados.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpEstados;
        private DataGridView dgvEstados;
        private Label lblVentasTitulo;
        private Label lblVentas;
        private Label lblOutboxTitulo;
        private Label lblOutbox;
        private Button btnActualizar;
        private GroupBox grpAlertas;
        private DataGridView dgvAlertas;
        private GroupBox grpMetricas;
        private DataGridView dgvMetricas;
        private Label lblInstancia;
    }
}
