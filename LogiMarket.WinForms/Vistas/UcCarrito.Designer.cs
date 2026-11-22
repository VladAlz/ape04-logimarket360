namespace LogiMarket.WinForms.Vistas
{
    partial class UcCarrito
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
            grpDatos = new GroupBox();
            lblBorrador = new Label();
            cmbBorrador = new ComboBox();
            btnNuevoBorrador = new Button();
            btnDuplicar = new Button();
            lblCliente = new Label();
            cmbCliente = new ComboBox();
            lblEntrega = new Label();
            cmbEntrega = new ComboBox();
            lblEstadoBorrador = new Label();
            grpLinea = new GroupBox();
            lblElemento = new Label();
            cmbElemento = new ComboBox();
            lblCantidad = new Label();
            nudCantidad = new NumericUpDown();
            btnAgregar = new Button();
            btnCambiarCantidad = new Button();
            btnQuitar = new Button();
            dgvLineas = new DataGridView();
            grpTotales = new GroupBox();
            lblSubtotalTitulo = new Label();
            lblSubtotal = new Label();
            lblPoliticaTitulo = new Label();
            lblPolitica = new Label();
            lblDescuentoTitulo = new Label();
            lblDescuento = new Label();
            lblTarifaTitulo = new Label();
            lblTarifa = new Label();
            lblTotalTitulo = new Label();
            lblTotal = new Label();
            lblUnidadesTitulo = new Label();
            lblUnidades = new Label();
            lblHistorial = new Label();
            btnDeshacer = new Button();
            btnRehacer = new Button();
            btnValidar = new Button();
            txtMensajes = new TextBox();
            grpDatos.SuspendLayout();
            grpLinea.SuspendLayout();
            grpTotales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLineas).BeginInit();
            SuspendLayout();
            // 
            // grpDatos
            // 
            grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpDatos.Controls.Add(lblBorrador);
            grpDatos.Controls.Add(cmbBorrador);
            grpDatos.Controls.Add(btnNuevoBorrador);
            grpDatos.Controls.Add(btnDuplicar);
            grpDatos.Controls.Add(lblCliente);
            grpDatos.Controls.Add(cmbCliente);
            grpDatos.Controls.Add(lblEntrega);
            grpDatos.Controls.Add(cmbEntrega);
            grpDatos.Controls.Add(lblEstadoBorrador);
            grpDatos.Location = new Point(12, 8);
            grpDatos.Size = new Size(1216, 100);
            grpDatos.Name = "grpDatos";
            grpDatos.TabIndex = 0;
            grpDatos.Text = "Borrador";
            grpDatos.TabStop = false;
            // 
            // lblBorrador
            // 
            lblBorrador.AutoSize = true;
            lblBorrador.Location = new Point(12, 28);
            lblBorrador.Name = "lblBorrador";
            lblBorrador.TabIndex = 0;
            lblBorrador.Text = "Borrador:";
            // 
            // cmbBorrador
            // 
            cmbBorrador.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBorrador.FormattingEnabled = true;
            cmbBorrador.Location = new Point(85, 25);
            cmbBorrador.Size = new Size(420, 23);
            cmbBorrador.Name = "cmbBorrador";
            cmbBorrador.TabIndex = 1;
            // 
            // btnNuevoBorrador
            // 
            btnNuevoBorrador.Location = new Point(515, 23);
            btnNuevoBorrador.Size = new Size(130, 27);
            btnNuevoBorrador.Name = "btnNuevoBorrador";
            btnNuevoBorrador.TabIndex = 2;
            btnNuevoBorrador.Text = "Nuevo borrador";
            btnNuevoBorrador.UseVisualStyleBackColor = true;
            // 
            // btnDuplicar
            // 
            btnDuplicar.Location = new Point(651, 23);
            btnDuplicar.Size = new Size(150, 27);
            btnDuplicar.Name = "btnDuplicar";
            btnDuplicar.TabIndex = 3;
            btnDuplicar.Text = "Duplicar borrador";
            btnDuplicar.UseVisualStyleBackColor = true;
            // 
            // lblCliente
            // 
            lblCliente.AutoSize = true;
            lblCliente.Location = new Point(12, 62);
            lblCliente.Name = "lblCliente";
            lblCliente.TabIndex = 4;
            lblCliente.Text = "Cliente:";
            // 
            // cmbCliente
            // 
            cmbCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCliente.FormattingEnabled = true;
            cmbCliente.Location = new Point(85, 59);
            cmbCliente.Size = new Size(300, 23);
            cmbCliente.Name = "cmbCliente";
            cmbCliente.TabIndex = 5;
            // 
            // lblEntrega
            // 
            lblEntrega.AutoSize = true;
            lblEntrega.Location = new Point(395, 62);
            lblEntrega.Name = "lblEntrega";
            lblEntrega.TabIndex = 6;
            lblEntrega.Text = "Entrega:";
            // 
            // cmbEntrega
            // 
            cmbEntrega.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEntrega.FormattingEnabled = true;
            cmbEntrega.Location = new Point(460, 59);
            cmbEntrega.Size = new Size(140, 23);
            cmbEntrega.Name = "cmbEntrega";
            cmbEntrega.TabIndex = 7;
            // 
            // lblEstadoBorrador
            // 
            lblEstadoBorrador.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblEstadoBorrador.Location = new Point(620, 62);
            lblEstadoBorrador.Size = new Size(580, 20);
            lblEstadoBorrador.Name = "lblEstadoBorrador";
            lblEstadoBorrador.TabIndex = 8;
            lblEstadoBorrador.Text = "";
            // 
            // grpLinea
            // 
            grpLinea.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpLinea.Controls.Add(lblElemento);
            grpLinea.Controls.Add(cmbElemento);
            grpLinea.Controls.Add(lblCantidad);
            grpLinea.Controls.Add(nudCantidad);
            grpLinea.Controls.Add(btnAgregar);
            grpLinea.Controls.Add(btnCambiarCantidad);
            grpLinea.Controls.Add(btnQuitar);
            grpLinea.Location = new Point(12, 114);
            grpLinea.Size = new Size(1216, 64);
            grpLinea.Name = "grpLinea";
            grpLinea.TabIndex = 1;
            grpLinea.Text = "Línea";
            grpLinea.TabStop = false;
            // 
            // lblElemento
            // 
            lblElemento.AutoSize = true;
            lblElemento.Location = new Point(12, 28);
            lblElemento.Name = "lblElemento";
            lblElemento.TabIndex = 0;
            lblElemento.Text = "Producto o kit:";
            // 
            // cmbElemento
            // 
            cmbElemento.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbElemento.FormattingEnabled = true;
            cmbElemento.Location = new Point(115, 25);
            cmbElemento.Size = new Size(420, 23);
            cmbElemento.Name = "cmbElemento";
            cmbElemento.TabIndex = 1;
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(545, 28);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.TabIndex = 2;
            lblCantidad.Text = "Cantidad:";
            // 
            // nudCantidad
            // 
            nudCantidad.Location = new Point(610, 25);
            nudCantidad.Size = new Size(70, 23);
            nudCantidad.Name = "nudCantidad";
            nudCantidad.TabIndex = 3;
            nudCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCantidad.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(690, 23);
            btnAgregar.Size = new Size(100, 27);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.TabIndex = 4;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = true;
            // 
            // btnCambiarCantidad
            // 
            btnCambiarCantidad.Location = new Point(796, 23);
            btnCambiarCantidad.Size = new Size(140, 27);
            btnCambiarCantidad.Name = "btnCambiarCantidad";
            btnCambiarCantidad.TabIndex = 5;
            btnCambiarCantidad.Text = "Cambiar cantidad";
            btnCambiarCantidad.UseVisualStyleBackColor = true;
            // 
            // btnQuitar
            // 
            btnQuitar.Location = new Point(942, 23);
            btnQuitar.Size = new Size(100, 27);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.TabIndex = 6;
            btnQuitar.Text = "Quitar línea";
            btnQuitar.UseVisualStyleBackColor = true;
            // 
            // dgvLineas
            // 
            dgvLineas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLineas.AllowUserToAddRows = false;
            dgvLineas.AllowUserToDeleteRows = false;
            dgvLineas.AllowUserToResizeRows = false;
            dgvLineas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLineas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLineas.MultiSelect = false;
            dgvLineas.ReadOnly = true;
            dgvLineas.RowHeadersVisible = false;
            dgvLineas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLineas.Location = new Point(12, 186);
            dgvLineas.Size = new Size(860, 330);
            dgvLineas.Name = "dgvLineas";
            dgvLineas.TabIndex = 2;
            // 
            // grpTotales
            // 
            grpTotales.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            grpTotales.Controls.Add(lblSubtotalTitulo);
            grpTotales.Controls.Add(lblSubtotal);
            grpTotales.Controls.Add(lblPoliticaTitulo);
            grpTotales.Controls.Add(lblPolitica);
            grpTotales.Controls.Add(lblDescuentoTitulo);
            grpTotales.Controls.Add(lblDescuento);
            grpTotales.Controls.Add(lblTarifaTitulo);
            grpTotales.Controls.Add(lblTarifa);
            grpTotales.Controls.Add(lblTotalTitulo);
            grpTotales.Controls.Add(lblTotal);
            grpTotales.Controls.Add(lblUnidadesTitulo);
            grpTotales.Controls.Add(lblUnidades);
            grpTotales.Controls.Add(lblHistorial);
            grpTotales.Controls.Add(btnDeshacer);
            grpTotales.Controls.Add(btnRehacer);
            grpTotales.Controls.Add(btnValidar);
            grpTotales.Location = new Point(884, 186);
            grpTotales.Size = new Size(344, 330);
            grpTotales.Name = "grpTotales";
            grpTotales.TabIndex = 3;
            grpTotales.Text = "Totales";
            grpTotales.TabStop = false;
            // 
            // lblSubtotalTitulo
            // 
            lblSubtotalTitulo.AutoSize = true;
            lblSubtotalTitulo.Location = new Point(12, 30);
            lblSubtotalTitulo.Name = "lblSubtotalTitulo";
            lblSubtotalTitulo.TabIndex = 0;
            lblSubtotalTitulo.Text = "Subtotal:";
            // 
            // lblSubtotal
            // 
            lblSubtotal.Location = new Point(180, 28);
            lblSubtotal.Size = new Size(150, 20);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.TabIndex = 1;
            lblSubtotal.Text = "-";
            lblSubtotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblPoliticaTitulo
            // 
            lblPoliticaTitulo.AutoSize = true;
            lblPoliticaTitulo.Location = new Point(12, 58);
            lblPoliticaTitulo.Name = "lblPoliticaTitulo";
            lblPoliticaTitulo.TabIndex = 2;
            lblPoliticaTitulo.Text = "Política:";
            // 
            // lblPolitica
            // 
            lblPolitica.Location = new Point(180, 56);
            lblPolitica.Size = new Size(150, 20);
            lblPolitica.Name = "lblPolitica";
            lblPolitica.TabIndex = 3;
            lblPolitica.Text = "-";
            lblPolitica.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDescuentoTitulo
            // 
            lblDescuentoTitulo.AutoSize = true;
            lblDescuentoTitulo.Location = new Point(12, 86);
            lblDescuentoTitulo.Name = "lblDescuentoTitulo";
            lblDescuentoTitulo.TabIndex = 4;
            lblDescuentoTitulo.Text = "Descuento:";
            // 
            // lblDescuento
            // 
            lblDescuento.Location = new Point(180, 84);
            lblDescuento.Size = new Size(150, 20);
            lblDescuento.Name = "lblDescuento";
            lblDescuento.TabIndex = 5;
            lblDescuento.Text = "-";
            lblDescuento.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTarifaTitulo
            // 
            lblTarifaTitulo.AutoSize = true;
            lblTarifaTitulo.Location = new Point(12, 114);
            lblTarifaTitulo.Name = "lblTarifaTitulo";
            lblTarifaTitulo.TabIndex = 6;
            lblTarifaTitulo.Text = "Tarifa de envío:";
            // 
            // lblTarifa
            // 
            lblTarifa.Location = new Point(180, 112);
            lblTarifa.Size = new Size(150, 20);
            lblTarifa.Name = "lblTarifa";
            lblTarifa.TabIndex = 7;
            lblTarifa.Text = "-";
            lblTarifa.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalTitulo
            // 
            lblTotalTitulo.AutoSize = true;
            lblTotalTitulo.Location = new Point(12, 142);
            lblTotalTitulo.Name = "lblTotalTitulo";
            lblTotalTitulo.TabIndex = 8;
            lblTotalTitulo.Text = "Total USD:";
            // 
            // lblTotal
            // 
            lblTotal.Location = new Point(180, 140);
            lblTotal.Size = new Size(150, 20);
            lblTotal.Name = "lblTotal";
            lblTotal.TabIndex = 9;
            lblTotal.Text = "-";
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblUnidadesTitulo
            // 
            lblUnidadesTitulo.AutoSize = true;
            lblUnidadesTitulo.Location = new Point(12, 170);
            lblUnidadesTitulo.Name = "lblUnidadesTitulo";
            lblUnidadesTitulo.TabIndex = 10;
            lblUnidadesTitulo.Text = "Unidades físicas:";
            // 
            // lblUnidades
            // 
            lblUnidades.Location = new Point(180, 168);
            lblUnidades.Size = new Size(150, 20);
            lblUnidades.Name = "lblUnidades";
            lblUnidades.TabIndex = 11;
            lblUnidades.Text = "-";
            lblUnidades.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblHistorial
            // 
            lblHistorial.Location = new Point(12, 212);
            lblHistorial.Size = new Size(320, 20);
            lblHistorial.Name = "lblHistorial";
            lblHistorial.TabIndex = 12;
            lblHistorial.Text = "Deshacer: 0 | Rehacer: 0";
            // 
            // btnDeshacer
            // 
            btnDeshacer.Location = new Point(12, 240);
            btnDeshacer.Size = new Size(100, 30);
            btnDeshacer.Name = "btnDeshacer";
            btnDeshacer.TabIndex = 13;
            btnDeshacer.Text = "Deshacer";
            btnDeshacer.UseVisualStyleBackColor = true;
            // 
            // btnRehacer
            // 
            btnRehacer.Location = new Point(118, 240);
            btnRehacer.Size = new Size(100, 30);
            btnRehacer.Name = "btnRehacer";
            btnRehacer.TabIndex = 14;
            btnRehacer.Text = "Rehacer";
            btnRehacer.UseVisualStyleBackColor = true;
            // 
            // btnValidar
            // 
            btnValidar.Location = new Point(12, 280);
            btnValidar.Size = new Size(206, 32);
            btnValidar.Name = "btnValidar";
            btnValidar.TabIndex = 15;
            btnValidar.Text = "Validar pedido";
            btnValidar.UseVisualStyleBackColor = true;
            // 
            // txtMensajes
            // 
            txtMensajes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMensajes.Multiline = true;
            txtMensajes.ReadOnly = true;
            txtMensajes.ScrollBars = ScrollBars.Vertical;
            txtMensajes.Font = new Font("Consolas", 9F);
            txtMensajes.Location = new Point(12, 524);
            txtMensajes.Size = new Size(1216, 104);
            txtMensajes.Name = "txtMensajes";
            txtMensajes.TabIndex = 4;
            // 
            // UcCarrito
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtMensajes);
            Controls.Add(grpTotales);
            Controls.Add(dgvLineas);
            Controls.Add(grpLinea);
            Controls.Add(grpDatos);
            Name = "UcCarrito";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLineas).EndInit();
            grpTotales.ResumeLayout(false);
            grpTotales.PerformLayout();
            grpLinea.ResumeLayout(false);
            grpLinea.PerformLayout();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpDatos;
        private Label lblBorrador;
        private ComboBox cmbBorrador;
        private Button btnNuevoBorrador;
        private Button btnDuplicar;
        private Label lblCliente;
        private ComboBox cmbCliente;
        private Label lblEntrega;
        private ComboBox cmbEntrega;
        private Label lblEstadoBorrador;
        private GroupBox grpLinea;
        private Label lblElemento;
        private ComboBox cmbElemento;
        private Label lblCantidad;
        private NumericUpDown nudCantidad;
        private Button btnAgregar;
        private Button btnCambiarCantidad;
        private Button btnQuitar;
        private DataGridView dgvLineas;
        private GroupBox grpTotales;
        private Label lblSubtotalTitulo;
        private Label lblSubtotal;
        private Label lblPoliticaTitulo;
        private Label lblPolitica;
        private Label lblDescuentoTitulo;
        private Label lblDescuento;
        private Label lblTarifaTitulo;
        private Label lblTarifa;
        private Label lblTotalTitulo;
        private Label lblTotal;
        private Label lblUnidadesTitulo;
        private Label lblUnidades;
        private Label lblHistorial;
        private Button btnDeshacer;
        private Button btnRehacer;
        private Button btnValidar;
        private TextBox txtMensajes;
    }
}
