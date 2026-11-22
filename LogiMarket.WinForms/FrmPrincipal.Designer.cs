namespace LogiMarket.WinForms
{
    partial class FrmPrincipal
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
            pnlCabecera = new Panel();
            lblUsuario = new Label();
            chkProbarPermisos = new CheckBox();
            btnCerrarSesion = new Button();
            tabPrincipal = new TabControl();
            tabPanel = new TabPage();
            tabCatalogo = new TabPage();
            tabClientes = new TabPage();
            tabCarrito = new TabPage();
            tabPedidos = new TabPage();
            tabCobro = new TabPage();
            tabDespacho = new TabPage();
            tabDevoluciones = new TabPage();
            tabInventario = new TabPage();
            tabReportes = new TabPage();
            tabAuditoria = new TabPage();
            lblEstado = new Label();
            pnlCabecera.SuspendLayout();
            tabPrincipal.SuspendLayout();
            tabPanel.SuspendLayout();
            tabCatalogo.SuspendLayout();
            tabClientes.SuspendLayout();
            tabCarrito.SuspendLayout();
            tabPedidos.SuspendLayout();
            tabCobro.SuspendLayout();
            tabDespacho.SuspendLayout();
            tabDevoluciones.SuspendLayout();
            tabInventario.SuspendLayout();
            tabReportes.SuspendLayout();
            tabAuditoria.SuspendLayout();
            SuspendLayout();
            // 
            // pnlCabecera
            // 
            pnlCabecera.Controls.Add(lblUsuario);
            pnlCabecera.Controls.Add(chkProbarPermisos);
            pnlCabecera.Controls.Add(btnCerrarSesion);
            pnlCabecera.Location = new Point(0, 0);
            pnlCabecera.Size = new Size(1264, 40);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.TabIndex = 0;
            pnlCabecera.Dock = DockStyle.Top;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(12, 12);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario";
            // 
            // chkProbarPermisos
            // 
            chkProbarPermisos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkProbarPermisos.AutoSize = true;
            chkProbarPermisos.Location = new Point(820, 11);
            chkProbarPermisos.Name = "chkProbarPermisos";
            chkProbarPermisos.TabIndex = 1;
            chkProbarPermisos.Text = "Habilitar todas las acciones (prueba de permisos)";
            chkProbarPermisos.TabStop = true;
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrarSesion.Location = new Point(1140, 6);
            btnCerrarSesion.Size = new Size(112, 28);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.TabIndex = 2;
            btnCerrarSesion.Text = "Cerrar sesión";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            // 
            // tabPrincipal
            // 
            tabPrincipal.Controls.Add(tabPanel);
            tabPrincipal.Controls.Add(tabCatalogo);
            tabPrincipal.Controls.Add(tabClientes);
            tabPrincipal.Controls.Add(tabCarrito);
            tabPrincipal.Controls.Add(tabPedidos);
            tabPrincipal.Controls.Add(tabCobro);
            tabPrincipal.Controls.Add(tabDespacho);
            tabPrincipal.Controls.Add(tabDevoluciones);
            tabPrincipal.Controls.Add(tabInventario);
            tabPrincipal.Controls.Add(tabReportes);
            tabPrincipal.Controls.Add(tabAuditoria);
            tabPrincipal.Location = new Point(0, 40);
            tabPrincipal.Size = new Size(1264, 677);
            tabPrincipal.Name = "tabPrincipal";
            tabPrincipal.TabIndex = 1;
            tabPrincipal.Dock = DockStyle.Fill;
            // 
            // tabPanel
            // 
            tabPanel.Location = new Point(4, 24);
            tabPanel.Size = new Size(1256, 649);
            tabPanel.Name = "tabPanel";
            tabPanel.TabIndex = 0;
            tabPanel.Text = "Panel general";
            tabPanel.Padding = new Padding(3);
            tabPanel.UseVisualStyleBackColor = true;
            // 
            // tabCatalogo
            // 
            tabCatalogo.Location = new Point(4, 24);
            tabCatalogo.Size = new Size(1256, 649);
            tabCatalogo.Name = "tabCatalogo";
            tabCatalogo.TabIndex = 1;
            tabCatalogo.Text = "Catálogo y kits";
            tabCatalogo.Padding = new Padding(3);
            tabCatalogo.UseVisualStyleBackColor = true;
            // 
            // tabClientes
            // 
            tabClientes.Location = new Point(4, 24);
            tabClientes.Size = new Size(1256, 649);
            tabClientes.Name = "tabClientes";
            tabClientes.TabIndex = 2;
            tabClientes.Text = "Clientes";
            tabClientes.Padding = new Padding(3);
            tabClientes.UseVisualStyleBackColor = true;
            // 
            // tabCarrito
            // 
            tabCarrito.Location = new Point(4, 24);
            tabCarrito.Size = new Size(1256, 649);
            tabCarrito.Name = "tabCarrito";
            tabCarrito.TabIndex = 3;
            tabCarrito.Text = "Carrito / borrador";
            tabCarrito.Padding = new Padding(3);
            tabCarrito.UseVisualStyleBackColor = true;
            // 
            // tabPedidos
            // 
            tabPedidos.Location = new Point(4, 24);
            tabPedidos.Size = new Size(1256, 649);
            tabPedidos.Name = "tabPedidos";
            tabPedidos.TabIndex = 4;
            tabPedidos.Text = "Pedidos e historial";
            tabPedidos.Padding = new Padding(3);
            tabPedidos.UseVisualStyleBackColor = true;
            // 
            // tabCobro
            // 
            tabCobro.Location = new Point(4, 24);
            tabCobro.Size = new Size(1256, 649);
            tabCobro.Name = "tabCobro";
            tabCobro.TabIndex = 5;
            tabCobro.Text = "Cobro";
            tabCobro.Padding = new Padding(3);
            tabCobro.UseVisualStyleBackColor = true;
            // 
            // tabDespacho
            // 
            tabDespacho.Location = new Point(4, 24);
            tabDespacho.Size = new Size(1256, 649);
            tabDespacho.Name = "tabDespacho";
            tabDespacho.TabIndex = 6;
            tabDespacho.Text = "Despacho";
            tabDespacho.Padding = new Padding(3);
            tabDespacho.UseVisualStyleBackColor = true;
            // 
            // tabDevoluciones
            // 
            tabDevoluciones.Location = new Point(4, 24);
            tabDevoluciones.Size = new Size(1256, 649);
            tabDevoluciones.Name = "tabDevoluciones";
            tabDevoluciones.TabIndex = 7;
            tabDevoluciones.Text = "Devoluciones";
            tabDevoluciones.Padding = new Padding(3);
            tabDevoluciones.UseVisualStyleBackColor = true;
            // 
            // tabInventario
            // 
            tabInventario.Location = new Point(4, 24);
            tabInventario.Size = new Size(1256, 649);
            tabInventario.Name = "tabInventario";
            tabInventario.TabIndex = 8;
            tabInventario.Text = "Inventario";
            tabInventario.Padding = new Padding(3);
            tabInventario.UseVisualStyleBackColor = true;
            // 
            // tabReportes
            // 
            tabReportes.Location = new Point(4, 24);
            tabReportes.Size = new Size(1256, 649);
            tabReportes.Name = "tabReportes";
            tabReportes.TabIndex = 9;
            tabReportes.Text = "Reportes y comprobantes";
            tabReportes.Padding = new Padding(3);
            tabReportes.UseVisualStyleBackColor = true;
            // 
            // tabAuditoria
            // 
            tabAuditoria.Location = new Point(4, 24);
            tabAuditoria.Size = new Size(1256, 649);
            tabAuditoria.Name = "tabAuditoria";
            tabAuditoria.TabIndex = 10;
            tabAuditoria.Text = "Auditoría y demostración";
            tabAuditoria.Padding = new Padding(3);
            tabAuditoria.UseVisualStyleBackColor = true;
            // 
            // lblEstado
            // 
            lblEstado.Location = new Point(0, 717);
            lblEstado.Size = new Size(1264, 24);
            lblEstado.Name = "lblEstado";
            lblEstado.TabIndex = 2;
            lblEstado.Text = "";
            lblEstado.Dock = DockStyle.Bottom;
            lblEstado.BorderStyle = BorderStyle.Fixed3D;
            lblEstado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 741);
            Controls.Add(lblEstado);
            Controls.Add(tabPrincipal);
            Controls.Add(pnlCabecera);
            Name = "FrmPrincipal";
            Text = "LogiMarket 360";
            MinimumSize = new Size(1120, 760);
            StartPosition = FormStartPosition.CenterScreen;
            tabAuditoria.ResumeLayout(false);
            tabAuditoria.PerformLayout();
            tabReportes.ResumeLayout(false);
            tabReportes.PerformLayout();
            tabInventario.ResumeLayout(false);
            tabInventario.PerformLayout();
            tabDevoluciones.ResumeLayout(false);
            tabDevoluciones.PerformLayout();
            tabDespacho.ResumeLayout(false);
            tabDespacho.PerformLayout();
            tabCobro.ResumeLayout(false);
            tabCobro.PerformLayout();
            tabPedidos.ResumeLayout(false);
            tabPedidos.PerformLayout();
            tabCarrito.ResumeLayout(false);
            tabCarrito.PerformLayout();
            tabClientes.ResumeLayout(false);
            tabClientes.PerformLayout();
            tabCatalogo.ResumeLayout(false);
            tabCatalogo.PerformLayout();
            tabPanel.ResumeLayout(false);
            tabPanel.PerformLayout();
            tabPrincipal.ResumeLayout(false);
            tabPrincipal.PerformLayout();
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlCabecera;
        private Label lblUsuario;
        private CheckBox chkProbarPermisos;
        private Button btnCerrarSesion;
        private TabControl tabPrincipal;
        private TabPage tabPanel;
        private TabPage tabCatalogo;
        private TabPage tabClientes;
        private TabPage tabCarrito;
        private TabPage tabPedidos;
        private TabPage tabCobro;
        private TabPage tabDespacho;
        private TabPage tabDevoluciones;
        private TabPage tabInventario;
        private TabPage tabReportes;
        private TabPage tabAuditoria;
        private Label lblEstado;
    }
}
