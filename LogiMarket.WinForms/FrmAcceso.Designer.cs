namespace LogiMarket.WinForms
{
    partial class FrmAcceso
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
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            btnIngresar = new Button();
            btnSalir = new Button();
            lblMensaje = new Label();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "LogiMarket 360";
            lblTitulo.Font = new Font("Segoe UI", 14F);
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Location = new Point(32, 52);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Acceso local";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(32, 92);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.TabIndex = 2;
            lblUsuario.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(130, 89);
            txtUsuario.Size = new Size(230, 23);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.TabIndex = 3;
            // 
            // lblContrasena
            // 
            lblContrasena.AutoSize = true;
            lblContrasena.Location = new Point(32, 128);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.TabIndex = 4;
            lblContrasena.Text = "Contraseña:";
            // 
            // txtContrasena
            // 
            txtContrasena.Location = new Point(130, 125);
            txtContrasena.Size = new Size(230, 23);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.TabIndex = 5;
            txtContrasena.UseSystemPasswordChar = true;
            // 
            // btnIngresar
            // 
            btnIngresar.Location = new Point(130, 166);
            btnIngresar.Size = new Size(110, 30);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.TabIndex = 6;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(250, 166);
            btnSalir.Size = new Size(110, 30);
            btnSalir.Name = "btnSalir";
            btnSalir.TabIndex = 7;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            // 
            // lblMensaje
            // 
            lblMensaje.Location = new Point(32, 210);
            lblMensaje.Size = new Size(330, 44);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.TabIndex = 8;
            lblMensaje.Text = "";
            // 
            // FrmAcceso
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 270);
            Controls.Add(lblMensaje);
            Controls.Add(btnSalir);
            Controls.Add(btnIngresar);
            Controls.Add(txtContrasena);
            Controls.Add(lblContrasena);
            Controls.Add(txtUsuario);
            Controls.Add(lblUsuario);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblTitulo);
            Name = "FrmAcceso";
            Text = "LogiMarket 360 - Acceso";
            AcceptButton = btnIngresar;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblContrasena;
        private TextBox txtContrasena;
        private Button btnIngresar;
        private Button btnSalir;
        private Label lblMensaje;
    }
}
