namespace IntegracaoRockye
{
    partial class frmLogs
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txocorrencia = new System.Windows.Forms.TextBox();
            this.txpedido = new System.Windows.Forms.TextBox();
            this.btbuscar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dggrid = new System.Windows.Forms.DataGridView();
            this.dtfim = new System.Windows.Forms.DateTimePicker();
            this.dtinicio = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btlimparLog = new System.Windows.Forms.Button();
            this.cbintegracao = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dggrid)).BeginInit();
            this.SuspendLayout();
            // 
            // txocorrencia
            // 
            this.txocorrencia.Location = new System.Drawing.Point(466, 36);
            this.txocorrencia.Name = "txocorrencia";
            this.txocorrencia.Size = new System.Drawing.Size(280, 20);
            this.txocorrencia.TabIndex = 0;
            // 
            // txpedido
            // 
            this.txpedido.Location = new System.Drawing.Point(339, 36);
            this.txpedido.Name = "txpedido";
            this.txpedido.Size = new System.Drawing.Size(121, 20);
            this.txpedido.TabIndex = 1;
            // 
            // btbuscar
            // 
            this.btbuscar.Location = new System.Drawing.Point(763, 21);
            this.btbuscar.Name = "btbuscar";
            this.btbuscar.Size = new System.Drawing.Size(156, 35);
            this.btbuscar.TabIndex = 2;
            this.btbuscar.Text = "Buscar";
            this.btbuscar.UseVisualStyleBackColor = true;
            this.btbuscar.Click += new System.EventHandler(this.Btbuscar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(336, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pedido:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(463, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ocorrência:";
            // 
            // dggrid
            // 
            this.dggrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dggrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dggrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dggrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.dggrid.Location = new System.Drawing.Point(5, 77);
            this.dggrid.MultiSelect = false;
            this.dggrid.Name = "dggrid";
            this.dggrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dggrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dggrid.ShowCellErrors = false;
            this.dggrid.ShowEditingIcon = false;
            this.dggrid.Size = new System.Drawing.Size(924, 412);
            this.dggrid.TabIndex = 5;
            this.dggrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Dggrid_CellMouseDoubleClick);
            // 
            // dtfim
            // 
            this.dtfim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtfim.Location = new System.Drawing.Point(228, 36);
            this.dtfim.Name = "dtfim";
            this.dtfim.Size = new System.Drawing.Size(100, 20);
            this.dtfim.TabIndex = 6;
            // 
            // dtinicio
            // 
            this.dtinicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtinicio.Location = new System.Drawing.Point(122, 36);
            this.dtinicio.Name = "dtinicio";
            this.dtinicio.Size = new System.Drawing.Size(100, 20);
            this.dtinicio.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(119, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Inicio:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(225, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Fim:";
            // 
            // btlimparLog
            // 
            this.btlimparLog.Location = new System.Drawing.Point(796, 495);
            this.btlimparLog.Name = "btlimparLog";
            this.btlimparLog.Size = new System.Drawing.Size(133, 22);
            this.btlimparLog.TabIndex = 10;
            this.btlimparLog.Text = "Limpar Log";
            this.btlimparLog.UseVisualStyleBackColor = true;
            this.btlimparLog.Click += new System.EventHandler(this.BtlimparLog_Click);
            // 
            // cbintegracao
            // 
            this.cbintegracao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbintegracao.FormattingEnabled = true;
            this.cbintegracao.Items.AddRange(new object[] {
            "App",
            "Site",
            "Macro",
            "Tray",
            "Magento"});
            this.cbintegracao.Location = new System.Drawing.Point(5, 36);
            this.cbintegracao.Name = "cbintegracao";
            this.cbintegracao.Size = new System.Drawing.Size(111, 21);
            this.cbintegracao.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Integração:";
            // 
            // frmLogs
            // 
            this.AcceptButton = this.btbuscar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 524);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbintegracao);
            this.Controls.Add(this.btlimparLog);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtinicio);
            this.Controls.Add(this.dtfim);
            this.Controls.Add(this.dggrid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btbuscar);
            this.Controls.Add(this.txpedido);
            this.Controls.Add(this.txocorrencia);
            this.Name = "frmLogs";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Logs";
            this.Load += new System.EventHandler(this.FrmLogs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dggrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txocorrencia;
        private System.Windows.Forms.TextBox txpedido;
        private System.Windows.Forms.Button btbuscar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtfim;
        private System.Windows.Forms.DateTimePicker dtinicio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dggrid;
        private System.Windows.Forms.Button btlimparLog;
        private System.Windows.Forms.ComboBox cbintegracao;
        private System.Windows.Forms.Label label5;
    }
}