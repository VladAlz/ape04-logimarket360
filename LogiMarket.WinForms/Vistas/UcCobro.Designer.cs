namespace LogiMarket.WinForms.Vistas
{
    partial class UcCobro
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
            grpPedido = new GroupBox();
            lblPedido = new Label();
            cmbPedido = new ComboBox();
            lblClienteTitulo = new Label();
            lblCliente = new Label();
            lblSubtotalTitulo = new Label();
            lblSubtotal = new Label();
            lblDescuentoTitulo = new Label();
            lblDescuento = new Label();
            lblTarifaTitulo = new Label();
            lblTarifa = new Label();
            lblTotalTitulo = new Label();
            lblTotal = new Label();
            lblProveedorTitulo = new Label();
            lblProveedor = new Label();
            grpCobro = new GroupBox();
            lblClave = new Label();
            txtClave = new TextBox();
            btnNuevaClave = new Button();
            btnConfirmar = new Button();
            btnRepetir = new Button();
            lblResultadoCobro = new Label();
            lblTrazas = new Label();
            txtTrazas = new TextBox();
            grpPedido.SuspendLayout();
            grpCobro.SuspendLayout();
            SuspendLayout();
            // 
            // grpPedido
            // 
            grpPedido.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            grpPedido.Controls.Add(lblPedido);
            grpPedido.Controls.Add(cmbPedido);
            grpPedido.Controls.Add(lblClienteTitulo);
            grpPedido.Controls.Add(lblCliente);
            grpPedido.Controls.Add(lblSubtotalTitulo);
            grpPedido.Controls.Add(lblSubtotal);
            grpPedido.Controls.Add(lblDescuentoTitulo);
            grpPedido.Controls.Add(lblDescuento);
            grpPedido.Controls.Add(lblTarifaTitulo);
            grpPedido.Controls.Add(lblTarifa);
            grpPedido.Controls.Add(lblTotalTitulo);
            grpPedido.Controls.Add(lblTotal);
            grpPedido.Controls.Add(lblProveedorTitulo);
            grpPedido.Controls.Add(lblProveedor);
            grpPedido.Location = new Point(12, 8);
            grpPedido.Size = new Size(600, 300);
            grpPedido.Name = "grpPedido";
            grpPedido.TabIndex = 0;
            grpPedido.Text = "Pedido a cobrar";
            grpPedido.TabStop = false;
            // 
            // lblPedido
            // 
            lblPedido.AutoSize = true;
            lblPedido.Location = new Point(12, 30);
            lblPedido.Name = "lblPedido";
            lblPedido.TabIndex = 0;
            lblPedido.Text = "Pedido VALIDADO:";
            // 
            // cmbPedido
            // 
            cmbPedido.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPedido.FormattingEnabled = true;
            cmbPedido.Location = new Point(130, 27);
            cmbPedido.Size = new Size(450, 23);
            cmbPedido.Name = "cmbPedido";
            cmbPedido.TabIndex = 1;
            // 
            // lblClienteTitulo
            // 
            lblClienteTitulo.AutoSize = true;
            lblClienteTitulo.Location = new Point(12, 68);
            lblClienteTitulo.Name = "lblClienteTitulo";
            lblClienteTitulo.TabIndex = 2;
            lblClienteTitulo.Text = "Cliente:";
            // 
            // lblCliente
            // 
            lblCliente.Location = new Point(130, 66);
            lblCliente.Size = new Size(300, 20);
            lblCliente.Name = "lblCliente";
            lblCliente.TabIndex = 3;
            lblCliente.Text = "-";
            // 
            // lblSubtotalTitulo
            // 
            lblSubtotalTitulo.AutoSize = true;
            lblSubtotalTitulo.Location = new Point(12, 94);
            lblSubtotalTitulo.Name = "lblSubtotalTitulo";
            lblSubtotalTitulo.TabIndex = 4;
            lblSubtotalTitulo.Text = "Subtotal:";
            // 
            // lblSubtotal
            // 
            lblSubtotal.Location = new Point(130, 92);
            lblSubtotal.Size = new Size(120, 20);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.TabIndex = 5;
            lblSubtotal.Text = "-";
            lblSubtotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDescuentoTitulo
            // 
            lblDescuentoTitulo.AutoSize = true;
            lblDescuentoTitulo.Location = new Point(12, 120);
            lblDescuentoTitulo.Name = "lblDescuentoTitulo";
            lblDescuentoTitulo.TabIndex = 6;
            lblDescuentoTitulo.Text = "Descuento:";
            // 
            // lblDescuento
            // 
            lblDescuento.Location = new Point(130, 118);
            lblDescuento.Size = new Size(120, 20);
            lblDescuento.Name = "lblDescuento";
            lblDescuento.TabIndex = 7;
            lblDescuento.Text = "-";
            lblDescuento.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTarifaTitulo
            // 
            lblTarifaTitulo.AutoSize = true;
            lblTarifaTitulo.Location = new Point(12, 146);
            lblTarifaTitulo.Name = "lblTarifaTitulo";
            lblTarifaTitulo.TabIndex = 8;
            lblTarifaTitulo.Text = "Tarifa:";
            // 
            // lblTarifa
            // 
            lblTarifa.Location = new Point(130, 144);
            lblTarifa.Size = new Size(120, 20);
            lblTarifa.Name = "lblTarifa";
            lblTarifa.TabIndex = 9;
            lblTarifa.Text = "-";
            lblTarifa.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalTitulo
            // 
            lblTotalTitulo.AutoSize = true;
            lblTotalTitulo.Location = new Point(12, 172);
            lblTotalTitulo.Name = "lblTotalTitulo";
            lblTotalTitulo.TabIndex = 10;
            lblTotalTitulo.Text = "Total USD:";
            // 
            // lblTotal
            // 
            lblTotal.Location = new Point(130, 170);
            lblTotal.Size = new Size(120, 20);
            lblTotal.Name = "lblTotal";
            lblTotal.TabIndex = 11;
            lblTotal.Text = "-";
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblProveedorTitulo
            // 
            lblProveedorTitulo.AutoSize = true;
            lblProveedorTitulo.Location = new Point(12, 206);
            lblProveedorTitulo.Name = "lblProveedorTitulo";
            lblProveedorTitulo.TabIndex = 12;
            lblProveedorTitulo.Text = "Proveedor activo:";
            // 
            // lblProveedor
            // 
            lblProveedor.Location = new Point(130, 204);
            lblProveedor.Size = new Size(300, 20);
            lblProveedor.Name = "lblProveedor";
            lblProveedor.TabIndex = 13;
            lblProveedor.Text = "-";
            // 
            // grpCobro
            // 
            grpCobro.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpCobro.Controls.Add(lblClave);
            grpCobro.Controls.Add(txtClave);
            grpCobro.Controls.Add(btnNuevaClave);
            grpCobro.Controls.Add(btnConfirmar);
            grpCobro.Controls.Add(btnRepetir);
            grpCobro.Controls.Add(lblResultadoCobro);
            grpCobro.Location = new Point(624, 8);
            grpCobro.Size = new Size(604, 300);
            grpCobro.Name = "grpCobro";
            grpCobro.TabIndex = 1;
            grpCobro.Text = "Confirmación protegida";
            grpCobro.TabStop = false;
            // 
            // lblClave
            // 
            lblClave.AutoSize = true;
            lblClave.Location = new Point(12, 30);
            lblClave.Name = "lblClave";
            lblClave.TabIndex = 0;
            lblClave.Text = "Clave de idempotencia:";
            // 
            // txtClave
            // 
            txtClave.Location = new Point(160, 27);
            txtClave.Size = new Size(330, 23);
            txtClave.Name = "txtClave";
            txtClave.TabIndex = 1;
            txtClave.ReadOnly = true;
            // 
            // btnNuevaClave
            // 
            btnNuevaClave.Location = new Point(496, 25);
            btnNuevaClave.Size = new Size(96, 27);
            btnNuevaClave.Name = "btnNuevaClave";
            btnNuevaClave.TabIndex = 2;
            btnNuevaClave.Text = "Nueva clave";
            btnNuevaClave.UseVisualStyleBackColor = true;
            // 
            // btnConfirmar
            // 
            btnConfirmar.Location = new Point(12, 66);
            btnConfirmar.Size = new Size(220, 34);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.TabIndex = 3;
            btnConfirmar.Text = "Confirmar y cobrar";
            btnConfirmar.UseVisualStyleBackColor = true;
            // 
            // btnRepetir
            // 
            btnRepetir.Location = new Point(240, 66);
            btnRepetir.Size = new Size(250, 34);
            btnRepetir.Name = "btnRepetir";
            btnRepetir.TabIndex = 4;
            btnRepetir.Text = "Repetir con la misma clave";
            btnRepetir.UseVisualStyleBackColor = true;
            // 
            // lblResultadoCobro
            // 
            lblResultadoCobro.Location = new Point(12, 112);
            lblResultadoCobro.Size = new Size(580, 176);
            lblResultadoCobro.Name = "lblResultadoCobro";
            lblResultadoCobro.TabIndex = 5;
            lblResultadoCobro.Text = "";
            // 
            // lblTrazas
            // 
            lblTrazas.AutoSize = true;
            lblTrazas.Location = new Point(12, 316);
            lblTrazas.Name = "lblTrazas";
            lblTrazas.TabIndex = 2;
            lblTrazas.Text = "Secuencia registrada:";
            // 
            // txtTrazas
            // 
            txtTrazas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtTrazas.Multiline = true;
            txtTrazas.ReadOnly = true;
            txtTrazas.ScrollBars = ScrollBars.Vertical;
            txtTrazas.Font = new Font("Consolas", 9F);
            txtTrazas.Location = new Point(12, 336);
            txtTrazas.Size = new Size(1216, 292);
            txtTrazas.Name = "txtTrazas";
            txtTrazas.TabIndex = 3;
            // 
            // UcCobro
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtTrazas);
            Controls.Add(lblTrazas);
            Controls.Add(grpCobro);
            Controls.Add(grpPedido);
            Name = "UcCobro";
            Size = new Size(1240, 640);
            grpCobro.ResumeLayout(false);
            grpCobro.PerformLayout();
            grpPedido.ResumeLayout(false);
            grpPedido.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpPedido;
        private Label lblPedido;
        private ComboBox cmbPedido;
        private Label lblClienteTitulo;
        private Label lblCliente;
        private Label lblSubtotalTitulo;
        private Label lblSubtotal;
        private Label lblDescuentoTitulo;
        private Label lblDescuento;
        private Label lblTarifaTitulo;
        private Label lblTarifa;
        private Label lblTotalTitulo;
        private Label lblTotal;
        private Label lblProveedorTitulo;
        private Label lblProveedor;
        private GroupBox grpCobro;
        private Label lblClave;
        private TextBox txtClave;
        private Button btnNuevaClave;
        private Button btnConfirmar;
        private Button btnRepetir;
        private Label lblResultadoCobro;
        private Label lblTrazas;
        private TextBox txtTrazas;
    }
}
