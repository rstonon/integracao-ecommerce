using IntegracaoRockye.Rocky.DB;
using IntegracaoRockye.Rocky.Model;
using IntegracaoRockye.Rocky.Model.List;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntegracaoRockye.Rocky;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Model;
using IntegracaoRockye.Versatil.Funcoes;
using IntegracaoRockye.App.APFuncoes;
using IntegracaoRockye.Macro;
using IntegracaoRockye.Tray;
using IntegracaoRockye.MundoWine;
using IntegracaoRockye.Magento;

namespace IntegracaoRockye
{
    public partial class Form1 : Form
    {
        int Tempo = 0;
        int TempoAPP = 0;
        string CodigoEmpresa = "";
        string Empresa = "";
        List<VerConfiguracao> ListaConfiguracao { get; set; }
        //string CodigoEmpresa = "1";
        //string Empresa = "MODELO";

        OleDbConnection Connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\ERP Versátil\Configurações.mdb");

        string Conn = "";
        frmLoad Load = new frmLoad();
        public Form1(string[] _args)
        {
            InitializeComponent();

            try
            {
                //CodigoEmpresa = "1";
                //Empresa = "MODELO - EPIMAQ";
                // Empresa = "AGENDA";
                //Empresa = "MODELO ATELIE - TRAY";
                //Empresa = "RELATORIOS";
                //Empresa = "FANES";

                CodigoEmpresa = _args[1];
                Empresa = _args[2];
                //
                Empresa = Empresa.Replace("*", " ");

                BuscarDadosLocais();
                DBConnectionMySql.strConnection = Conn;
                BuscarConfiguracao();

                timerTempo.Interval = (1000 * (Tempo * 60));
                timerMacro.Interval = (1000 * (Tempo * 60));
                timerTray.Interval = (1000 * (Tempo * 60));
                TimerApp.Interval = (1000 * (TempoAPP * 60));
                timerMundoWine.Interval = (1000 * (Tempo * 60));
                TimerMagento.Interval = (1000 * (Tempo * 60));
                timerCalculaEstoqueVendedor.Interval = (1000 * (80 * 60));

                // MAGENTO //////////////////////////////////
                try
                {
                    tabControl1.TabPages.Remove(tabPage6);

                    if (DadosConfiguracao.Config.EcommerceMagento)
                    {
                        tabControl1.TabPages.Add(tabPage6);
                        tabControl1.SelectedTab = tabPage6;
                    }
                }
                catch
                { }

                //Alterar para true quando não for para a FANES
                if (true)
                {
                    if (DadosConfiguracao.Config.UtilizarCodigoVendedorComoEmpresaAPP)
                    {
                        timerCalculaEstoqueVendedor.Start();
                    }

                    try
                    {
                        if (DadosConfiguracao.Config.TokenApiApp.Length < 10)
                        {
                            tabControl1.TabPages.Remove(tabPage1);
                        }
                    }
                    catch
                    {
                        tabControl1.TabPages.Remove(tabPage1);
                    }
                }
                else
                {
                    tabControl1.TabPages.Remove(tabPage1);
                }

                try
                {
                    if (DadosConfiguracao.Config.TokenRocky.Length < 10)
                    {
                        tabControl1.TabPages.Remove(tabPage2);
                    }
                }
                catch
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }


                try
                {
                    if (DadosConfiguracao.Config.TokenMacro.Length < 5 || DadosConfiguracao.Config.TokenMacro.Equals(null))
                    {
                        tabControl1.TabPages.Remove(tabPage3);
                    }
                }
                catch
                {
                    tabControl1.TabPages.Remove(tabPage3);
                }

                try
                {
                    if (DadosConfiguracao.Config.TokenApiApp.Length > 10 && DadosConfiguracao.Config.TokenRocky.Length > 10)
                    {
                        tabControl1.SelectedTab = tabPage2;
                    }
                }
                catch
                {
                    tabControl1.SelectedTab = tabPage2;
                }

                //TRAY /////////////////////////////////////
                try
                {
                    if (DadosConfiguracao.Config.Consumer_secretTray.Length > 10 && DadosConfiguracao.Config.Consumer_keyTray.Length > 10 && DadosConfiguracao.Config.CodeTray.Length > 10)
                    {
                        tabControl1.SelectedTab = tabPage4;
                        timerAuthTray.Start();
                    }
                }
                catch
                {
                    tabControl1.SelectedTab = tabPage4;
                }

                try
                {
                    if (DadosConfiguracao.Config.Consumer_secretTray.Length < 5 || DadosConfiguracao.Config.Consumer_secretTray.Equals(null) || DadosConfiguracao.Config.Consumer_keyTray.Length < 5 || DadosConfiguracao.Config.Consumer_keyTray.Equals(null))
                    {
                        tabControl1.TabPages.Remove(tabPage4);
                    }
                }
                catch
                {
                    tabControl1.TabPages.Remove(tabPage4);
                }



                // Mundo Wine //////////////////////////////////
                try
                {
                    tabControl1.TabPages.Remove(tabPage5);

                    if ((DadosConfiguracao.Config.CnpjEmpresa.Replace(".", "").Replace("-", "").Replace("/", "")).Equals("21069246000190"))
                    {
                        tabControl1.TabPages.Add(tabPage5);
                        tabControl1.SelectedTab = tabPage5;
                    }
                }
                catch
                { }

                // Rocky ////////////////////
                if (DadosConfiguracao.Config.VersaoRocky.Equals("V1"))
                {
                    panelrockyV1.Visible = true;
                    panelrockyV2.Visible = false;
                }
                else if (DadosConfiguracao.Config.VersaoRocky.Equals("V1"))
                {
                    panelrockyV1.Visible = false;
                    panelrockyV2.Visible = true;
                }



            }
            catch
            {
                MessageBox.Show("Não foi possível iniciar", "ERP Versátil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        public void BuscarDadosLocais()
        {
            try
            {
                //Declara Comando
                OleDbCommand Command = Connection.CreateCommand();
                //abre a conexao com o Acess
                Connection.Open();
                Command.CommandText = "select * from tblEmpresas where Código = '" + CodigoEmpresa + "' and Empresa = '" + Empresa + "'";

                //Declara DataReader
                OleDbDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    string Ip = Reader["Servidor"].ToString();
                    string User = Reader["Usuário"].ToString();
                    string Password = Reader["Senha"].ToString();
                    string Database = Reader["Banco de Dados"].ToString();

                    Conn = "server = " + Ip + "; user = " + User + "; database = " + Database + "; password = " + Password + ";";
                    //Fecha DataReader e conexão
                    Reader.Close();
                }

                Connection.Close();
            }
            catch (Exception ex)
            {
                Connection.Close();
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void BuscarConfiguracao()
        {
            try
            {
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                try
                {
                    DadosConfiguracao.Config = new VerConfiguracao();

                    string Query = "select co.usuariorazaosocial, c.ecommercemagento, c.tokenecommercerokey, c.tempominutossincronizacaoecommerce, c.substatusaoimportarpedidosite, co.contadebitopadraoatendimento, co.contacontabilvenda, co.tipodocumentopadrao, co.tipodocumentopadraoavista, co.vendedorpadrao, cd.substatusaoimportarpedidoapp, co.usuariocnpj, c.tokenapiapp, c.tempominutossincronizacaoapp, c.sincronizacaoagendaapp, c.sincronizacaoprodutosapp, c.sincronizacaohistoricofinanceiroapp, c.utilizarcodigodovendedorcomoempresaapp, c.tokenmacro, c.diretoriodeimagensdosprodutos, c.consumerkeytray, c.consumersecrettray ,c.codetray, c.versaorocky, c.calcularestoquepelosku, c.ajustarcodigoecommerce, c.praticadopadraoecommerce, c.usarsequenciacomocodigorocky, c.numeroestoquedeposito from configuracoestres c LEFT JOIN configuracoes co on co.codigoempresa = c.codigoempresa left join configuracoesdois cd on cd.codigoempresa = c.codigoempresa where c.codigoempresa = '" + CodigoEmpresa + "'";

                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.Read())
                    {
                        label1.Text = Reader["usuariorazaosocial"].ToString();

                        DadosConfiguracao.Config.CodigoConfiguracao = CodigoEmpresa;
                        DadosConfiguracao.Config.CodigoContaCredito = Reader["contacontabilvenda"].ToString();
                        DadosConfiguracao.Config.CodigoContaDebito = Reader["contadebitopadraoatendimento"].ToString();
                        DadosConfiguracao.Config.CodigoDocumentoaPrazo = Reader["tipodocumentopadrao"].ToString();
                        DadosConfiguracao.Config.CodigoDocumentoaVista = Reader["tipodocumentopadraoavista"].ToString();
                        DadosConfiguracao.Config.CodigoSubStatusSite = Reader["substatusaoimportarpedidosite"].ToString();
                        DadosConfiguracao.Config.CodigoSubStatusApp = Reader["substatusaoimportarpedidoapp"].ToString();
                        DadosConfiguracao.Config.CodigoVendedorPadrao = Reader["vendedorpadrao"].ToString();
                        DadosConfiguracao.Config.TokenRocky = Reader["tokenecommercerokey"].ToString();
                        DadosConfiguracao.Config.CnpjEmpresa = Reader["usuariocnpj"].ToString().Replace(".", "").Replace("/", "").Replace("-", "");
                        DadosConfiguracao.Config.TokenApiApp = Reader["tokenapiapp"].ToString();
                        DadosConfiguracao.Config.SincronizarAgendaApp = Convert.ToBoolean(Reader["sincronizacaoagendaapp"].ToString());
                        DadosConfiguracao.Config.SincronizarProdutosApp = Convert.ToBoolean(Reader["sincronizacaoprodutosapp"].ToString());
                        DadosConfiguracao.Config.SincronizarHistoricoFinanceiroApp = Convert.ToBoolean(Reader["sincronizacaohistoricofinanceiroapp"].ToString());


                        //Fanes
                        DadosConfiguracao.Config.UtilizarCodigoVendedorComoEmpresaAPP = Convert.ToBoolean(Reader["utilizarcodigodovendedorcomoempresaapp"].ToString());

                        DadosConfiguracao.Config.DiretorioImagensProdutos = Reader["diretoriodeimagensdosprodutos"].ToString();
                        //Macro
                        DadosConfiguracao.Config.TokenMacro = Reader["tokenmacro"].ToString();

                        //Tray
                        DadosConfiguracao.Config.Consumer_keyTray = Reader["consumerkeytray"].ToString();
                        DadosConfiguracao.Config.Consumer_secretTray = Reader["consumersecrettray"].ToString();
                        DadosConfiguracao.Config.CodeTray = Reader["codetray"].ToString();

                        DadosConfiguracao.Config.VersaoRocky = Reader["versaorocky"].ToString();

                        DadosConfiguracao.Config.EnviarEstoqueDeposito = Reader["numeroestoquedeposito"].ToString();

                        try
                        {
                            TempoAPP = Convert.ToInt32(Reader["tempominutossincronizacaoapp"].ToString());
                        }
                        catch
                        {
                            TempoAPP = 60;
                        }
                        try
                        {
                            Tempo = Convert.ToInt32(Reader["tempominutossincronizacaoecommerce"].ToString());
                        }
                        catch
                        {
                            Tempo = 60;
                        }
                        try
                        {
                            DadosConfiguracao.Config.CalcularEstoquePeloSKURocky = Convert.ToBoolean(Reader["calcularestoquepelosku"].ToString());
                        }
                        catch
                        {
                            DadosConfiguracao.Config.CalcularEstoquePeloSKURocky = false;
                        }
                        try
                        {
                            DadosConfiguracao.Config.Ajustarcodigoecommerce = Convert.ToBoolean(Reader["ajustarcodigoecommerce"].ToString());
                        }
                        catch
                        {
                            DadosConfiguracao.Config.Ajustarcodigoecommerce = false;
                        }
                        try
                        {
                            DadosConfiguracao.Config.UstilizarSequenciacomoCodigoRocky = Convert.ToBoolean(Reader["usarsequenciacomocodigorocky"].ToString());
                        }
                        catch
                        {
                            DadosConfiguracao.Config.UstilizarSequenciacomoCodigoRocky = false;
                        }

                        DadosConfiguracao.Config.PraticadoPadraoEcommerce = Reader["praticadopadraoecommerce"].ToString();

                        try
                        {
                            DadosConfiguracao.Config.EcommerceMagento = Convert.ToBoolean(Reader["ecommercemagento"].ToString());
                        }
                        catch
                        {
                            DadosConfiguracao.Config.EcommerceMagento = false;
                        }

                        btAjustarCodigosRockyV2.Visible = DadosConfiguracao.Config.Ajustarcodigoecommerce;
                        btAjustarCodigoEcommerce.Visible = DadosConfiguracao.Config.Ajustarcodigoecommerce;

                        Reader.Close();
                    }


                    if (DadosConfiguracao.Config.IntegracaoVendedorEmpresa)
                    {
                        CarregarVendedoresIntegracao_VendedorEmpresa();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Pega Todas as Configurações do Banco de dados ---> Somente quando o vendedor é considerado empresa
        public void CarregarVendedoresIntegracao_VendedorEmpresa()
        {
            try
            {
                ListaConfiguracao = new List<VerConfiguracao>();
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                string Query = "select co.usuariorazaosocial, c.tokenecommercerokey, c.tempominutossincronizacaoecommerce, c.substatusaoimportarpedidosite, co.contadebitopadraoatendimento, co.contacontabilvenda, co.tipodocumentopadrao, co.tipodocumentopadraoavista, co.vendedorpadrao, cd.substatusaoimportarpedidoapp, co.usuariocnpj, c.tokenapiapp, c.tempominutossincronizacaoapp, c.sincronizacaoagendaapp, c.sincronizacaoprodutosapp, c.sincronizacaohistoricofinanceiroapp, c.integracaovendedorempresa, c.appenviarprodutosdaempresa, c.appenviarclientesdaempresa from configuracoestres c LEFT JOIN configuracoes co on co.codigoempresa = c.codigoempresa left join configuracoesdois cd on cd.codigoempresa = c.codigoempresa";
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                {
                    var Configuracao = new VerConfiguracao();

                    Configuracao.CodigoConfiguracao = CodigoEmpresa;
                    Configuracao.CodigoContaCredito = Reader["contacontabilvenda"].ToString();
                    Configuracao.CodigoContaDebito = Reader["contadebitopadraoatendimento"].ToString();
                    Configuracao.CodigoDocumentoaPrazo = Reader["tipodocumentopadrao"].ToString();
                    Configuracao.CodigoDocumentoaVista = Reader["tipodocumentopadraoavista"].ToString();
                    Configuracao.CodigoSubStatusSite = Reader["substatusaoimportarpedidosite"].ToString();
                    Configuracao.CodigoSubStatusApp = Reader["substatusaoimportarpedidoapp"].ToString();
                    Configuracao.CodigoVendedorPadrao = Reader["vendedorpadrao"].ToString();
                    Configuracao.TokenRocky = Reader["tokenecommercerokey"].ToString();
                    Configuracao.CnpjEmpresa = Reader["usuariocnpj"].ToString();
                    Configuracao.TokenApiApp = Reader["tokenapiapp"].ToString();
                    Configuracao.SincronizarAgendaApp = Convert.ToBoolean(Reader["sincronizacaoagendaapp"].ToString());
                    Configuracao.SincronizarProdutosApp = Convert.ToBoolean(Reader["sincronizacaoprodutosapp"].ToString());
                    Configuracao.SincronizarHistoricoFinanceiroApp = Convert.ToBoolean(Reader["sincronizacaohistoricofinanceiroapp"].ToString());
                    Configuracao.IntegracaoVendedorEmpresa = Convert.ToBoolean(Reader["integracaovendedorempresa"].ToString());
                    Configuracao.EnviarProdutosdaEmpresa = Convert.ToBoolean(Reader["appenviarprodutosdaempresa"].ToString());
                    Configuracao.EnviarClientesdaEmpresa = Convert.ToBoolean(Reader["appenviarclientesdaempresa"].ToString());

                    ListaConfiguracao.Add(Configuracao);
                }

                Reader.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }






        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                var ProdutosApi = RKConectionWebService.BuscarProdutos();

                foreach (var item in ProdutosApi)
                {
                    try
                    {
                        if (item.id != "11")
                        {
                            RKConectionWebService.DeletarProdutos(item.id);
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TimerTempo_Tick(object sender, EventArgs e)
        {
            try
            {
                if (DadosConfiguracao.Config.VersaoRocky.Equals("V1"))
                {
                    RKFuncoes.EnviarProdutos();
                    RKFuncoes.Atualizar_Estoque_Produto_Variacoes();
                    RKFuncoes.CopiarPedidos();
                }
                else if (DadosConfiguracao.Config.VersaoRocky.Equals("V2"))
                {
                    RKFuncoes.CopiarPedidosV2();
                    RKFuncoes.EnviarProdutosV2();
                    RKFuncoes.UpdateProdutosV2();

                    if (DadosConfiguracao.Config.CalcularEstoquePeloSKURocky)
                    {
                        RKFuncoes.CalcularEstoqueEcommerceV2();
                    }

                    RKFuncoes.EnviarVariacoesProdutosV2();
                    RKFuncoes.UpdateVariacoesProdutosV2();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }

            // Activate the form.
            this.Activate();

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Visible = false;
                }
            }
            catch { }
        }

        //Inicia a Sincronização automatica
        private void BiIniciar_Click(object sender, EventArgs e)
        {
            AlteraControles(false);
            biIniciar.Enabled = false;
            btParar.Enabled = true;
            timerTempo.Start();
        }

        //Para a Sincronização automatica
        private void BtParar_Click(object sender, EventArgs e)
        {
            AlteraControles(true);
            biIniciar.Enabled = true;
            btParar.Enabled = false;
            timerTempo.Stop();
        }

        //Bloqueia ou Desbloqueia
        public void AlteraControles(bool Status)
        {
            try
            {
                panelrockyV2.Enabled = Status;
                btAtivarProdutosSite.Enabled = Status;
                btcopiarpedido.Enabled = Status;
                btDeletarProdutos.Enabled = Status;
                btenviarprodutos.Enabled = Status;
            }
            catch { }
        }

        //////////////////////////////////  Abre a Tela de Logs  ////////////////////////////////////
        private void BtLogs_Click(object sender, EventArgs e)
        {
            try
            {
                frmLogs Log = new frmLogs("Site");
                Log.ShowDialog();
            }
            catch { }
        }

        //////////////////////////////// Integração com Ecommerce da Rockey /////////////////////////
        private async void Btenviarprodutos_Click(object sender, EventArgs e)
        {
            try
            {
                Load.Show();
                await Task.Run(() =>
                {
                    RKFuncoes.EnviarProdutos();
                });
                Load.Hide();

                MessageBox.Show("Produtos Enviados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void Btcopiarpedido_Click(object sender, EventArgs e)
        {
            try
            {
                Load.Show();
                await Task.Run(() =>
                {
                    RKFuncoes.CopiarPedidos();
                });
                Load.Hide();

                MessageBox.Show("Pedidos Copiados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtAtivarProdutosSite_Click(object sender, EventArgs e)
        {
            try
            {
                Load.Show();
                await Task.Run(() =>
                {
                    RKFuncoes.Atualizar_Estoque_Produto_Variacoes();
                });
                Load.Hide();

                MessageBox.Show("Operação Efetuada com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        // //////////////////////////////     Rocky V2.0    ////////////////////////////////////////////

        private async void btEnviarProdutosRockyV2_Click(object sender, EventArgs e)
        {
            try
            {
                Load.Show();
                await Task.Run(() =>
                {
                    RKFuncoes.EnviarProdutosV2();
                    RKFuncoes.UpdateProdutosV2();
                });
                Load.Hide();

                MessageBox.Show("Produtos Enviado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btRKVariacoesProdutosV2_Click(object sender, EventArgs e)
        {
            try
            {
                Load.Show();
                await Task.Run(() =>
                {
                    if (DadosConfiguracao.Config.CalcularEstoquePeloSKURocky)
                    {
                        RKFuncoes.CalcularEstoqueEcommerceV2();
                    }

                    RKFuncoes.EnviarVariacoesProdutosV2();
                    RKFuncoes.UpdateVariacoesProdutosV2();
                });
                Load.Hide();

                MessageBox.Show("Variações Enviadas com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btRKCopiarPedidosV2_Click(object sender, EventArgs e)
        {
            try
            {
                Load.Show();
                await Task.Run(() =>
                {
                    RKFuncoes.CopiarPedidosV2();
                });
                Load.Hide();

                MessageBox.Show("Pedidos Copiados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btAjustarCodigosRockyV2_Click(object sender, EventArgs e)
        {
            try
            {
                Load.Show();
                await Task.Run(() =>
                {
                    RKFuncoes.AjustarCodigosProdutosV2();
                    RKFuncoes.AjustarCodigosVaricoesV2();
                });
                Load.Hide();

                MessageBox.Show("Operação Efetuada com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private async void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Load.Show();
                await Task.Run(() =>
                {
                    RKFuncoes.CalcularEstoqueEcommerceV2();
                });
                Load.Hide();

                MessageBox.Show("Operação efetuada com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }




        /////////////////////////////////////////////////////////////////////////////////////////////


        //////////////////////////////  APP  ///////////////////////////////////////////////////////
        private async void BtenviaClientes_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                APFuncoes.EnviarClientes();
            });
            Load.Hide();
            MessageBox.Show("Clientes Enviado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            if (DadosConfiguracao.Config.SincronizarProdutosApp)
            {
                Load.Show();
                await Task.Run(() =>
                {
                    APFuncoes.EnviarProdutos();
                });
                Load.Hide();

                MessageBox.Show("Produtos Enviado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btEnviarEstoqueApp_Click(object sender, EventArgs e)
        {
            if (DadosConfiguracao.Config.SincronizarProdutosApp)
            {
                Load.Show();
                await Task.Run(() =>
                {
                    if (DadosConfiguracao.Config.UtilizarCodigoVendedorComoEmpresaAPP)
                    {
                        APFuncoes.EnviarEstoqueEmpresa();
                    }
                });
                Load.Hide();

                MessageBox.Show("Produtos Enviado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtenviarTabelas_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                APFuncoes.EnviaTabelaPrecosPersonalizadas();
                APFuncoes.EnviarOcorrencias();
                APFuncoes.EnviarTabeladePrecos();
                APFuncoes.EnviarCondicoesPagamento();
                APFuncoes.EnviarDocumentos();
            });
            Load.Hide();

            MessageBox.Show("Tabelas Enviadas com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void BtcopiarPedidos_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                APFuncoes.ReceberPedidos();
            });
            Load.Hide();

            MessageBox.Show("Pedidos Copiados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void Btcontas_Click(object sender, EventArgs e)
        {
            if (DadosConfiguracao.Config.SincronizarHistoricoFinanceiroApp)
            {
                Load.Show();
                await Task.Run(() =>
                {
                    APFuncoes.EnviarContas();
                });
                Load.Hide();

                MessageBox.Show("Histórico Financeiro Enviado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtAgenda_Click(object sender, EventArgs e)
        {
            if (DadosConfiguracao.Config.SincronizarAgendaApp)
            {
                Load.Show();
                await Task.Run(() =>
                {
                    APFuncoes.EnviarUsuariosSistema();
                    APFuncoes.EnviarOcorrenciasV2();
                    APFuncoes.ReceberContatosV2();
                    APFuncoes.EnviarContatosV2();
                });
                Load.Hide();

                MessageBox.Show("Agenda Sincronizada com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void timerCalculaEstoqueVendedor_Tick(object sender, EventArgs e)
        {
            CalculaEstoque();
        }

        public async void CalculaEstoque()
        {
            await Task.Run(() =>
            {
                APFuncoes.CalcularEstoqueEmpresaVendedor();
            });
        }

        private void Btlogapp_Click(object sender, EventArgs e)
        {
            try
            {
                frmLogs log = new frmLogs("App");
                log.Show();
            }
            catch { }
        }


        private void Btiniciarapp_Click(object sender, EventArgs e)
        {
            AlteraControlesApp(false);
            btiniciarapp.Enabled = false;
            btpararapp.Enabled = true;
            TimerApp.Start();
        }

        private void Btpararapp_Click(object sender, EventArgs e)
        {
            AlteraControlesApp(true);
            btiniciarapp.Enabled = true;
            btpararapp.Enabled = false;
            timerCalculaEstoqueVendedor.Stop();
            TimerApp.Stop();
        }

        //Bloqueia ou Desbloqueia
        public void AlteraControlesApp(bool Statusapp)
        {
            try
            {
                btenviaClientes.Enabled = Statusapp;
                button1.Enabled = Statusapp;
                btenviarTabelas.Enabled = Statusapp;
                btcopiarPedidos.Enabled = Statusapp;
                btcontas.Enabled = Statusapp;
                BtAgenda.Enabled = Statusapp;
                btAtualizarImagensApp.Enabled = Statusapp;
                btEnviarEstoqueApp.Enabled = Statusapp;
                btCopiarEventosAPP.Enabled = Statusapp;
                btEnviarRotasAPP.Enabled = Statusapp;
            }
            catch { }
        }

        public void EnviarDados()
        {
            try
            {
                APFuncoes.ReceberPedidos();
                APFuncoes.EnviarClientes();

                if (DadosConfiguracao.Config.SincronizarProdutosApp)
                {
                    APFuncoes.EnviarProdutos();

                    if (DadosConfiguracao.Config.UtilizarCodigoVendedorComoEmpresaAPP)
                    {
                        APFuncoes.EnviarEstoqueEmpresa();
                    }
                }

                APFuncoes.EnviarTabeladePrecos();
                APFuncoes.EnviarCondicoesPagamento();
                APFuncoes.EnviarDocumentos();

                if (DadosConfiguracao.Config.SincronizarHistoricoFinanceiroApp)
                {
                    APFuncoes.EnviarContas();
                }

                if (DadosConfiguracao.Config.SincronizarAgendaApp)
                {
                    APFuncoes.EnviarUsuariosSistema();
                    APFuncoes.EnviarOcorrenciasV2();
                    APFuncoes.ReceberContatosV2();
                    APFuncoes.EnviarContatosV2();
                }

                //APFuncoes.ReceberEventos();
            }
            catch { }
        }

        private void TimerApp_Tick(object sender, EventArgs e)
        {
            EnviarDados();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            APFuncoes.EnviarOcorrencias();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            APFuncoes.ReceberContatosV2();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            APFuncoes.EnviarContatosV2();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            APFuncoes.EnviarEstoqueEmpresa();
        }

        private async void btAtualizarImagensApp_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                APFuncoes.EnviarImagens();
            });
            Load.Hide();
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                DAOProdutos.CalculaEstoqueDisponivel();
            });
            Load.Hide();
            MessageBox.Show("Operação Efetuada com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btCopiarEventosAPP_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                //APFuncoes.ReceberEventos();
            });
            Load.Hide();

            MessageBox.Show("Eventos Copiados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btEnviarRotasAPP_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                APFuncoes.EnviaRotas();
                APFuncoes.EnviaRotasCidades();
            });
            Load.Hide();

            MessageBox.Show("Rotas Enviadas com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////         MACRO         //////////////////////////////////////////////////////////////////////

        private async void btCopiarClientes_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MacroFuncoes.BuscarUsuarios();
            });
            Load.Hide();
            MessageBox.Show("Clientes Enviado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private async void btCopiarPedidos_Macro_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MacroFuncoes.BuscarPedidos();
            });
            Load.Hide();
            MessageBox.Show("Pedidos Copiados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private async void btEnviarProdMacro_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MacroFuncoes.EnviarProdutos();
            });
            Load.Hide();
            MessageBox.Show("Produtos Enviado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btEnviarVariacoes_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MacroFuncoes.EnviarVariacoes();
            });
            Load.Hide();
            MessageBox.Show("Variações Enviadas com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btEnviarGruposMacr_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MacroFuncoes.EnviarGrupos();
            });
            Load.Hide();
            MessageBox.Show("Grupos Enviado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btEnviarSkuMacro_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MacroFuncoes.AtualizarEstoque();
            });
            Load.Hide();
            MessageBox.Show("Estoque Atualizado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btAtualizarPrecosMacro_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MacroFuncoes.AtualizarPreco();
            });
            Load.Hide();
            MessageBox.Show("Preços Atualizados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void timerMacroToken_Tick(object sender, EventArgs e)
        {
            try
            {
                //MacroFuncoes.EnviarVariacoes();
                //MacroFuncoes.EnviarProdutos();
                MacroFuncoes.AtualizarEstoque();
                MacroFuncoes.AtualizarPreco();
                MacroFuncoes.BuscarUsuarios();
                MacroFuncoes.BuscarPedidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AlteraControlesMacro(false);
            button10.Enabled = false;
            button12.Enabled = true;
            btEnviarGruposMacr.Enabled = false;
            timerMacro.Start();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AlteraControlesMacro(true);
            button10.Enabled = true;
            button12.Enabled = false;
            btEnviarGruposMacr.Enabled = true;
            timerMacro.Stop();
        }

        //Bloqueia ou Desbloqueia
        public void AlteraControlesMacro(bool Status)
        {
            try
            {
                btCopiarClientes.Enabled = Status;
                btCopiarPedidos_Macro.Enabled = Status;
                btEnviarProdMacro.Enabled = Status;
                btEnviarSkuMacro.Enabled = Status;
                btEnviarVariacoes.Enabled = Status;

            }
            catch { }
        }

        private void btlogMacro_Click(object sender, EventArgs e)
        {
            try
            {
                frmLogs log = new frmLogs("Macro");
                log.Show();
            }
            catch { }
        }

        // //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////   TRAY   ///////////////////////////////////////////////////////////////////////////////


        //Controla as Solicitações de Novas Credenciais
        private void timerAuthTray_Tick(object sender, EventArgs e)
        {
            try
            {
                timerAuthTray.Stop();
                TrayFuncoes.Auth();

                var TempoRestante = DadosConfiguracao.Config.AuthTray.date_expiration_access_token - DateTime.Now;
                var Tempo = (Convert.ToInt32(TempoRestante.TotalMilliseconds) - 100000);

                if (Tempo > 0)
                {
                    timerAuthTray.Interval = Tempo;
                }
                else
                {
                    TrayFuncoes.AuthRefresh();

                    TempoRestante = DadosConfiguracao.Config.AuthTray.date_expiration_access_token - DateTime.Now;
                    Tempo = (Convert.ToInt32(TempoRestante.TotalMilliseconds) - 10000);
                    timerAuthTray.Interval = Tempo;
                }

                timerAuthTray.Start();

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Timer Auth Tray ", ex.Message, "Tray");
            }
        }

        //Chama as Funções de Tempo em Tempo
        private void timerTray_Tick(object sender, EventArgs e)
        {
            try
            {
                TrayFuncoes.BuscaPedidos();
                TrayFuncoes.EnviaProdutos();
                TrayFuncoes.EnviaVariacoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Envia os produtos para o Tray
        private async void button8_Click_1(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                TrayFuncoes.EnviaProdutos();
            });
            Load.Hide();
            MessageBox.Show("Produtos Enviados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Verifica os produtos e atualiza o Codigo do ecommerce no cadastro do Produto - Sistema
        private async void btAjustarCodigoEcommerce_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                TrayFuncoes.AjustaCodigoEcommercedoPeodutoSistema();
                TrayFuncoes.AjustaCodigoVariacaodoProdutoSistema();
            });
            Load.Hide();
            MessageBox.Show("Operação Efetuada com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Envia as Variações
        private async void btEnviarVariacoesTray_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                TrayFuncoes.EnviaVariacoes();
            });
            Load.Hide();
            MessageBox.Show("Variações Enviadas com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Copia os Pedidos
        private async void btCopiarPedidosTray_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                TrayFuncoes.BuscaPedidos();
            });
            Load.Hide();
            MessageBox.Show("Pedidos Copiados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Abra a Tela de Logs
        private void btLogsTray_Click(object sender, EventArgs e)
        {
            try
            {
                frmLogs log = new frmLogs("Tray");
                log.Show();
            }
            catch { }
        }

        private void AlteraControlesTray(bool Status)
        {
            try
            {
                btCopiarPedidosTray.Enabled = Status;
                btEnviarVariacoesTray.Enabled = Status;
                button8.Enabled = Status;
                btAjustarCodigoEcommerce.Enabled = Status;

            }
            catch { }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            AlteraControlesTray(false);
            button14.Enabled = false;
            button15.Enabled = true;
            timerTray.Start();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            AlteraControlesTray(true);
            button14.Enabled = true;
            button15.Enabled = false;
            timerTray.Stop();
        }

        // /////////////////////////////////////////////////////////////  MUNDO WINE ////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void btIniciarMundoWine_Click(object sender, EventArgs e)
        {
            timerMundoWine.Start();
            AlteraControlesMundoWine(false);
        }

        private void btPararMundoWine_Click(object sender, EventArgs e)
        {
            timerMundoWine.Stop();
            AlteraControlesMundoWine(true);
        }

        private async void btAtualizarEstoqueMundoWine_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MundoWineFuncoes.EnviarEstoque();
            });
            Load.Hide();
            MessageBox.Show("Estoque Atualizado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MundoWineFuncoes.EnviarEstoqueProdutoUnico(textBox1.Text);
            });
            Load.Hide();
            MessageBox.Show("Estoque Atualizado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AlteraControlesMundoWine(bool Status)
        {
            try
            {

                btIniciarMundoWine.Enabled = Status;
                btPararMundoWine.Enabled = !Status;
                btAtualizarEstoqueMundoWine.Enabled = Status;
                btPrecosMundoWine.Enabled = Status;
                btAtualizarNFeMundoWine.Enabled = Status;
            }
            catch { }
        }

        private void timerMundoWine_Tick(object sender, EventArgs e)
        {
            try
            {
                MundoWineFuncoes.EnviarEstoque();
                MundoWineFuncoes.EnviarPraticado();
                MundoWineFuncoes.EnviarNFe();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btPrecosMundoWine_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MundoWineFuncoes.EnviarPraticado();
            });
            Load.Hide();
            MessageBox.Show("Preços Atualizados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private async void button7_Click_1(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MundoWineFuncoes.EnviarPraticadoProdutoUnico(textBox1.Text);
            });
            Load.Hide();
            MessageBox.Show("Praticado Atualizado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // //////////////////////////////// /////// TINY /////// /////////////////////////////////////////////////////////////////

        // //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btIniciarMagento_Click(object sender, EventArgs e)
        {
            TimerMagento.Start();
            AlteraControlesMagento(false);
        }

        private void btPararMagento_Click(object sender, EventArgs e)
        {
            TimerMagento.Stop();
            AlteraControlesMagento(true);
        }

        private void AlteraControlesMagento(bool Status)
        {
            try
            {
                btEnviarProdutosMagento.Enabled = Status;
                btIniciarMagento.Enabled = Status;
                btPararMagento.Enabled = !Status;
                btPedidosMagento.Enabled = Status;
                btAtualizarEstoqueMagento.Enabled = Status;
                btAtualizarPrecosMagento.Enabled = Status;
            }
            catch { }
        }

        private void btLoagsMagento_Click(object sender, EventArgs e)
        {
            try
            {
                frmLogs Log = new frmLogs("Magento");
                Log.ShowDialog();
            }
            catch { }
        }

        private void TimerMagento_Tick(object sender, EventArgs e)
        {
            try
            {
                var WS = new FuncoesMagento();

                WS.CopiarPedidos();
                WS.AtualizarPraticado();
                WS.AtualizarEstoque();
                WS.CloseSession();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var Session = new Magento.Models.MagentoWebService();
            //Session.CreateProduct();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //var Session = new Magento.Models.MagentoWebService();
            //Session.UpdateProduct();
        }



        //Update Estoque
        private async void btAtualizarEstoqueMagento_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                var WS = new FuncoesMagento();
                WS.AtualizarEstoque();
                WS.CloseSession();
            });
            Load.Hide();
            MessageBox.Show("Estoque Atualizado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Copia os Pedidos do Ecommerce
        private async void btPedidosMagento_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                var WS = new FuncoesMagento();
                WS.CopiarPedidos();
                WS.CloseSession();
            });
            Load.Hide();
            MessageBox.Show("Pedidos Copiados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        //Atualiza Preços dos Produtos
        private async void btAtualizarPrecosMagento_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                var WS = new FuncoesMagento();
                WS.AtualizarPraticado();
                WS.CloseSession();
            });
            Load.Hide();
            MessageBox.Show("Praticado Atualizado com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Envia os Produtos para o Magento
        private async void btEnviarProdutosMagento_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                var WS = new FuncoesMagento();
                WS.EnviarProdutos();
                WS.CloseSession();
            });
            Load.Hide();
            MessageBox.Show("Produtos Enviados com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load_2(object sender, EventArgs e)
        {

        }

        private async void btAtualizarNFeMundoWine_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MundoWineFuncoes.EnviarNFe();
            });
            Load.Hide();
            MessageBox.Show("NFe Atualizada com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void button16_Click(object sender, EventArgs e)
        {
            Load.Show();
            await Task.Run(() =>
            {
                MundoWineFuncoes.EnviarNFeUnica(textBox1.Text);
            });
            Load.Hide();
            MessageBox.Show("NFe Atualizada com Sucesso", "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
