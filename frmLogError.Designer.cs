namespace IntegracaoRockye
{
    partial class frmLogError
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txerro = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbobs = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbpedido = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txerro
            // 
            this.txerro.Location = new System.Drawing.Point(12, 97);
            this.txerro.Multiline = true;
            this.txerro.Name = "txerro";
            this.txerro.Size = new System.Drawing.Size(866, 357);
            this.txerro.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Erro:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Obs:";
            // 
            // lbobs
            // 
            this.lbobs.AutoSize = true;
            this.lbobs.Location = new System.Drawing.Point(47, 51);
            this.lbobs.Name = "lbobs";
            this.lbobs.Size = new System.Drawing.Size(26, 13);
            this.lbobs.TabIndex = 3;
            this.lbobs.Text = "Obs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Pedido:";
            // 
            // lbpedido
            // 
            this.lbpedido.AutoSize = true;
            this.lbpedido.Location = new System.Drawing.Point(69, 20);
            this.lbpedido.Name = "lbpedido";
            this.lbpedido.Size = new System.Drawing.Size(13, 13);
            this.lbpedido.TabIndex = 5;
            this.lbpedido.Text = "0";
            // 
            // frmLogError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 466);
            this.Controls.Add(this.lbpedido);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbobs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txerro);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogError";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log";
            this.Load += new System.EventHandler(this.FrmLogError_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txerro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbobs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbpedido;
    }
}