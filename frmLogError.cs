using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegracaoRockye
{
    public partial class frmLogError : Form
    {
        public frmLogError(string Pedido, string Obs, string Erro)
        {
            InitializeComponent();
            lbpedido.Text = Pedido;
            lbobs.Text = Obs;
            txerro.Text = Erro;
        }

        private void FrmLogError_Load(object sender, EventArgs e)
        {

        }
    }
}
