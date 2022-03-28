using IntegracaoRockye.Rocky.DB;
using IntegracaoRockye.Versatil.DB;
using MySql.Data.MySqlClient;
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
    public partial class frmLogs : Form
    {
        public frmLogs(string Integracao)
        {
            InitializeComponent();
            dtinicio.Value = DateTime.Now;
            dtfim.Value = DateTime.Now;

            cbintegracao.Text = Integracao;
            PesquisaLog();
        }

        private void Btbuscar_Click(object sender, EventArgs e)
        {
            PesquisaLog();
        }

        public void PesquisaLog()
        {
            try
            {
                string Tipo = "";

                if (cbintegracao.Text.Equals("Site"))
                {
                    Tipo = "Site";
                    this.Text = "Logs Site";
                }
                else if(cbintegracao.Text.Equals("App"))
                {
                    Tipo = "APP";
                    this.Text = "Logs App";
                }
                else if (cbintegracao.Text.Equals("Macro"))
                {
                    Tipo = "Macro";
                    this.Text = "Logs Macro";
                }
                else if (cbintegracao.Text.Equals("Tray"))
                {
                    Tipo = "Tray";
                    this.Text = "Logs Tray";
                }
                else if (cbintegracao.Text.Equals("Magento"))
                {
                    Tipo = "Magento";
                    this.Text = "Logs Magento";
                }


                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                string Query = "select numeropedido as Pedido, data as Data, hora as Hora, obs as Obs, errosistema from logsincronizacao where data between '" + dtinicio.Value.Date.ToString("yyy-MM-dd") + "' and '" + dtfim.Value.Date.ToString("yyy-MM-dd") + "' and numeropedido like '%" + txpedido.Text + "%' and obs like '%" + txocorrencia.Text + "%' and sistema = '"+Tipo+"'";
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(Query, DBMySql);
                da.Fill(dt);
                dggrid.DataSource = dt;

                dggrid.Columns[3].Width = 580;
                dggrid.Columns[4].Visible = false;

                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }
            catch { }
        }


        private void Dggrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string pedido = dggrid.CurrentRow.Cells[0].Value.ToString();
                string obs = dggrid.CurrentRow.Cells[3].Value.ToString();
                string mensagem = dggrid.CurrentRow.Cells[4].Value.ToString();
                frmLogError log = new frmLogError(pedido, obs, mensagem);
                log.ShowDialog();
            }
            catch { }
        }

        private void FrmLogs_Load(object sender, EventArgs e)
        {

        }

        private void BtlimparLog_Click(object sender, EventArgs e)
        {
            try
            {
                string Tipo = "";

                if (cbintegracao.Text.Equals("Site"))
                {
                    Tipo = "Site";
                }
                else if(cbintegracao.Text.Equals("App"))
                {
                    Tipo = "APP";
                }
                else if(cbintegracao.Text.Equals("Macro"))
                {
                    Tipo = "Macro";
                }
                else if (cbintegracao.Text.Equals("Magento"))
                {
                    Tipo = "Magento";
                }
                else
                {
                    Tipo = "Tray";
                }

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand Comando = new MySqlCommand("delete from logsincronizacao where sistema = '"+ Tipo + "'", DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                Comando.ExecuteNonQuery();
                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }
            catch { }
        }
    }
}
