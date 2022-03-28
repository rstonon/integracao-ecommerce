using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntegracaoRockye.App;
using IntegracaoRockye.App.APModels;
using IntegracaoRockye.Versatil.APModels;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Funcoes;
using IntegracaoRockye.Versatil.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace IntegracaoRockye.App.APFuncoes
{
    public static class APFuncoes
    {
        private static string UrlApi = "http://api.sistemaversatil.com.br/api/";

        public static decimal ConverterDecimal(string Valor)
        {
            try
            {
                return Convert.ToDecimal(Valor);
            }
            catch
            {
                return 0;
            }
        }

        //Envia Produtos API APP 
        public static void EnviarProdutos()
        {
            try
            {
                string RespostaApi = "";
                List<APProdutos> ListaProdutos = new List<APProdutos>();

                var Lista = DAOProdutos.BuscaProdutos();

                double Res = Lista.Max(x => x.N);
                double Registros = (Res / 15000) + 1;

                for (int i = 1; i < Registros; i++)
                {
                    ListaProdutos = new List<APProdutos>();

                    foreach (var p in Lista.Where(x => x.N <= (15000 * i) && x.N >= (15000 * (i - 1))))
                    {
                        APProdutos Produto = new APProdutos();

                        Produto.Codigo = p.Codigo;
                        Produto.Cnpj = DadosConfiguracao.Config.CnpjEmpresa;
                        Produto.Tipo = p.Tipo;
                        Produto.Situacao = p.Situacao;
                        Produto.Descricao = p.Descricao;
                        Produto.ValorMercadoria = p.Valormercadoria;
                        Produto.Praticado = p.Praticado;
                        Produto.Referencia = p.Referencia;
                        Produto.Unidade = p.Unidade;
                        Produto.CodigoEan = p.Codigoean;
                        Produto.Praticado2 = p.Praticado2;
                        Produto.Praticado3 = p.Praticado3;
                        Produto.EstoqueDisponivel = p.Estoquedisponivel;
                        Produto.DescontoMaximo = p.Descontomaximo;
                        Produto.Grupo = p.Grupo;
                        Produto.Marca = p.Marca;
                        Produto.Cor = p.Cor;
                        Produto.Tamanho = p.Tamanho;
                        Produto.Observacoes = p.Observacoes;
                        Produto.Especificacoestecnicas = p.Especificacoestecnicas;
                        Produto.Aplicacao = p.Aplicacao;
                        Produto.Customercadoria = p.Customercadoria;
                        Produto.Custorealmercadoria = p.Custorealmercadoria;
                        Produto.DataImagem = p.DataImagem;

                        ListaProdutos.Add(Produto);
                    }

                    string Url = UrlApi + "produtos";
                    var Json = new
                    {
                        ListaProdutos
                    };

                    string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                    var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                    var requisicaoWeb = WebRequest.CreateHttp(Url);
                    requisicaoWeb.Timeout = 1600000;
                    requisicaoWeb.Method = "POST";
                    requisicaoWeb.ContentType = "application/json";
                    //requisicaoWeb.Accept = "application/json";
                    requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                    requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                    requisicaoWeb.UserAgent = "POSTProdutos";

                    try
                    {
                        //Envia os dados POST
                        using (var stream = requisicaoWeb.GetRequestStream())
                        {
                            stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                            stream.Close();
                        }

                        //Obtem a resposta do servidor
                        var httpResponse = requisicaoWeb.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            RespostaApi = streamReader.ReadToEnd();
                        }
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Produtos - Erro no envio de Produtos - WebRequest", e.Message + " ----- " + e.Status, "APP");
                            }
                        }
                    }

                }


            }
            catch (MySqlException ex)
            {
                DAOLogDB.SalvarLogs("", "Produtos - Erro nos Produtos", ex.Message, "APP");
            }
        }

        public static void EnviaProdutosAPI(List<APProdutos> ListaProdutos)
        {
            string Url = UrlApi + "produtos";

            var Json = new
            {
                ListaProdutos
            };

            string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            try
            {
                //Envia os dados POST
                using (var stream = requisicaoWeb.GetRequestStream())
                {
                    stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                    stream.Close();
                }

                //Obtem a resposta do servidor
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                { }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string Resposta = reader.ReadToEnd();
                        reader.Close();
                        data.Close();
                        httpResponse.Close();
                        requisicaoWeb.Abort();
                        DAOLogDB.SalvarLogs("", "Produtos - Erro no envio de Produtos - WebRequest", e.Message + " ----- " + e.Status, "APP");
                    }
                }
            }

        }

        //Envia as Tebelas de Preços Personalizada
        public static void EnviaTabelaPrecosPersonalizadas()
        {
            try
            {
                string RespostaApi = "";
                var Tabelas = new List<APTabelasPrecosPersonalizada>();

                string Sql = "SELECT p.nometabela, p.produto, p.valor FROM tabeladeprecospersonalizada p";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var Tabela = new APTabelasPrecosPersonalizada();

                    Tabela.Cnpj = DadosConfiguracao.Config.CnpjEmpresa;
                    Tabela.CodigoProduto = dr["produto"].ToString();
                    Tabela.CodigoTabela = dr["nometabela"].ToString();
                    Tabela.Valor = ConverterDecimal(dr["valor"].ToString());

                    Tabelas.Add(Tabela);
                }
                dr.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);


                string Url = UrlApi + "tabelasdeprecospersonalizadas";
                var Json = new
                {
                    Tabelas
                };

                string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Timeout = 600000;
                requisicaoWeb.Method = "POST";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    //Envia os dados POST
                    using (var stream = requisicaoWeb.GetRequestStream())
                    {
                        stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                        stream.Close();
                    }

                    //Obtem a resposta do servidor
                    var httpResponse = requisicaoWeb.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        RespostaApi = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    try
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Tabelas de Preços Personalizadas - Erro no envio de Tabelas de Preços - WebRequest", Resposta, "APP");
                            }
                        }
                    }
                    catch
                    {
                        DAOLogDB.SalvarLogs("", "Tabelas de Preços Personalizadas - Erro no envio de Tabelas de Preços - WebRequest", e.Message + "  ---  " + e.Status, "APP");
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Tabelas de Preços Personalizadas - Erro nas Tabelas de Preços", ex.Message, "APP");
            }
        }


        //Envia Clientes API APP
        public static void EnviarClientes()
        {
            try
            {
                string RespostaApi = "";
                List<APColaboradores> Colaborador = new List<APColaboradores>();
                var Lista = DAOClientes.BuscarClientes();

                foreach (var cli in Lista)
                {
                    APColaboradores Cliente = new APColaboradores();

                    Cliente.Codigocolaborador = cli.Codigo;
                    Cliente.CodigoSistema = cli.CodigoSistema;
                    Cliente.Cnpj = DadosConfiguracao.Config.CnpjEmpresa;
                    Cliente.Tipocadastro = cli.Tipo;
                    Cliente.Situacao = cli.Situacao;
                    Cliente.Razaosocial = cli.Nomerazaosocial;
                    Cliente.Nomefantasia = cli.Nomefantasia;
                    Cliente.Cpfcnpj = cli.Cpfcnpj;
                    Cliente.Ie = cli.Inscricaoestadual;
                    Cliente.Endereco = cli.Endereco;
                    Cliente.Numero = cli.Numero;
                    Cliente.Complemento = cli.Complemento;
                    Cliente.Bairro = cli.Bairro;
                    Cliente.Telefone = cli.Telefone;
                    Cliente.Celular = cli.Celular;
                    Cliente.Email = cli.Email;
                    Cliente.Observacao = cli.Observacao;
                    Cliente.Cep = cli.Cep;
                    Cliente.Codigocidade = cli.Codigocidade;
                    Cliente.Bloqueado = cli.Bloqueado;
                    Cliente.EmAtraso = cli.EmAtraso;
                    Cliente.Senha = cli.Senha;
                    Cliente.Codigovendedor = cli.Codigovendedor;
                    Cliente.CodigoTabelaPadrao = cli.Codigotabelapadrao;
                    Cliente.DataNascimento = cli.Datanascimento;
                    Cliente.Genero = cli.Genero;
                    Cliente.Contato = cli.Contato;

                    if (DadosConfiguracao.Config.UtilizarCodigoVendedorComoEmpresaAPP)
                    {
                        Cliente.Codigovendedor = cli.Empresa;
                    }

                    Colaborador.Add(Cliente);
                }

                string Url = UrlApi + "colaboradores";
                var Json = new
                {
                    Colaborador
                };

                string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Timeout = 600000;
                requisicaoWeb.Method = "POST";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    //Envia os dados POST
                    using (var stream = requisicaoWeb.GetRequestStream())
                    {
                        stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                        stream.Close();
                    }

                    //Obtem a resposta do servidor
                    var httpResponse = requisicaoWeb.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        RespostaApi = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string Resposta = reader.ReadToEnd();
                            reader.Close();
                            data.Close();
                            httpResponse.Close();
                            requisicaoWeb.Abort();
                            DAOLogDB.SalvarLogs("", "Clientes - Erro no envio de Clientes - WebRequest", Resposta, "APP");
                        }
                    }
                }

                //MessageBox.Show(RespostaApi, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Clientes - Erro no envio de Clientes", ex.Message, "APP");
            }
        }

        //Envia Contas API APP
        public static void EnviarContas()
        {
            try
            {
                string RespostaApi = "";
                List<APContas> Contas = new List<APContas>();


                MySqlConnection DBMySql2 = new MySqlConnection(DBConnectionMySql.strConnection);

                string SqlC = "SELECT c.codigo FROM colaboradores c WHERE c.situacao = 'Ativo' AND c.tipo = 'Cliente' and c.cpfcnpj <> '' GROUP BY c.codigo";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmdc = new MySqlCommand(SqlC, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader drc = cmdc.ExecuteReader();

                while (drc.Read())
                {

                    string Sql = "SELECT c.codigo, c.colaborador, c.numerodocumento, c.status, d.documento, c.emissao, c.quitacao, c.vencimento, c.saldo, c.valorinicial, c.valorquitacao, c.titular FROM contas c left JOIN documentos d ON d.codigo = c.tipodocumento WHERE c.status = 'Aberta' AND c.tipodaconta = 'A Receber' AND c.vencimento IS NOT null AND c.colaborador = '" + drc["codigo"].ToString() + "' ORDER BY c.vencimento";// LIMIT 5";

                    MySqlCommand cmd = new MySqlCommand(Sql, DBMySql2);
                    DBMySql2.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        APContas Conta = new APContas();

                        try
                        {
                            Conta.CodigoConta = dr["codigo"].ToString();
                            Conta.Cnpj = DadosConfiguracao.Config.CnpjEmpresa;
                            Conta.CodigoColaborador = dr["colaborador"].ToString();
                            Conta.NumeroDocumento = dr["numerodocumento"].ToString();
                            Conta.TipoDocumento = dr["documento"].ToString();
                            Conta.DataEmissao = Convert.ToDateTime(dr["emissao"].ToString()).Date;
                            Conta.DataVencimento = Convert.ToDateTime(dr["vencimento"].ToString()).Date;
                            Conta.DataQuitacao = dr["quitacao"].ToString();
                            Conta.ValorSaldo = ConverterDecimal(dr["saldo"].ToString());
                            Conta.ValorInicial = ConverterDecimal(dr["valorinicial"].ToString());
                            Conta.ValorQuitado = ConverterDecimal(dr["valorquitacao"].ToString());
                            Conta.CodigoTitular = dr["titular"].ToString();
                            Conta.Status = dr["status"].ToString();

                            Contas.Add(Conta);

                        }
                        catch (Exception es)
                        {
                            DAOLogDB.SalvarLogs("", "Histórico Financeiro - Erro no Read - Financeiro - WebRequest", es.Message + " - Cod Conta: " + Conta.CodigoConta, "APP");
                            throw new Exception(es.Message + " - Cod Conta: " + Conta.CodigoConta);
                        }
                    }
                    dr.Close();
                    DBMySql2.Close();

                }
                drc.Close();

                DBConnectionMySql.FechaConexaoBD(DBMySql);

                string Url = UrlApi + "contas/historico";
                var Json = new
                {
                    Contas
                };

                string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Timeout = 600000;
                requisicaoWeb.Method = "POST";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    //Envia os dados POST
                    using (var stream = requisicaoWeb.GetRequestStream())
                    {
                        stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                        stream.Close();
                    }

                    //Obtem a resposta do servidor
                    var httpResponse = requisicaoWeb.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        RespostaApi = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string Resposta = reader.ReadToEnd();
                            reader.Close();
                            data.Close();
                            httpResponse.Close();
                            requisicaoWeb.Abort();
                            DAOLogDB.SalvarLogs("", "Histórico Financeiro - Erro no envio do Histórico Financeiro - WebRequest", Resposta, "APP");
                        }
                    }
                }
                // });

                // MessageBox.Show(RespostaApi, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Histórico Financeiro - Erro no Histórico Financeiro - WebRequest", ex.Message, "APP");
            }
        }

        //Envia Tabelas de Preços API APP
        public static void EnviarTabeladePrecos()
        {
            try
            {
                string RespostaApi = "";

                List<APTabelasdePrecos> Tabelas = new List<APTabelasdePrecos>();

                string Sql = "SELECT p.codigo, p.nometabela, p.percentualdesconto, p.praticadopadrao FROM tabelasdeprecos p";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    APTabelasdePrecos Tabela = new APTabelasdePrecos();

                    Tabela.Codigo = dr["codigo"].ToString();
                    Tabela.Cnpj = DadosConfiguracao.Config.CnpjEmpresa;
                    Tabela.NomeTabela = dr["nometabela"].ToString();
                    Tabela.Desconto = ConverterDecimal(dr["percentualdesconto"].ToString());
                    Tabela.PraticadoPadrao = dr["praticadopadrao"].ToString();

                    Tabelas.Add(Tabela);
                }
                dr.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);


                string Url = UrlApi + "tabelasdeprecos";
                var Json = new
                {
                    Tabelas
                };

                string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Timeout = 600000;
                requisicaoWeb.Method = "POST";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    //Envia os dados POST
                    using (var stream = requisicaoWeb.GetRequestStream())
                    {
                        stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                        stream.Close();
                    }

                    //Obtem a resposta do servidor
                    var httpResponse = requisicaoWeb.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        RespostaApi = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    try
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Tabelas de Preços - Erro no envio de Tabelas de Preços - WebRequest", Resposta, "APP");
                            }
                        }
                    }
                    catch
                    {
                        DAOLogDB.SalvarLogs("", "Tabelas de Preços - Erro no envio de Tabelas de Preços - WebRequest", e.Message + "  ---  " + e.Status, "APP");
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Tabelas de Preços - Erro nas Tabelas de Preços", ex.Message, "APP");
            }
        }

        //Envia os Tipos de Documentos
        public static void EnviarDocumentos()
        {
            try
            {
                string RespostaApi = "";
                List<APDocumentos> Documentos = new List<APDocumentos>();

                string Sql = "select * from documentos d where d.status = 'Ativo'";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    APDocumentos Documento = new APDocumentos();

                    Documento.CodigoDocumento = dr["codigo"].ToString();
                    Documento.Cnpj = DadosConfiguracao.Config.CnpjEmpresa;
                    Documento.DescricaoDocumento = dr["documento"].ToString();
                    Documento.ParcelaMinima = ConverterDecimal(dr["parcelaminima"].ToString());
                    Documento.ConsiderarBloqueioAutomatico = Convert.ToBoolean(dr["considerarnobloqueioautomatico"].ToString());

                    Documentos.Add(Documento);
                }
                dr.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);


                string Url = UrlApi + "documentos";
                var Json = new
                {
                    Documentos
                };

                string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Timeout = 600000;
                requisicaoWeb.Method = "POST";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    //Envia os dados POST
                    using (var stream = requisicaoWeb.GetRequestStream())
                    {
                        stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                        stream.Close();
                    }

                    //Obtem a resposta do servidor
                    var httpResponse = requisicaoWeb.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        RespostaApi = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    try
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Documentos - Erro no envio dos Documentos - WebRequest", Resposta, "APP");
                            }
                        }
                    }
                    catch
                    {
                        DAOLogDB.SalvarLogs("", "Documentos - Erro no envio dos Documentos - WebRequest", e.Message + "  ---  " + e.Status, "APP");
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Documentos - Erro nos Documentos", ex.Message, "APP");
            }
        }

        //Envia Condições de Pagamento API APP
        public static void EnviarCondicoesPagamento()
        {
            try
            {
                string RespostaApi = "";

                List<APCondicaoPagamento> Condicoes = new List<APCondicaoPagamento>();

                string Sql = "SELECT * FROM condicoespagamento c WHERE c.`status` = 'Ativo'";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    APCondicaoPagamento Condicao = new APCondicaoPagamento();

                    Condicao.CodigoCondicao = dr["codigo"].ToString();
                    Condicao.Cnpj = DadosConfiguracao.Config.CnpjEmpresa;
                    Condicao.DescricaoCondicao = dr["condicao"].ToString();
                    Condicao.Desconto = ConverterDecimal(dr["pdesconto"].ToString());

                    Condicao.PercentualEntrada = ConverterDecimal(dr["pentrada"].ToString());
                    Condicao.ParcelasPrazo = Convert.ToInt32(ConverterDecimal(dr["parcelasprazo"].ToString()));
                    Condicao.CodigoDocumentoPrazo = dr["documentopadraoprazo"].ToString();
                    Condicao.CodigoDocumentoVista = dr["documentopadraovista"].ToString();
                    Condicao.Intervalo = dr["intervaloparcelas"].ToString();
                    Condicao.DiaPadrao = dr["diapadraovencimento"].ToString();

                    Condicoes.Add(Condicao);
                }
                dr.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);


                string Url = UrlApi + "condicoespagamento";
                var Json = new
                {
                    Condicoes
                };

                string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Timeout = 600000;
                requisicaoWeb.Method = "POST";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    //Envia os dados POST
                    using (var stream = requisicaoWeb.GetRequestStream())
                    {
                        stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                        stream.Close();
                    }


                    //Obtem a resposta do servidor
                    var httpResponse = requisicaoWeb.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        RespostaApi = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string Resposta = reader.ReadToEnd();
                            reader.Close();
                            data.Close();
                            httpResponse.Close();
                            requisicaoWeb.Abort();
                            DAOLogDB.SalvarLogs("", "Condição de Pagamento - Erro no envio de Condições de Pagamento - WebRequest", Resposta, "APP");
                        }
                    }
                }
                // });

                // MessageBox.Show(RespostaApi, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Condição de Pagamento - Erro nas Condições de Pagamento", ex.Message, "APP");
            }
        }

        //Recebe Pedidos da API APP
        public static void ReceberPedidos()
        {
            try
            {
                List<APPedidos> ListadePedidos = new List<APPedidos>();

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                string Url = UrlApi + "atendimentos/" + DadosConfiguracao.Config.CnpjEmpresa;
                string Json = "";
                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Method = "GET";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    using (var resposta = requisicaoWeb.GetResponse())
                    {
                        var streamDados = resposta.GetResponseStream();
                        StreamReader reader = new StreamReader(streamDados, Encoding.Default);
                        Json = reader.ReadToEnd();
                        streamDados.Close();
                        resposta.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string Resposta = reader.ReadToEnd();
                            reader.Close();
                            data.Close();
                            httpResponse.Close();
                            requisicaoWeb.Abort();
                            DAOLogDB.SalvarLogs("", "Pedidos - Erro na busca de pedidos - WebRequest", Resposta, "APP");
                        }
                    }
                }

                if (Json != "null" && Json.Length > 0)
                {
                    ListadePedidos = JsonConvert.DeserializeObject<List<APPedidos>>(Json);
                }

                //Cadastra os Pedidos no Sistema
                foreach (var Pedido in ListadePedidos)
                {
                    try
                    {
                        string CodigoPedido = "";
                        string Query = "SELECT a.codigo FROM atendimentos a WHERE a.pedidoapp ='" + Pedido.Codigo + "'";
                        MySqlCommand Comando0 = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando0.ExecuteReader();

                        if (Reader.Read())
                        {
                            CodigoPedido = Reader["codigo"].ToString();
                            Reader.Close();
                        }
                        DBConnectionMySql.FechaConexaoBD(DBMySql);

                        if (CodigoPedido.Equals(""))
                        {

                            VerPedidos PedidoAdd = new VerPedidos();
                            PedidoAdd.Itens = new List<VerItens>();
                            PedidoAdd.Contas = new List<VerContas>();

                            string CodigoCliente = ConsultaColaboradorAPP(Pedido.CodigoColaborador);
                            decimal pFrete = 0;

                            if (Pedido.ValorFrete > 0)
                            {
                                pFrete = Pedido.ValorFrete / (Pedido.ValorTotal - Pedido.ValorFrete);
                            }

                            if (Pedido.Status.Equals("Faturado"))
                            {
                                PedidoAdd.Status = "Faturado";
                            }
                            else
                            {
                                PedidoAdd.Status = "Não Faturado";
                            }

                            //Adiciona os Itens no Pedido
                            foreach (var Itens in Pedido.ItensPedido)
                            {
                                VerItens Item = new VerItens();

                                Item.Codigoproduto = Itens.CodigoProduto;
                                Item.Produto = Itens.DescricaoItem;
                                Item.Quantidade = Itens.Quantidade;
                                Item.Tabela = Itens.Praticado;
                                Item.Tabelacomdesconto = Itens.Praticado;
                                Item.Total = (Itens.Praticado * Itens.Quantidade);
                                Item.Totalcomdesconto = Itens.Valortotal;
                                Item.ObservacoesItem = Itens.ObservacoesItem;

                                var ValorFreteItem = Convert.ToDecimal((Itens.Valortotal * pFrete).ToString("f2"));
                                var TotalFrete = PedidoAdd.Itens.Sum(x => x.FreteItem);

                                if (Pedido.ValorFrete >= (TotalFrete + ValorFreteItem))
                                {
                                    Item.FreteItem = ValorFreteItem;
                                }
                                else
                                {
                                    Item.FreteItem = ValorFreteItem - ((TotalFrete + ValorFreteItem) - Pedido.ValorFrete);
                                }

                                PedidoAdd.Itens.Add(Item);
                            }

                            //Adiciona as Contas do Pedido
                            foreach (var Contas in Pedido.ContasPedido)
                            {
                                VerContas Conta = new VerContas();

                                Conta.CodigoConta = Contas.CodigoConta;
                                Conta.CodigoColaborador = Contas.CodigoColaborador;
                                Conta.CodigoAtendimento = Contas.CodigoAtendimento;
                                Conta.CodigoDocumento = Contas.CodigoDocumento;
                                Conta.CodigoTitular = "";

                                if (Contas.CodigoTitular != "")
                                {
                                    Conta.CodigoTitular = ConsultaColaboradorAPP(Contas.CodigoTitular);
                                }

                                Conta.DataEmissao = Contas.DataEmissao;
                                Conta.DataQuitacao = Contas.DataQuitacao;
                                Conta.DataVencimento = Contas.DataVencimento;
                                Conta.NumeroDocumento = Contas.NumeroDocumento;

                                if (PedidoAdd.Status.Equals("Faturado"))
                                {
                                    Conta.Status = "Aberta";
                                }
                                else
                                {
                                    Conta.Status = "Provisão";
                                }

                                Conta.ValorInicial = Contas.ValorInicial;
                                Conta.ValorQuitado = Contas.ValorQuitado;
                                Conta.ValorSaldo = Contas.ValorSaldo;

                                PedidoAdd.Contas.Add(Conta);
                            }


                            if (Pedido.Documento.Equals("Bonificação"))
                            {
                                Pedido.Documento = "Bonificação Saída";
                            }


                            PedidoAdd.Codigocolaborador = CodigoCliente;
                            PedidoAdd.Codigovendedor = Pedido.CodigoVendedor;
                            PedidoAdd.Codigotransportadora = Pedido.CodigoTransportadora;
                            PedidoAdd.Documento = Pedido.Documento;
                            PedidoAdd.Observacoes = Pedido.Observacoes;
                            PedidoAdd.Subtotal = Pedido.SubTotal;
                            PedidoAdd.Valorfrete = Convert.ToDecimal(Pedido.ValorFrete);
                            PedidoAdd.Valortotal = Pedido.ValorTotal;
                            PedidoAdd.Totalnf = Pedido.ValorTotal;
                            PedidoAdd.Valordesconto = Pedido.ValorDesconto;
                            PedidoAdd.Substatus = DadosConfiguracao.Config.CodigoSubStatusApp;
                            PedidoAdd.Pedidoapp = Pedido.Codigo;
                            PedidoAdd.Codigocondicaopagamento = Pedido.CodigoCondicaoPagamento;

                            if (!PedidoAdd.Codigocondicaopagamento.Equals(""))
                            {
                                PedidoAdd.Descricaocondicaopagamento = DAOPedidos.BuscarNomeCondicao(PedidoAdd.Codigocondicaopagamento);
                            }

                            PedidoAdd.Tabeladepreco = Pedido.CodigoTabelaPreco;
                            PedidoAdd.Especificardescontonoatendimento = Convert.ToBoolean(Pedido.EspecificarDescontoAtendimento);

                            if (DadosConfiguracao.Config.UtilizarCodigoVendedorComoEmpresaAPP)
                            {
                                PedidoAdd.Empresa = Pedido.CodigoVendedor;
                            }
                            else
                            {
                                PedidoAdd.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;
                            }

                            PedidoAdd.CodigoDocumentoaVista = Pedido.CodigoDocumentoaVista;
                            PedidoAdd.CodigoDocumentoPrazo = Pedido.CodigoDocumentoPrazo;
                            PedidoAdd.pEntrada = Pedido.pEntrada;
                            PedidoAdd.ValorEntrada = Pedido.ValorEntrada;
                            PedidoAdd.ValoraPrazo = Pedido.ValoraPrazo;
                            PedidoAdd.NumerodeParcelas = Pedido.NumerodeParcelas;


                            DAOPedidos.CadastrarPedido(PedidoAdd);
                            ConfirmaRecebimentoPedidos(Pedido);
                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs(Pedido.Codigo, "Pedidos - Erro no cadastro de pedido", ex.Message, "APP");
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Pedidos - Erro na requisição Web", ex.Message, "APP");
            }
        }

        //Confirma para a API que Recebeu o Pedido
        public static void ConfirmaRecebimentoPedidos(APPedidos DadosPedido)
        {
            List<APPedidos> Pedido = new List<APPedidos>();
            Pedido.Add(DadosPedido);

            string Url = UrlApi + "atendimentos";
            var Json = new
            {
                Pedido
            };

            string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "PUT";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            //Envia os dados POST
            using (var stream = requisicaoWeb.GetRequestStream())
            {
                stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                stream.Close();
            }

            try
            {
                //Obtem a resposta do servidor
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    // RespostaApi = streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string Resposta = reader.ReadToEnd();
                        reader.Close();
                        data.Close();
                        httpResponse.Close();
                        requisicaoWeb.Abort();
                        DAOLogDB.SalvarLogs(DadosPedido.Codigo, "Pedido - Erro no update do status do pedido", Resposta, "APP");
                    }
                }
            }
        }

        //Consulta o Cliente na API e Cadastra ou Atualiza os Dados no Sistema
        public static string ConsultaColaboradorAPP(string _CodigoCliente)
        {
            string Url = UrlApi + "colaboradores/" + DadosConfiguracao.Config.CnpjEmpresa + "/" + _CodigoCliente;
            string Json = "";
            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            try
            {
                using (var resposta = requisicaoWeb.GetResponse())
                {
                    var streamDados = resposta.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados, Encoding.Default);
                    Json = reader.ReadToEnd();
                    streamDados.Close();
                    resposta.Close();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string Resposta = reader.ReadToEnd();
                        reader.Close();
                        data.Close();
                        httpResponse.Close();
                        requisicaoWeb.Abort();
                        DAOLogDB.SalvarLogs("", "Clientes - Erro na consulta de clientes - WebRequest", Resposta, "APP");
                    }
                }
            }

            var Dados = JsonConvert.DeserializeObject<List<APColaboradores>>(Json);

            foreach (var Cliente in Dados)
            {
                string Codigo = DAOClientes.ConsultaColaborador(Cliente.Cpfcnpj);

                VerColaboradores Colaborador = new VerColaboradores();

                Colaborador.Nomerazaosocial = Cliente.Razaosocial;
                Colaborador.Nomefantasia = Cliente.Nomefantasia;
                Colaborador.Cpfcnpj = Cliente.Cpfcnpj;
                Colaborador.Inscricaoestadual = Cliente.Ie;
                Colaborador.Endereco = Cliente.Endereco;
                Colaborador.Numero = Cliente.Numero;
                Colaborador.Complemento = Cliente.Complemento;
                Colaborador.Bairro = Cliente.Bairro;
                Colaborador.Telefone = Cliente.Telefone;
                Colaborador.Celular = Cliente.Celular;
                Colaborador.Codigocidade = Cliente.Codigocidade;
                Colaborador.Email = Cliente.Email;
                Colaborador.Cep = Cliente.Cep;
                Colaborador.Observacao = Cliente.Observacao;
                Colaborador.CodigoSistema = Cliente.Codigocolaborador;
                Colaborador.Codigovendedor = Cliente.Codigovendedor;
                Colaborador.Datanascimento = Cliente.DataNascimento;
                Colaborador.Genero = Cliente.Genero;
                Colaborador.Contato = Cliente.Contato;

                if (DadosConfiguracao.Config.UtilizarCodigoVendedorComoEmpresaAPP)
                {
                    Colaborador.Empresa = Cliente.Codigovendedor;
                }
                else
                {
                    Colaborador.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;
                }

                Colaborador.Codigocidade = DAOCidades.BuscaCidadeAPP(Cliente.Codigocidade).CodigoCidade;


                if (!string.IsNullOrEmpty(Codigo))
                {
                    Colaborador.Codigo = Codigo;
                    DAOClientes.UpdateCliente(Colaborador);
                    return Codigo;
                }
                else
                {
                    return DAOClientes.CadastrarCliente(Colaborador);
                }
            }

            return "";
        }

        //Envia o Estoque da Empresa Tabela "tmpestoqueempresasapp" na API APP - FANES
        public static void EnviarEstoqueEmpresa()
        {
            try
            {
                List<APEstoque> EstoqueLista = new List<APEstoque>();

                string Sql = "Select * from tmpestoqueempresasapp";

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                cmd.CommandTimeout = 0;

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    APEstoque Estoque = new APEstoque();

                    Estoque.CodigoProduto = dr["produto"].ToString();
                    Estoque.CodigoVendedor = dr["empresa"].ToString();
                    Estoque.Estoque = ConverterDecimal(dr["estoquedisponivel"].ToString());

                    EstoqueLista.Add(Estoque);
                }
                dr.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);


                //Teste
                var Ts = new List<APEstoque>();

                int Cr = 0;
                int Ct = 0;

                foreach (var jn in EstoqueLista)
                {
                    Ts.Add(jn);

                    if (Cr == 5000)
                    {
                        Envia(Ts);
                        Ts.Clear();
                        Cr = 0;
                    }

                    Cr++;
                    Ct++;

                    var Valida = EstoqueLista.Count - Ct;

                    if (Valida == 0)
                    {
                        Envia(Ts);
                        Ts.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Estoque - Erro no Estoque", ex.Message, "APP");
            }

            void Envia(List<APEstoque> EstoqueLista)
            {

                try
                {
                    string RespostaApi = "";
                    string Url = UrlApi + "produtos/estoqueempresa";
                    var Json = new
                    {
                        EstoqueLista
                    };

                    string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                    var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                    var requisicaoWeb = WebRequest.CreateHttp(Url);
                    requisicaoWeb.Timeout = 900000;
                    requisicaoWeb.Method = "POST";
                    requisicaoWeb.ContentType = "application/json";
                    requisicaoWeb.Accept = "application/json";
                    requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                    requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                    requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                    try
                    {
                        //Envia os dados POST
                        using (var stream = requisicaoWeb.GetRequestStream())
                        {
                            stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                            stream.Close();
                        }

                        //Obtem a resposta do servidor
                        var httpResponse = requisicaoWeb.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            RespostaApi = streamReader.ReadToEnd();
                        }
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Estoque - Erro no envio do Estoque- WebRequest", Resposta, "APP");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DAOLogDB.SalvarLogs("", "Estoque - Erro no envio do Estoque", ex.Message, "APP");
                }
            }
        }

        // ///////////////////////////////////////////////   Sistema - Calcula o Estoque do Vendedor/Empresa e salva em uma tmp

        public static void CalcularEstoqueEmpresaVendedor()
        {
            try
            {
                DAOProdutos.CalculaEstoqueDisponivel();
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Estoque - Erro no Calculo do Estoque", ex.Message, "APP");
            }
        }

        // /////////////////////////////// AGENDA //////////////////////////////////////////

        //Envia os Usuarios do Sistema
        public static void EnviarUsuariosSistema()
        {
            try
            {
                string RespostaApi = "";
                List<APUsuariosSistema> Usuario = new List<APUsuariosSistema>();

                var Lista = DAOUsuariosSistema.BuscaUsuarios();

                foreach (var GetUsu in Lista)
                {
                    APUsuariosSistema Usu = new APUsuariosSistema();

                    Usu.Codigo = GetUsu.Codigo;
                    Usu.Usuario = GetUsu.Usuario;

                    Usuario.Add(Usu);
                }

                string Url = UrlApi + "usuariossistema";
                var Json = new
                {
                    Usuario
                };

                string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Timeout = 600000;
                requisicaoWeb.Method = "POST";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    //Envia os dados POST
                    using (var stream = requisicaoWeb.GetRequestStream())
                    {
                        stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                        stream.Close();
                    }

                    //Obtem a resposta do servidor
                    var httpResponse = requisicaoWeb.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        RespostaApi = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string Resposta = reader.ReadToEnd();
                            reader.Close();
                            data.Close();
                            httpResponse.Close();
                            requisicaoWeb.Abort();
                            DAOLogDB.SalvarLogs("", "Usuários Sistema - Erro no envio de Usuários - WebRequest", Resposta, "APP");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Usuários Sistema - Erro nos Usuários Sistema", ex.Message, "APP");
            }
        }

        //Envias as Ocorrencias API APP
        public static void EnviarOcorrencias()
        {
            try
            {
                string RespostaApi = "";
                List<APOcorrencias> Ocorrencia = new List<APOcorrencias>();

                var Lista = DAOOcorrencias.BuscaOcorrencias();

                foreach (var Ocor in Lista)
                {
                    APOcorrencias OcorrenciaCad = new APOcorrencias();

                    OcorrenciaCad.Codigoocorrencia = Ocor.Codigo;
                    OcorrenciaCad.Cnpj = DadosConfiguracao.Config.CnpjEmpresa;
                    OcorrenciaCad.Ocorrencia = Ocor.Ocorrencia;

                    Ocorrencia.Add(OcorrenciaCad);
                }

                string Url = UrlApi + "ocorrencias";
                var Json = new
                {
                    Ocorrencia
                };

                string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Timeout = 600000;
                requisicaoWeb.Method = "POST";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    //Envia os dados POST
                    using (var stream = requisicaoWeb.GetRequestStream())
                    {
                        stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                        stream.Close();
                    }

                    //Obtem a resposta do servidor
                    var httpResponse = requisicaoWeb.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        RespostaApi = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string Resposta = reader.ReadToEnd();
                            reader.Close();
                            data.Close();
                            httpResponse.Close();
                            requisicaoWeb.Abort();
                            DAOLogDB.SalvarLogs("", "Ocorrências - Erro no envio de Ocorrências - WebRequest", Resposta, "APP");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Ocorrências - Erro nas Ocorrências", ex.Message, "APP");
            }
        }


        // ///////////////// AGENDA V2 //////////////////////////////////////////////////////
        public static void EnviarContatosV2()
        {
            try
            {
                DateTime DataSincronizacao = DateTime.Now;
                string StatusCode = "";
                string RespostaApi = "";

                var Contatos = AP_agenda.BuscarContatos();

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                DBConnectionMySql.AbreConexaoBD(DBMySql);

                foreach (var contato in Contatos)
                {
                    try
                    {
                        var ContatoEnvio = new List<APContatos>();

                        if (!contato.Protocolo.Equals("") && !contato.Protocolo.Equals(null))
                            contato.OcorrenciasContato = AP_agenda.BuscarOcorrencias(contato.Protocolo);

                        ContatoEnvio.Add(contato);

                        var Retorno = Envia(ContatoEnvio);

                        if (Retorno.Equals("OK"))
                        {
                            string Query = "update contatos c SET c.appdatasincronizadoem = @data, c.apphorasincronizadoem = @hora where c.codigo = @codigo";
                            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                            Comando.Parameters.AddWithValue("@data", DataSincronizacao.Date.ToString("yyyy-MM-dd"));
                            Comando.Parameters.AddWithValue("@hora", DataSincronizacao.ToString("HH:mm:ss"));
                            Comando.Parameters.AddWithValue("@codigo", contato.Codigosistema);

                            Comando.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Contatos - Erro nas Contatos - Protocolo: " + contato.Protocolo, ex.Message, "APP");
                    }
                }

                DBConnectionMySql.FechaConexaoBD(DBMySql);

                string Envia(List<APContatos> Contato)
                {
                    string Url = UrlApi + "contatos/ERP";
                    var Json = new
                    {
                        Contato
                    };

                    string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                    var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                    var requisicaoWeb = WebRequest.CreateHttp(Url);
                    requisicaoWeb.Timeout = 600000;
                    requisicaoWeb.Method = "POST";
                    requisicaoWeb.ContentType = "application/json";
                    requisicaoWeb.Accept = "application/json";
                    requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                    requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                    requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                    try
                    {
                        using (var stream = requisicaoWeb.GetRequestStream())
                        {
                            stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                            stream.Close();
                        }

                        HttpWebResponse httpResponse = (HttpWebResponse)requisicaoWeb.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            RespostaApi = streamReader.ReadToEnd();
                            StatusCode = httpResponse.StatusCode.ToString();
                        }

                        return StatusCode;

                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Contatos - Erro no envio de Contatos - WebRequest", Resposta, "APP");
                            }
                        }

                        return "Erro";
                    }
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Contatos - Erro nas Contatos", ex.Message, "APP");
            }
        }

        public static void ReceberContatosV2()
        {
            try
            {
                DateTime DataSinc = DateTime.Now;

                List<APContatos> ListadeContatos = new List<APContatos>();

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                string Url = UrlApi + "contatos/todosP/" + DadosConfiguracao.Config.CnpjEmpresa;
                string Json = "";
                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Method = "GET";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    using (var resposta = requisicaoWeb.GetResponse())
                    {
                        var streamDados = resposta.GetResponseStream();
                        StreamReader reader = new StreamReader(streamDados, Encoding.Default);
                        Json = reader.ReadToEnd();
                        streamDados.Close();
                        resposta.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string Resposta = reader.ReadToEnd();
                            reader.Close();
                            data.Close();
                            httpResponse.Close();
                            requisicaoWeb.Abort();
                            DAOLogDB.SalvarLogs("", "Contatos - Erro na busca de contatos - WebRequest", Resposta, "APP");
                        }
                    }
                }

                if (Json != "null" && Json.Length > 0)
                {
                    ListadeContatos = JsonConvert.DeserializeObject<List<APContatos>>(Json);
                }

                var AlteraCodOcoContatos = new List<APOcorrenciasContatos>();
                var AlteraCodContatos = new List<APContatos>();

                //Cadastra os Pedidos no Sistema
                foreach (var Contato in ListadeContatos)
                {
                    try
                    {
                        var Operacao = new APConfirmaOperacao();
                        
                        //Adiciona as Ocorrencias do Contato
                        foreach (var Ocorrencias in Contato.OcorrenciasContato)
                        {
                            var Oco = new VerOcorrenciasContatos();

                            Oco.Codigoocorrencia = Ocorrencias.Codigosistema;
                            Oco.Codigoapp = Ocorrencias.Codigoocorrencia;
                            Oco.Datalancamento = Ocorrencias.Datalancamento;
                            Oco.Descricaoocorrencia = Ocorrencias.Descricaoocorrencia;
                            Oco.Hora = Ocorrencias.Hora;
                            Oco.Protocolo = Ocorrencias.Protocolo;
                            Oco.Usuariolancamento = Ocorrencias.Usuariolancamento;

                            string IdOco = DAOOcorrenciasContato.CadastrarOcorrenciasContato(Oco);

                            if (!IdOco.Equals(""))
                            {
                                var Oc = new APConfirmaOcorrencia();
                                Oc.CodigoSistema = IdOco;
                                Oc.CodigoApp = Oco.Codigoapp;

                                Operacao.Ocorrencias.Add(Oc);
                            }
                        }

                        string CodigoCliente = ConsultaColaboradorAPP(Contato.Codigocolaborador);

                        VerContatos Cont = new VerContatos();

                        Cont.Codigocontato = Contato.Codigosistema;
                        Cont.Codigosistema = Contato.Codigocontato;
                        Cont.Codigocolaborador = CodigoCliente;
                        Cont.Codigoocorrencia = Contato.Codigoocorrencia;
                        Cont.Atendimento = Contato.Atendimento;
                        Cont.Dataalteracao = Contato.Dataalteracao;
                        Cont.Dataconclusao = Contato.Dataconclusao;
                        Cont.Datalanacamento = Contato.Datalanacamento;
                        Cont.Diagnostico = Contato.Diagnostico;
                        Cont.Formadecontato = Contato.Formadecontato;
                        Cont.Horaalteracao = Contato.Horaalteracao;
                        Cont.Horaconclusao = Contato.Horaconclusao;
                        Cont.Horalancamento = Contato.Horalancamento;
                        Cont.Problema = Contato.Problema;
                        Cont.Protocolo = Contato.Protocolo;
                        Cont.Statusdocontato = Contato.Statusdocontato;
                        Cont.Tipodecontato = Contato.Tipodecontato;
                        Cont.Urgente = Contato.Urgente;
                        Cont.Usuario = Contato.Usuario;
                        Cont.Usuariolancamento = Contato.Usuariolancamento;

                        string Id = DAOContatos.CadastrarContato(Cont);

                        if (Id.Equals(""))
                        {
                            DAOContatos.UpdateContato(Cont);
                        }

                        Operacao.CodigoApp = Contato.Codigocontato;
                        Operacao.CodigoSistema = Id;
                        Operacao.DataSincronizacao = DataSinc.ToString("yyyy-MM-dd");
                        Operacao.HoraSincronizacao = DataSinc.ToString("HH:mm:ss");

                        UpdateDateSincronizacao(Operacao);
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Contatos - Erro no cadastro de contatos", ex.Message, "APP");
                    }
                }

                string UpdateDateSincronizacao(APConfirmaOperacao Operacao)
                {
                    string Url = UrlApi + "contatos/sincronizacao";

                    var Json = new
                    {
                        Operacao
                    };

                    string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                    var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                    var requisicaoWeb = WebRequest.CreateHttp(Url);
                    requisicaoWeb.Timeout = 100000;
                    requisicaoWeb.Method = "PUT";
                    requisicaoWeb.ContentType = "application/json";
                    requisicaoWeb.Accept = "application/json";
                    requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                    requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                    requisicaoWeb.UserAgent = "RequisicaoWebContatos";

                    try
                    {
                        string Retorno = "";
                        //Envia os dados POST
                        using (var stream = requisicaoWeb.GetRequestStream())
                        {
                            stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                            stream.Close();
                        }

                        //Obtem a resposta do servidor
                        var httpResponse = requisicaoWeb.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            Retorno = streamReader.ReadToEnd();
                        }

                        return Retorno;
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Contatos - Erro na atualização da data de sincronização - WebRequest", Resposta, "APP");
                            }
                        }

                        return "Erro";
                    }
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Contatos - Erro na requisição Web", ex.Message, "APP");
            }
        }

        public static void EnviarOcorrenciasV2()
        {
            try
            {
                string RespostaApi = "";
                List<APOcorrencias> Ocorrencia = new List<APOcorrencias>();

                var Lista = DAOOcorrencias.BuscaOcorrencias();

                foreach (var Ocor in Lista)
                {
                    APOcorrencias OcorrenciaCad = new APOcorrencias();

                    OcorrenciaCad.Codigoocorrencia = Ocor.Codigo;
                    OcorrenciaCad.Cnpj = DadosConfiguracao.Config.CnpjEmpresa;
                    OcorrenciaCad.Ocorrencia = Ocor.Ocorrencia;

                    Ocorrencia.Add(OcorrenciaCad);
                }

                string Url = UrlApi + "ocorrencias";
                var Json = new
                {
                    Ocorrencia
                };

                string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Timeout = 600000;
                requisicaoWeb.Method = "POST";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    //Envia os dados POST
                    using (var stream = requisicaoWeb.GetRequestStream())
                    {
                        stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                        stream.Close();
                    }

                    //Obtem a resposta do servidor
                    var httpResponse = requisicaoWeb.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        RespostaApi = streamReader.ReadToEnd();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string Resposta = reader.ReadToEnd();
                            reader.Close();
                            data.Close();
                            httpResponse.Close();
                            requisicaoWeb.Abort();
                            DAOLogDB.SalvarLogs("", "Ocorrências - Erro no envio de Ocorrências - WebRequest", Resposta, "APP");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Ocorrências - Erro nas Ocorrências", ex.Message, "APP");
            }
        }


        // ///////////////////////////////////////////////// ROTAS  //////////////////////////////////////
        //Recebe os Eventos - Rotas
        public static void ReceberEventos()
        {
            try
            {
                List<APContatos> ListadeContatos = new List<APContatos>();

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                string Url = UrlApi + "eventosagenda/recebe";
                string Json = "";
                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Method = "GET";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    using (var resposta = requisicaoWeb.GetResponse())
                    {
                        var streamDados = resposta.GetResponseStream();
                        StreamReader reader = new StreamReader(streamDados, Encoding.Default);
                        Json = reader.ReadToEnd();
                        streamDados.Close();
                        resposta.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string Resposta = reader.ReadToEnd();
                            reader.Close();
                            data.Close();
                            httpResponse.Close();
                            requisicaoWeb.Abort();
                            DAOLogDB.SalvarLogs("", "Eventos - Erro na busca de Eventos - WebRequest", Resposta, "APP");
                        }
                    }
                }

                if (Json != "null" && Json.Length > 0)
                {
                    ListadeContatos = JsonConvert.DeserializeObject<List<APContatos>>(Json);
                }

                //Cadastra os Pedidos no Sistema
                foreach (var Contato in ListadeContatos)
                {
                    try
                    {
                        string CodigoCliente = ConsultaColaboradorAPP(Contato.Codigocolaborador);

                        VerContatos Cont = new VerContatos();

                        Cont.Codigocontato = Contato.Codigosistema;
                        Cont.Codigosistema = Contato.Codigocontato;

                        Cont.Codigocolaborador = CodigoCliente;
                        Cont.Codigoocorrencia = Contato.Codigoocorrencia;
                        Cont.Atendimento = Contato.Atendimento;
                        Cont.Dataalteracao = Contato.Dataalteracao;
                        Cont.Dataconclusao = Contato.Dataconclusao;
                        Cont.Datalanacamento = Contato.Datalanacamento;
                        Cont.Diagnostico = Contato.Diagnostico;
                        Cont.Formadecontato = Contato.Formadecontato;
                        Cont.Horaalteracao = Contato.Horaalteracao;
                        Cont.Horaconclusao = Contato.Horaconclusao;
                        Cont.Horalancamento = Contato.Horalancamento;
                        Cont.Problema = Contato.Problema;
                        Cont.Protocolo = Contato.Protocolo;
                        Cont.Statusdocontato = Contato.Statusdocontato;
                        Cont.Tipodecontato = Contato.Tipodecontato;
                        Cont.Urgente = Contato.Urgente;
                        Cont.Usuario = Contato.Usuario;
                        Cont.Usuariolancamento = Contato.Usuariolancamento;

                        string Id = DAOContatos.CadastrarContato(Cont);

                        if (!Id.Equals(""))
                        {
                            DeleteEventoAPI(Contato.Codigocontato);
                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Evento - Erro no cadastro de Eventos", ex.Message, "APP");
                    }
                }

                void DeleteEventoAPI(string _codigo)
                {
                    string Url = UrlApi + "eventosagenda/delete/" + _codigo;
                    string Json = "";
                    var requisicaoWeb = WebRequest.CreateHttp(Url);
                    requisicaoWeb.Method = "POST";
                    requisicaoWeb.ContentType = "application/json";
                    requisicaoWeb.Accept = "application/json";
                    requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                    requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                    try
                    {
                        using (var resposta = requisicaoWeb.GetResponse())
                        {
                            var streamDados = resposta.GetResponseStream();
                            StreamReader reader = new StreamReader(streamDados, Encoding.Default);
                            Json = reader.ReadToEnd();
                            streamDados.Close();
                            resposta.Close();
                        }
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Eventos - Erro no deletar Evento - WebRequest", Resposta, "APP");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Eventos - Erro na requisição Web", ex.Message, "APP");
            }
        }

        public static void EnviaRotas()
        {
            try
            {
                string RespostaApi = "";
                var Rotas = new List<APRotas>();

                string SqlC = "select * from rotas";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmdc = new MySqlCommand(SqlC, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader drc = cmdc.ExecuteReader();

                while (drc.Read())
                {
                    var Rota = new APRotas();

                    Rota.codigo = drc["codigo"].ToString();
                    Rota.descricao = drc["rota"].ToString();

                    Rotas.Add(Rota);
                }
                drc.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);

                if (Rotas.Count > 0)
                {
                    string Url = UrlApi + "rotas/envio";
                    var Json = new
                    {
                        Rotas
                    };

                    string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                    var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                    var requisicaoWeb = WebRequest.CreateHttp(Url);
                    requisicaoWeb.Timeout = 600000;
                    requisicaoWeb.Method = "POST";
                    requisicaoWeb.ContentType = "application/json";
                    requisicaoWeb.Accept = "application/json";
                    requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                    requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                    requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                    try
                    {
                        //Envia os dados POST
                        using (var stream = requisicaoWeb.GetRequestStream())
                        {
                            stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                            stream.Close();
                        }

                        //Obtem a resposta do servidor
                        var httpResponse = requisicaoWeb.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            RespostaApi = streamReader.ReadToEnd();
                        }
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Rotas - Erro no envio de rotas - WebRequest", Resposta, "APP");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Rotas - Erro nas rotas - WebRequest", ex.Message, "APP");
            }
        }

        public static void EnviaRotasCidades()
        {
            try
            {
                string RespostaApi = "";
                var Rotas = new List<APRotasCidades>();

                string SqlC = "SELECT c.codigo, c.codigorota, c.ordem, ci.codigoibge as codigocidade, ci.cidade from rotascidades c LEFT JOIN cidades ci ON ci.codigo = c.codigocidade";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmdc = new MySqlCommand(SqlC, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader drc = cmdc.ExecuteReader();

                while (drc.Read())
                {

                    var Rota = new APRotasCidades();

                    Rota.codigo = drc["codigo"].ToString();
                    Rota.codigocidade = drc["codigocidade"].ToString();
                    Rota.codigorota = drc["codigorota"].ToString();

                    if (!Rota.codigocidade.Length.Equals(7))
                    {
                        DAOLogDB.SalvarLogs("", "Rotas Cidades - Erro nas rotas", "Cidade: " + drc["cidade"].ToString() + " está sem código IBGE", "APP");
                        return;
                    }

                    try
                    {
                        Rota.ordem = Convert.ToInt32(drc["ordem"].ToString());
                    }
                    catch
                    {
                        Rota.ordem = 0;
                    }

                    Rotas.Add(Rota);
                }
                drc.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);

                if (Rotas.Count > 0)
                {
                    string Url = UrlApi + "rotascidades/envio";
                    var Json = new
                    {
                        Rotas
                    };

                    string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                    var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                    var requisicaoWeb = WebRequest.CreateHttp(Url);
                    requisicaoWeb.Timeout = 600000;
                    requisicaoWeb.Method = "POST";
                    requisicaoWeb.ContentType = "application/json";
                    requisicaoWeb.Accept = "application/json";
                    requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                    requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                    requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                    try
                    {
                        //Envia os dados POST
                        using (var stream = requisicaoWeb.GetRequestStream())
                        {
                            stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                            stream.Close();
                        }

                        //Obtem a resposta do servidor
                        var httpResponse = requisicaoWeb.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            RespostaApi = streamReader.ReadToEnd();
                        }
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Rotas Cidades - Erro no envio de rotas - WebRequest", Resposta, "APP");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Rotas Cidades - Erro nas rotas", ex.Message, "APP");
            }
        }

        // //////////////////////////////////////////////     Imagens Produtos ///////////////////////////////////////////////////////////////
        public static void EnviarImagens()
        {
            try
            {
                var Imagens = new List<APImagens>();

                List<string> ImagensEncontradas = new List<string>();
                string DiretorioImagens = DadosConfiguracao.Config.DiretorioImagensProdutos;
                var Filtros = new string[] { "jpg", "jpeg", "png", "bmp" };

                //define as opções para exibir as imagens da pasta raiz
                var OpcaoDeBusca = false ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                foreach (var filtro in Filtros)
                {
                    ImagensEncontradas.AddRange(Directory.GetFiles(DiretorioImagens, string.Format("*.{0}", filtro), OpcaoDeBusca));
                }

                int Enviados = 1;
                foreach (var img in ImagensEncontradas)
                {
                    var Image = new APImagens();

                    FileInfo fileInfo = new FileInfo(img);
                    byte[] imageBytes = null;

                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(img))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            imageBytes = m.ToArray();

                            //Image.ImageBase64 = Convert.ToBase64String(imageBytes);
                            Image.NomeImagem = fileInfo.Name.Remove((fileInfo.Name).Length - 4);
                            Image.Extensao = fileInfo.Extension;
                            m.Close();

                        }
                    }

                    //Compacta a Imagem
                    using (MemoryStream Compress = new MemoryStream())
                    {
                        using (GZipStream zip = new GZipStream(Compress, CompressionMode.Compress, true))
                        {
                            zip.Write(imageBytes, 0, imageBytes.Length);
                            byte[] imageBytes1 = Compress.ToArray();
                            Image.ImagemByte = imageBytes1;
                        }
                    }





                    Imagens.Add(Image);

                    var Env = ImagensEncontradas.Count() - Enviados;

                    if (Imagens.Count() == 10 || Env <= 10)
                    {
                        Envia();
                        Imagens.Clear();
                    }
                }

                void Envia()
                {
                    string Url = UrlApi + "imagens";
                    var Json = new
                    {
                        Imagens
                    };



                    string ArquivoJson = JsonConvert.SerializeObject(Json); //Serealiza a lista de Colaboradores
                    var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

                    var requisicaoWeb = WebRequest.CreateHttp(Url);
                    requisicaoWeb.Timeout = 900000;
                    requisicaoWeb.Method = "POST";
                    requisicaoWeb.ContentType = "application/json";
                    requisicaoWeb.Accept = "application/json";
                    requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                    requisicaoWeb.ContentLength = ArquivoEnvio.Length;
                    requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                    try
                    {
                        //Envia os dados POST
                        using (var stream = requisicaoWeb.GetRequestStream())
                        {
                            stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                            stream.Close();
                        }

                        //Obtem a resposta do servidor
                        var httpResponse = requisicaoWeb.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            streamReader.ReadToEnd();
                        }
                    }
                    catch (WebException e)
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string Resposta = reader.ReadToEnd();
                                reader.Close();
                                data.Close();
                                httpResponse.Close();
                                requisicaoWeb.Abort();
                                DAOLogDB.SalvarLogs("", "Imagens - Erro no envio das Imagens - WebRequest", Resposta, "APP");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Imagens - Erro na requisição Web", ex.Message, "APP");
            }
        }

    }
}
