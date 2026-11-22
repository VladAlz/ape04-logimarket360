namespace LogiMarket.WinForms
{
    partial class FrmVistaPrevia
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
            txtContenido = new TextBox();
            btnCerrar = new Button();
            SuspendLayout();
            // 
            // txtContenido
            // 
            txtContenido.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtContenido.Multiline = true;
            txtContenido.ReadOnly = true;
            txtContenido.ScrollBars = ScrollBars.Both;
            txtContenido.WordWrap = false;
            txtContenido.Font = new Font("Consolas", 9F);
            txtContenido.Location = new Point(12, 12);
            txtContenido.Size = new Size(660, 420);
            txtContenido.Name = "txtContenido";
            txtContenido.TabIndex = 0;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCerrar.Location = new Point(572, 442);
            btnCerrar.Size = new Size(100, 30);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // FrmVistaPrevia
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 484);
            Controls.Add(btnCerrar);
            Controls.Add(txtContenido);
            Name = "FrmVistaPrevia";
            Text = "Vista previa";
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtContenido;
        private Button btnCerrar;
    }
}
