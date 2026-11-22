namespace LogiMarket.WinForms.Vistas
{
    partial class UcCatalogo
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
            btnLimpiarBusqueda = new Button();
            dgvProductos = new DataGridView();
            grpProducto = new GroupBox();
            lblSku = new Label();
            txtSku = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblCategoria = new Label();
            txtCategoria = new TextBox();
            lblDescripcion = new Label();
            txtDescripcion = new TextBox();
            lblPrecio = new Label();
            txtPrecio = new TextBox();
            lblPeso = new Label();
            txtPeso = new TextBox();
            lblStock = new Label();
            nudStock = new NumericUpDown();
            btnNuevoProducto = new Button();
            btnGuardarProducto = new Button();
            btnEliminarProducto = new Button();
            lblMensajeProducto = new Label();
            grpKits = new GroupBox();
            dgvKits = new DataGridView();
            grpEditorKit = new GroupBox();
            lblCodigoKit = new Label();
            txtCodigoKit = new TextBox();
            lblNombreKit = new Label();
            txtNombreKit = new TextBox();
            lblComponente = new Label();
            cmbComponente = new ComboBox();
            lblCantidadComponente = new Label();
            nudCantidadComponente = new NumericUpDown();
            btnAgregarComponente = new Button();
            lstComponentes = new ListBox();
            btnQuitarComponente = new Button();
            btnNuevoKit = new Button();
            btnGuardarKit = new Button();
            lblMensajeKit = new Label();
            grpDesglose = new GroupBox();
            btnDesglosar = new Button();
            txtDesglose = new TextBox();
            grpProducto.SuspendLayout();
            grpKits.SuspendLayout();
            grpEditorKit.SuspendLayout();
            grpDesglose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStock).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvKits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCantidadComponente).BeginInit();
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
            // btnLimpiarBusqueda
            // 
            btnLimpiarBusqueda.Location = new Point(474, 11);
            btnLimpiarBusqueda.Size = new Size(90, 25);
            btnLimpiarBusqueda.Name = "btnLimpiarBusqueda";
            btnLimpiarBusqueda.TabIndex = 3;
            btnLimpiarBusqueda.Text = "Limpiar";
            btnLimpiarBusqueda.UseVisualStyleBackColor = true;
            // 
            // dgvProductos
            // 
            dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AllowUserToResizeRows = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.MultiSelect = false;
            dgvProductos.ReadOnly = true;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Location = new Point(12, 44);
            dgvProductos.Size = new Size(600, 300);
            dgvProductos.Name = "dgvProductos";
            dgvProductos.TabIndex = 4;
            // 
            // grpProducto
            // 
            grpProducto.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            grpProducto.Controls.Add(lblSku);
            grpProducto.Controls.Add(txtSku);
            grpProducto.Controls.Add(lblNombre);
            grpProducto.Controls.Add(txtNombre);
            grpProducto.Controls.Add(lblCategoria);
            grpProducto.Controls.Add(txtCategoria);
            grpProducto.Controls.Add(lblDescripcion);
            grpProducto.Controls.Add(txtDescripcion);
            grpProducto.Controls.Add(lblPrecio);
            grpProducto.Controls.Add(txtPrecio);
            grpProducto.Controls.Add(lblPeso);
            grpProducto.Controls.Add(txtPeso);
            grpProducto.Controls.Add(lblStock);
            grpProducto.Controls.Add(nudStock);
            grpProducto.Controls.Add(btnNuevoProducto);
            grpProducto.Controls.Add(btnGuardarProducto);
            grpProducto.Controls.Add(btnEliminarProducto);
            grpProducto.Controls.Add(lblMensajeProducto);
            grpProducto.Location = new Point(12, 352);
            grpProducto.Size = new Size(600, 276);
            grpProducto.Name = "grpProducto";
            grpProducto.TabIndex = 5;
            grpProducto.Text = "Datos del producto";
            grpProducto.TabStop = false;
            // 
            // lblSku
            // 
            lblSku.AutoSize = true;
            lblSku.Location = new Point(12, 28);
            lblSku.Name = "lblSku";
            lblSku.TabIndex = 0;
            lblSku.Text = "SKU:";
            // 
            // txtSku
            // 
            txtSku.Location = new Point(100, 25);
            txtSku.Size = new Size(120, 23);
            txtSku.Name = "txtSku";
            txtSku.TabIndex = 1;
            txtSku.CharacterCasing = CharacterCasing.Upper;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(240, 28);
            lblNombre.Name = "lblNombre";
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(330, 25);
            txtNombre.Size = new Size(250, 23);
            txtNombre.Name = "txtNombre";
            txtNombre.TabIndex = 3;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Location = new Point(12, 60);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.TabIndex = 4;
            lblCategoria.Text = "Categoría:";
            // 
            // txtCategoria
            // 
            txtCategoria.Location = new Point(100, 57);
            txtCategoria.Size = new Size(120, 23);
            txtCategoria.Name = "txtCategoria";
            txtCategoria.TabIndex = 5;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Location = new Point(240, 60);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.TabIndex = 6;
            lblDescripcion.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(330, 57);
            txtDescripcion.Size = new Size(250, 23);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.TabIndex = 7;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(12, 92);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.TabIndex = 8;
            lblPrecio.Text = "Precio USD:";
            // 
            // txtPrecio
            // 
            txtPrecio.Location = new Point(100, 89);
            txtPrecio.Size = new Size(120, 23);
            txtPrecio.Name = "txtPrecio";
            txtPrecio.TabIndex = 9;
            // 
            // lblPeso
            // 
            lblPeso.AutoSize = true;
            lblPeso.Location = new Point(240, 92);
            lblPeso.Name = "lblPeso";
            lblPeso.TabIndex = 10;
            lblPeso.Text = "Peso kg:";
            // 
            // txtPeso
            // 
            txtPeso.Location = new Point(330, 89);
            txtPeso.Size = new Size(80, 23);
            txtPeso.Name = "txtPeso";
            txtPeso.TabIndex = 11;
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(420, 92);
            lblStock.Name = "lblStock";
            lblStock.TabIndex = 12;
            lblStock.Text = "Stock:";
            // 
            // nudStock
            // 
            nudStock.Location = new Point(470, 89);
            nudStock.Size = new Size(110, 23);
            nudStock.Name = "nudStock";
            nudStock.TabIndex = 13;
            nudStock.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // btnNuevoProducto
            // 
            btnNuevoProducto.Location = new Point(100, 128);
            btnNuevoProducto.Size = new Size(100, 30);
            btnNuevoProducto.Name = "btnNuevoProducto";
            btnNuevoProducto.TabIndex = 14;
            btnNuevoProducto.Text = "Nuevo";
            btnNuevoProducto.UseVisualStyleBackColor = true;
            // 
            // btnGuardarProducto
            // 
            btnGuardarProducto.Location = new Point(206, 128);
            btnGuardarProducto.Size = new Size(100, 30);
            btnGuardarProducto.Name = "btnGuardarProducto";
            btnGuardarProducto.TabIndex = 15;
            btnGuardarProducto.Text = "Guardar";
            btnGuardarProducto.UseVisualStyleBackColor = true;
            // 
            // btnEliminarProducto
            // 
            btnEliminarProducto.Location = new Point(312, 128);
            btnEliminarProducto.Size = new Size(100, 30);
            btnEliminarProducto.Name = "btnEliminarProducto";
            btnEliminarProducto.TabIndex = 16;
            btnEliminarProducto.Text = "Eliminar";
            btnEliminarProducto.UseVisualStyleBackColor = true;
            // 
            // lblMensajeProducto
            // 
            lblMensajeProducto.Location = new Point(12, 168);
            lblMensajeProducto.Size = new Size(570, 96);
            lblMensajeProducto.Name = "lblMensajeProducto";
            lblMensajeProducto.TabIndex = 17;
            lblMensajeProducto.Text = "";
            // 
            // grpKits
            // 
            grpKits.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpKits.Controls.Add(dgvKits);
            grpKits.Location = new Point(624, 8);
            grpKits.Size = new Size(604, 130);
            grpKits.Name = "grpKits";
            grpKits.TabIndex = 6;
            grpKits.Text = "Kits";
            grpKits.TabStop = false;
            // 
            // dgvKits
            // 
            dgvKits.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvKits.AllowUserToAddRows = false;
            dgvKits.AllowUserToDeleteRows = false;
            dgvKits.AllowUserToResizeRows = false;
            dgvKits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKits.MultiSelect = false;
            dgvKits.ReadOnly = true;
            dgvKits.RowHeadersVisible = false;
            dgvKits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKits.Location = new Point(12, 22);
            dgvKits.Size = new Size(580, 96);
            dgvKits.Name = "dgvKits";
            dgvKits.TabIndex = 0;
            // 
            // grpEditorKit
            // 
            grpEditorKit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpEditorKit.Controls.Add(lblCodigoKit);
            grpEditorKit.Controls.Add(txtCodigoKit);
            grpEditorKit.Controls.Add(lblNombreKit);
            grpEditorKit.Controls.Add(txtNombreKit);
            grpEditorKit.Controls.Add(lblComponente);
            grpEditorKit.Controls.Add(cmbComponente);
            grpEditorKit.Controls.Add(lblCantidadComponente);
            grpEditorKit.Controls.Add(nudCantidadComponente);
            grpEditorKit.Controls.Add(btnAgregarComponente);
            grpEditorKit.Controls.Add(lstComponentes);
            grpEditorKit.Controls.Add(btnQuitarComponente);
            grpEditorKit.Controls.Add(btnNuevoKit);
            grpEditorKit.Controls.Add(btnGuardarKit);
            grpEditorKit.Controls.Add(lblMensajeKit);
            grpEditorKit.Location = new Point(624, 144);
            grpEditorKit.Size = new Size(604, 206);
            grpEditorKit.Name = "grpEditorKit";
            grpEditorKit.TabIndex = 7;
            grpEditorKit.Text = "Editar kit";
            grpEditorKit.TabStop = false;
            // 
            // lblCodigoKit
            // 
            lblCodigoKit.AutoSize = true;
            lblCodigoKit.Location = new Point(12, 28);
            lblCodigoKit.Name = "lblCodigoKit";
            lblCodigoKit.TabIndex = 0;
            lblCodigoKit.Text = "Código:";
            // 
            // txtCodigoKit
            // 
            txtCodigoKit.Location = new Point(80, 25);
            txtCodigoKit.Size = new Size(170, 23);
            txtCodigoKit.Name = "txtCodigoKit";
            txtCodigoKit.TabIndex = 1;
            txtCodigoKit.CharacterCasing = CharacterCasing.Upper;
            // 
            // lblNombreKit
            // 
            lblNombreKit.AutoSize = true;
            lblNombreKit.Location = new Point(262, 28);
            lblNombreKit.Name = "lblNombreKit";
            lblNombreKit.TabIndex = 2;
            lblNombreKit.Text = "Nombre:";
            // 
            // txtNombreKit
            // 
            txtNombreKit.Location = new Point(325, 25);
            txtNombreKit.Size = new Size(265, 23);
            txtNombreKit.Name = "txtNombreKit";
            txtNombreKit.TabIndex = 3;
            // 
            // lblComponente
            // 
            lblComponente.AutoSize = true;
            lblComponente.Location = new Point(12, 62);
            lblComponente.Name = "lblComponente";
            lblComponente.TabIndex = 4;
            lblComponente.Text = "Componente:";
            // 
            // cmbComponente
            // 
            cmbComponente.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbComponente.FormattingEnabled = true;
            cmbComponente.Location = new Point(95, 59);
            cmbComponente.Size = new Size(230, 23);
            cmbComponente.Name = "cmbComponente";
            cmbComponente.TabIndex = 5;
            // 
            // lblCantidadComponente
            // 
            lblCantidadComponente.AutoSize = true;
            lblCantidadComponente.Location = new Point(335, 62);
            lblCantidadComponente.Name = "lblCantidadComponente";
            lblCantidadComponente.TabIndex = 6;
            lblCantidadComponente.Text = "Cant.:";
            // 
            // nudCantidadComponente
            // 
            nudCantidadComponente.Location = new Point(380, 59);
            nudCantidadComponente.Size = new Size(60, 23);
            nudCantidadComponente.Name = "nudCantidadComponente";
            nudCantidadComponente.TabIndex = 7;
            nudCantidadComponente.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCantidadComponente.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAgregarComponente
            // 
            btnAgregarComponente.Location = new Point(450, 57);
            btnAgregarComponente.Size = new Size(140, 27);
            btnAgregarComponente.Name = "btnAgregarComponente";
            btnAgregarComponente.TabIndex = 8;
            btnAgregarComponente.Text = "Agregar componente";
            btnAgregarComponente.UseVisualStyleBackColor = true;
            // 
            // lstComponentes
            // 
            lstComponentes.FormattingEnabled = true;
            lstComponentes.ItemHeight = 15;
            lstComponentes.Location = new Point(12, 92);
            lstComponentes.Size = new Size(310, 94);
            lstComponentes.Name = "lstComponentes";
            lstComponentes.TabIndex = 9;
            // 
            // btnQuitarComponente
            // 
            btnQuitarComponente.Location = new Point(334, 92);
            btnQuitarComponente.Size = new Size(120, 27);
            btnQuitarComponente.Name = "btnQuitarComponente";
            btnQuitarComponente.TabIndex = 10;
            btnQuitarComponente.Text = "Quitar";
            btnQuitarComponente.UseVisualStyleBackColor = true;
            // 
            // btnNuevoKit
            // 
            btnNuevoKit.Location = new Point(334, 126);
            btnNuevoKit.Size = new Size(120, 27);
            btnNuevoKit.Name = "btnNuevoKit";
            btnNuevoKit.TabIndex = 11;
            btnNuevoKit.Text = "Nuevo kit";
            btnNuevoKit.UseVisualStyleBackColor = true;
            // 
            // btnGuardarKit
            // 
            btnGuardarKit.Location = new Point(334, 160);
            btnGuardarKit.Size = new Size(120, 27);
            btnGuardarKit.Name = "btnGuardarKit";
            btnGuardarKit.TabIndex = 12;
            btnGuardarKit.Text = "Guardar kit";
            btnGuardarKit.UseVisualStyleBackColor = true;
            // 
            // lblMensajeKit
            // 
            lblMensajeKit.Location = new Point(462, 92);
            lblMensajeKit.Size = new Size(132, 104);
            lblMensajeKit.Name = "lblMensajeKit";
            lblMensajeKit.TabIndex = 13;
            lblMensajeKit.Text = "";
            // 
            // grpDesglose
            // 
            grpDesglose.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpDesglose.Controls.Add(btnDesglosar);
            grpDesglose.Controls.Add(txtDesglose);
            grpDesglose.Location = new Point(624, 356);
            grpDesglose.Size = new Size(604, 272);
            grpDesglose.Name = "grpDesglose";
            grpDesglose.TabIndex = 8;
            grpDesglose.Text = "Desglose del kit seleccionado";
            grpDesglose.TabStop = false;
            // 
            // btnDesglosar
            // 
            btnDesglosar.Location = new Point(12, 22);
            btnDesglosar.Size = new Size(120, 27);
            btnDesglosar.Name = "btnDesglosar";
            btnDesglosar.TabIndex = 0;
            btnDesglosar.Text = "Desglosar";
            btnDesglosar.UseVisualStyleBackColor = true;
            // 
            // txtDesglose
            // 
            txtDesglose.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtDesglose.Multiline = true;
            txtDesglose.ReadOnly = true;
            txtDesglose.ScrollBars = ScrollBars.Both;
            txtDesglose.WordWrap = false;
            txtDesglose.Font = new Font("Consolas", 9F);
            txtDesglose.Location = new Point(12, 54);
            txtDesglose.Size = new Size(580, 206);
            txtDesglose.Name = "txtDesglose";
            txtDesglose.TabIndex = 1;
            // 
            // UcCatalogo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(grpDesglose);
            Controls.Add(grpEditorKit);
            Controls.Add(grpKits);
            Controls.Add(grpProducto);
            Controls.Add(dgvProductos);
            Controls.Add(btnLimpiarBusqueda);
            Controls.Add(btnBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(lblBuscar);
            Name = "UcCatalogo";
            Size = new Size(1240, 640);
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStock).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvKits).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCantidadComponente).EndInit();
            grpDesglose.ResumeLayout(false);
            grpDesglose.PerformLayout();
            grpEditorKit.ResumeLayout(false);
            grpEditorKit.PerformLayout();
            grpKits.ResumeLayout(false);
            grpKits.PerformLayout();
            grpProducto.ResumeLayout(false);
            grpProducto.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBuscar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Button btnLimpiarBusqueda;
        private DataGridView dgvProductos;
        private GroupBox grpProducto;
        private Label lblSku;
        private TextBox txtSku;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblCategoria;
        private TextBox txtCategoria;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private Label lblPrecio;
        private TextBox txtPrecio;
        private Label lblPeso;
        private TextBox txtPeso;
        private Label lblStock;
        private NumericUpDown nudStock;
        private Button btnNuevoProducto;
        private Button btnGuardarProducto;
        private Button btnEliminarProducto;
        private Label lblMensajeProducto;
        private GroupBox grpKits;
        private DataGridView dgvKits;
        private GroupBox grpEditorKit;
        private Label lblCodigoKit;
        private TextBox txtCodigoKit;
        private Label lblNombreKit;
        private TextBox txtNombreKit;
        private Label lblComponente;
        private ComboBox cmbComponente;
        private Label lblCantidadComponente;
        private NumericUpDown nudCantidadComponente;
        private Button btnAgregarComponente;
        private ListBox lstComponentes;
        private Button btnQuitarComponente;
        private Button btnNuevoKit;
        private Button btnGuardarKit;
        private Label lblMensajeKit;
        private GroupBox grpDesglose;
        private Button btnDesglosar;
        private TextBox txtDesglose;
    }
}
