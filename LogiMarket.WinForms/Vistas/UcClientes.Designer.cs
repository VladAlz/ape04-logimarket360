namespace LogiMarket.WinForms.Vistas
{
    partial class UcClientes
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
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            btnLimpiar = new Button();
            dgvClientes = new DataGridView();
            grpCliente = new GroupBox();
            lblId = new Label();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblIdentificacion = new Label();
            txtIdentificacion = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblCategoria = new Label();
            cmbCategoria = new ComboBox();
            lblCanal = new Label();
            cmbCanal = new ComboBox();
            chkActivo = new CheckBox();
            btnNuevoCliente = new Button();
            btnGuardarCliente = new Button();
            lblMensaje = new Label();
            grpCliente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            SuspendLayout();
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Location = new Point(12, 15);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(70, 12);
            txtBuscar.Size = new Size(300, 23);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.TabIndex = 1;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(378, 11);
            btnBuscar.Size = new Size(90, 25);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(474, 11);
            btnLimpiar.Size = new Size(90, 25);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.TabIndex = 3;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // dgvClientes
            // 
            dgvClientes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dgvClientes.AllowUserToResizeRows = false;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.MultiSelect = false;
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Location = new Point(12, 44);
            dgvClientes.Size = new Size(760, 584);
            dgvClientes.Name = "dgvClientes";
            dgvClientes.TabIndex = 4;
            // 
            // grpCliente
            // 
            grpCliente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpCliente.Controls.Add(lblId);
            grpCliente.Controls.Add(lblNombre);
            grpCliente.Controls.Add(txtNombre);
            grpCliente.Controls.Add(lblIdentificacion);
            grpCliente.Controls.Add(txtIdentificacion);
            grpCliente.Controls.Add(lblEmail);
            grpCliente.Controls.Add(txtEmail);
            grpCliente.Controls.Add(lblTelefono);
            grpCliente.Controls.Add(txtTelefono);
            grpCliente.Controls.Add(lblCategoria);
            grpCliente.Controls.Add(cmbCategoria);
            grpCliente.Controls.Add(lblCanal);
            grpCliente.Controls.Add(cmbCanal);
            grpCliente.Controls.Add(chkActivo);
            grpCliente.Controls.Add(btnNuevoCliente);
            grpCliente.Controls.Add(btnGuardarCliente);
            grpCliente.Location = new Point(784, 8);
            grpCliente.Size = new Size(444, 330);
            grpCliente.Name = "grpCliente";
            grpCliente.TabIndex = 5;
            grpCliente.Text = "Datos del cliente";
            grpCliente.TabStop = false;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Location = new Point(12, 26);
            lblId.Name = "lblId";
            lblId.TabIndex = 0;
            lblId.Text = "Cliente nuevo";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(12, 58);
            lblNombre.Name = "lblNombre";
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(110, 55);
            txtNombre.Size = new Size(310, 23);
            txtNombre.Name = "txtNombre";
            txtNombre.TabIndex = 2;
            // 
            // lblIdentificacion
            // 
            lblIdentificacion.AutoSize = true;
            lblIdentificacion.Location = new Point(12, 90);
            lblIdentificacion.Name = "lblIdentificacion";
            lblIdentificacion.TabIndex = 3;
            lblIdentificacion.Text = "Identificación:";
            // 
            // txtIdentificacion
            // 
            txtIdentificacion.Location = new Point(110, 87);
            txtIdentificacion.Size = new Size(150, 23);
            txtIdentificacion.Name = "txtIdentificacion";
            txtIdentificacion.TabIndex = 4;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(12, 122);
            lblEmail.Name = "lblEmail";
            lblEmail.TabIndex = 5;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(110, 119);
            txtEmail.Size = new Size(310, 23);
            txtEmail.Name = "txtEmail";
            txtEmail.TabIndex = 6;
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(12, 154);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.TabIndex = 7;
            lblTelefono.Text = "Teléfono:";
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(110, 151);
            txtTelefono.Size = new Size(150, 23);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.TabIndex = 8;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(12, 186);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.TabIndex = 9;
            lblCategoria.Text = "Categoría:";
            // 
            // cmbCategoria
            // 
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Location = new Point(110, 183);
            cmbCategoria.Size = new Size(150, 23);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.TabIndex = 10;
            // 
            // lblCanal
            // 
            lblCanal.AutoSize = true;
            lblCanal.Location = new Point(12, 218);
            lblCanal.Name = "lblCanal";
            lblCanal.TabIndex = 11;
            lblCanal.Text = "Aviso por:";
            // 
            // cmbCanal
            // 
            cmbCanal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCanal.FormattingEnabled = true;
            cmbCanal.Location = new Point(110, 215);
            cmbCanal.Size = new Size(150, 23);
            cmbCanal.Name = "cmbCanal";
            cmbCanal.TabIndex = 12;
            // 
            // chkActivo
            // 
            chkActivo.AutoSize = true;
            chkActivo.Location = new Point(110, 248);
            chkActivo.Name = "chkActivo";
            chkActivo.TabIndex = 13;
            chkActivo.Text = "Activo";
            chkActivo.TabStop = true;
            // 
            // btnNuevoCliente
            // 
            btnNuevoCliente.Location = new Point(110, 280);
            btnNuevoCliente.Size = new Size(100, 30);
            btnNuevoCliente.Name = "btnNuevoCliente";
            btnNuevoCliente.TabIndex = 14;
            btnNuevoCliente.Text = "Nuevo";
            btnNuevoCliente.UseVisualStyleBackColor = true;
            // 
            // btnGuardarCliente
            // 
            btnGuardarCliente.Location = new Point(216, 280);
            btnGuardarCliente.Size = new Size(100, 30);
            btnGuardarCliente.Name = "btnGuardarCliente";
            btnGuardarCliente.TabIndex = 15;
            btnGuardarCliente.Text = "Guardar";
            btnGuardarCliente.UseVisualStyleBackColor = true;
            // 
            // lblMensaje
            // 
            lblMensaje.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblMensaje.Location = new Point(784, 348);
            lblMensaje.Size = new Size(444, 90);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.TabIndex = 6;
            lblMensaje.Text = "";
            // 
            // UcClientes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblMensaje);
            Controls.Add(grpCliente);
            Controls.Add(dgvClientes);
            Controls.Add(btnLimpiar);
            Controls.Add(btnBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Name = "UcClientes";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            grpCliente.ResumeLayout(false);
            grpCliente.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBuscar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Button btnLimpiar;
        private DataGridView dgvClientes;
        private GroupBox grpCliente;
        private Label lblId;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblIdentificacion;
        private TextBox txtIdentificacion;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblCategoria;
        private ComboBox cmbCategoria;
        private Label lblCanal;
        private ComboBox cmbCanal;
        private CheckBox chkActivo;
        private Button btnNuevoCliente;
        private Button btnGuardarCliente;
        private Label lblMensaje;
    }
}
