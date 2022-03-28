using IntegracaoRockye.Rocky.DB;
using IntegracaoRockye.Rocky.Estoque;
using IntegracaoRockye.Rocky.Model;
using IntegracaoRockye.Rocky.Model.List;
using IntegracaoRockye.Rocky.V2Model;
using IntegracaoRockye.Tray;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Funcoes;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegracaoRockye.Rocky
{
    public static class RKFuncoes
    {
        ////////////////////////////////    Versão 1.0      //////////////////////////////////////////////////

        public static void Atualizar_Estoque_Produto_Variacoes()
        {
            try
            {
                var ListaProdutosApi = RKConectionWebService.BuscarProdutos();
                var Lista_SkuApi = RKConectionWebService.ReceberProdutos_Variacoes();

                foreach (var ProdutoApi in ListaProdutosApi)
                {
                    try
                    {
                        var Produto = DAOProdutos.GetProdutosCadastroEstoque(ProdutoApi.referencia);

                        if (Produto.referencia != "")
                        {
                            RKSku PV = new RKSku();

                            PV.id = Produto.id;
                            PV.id_produto = Produto.id;
                            PV.sku = Produto.id;
                            PV.nome = Produto.nome;
                            PV.status = 1;
                            PV.quantidade = Produto.quantidade;
                            PV.preco = Produto.preco;
                            PV.preco_base = Produto.preco_base;
                            PV.peso = Produto.peso;
                            PV.altura = Produto.altura;
                            PV.comprimento = Produto.comprimento;
                            PV.largura = Produto.largura;

                            var Sku = Lista_SkuApi.Where(s => s.sku == Produto.referencia);

                            if (Sku.Count() > 0)
                            {
                                foreach (var s in Sku)
                                {
                                    PV.id = s.id;
                                    PV.status = s.status;

                                    if (PV.preco > 0)
                                    {
                                        RKConectionWebService.AtualizarProdutos_Variacoes(PV);
                                    }
                                }
                            }
                            else
                            {
                                if (PV.preco > 0)
                                {
                                    PV.status = 0;
                                    RKConectionWebService.EnviarProdutos_Variacoes(PV);
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Sku - Erro - Produto " + ProdutoApi.referencia + "", ex.Message, "Site");
                    }
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Sku - Erro", ex.Message, "Site");
            }
        }

        public static void CopiarPedidos()
        {
            try
            {
                var Pedidos = RKConectionWebService.ReceberPedidosLista();

                foreach (var Pedido in Pedidos)
                {
                    if (Pedido.status == "3")
                    {
                        PedidosAdd.AddPedido(Pedido);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void EnviarProdutos()
        {
            try
            {
                var ProdutosApi = RKConectionWebService.BuscarProdutos();
                var Lista = DAOProdutos.GetProdutosCadastro();

                foreach (var Produto in Lista)
                {
                    if (ProdutosApi.Where(l => l.referencia == Produto.id).Count() > 0)
                    {
                        Produto.referencia = "";

                        if (Produto.preco > 0)
                        {
                            var Resposta = RKConectionWebService.AtualizarProdutos(Produto);
                        }
                    }
                    else
                    {
                        if (Produto.preco > 0)
                        {
                            var Resposta = RKConectionWebService.EnviarProdutos(Produto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////   Versão 2.0    ///////////////////////////////////////////////////
        public static void EnviarProdutosV2()
        {
            try
            {
                var Lista = GetProdutos();

                foreach (var Produto in Lista)
                {
                    var Resposta = RKConectionWebService.EnviarProdutosV2(Produto);

                    if (Resposta.Code.Equals("200"))
                    {
                        AlteraStatusEnviado(Resposta.Response, Produto.codigoproduto);
                    }
                }

                List<RKProdutos> GetProdutos()
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        List<RKProdutos> ListaProdutos = new List<RKProdutos>();

                        string Query = "select p.codigo, p.sequencia, p.referencia, p.referenciaecommerce, p.descricao, p.nomecomercial, p.praticado, p.pesobruto, p.quantidadeembalagem, p.largura, p.altura, p.comprimento, m.codigoadicional as codigomarca, p.estoquedisponivel, p.praticado2, p.praticado3, p.praticado4, p.praticado5 from produtoseservicos p inner join marcas m on m.codigo = p.codigomarca where p.tipo = 'Produto' and p.situacao = 'Ativo' and (m.codigoadicional is not null and m.codigoadicional <> '') and p.site = '1' and p.situacaoecommerce = '1' and p.enviadoecommerce = '0'";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            RKProdutos Produto = new RKProdutos();

                            Produto.id = Reader["codigo"].ToString();

                            if(DadosConfiguracao.Config.UstilizarSequenciacomoCodigoRocky)
                            {
                                Produto.id = Reader["sequencia"].ToString();
                            }

                            Produto.codigoproduto = Reader["codigo"].ToString();

                            if (!string.IsNullOrEmpty(Reader["nomecomercial"].ToString()))
                            {
                                Produto.nome = Reader["nomecomercial"].ToString();
                            }
                            else
                            {
                                Produto.nome = Reader["descricao"].ToString();
                            }

                            //Produto.referencia = Produto.id; ??
                            Produto.referencia = Reader["referenciaecommerce"].ToString();  // ??

                            //Produto.descricao = "<p>" + Produto.nome + "</p>";
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 1"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 2"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado2"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 3"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado3"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 4"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado4"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 5"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado5"].ToString());
                            }

                            Produto.preco = Produto.preco_base;
                            Produto.quantidade = ConverterDecimal(Reader["estoquedisponivel"].ToString());
                            Produto.preco_promocional = 0;
                            Produto.peso = ConverterDecimal(Reader["pesobruto"].ToString()) * 1000;
                            Produto.comprimento = ConverterDecimal(Reader["comprimento"].ToString()).ToString();
                            Produto.altura = ConverterDecimal(Reader["altura"].ToString()).ToString();
                            Produto.largura = ConverterDecimal(Reader["largura"].ToString()).ToString();
                            Produto.multiplos = Reader["quantidadeembalagem"].ToString();
                            Produto.marca = Reader["codigomarca"].ToString();

                            ListaProdutos.Add(Produto);
                        }

                        Reader.Close();

                        return ListaProdutos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Site");
                        return new List<RKProdutos>();
                    }
                    finally
                    {
                        DBConnectionMySql.FechaConexaoBD(DBMySql);
                    }
                }

                void AlteraStatusEnviado(string _Retorno, string _CodProd)
                {
                    var Json = _Retorno.Replace("[", "").Replace("]", "").Replace("ref", "referencia");


                    var Prod = JsonConvert.DeserializeObject<RKRetornoCadastroProduto>(Json);

                    string Sql = "update produtoseservicos set enviadoecommerce = @enviadoecommerce, codigoecommerce = @codigoecommerce where codigo = @codigo";
                    var DBMySqlI = new MySqlConnection(DBConnectionMySql.strConnection);
                    var cmdI = new MySqlCommand(Sql, DBMySqlI);

                    cmdI.Parameters.AddWithValue("@codigo", _CodProd);
                    cmdI.Parameters.AddWithValue("@enviadoecommerce", "1");
                    cmdI.Parameters.AddWithValue("@codigoecommerce", Prod.product.id);

                    DBConnectionMySql.AbreConexaoBD(DBMySqlI);
                    cmdI.CommandTimeout = 0;
                    cmdI.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(DBMySqlI);
                }

                decimal ConverterDecimal(string Valor)
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
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Produtos - Erro nos Produtos", ex.Message, "Site");
            }
        }

        public static void UpdateProdutosV2()
        {
            try
            {
                var Lista = GetProdutos();

                foreach (var Produto in Lista)
                {
                    var Resposta = RKConectionWebService.UpdateProdutosV2(Produto);
                }

                List<RKProdutosUpdate> GetProdutos()
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        List<RKProdutosUpdate> ListaProdutos = new List<RKProdutosUpdate>();

                        string Query = "select p.codigo, p.sequencia, p.codigoecommerce, p.referenciaecommerce, p.referencia, p.descricao, p.nomecomercial, p.praticado, p.pesobruto, p.quantidadeembalagem, p.largura, p.altura, p.comprimento, m.codigoadicional as codigomarca, p.estoquedisponivel, p.praticado2, p.praticado3, p.praticado4, p.praticado5 from produtoseservicos p inner join marcas m on m.codigo = p.codigomarca where p.tipo = 'Produto' and p.situacao = 'Ativo' and (m.codigoadicional is not null and m.codigoadicional <> '') and p.site = '1' and p.situacaoecommerce = '1' and p.enviadoecommerce = '1'";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            RKProdutosUpdate Produto = new RKProdutosUpdate();

                            Produto.id = Reader["codigoecommerce"].ToString();
                            Produto.codigoproduto = Reader["codigo"].ToString();

                            if (!string.IsNullOrEmpty(Reader["nomecomercial"].ToString()))
                            {
                                Produto.nome = Reader["nomecomercial"].ToString();
                            }
                            else
                            {
                                Produto.nome = Reader["descricao"].ToString();
                            }

                            //Produto.referencia = Reader["codigo"].ToString();
                            //Produto.referencia = Reader["referenciaecommerce"].ToString();
                            //Produto.descricao = "<p>" + Produto.nome + "</p>";
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 1"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 2"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado2"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 3"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado3"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 4"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado4"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 5"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado5"].ToString());
                            }


                            Produto.preco = Produto.preco_base;
                            Produto.quantidade = ConverterDecimal(Reader["estoquedisponivel"].ToString());
                            Produto.preco_promocional = 0;
                            Produto.peso = ConverterDecimal(Reader["pesobruto"].ToString()) * 1000;
                            Produto.comprimento = ConverterDecimal(Reader["comprimento"].ToString()).ToString();
                            Produto.altura = ConverterDecimal(Reader["altura"].ToString()).ToString();
                            Produto.largura = ConverterDecimal(Reader["largura"].ToString()).ToString();
                            Produto.multiplos = Reader["quantidadeembalagem"].ToString();
                            Produto.marca = Reader["codigomarca"].ToString();

                            ListaProdutos.Add(Produto);
                        }

                        Reader.Close();

                        return ListaProdutos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Site");
                        return new List<RKProdutosUpdate>();
                    }
                    finally
                    {
                        DBConnectionMySql.FechaConexaoBD(DBMySql);
                    }
                }

                decimal ConverterDecimal(string Valor)
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
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Produtos - Erro nos Produtos", ex.Message, "Site");
            }
        }

        public static void EnviarVariacoesProdutosV2()
        {
            try
            {
                var ProdutosEnviados = GetProdutosEnviados();

                foreach (var ProdutoApi in ProdutosEnviados)
                {
                    try
                    {
                        var Produtos = DadosVariacao(ProdutoApi.referenciaecommerce);

                        foreach (var Produto in Produtos)
                        {
                            var PV = new RKProduct_variationInsertV2();

                            PV.id_produto = ProdutoApi.id;
                            PV.codigo = Produto.id;
                            PV.nome = Produto.nome;
                            PV.status = 1;
                            PV.quantidade = Produto.quantidade;
                            PV.preco = Produto.preco;
                            PV.preco_base = Produto.preco_base;
                            PV.peso = Produto.peso;
                            PV.altura = Produto.altura;
                            PV.comprimento = Produto.comprimento;
                            PV.largura = Produto.largura;

                            PV.brand = new RKBrand();
                            PV.brand.id = Produto.marca;
                            PV.brand.nome = Produto.descmarca;


                            if (Produto.codigotamanho.Length > 0 && Produto.codigocor.Length > 0)
                            {
                                PV.configurations = new string[2];
                                PV.configurations[0] = Produto.codigotamanho;
                                PV.configurations[1] = Produto.codigocor;
                            }
                            if (Produto.codigotamanho.Length > 0 && Produto.codigocor.Length == 0)
                            {
                                PV.configurations = new string[1];
                                PV.configurations[0] = Produto.codigotamanho;
                            }
                            if (Produto.codigotamanho.Length == 0 && Produto.codigocor.Length > 0)
                            {
                                PV.configurations = new string[1];
                                PV.configurations[0] = Produto.codigocor;
                            }


                            //Referencia
                            PV.product_ref = new List<RKsku_product_ref>();
                            var refe = new RKsku_product_ref();
                            refe.referencia = ProdutoApi.id;
                            PV.product_ref.Add(refe);

                            var Response = RKConectionWebService.EnviarProdutos_VariacoesV2(PV);

                            if (Response.Code.Equals("200"))
                            {
                                AlteraAjustaCodigo(Response.Response, PV.codigo);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Sku - Erro - Produto " + ProdutoApi.referencia + "", ex.Message, "Site");
                    }
                }

                void AlteraAjustaCodigo(string _Retorno, string _CodProd)
                {
                    var Json = _Retorno.Replace("\"ref", "\"referencia");

                    var Prod = JsonConvert.DeserializeObject<RKRetornoCadastroVariacaoProduto>(Json);

                    string Sql = "update produtoseservicos set codigovariacao = @codigovariacao, enviadoecommerce = @enviadoecommerce where codigo = @codigo";
                    var DBMySqlI = new MySqlConnection(DBConnectionMySql.strConnection);
                    var cmdI = new MySqlCommand(Sql, DBMySqlI);

                    cmdI.Parameters.AddWithValue("@codigo", _CodProd);
                    cmdI.Parameters.AddWithValue("@enviadoecommerce", "1");
                    cmdI.Parameters.AddWithValue("@codigovariacao", Prod.product_variation.id);

                    DBConnectionMySql.AbreConexaoBD(DBMySqlI);
                    cmdI.CommandTimeout = 0;
                    cmdI.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(DBMySqlI);
                }

                List<RKProdutos> GetProdutosEnviados()
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        List<RKProdutos> ListaProdutos = new List<RKProdutos>();

                        string Query = "select p.codigo, p.referencia, p.codigoecommerce, p.referenciaecommerce from produtoseservicos p inner join marcas m on m.codigo = p.codigomarca where p.tipo = 'Produto' and p.situacao = 'Ativo' and (m.codigoadicional is not null and m.codigoadicional <> '') and p.site = '1' and p.situacaoecommerce = '1' and p.enviadoecommerce = '1' and (p.codigoecommerce <> '' and p.codigoecommerce is not null)";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            RKProdutos Produto = new RKProdutos();

                            Produto.id = Reader["codigoecommerce"].ToString();
                            Produto.referencia = Reader["codigo"].ToString();
                            Produto.referenciaecommerce = Reader["referenciaecommerce"].ToString();

                            ListaProdutos.Add(Produto);
                        }

                        Reader.Close();

                        return ListaProdutos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Site");
                        return new List<RKProdutos>();
                    }
                    finally
                    {
                        DBConnectionMySql.FechaConexaoBD(DBMySql);
                    }
                }

                List<RKProdutos> DadosVariacao(string _ReferenciaEconnerce)
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    var Produtos = new List<RKProdutos>();

                    try
                    {
                        string Query = "select p.codigo, p.sequencia, p.referencia, p.descricao, p.nomecomercial, p.quantidadeembalagem, p.pesobruto, p.largura, p.altura, p.comprimento, m.codigoadicional as codigomarca, m.marca, co.codigoadicional AS codigocor, co.cor, t.codigoadicional AS codigotamanho, t.tamanho, p.estoquedisponivel, p.sku, p.praticado, p.praticado2, p.praticado3, p.praticado4, p.praticado5 from produtoseservicos p " +
                            "inner join marcas m on m.codigo = p.codigomarca left join cores co on co.codigo = p.codigocor" +
                            " LEFT JOIN tamanhos t ON t.codigo = p.codigotamanho where (p.referenciaecommerce = '" + _ReferenciaEconnerce + "' AND p.referenciaecommerce <> '' AND p.referenciaecommerce IS NOT NULL) and (p.codigovariacao = '' or p.codigovariacao IS NULL)";

                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var Produto = new RKProdutos();

                            Produto.id = Reader["codigo"].ToString();
                            
                            //if (DadosConfiguracao.Config.UstilizarSequenciacomoCodigoRocky)
                            //{
                            //    Produto.id = Reader["sequencia"].ToString();
                            //}

                            Produto.codigoproduto = Reader["codigo"].ToString();

                            if (!string.IsNullOrEmpty(Reader["nomecomercial"].ToString()))
                            {
                                Produto.nome = Reader["nomecomercial"].ToString();
                            }
                            else
                            {
                                Produto.nome = Reader["descricao"].ToString();
                            }

                            Produto.referencia = Produto.codigoproduto;
                            //Produto.descricao = "<p>" + Produto.nome + "</p>";

                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 1"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 2"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado2"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 3"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado3"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 4"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado4"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 5"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado5"].ToString());
                            }

                            Produto.preco = Produto.preco_base;

                           // Produto.quantidade = ConverterDecimal(Reader["estoquedisponivel"].ToString());
                            Produto.preco_promocional = 0;
                            Produto.peso = ConverterDecimal(Reader["pesobruto"].ToString()) * 1000;
                            Produto.comprimento = ConverterDecimal(Reader["comprimento"].ToString()).ToString();
                            Produto.altura = ConverterDecimal(Reader["altura"].ToString()).ToString();
                            Produto.largura = ConverterDecimal(Reader["largura"].ToString()).ToString();
                            Produto.multiplos = Reader["quantidadeembalagem"].ToString();

                            Produto.marca = Reader["codigomarca"].ToString();
                            Produto.descmarca = Reader["marca"].ToString();

                            Produto.codigocor = Reader["codigocor"].ToString();
                            Produto.cor = Reader["cor"].ToString();

                            Produto.codigotamanho = Reader["codigotamanho"].ToString();
                            Produto.tamanho = Reader["tamanho"].ToString();


                            //Estoque ------------------------------------------------------------------
                            Produto.quantidade = ConverterDecimal(Reader["estoquedisponivel"].ToString());

                            string Sku = Reader["sku"].ToString();

                            if (DadosConfiguracao.Config.CalcularEstoquePeloSKURocky)
                            {
                                Produto.quantidade = CarregaEstoqueV2(Sku);
                            }
                            //---------------------------------------------------------------------------

                            //Estoque por "Deposito" -- Utilizada quando o Cliente quer Separar os Estoques
                            if (DadosConfiguracao.Config.EnviarEstoqueDeposito.Length > 0)
                            {
                                Produto.quantidade = DAOProdutos.CalculaEstoqueDeposito(Produto.codigoproduto);
                            }
                            //----------------------------------------------------------------------------

                            Produtos.Add(Produto);
                        }

                        Reader.Close();

                        return Produtos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Site");
                        return Produtos;
                    }
                    finally
                    {
                        DBConnectionMySql.FechaConexaoBD(DBMySql);
                    }
                }

                decimal ConverterDecimal(string Valor)
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

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Sku - Erro", ex.Message, "Site");
            }
        }

        public static void UpdateVariacoesProdutosV2()
        {
            try
            {
                var ProdutosEnviados = GetProdutosEnviados();

                foreach (var ProdutoApi in ProdutosEnviados)
                {
                    try
                    {
                        var Produtos = DadosVariacao(ProdutoApi.referenciaecommerce);

                        foreach (var Produto in Produtos)
                        {
                            var PV = new RKProduct_variationV2();

                            PV.id = Produto.codigovariacao;
                            PV.id_produto = ProdutoApi.id;
                            //PV.sku = ProdutoApi.id + "-" + Produto.id;
                            PV.codigo = Produto.id;
                            PV.nome = Produto.nome;
                            PV.status = 1;
                            PV.quantidade = Produto.quantidade;
                            PV.preco = Produto.preco;
                            PV.preco_base = Produto.preco_base;
                            PV.peso = Produto.peso;
                            PV.altura = Produto.altura;
                            PV.comprimento = Produto.comprimento;
                            PV.largura = Produto.largura;

                            PV.brand = new RKBrand();
                            PV.brand.id = Produto.marca;
                            PV.brand.nome = Produto.descmarca;

                            //Update Não Altera a Configurations
                            //if (Produto.codigotamanho.Length > 0 && Produto.codigocor.Length > 0)
                            //{
                            //    PV.configurations = new string[2];
                            //    PV.configurations[0] = Produto.codigotamanho;
                            //    PV.configurations[1] = Produto.codigocor;
                            //}
                            //if (Produto.codigotamanho.Length > 0 && Produto.codigocor.Length == 0)
                            //{
                            //    PV.configurations = new string[1];
                            //    PV.configurations[0] = Produto.codigotamanho;
                            //}
                            //if (Produto.codigotamanho.Length == 0 && Produto.codigocor.Length > 0)
                            //{
                            //    PV.configurations = new string[1];
                            //    PV.configurations[0] = Produto.codigocor;
                            //}


                            //Referencia
                            PV.product_ref = new List<RKsku_product_ref>();
                            var refe = new RKsku_product_ref();
                            refe.referencia = ProdutoApi.id;
                            PV.product_ref.Add(refe);

                            var Response = RKConectionWebService.UpdateProdutos_VariacoesV2(PV);

                            if (Response.Code.Equals("200"))
                            {
                                AlteraAjustaCodigo(Response.Response, PV.codigo);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Sku - Erro - Produto " + ProdutoApi.referencia + "", ex.Message, "Site");
                    }
                }

                void AlteraAjustaCodigo(string _Retorno, string _CodProd)
                {
                    var Json = _Retorno.Replace("\"ref", "\"referencia");

                    var Prod = JsonConvert.DeserializeObject<RKRetornoCadastroVariacaoProduto>(Json);

                    string Sql = "update produtoseservicos set codigovariacao = @codigovariacao, enviadoecommerce = @enviadoecommerce where codigo = @codigo";
                    var DBMySqlI = new MySqlConnection(DBConnectionMySql.strConnection);
                    var cmdI = new MySqlCommand(Sql, DBMySqlI);

                    cmdI.Parameters.AddWithValue("@codigo", _CodProd);
                    cmdI.Parameters.AddWithValue("@enviadoecommerce", "1");
                    cmdI.Parameters.AddWithValue("@codigovariacao", Prod.product_variation.id);

                    DBConnectionMySql.AbreConexaoBD(DBMySqlI);
                    cmdI.CommandTimeout = 0;
                    cmdI.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(DBMySqlI);
                }


                List<RKProdutos> GetProdutosEnviados()
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        List<RKProdutos> ListaProdutos = new List<RKProdutos>();

                        string Query = "select p.codigo, p.referencia, p.codigoecommerce, p.referenciaecommerce from produtoseservicos p inner join marcas m on m.codigo = p.codigomarca where p.tipo = 'Produto' and p.situacao = 'Ativo' and (m.codigoadicional is not null and m.codigoadicional <> '') and p.site = '1' and p.situacaoecommerce = '1' and p.enviadoecommerce = '1' and (p.codigoecommerce <> '' and p.codigoecommerce is not null)";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            RKProdutos Produto = new RKProdutos();

                            Produto.id = Reader["codigoecommerce"].ToString();
                            Produto.referencia = Reader["codigo"].ToString();
                            Produto.referenciaecommerce = Reader["referenciaecommerce"].ToString();

                            ListaProdutos.Add(Produto);
                        }

                        Reader.Close();

                        return ListaProdutos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Site");
                        return new List<RKProdutos>();
                    }
                    finally
                    {
                        DBConnectionMySql.FechaConexaoBD(DBMySql);
                    }
                }

                List<RKProdutos> DadosVariacao(string _ReferenciaEconnerce)
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    var Produtos = new List<RKProdutos>();

                    try
                    {
                        string Query = "select p.codigo, p.codigovariacao, p.referencia, p.descricao, p.nomecomercial, p.quantidadeembalagem, p.pesobruto, p.largura, p.altura, p.comprimento, m.codigoadicional as codigomarca, m.marca, co.codigoadicional AS codigocor, co.cor, t.codigoadicional AS codigotamanho, t.tamanho, p.estoquedisponivel, p.sku, p.praticado, p.praticado2, p.praticado3, p.praticado4, p.praticado5 from produtoseservicos p " +
                            "inner join marcas m on m.codigo = p.codigomarca left join cores co on co.codigo = p.codigocor" +
                            " LEFT JOIN tamanhos t ON t.codigo = p.codigotamanho where (p.referenciaecommerce = '" + _ReferenciaEconnerce + "' AND p.referenciaecommerce <> '' AND p.referenciaecommerce IS NOT NULL) and p.enviadoecommerce = '1' and (p.codigovariacao <> '' and p.codigovariacao is not null)";

                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var Produto = new RKProdutos();

                            Produto.id = Reader["codigo"].ToString();
                            Produto.codigoproduto = Reader["codigo"].ToString();
                            Produto.codigovariacao = Reader["codigovariacao"].ToString();

                            if (!string.IsNullOrEmpty(Reader["nomecomercial"].ToString()))
                            {
                                Produto.nome = Reader["nomecomercial"].ToString();
                            }
                            else
                            {
                                Produto.nome = Reader["descricao"].ToString();
                            }

                            Produto.referencia = Reader["codigo"].ToString();
                            //Produto.descricao = "<p>" + Produto.nome + "</p>";

                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 1"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 2"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado2"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 3"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado3"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 4"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado4"].ToString());
                            }
                            if (DadosConfiguracao.Config.PraticadoPadraoEcommerce.Equals("Praticado 5"))
                            {
                                Produto.preco_base = ConverterDecimal(Reader["praticado5"].ToString());
                            }

                            Produto.preco = Produto.preco_base;
                            
                            Produto.preco_promocional = 0;
                            Produto.peso = ConverterDecimal(Reader["pesobruto"].ToString()) * 1000;
                            Produto.comprimento = ConverterDecimal(Reader["comprimento"].ToString()).ToString();
                            Produto.altura = ConverterDecimal(Reader["altura"].ToString()).ToString();
                            Produto.largura = ConverterDecimal(Reader["largura"].ToString()).ToString();
                            Produto.multiplos = Reader["quantidadeembalagem"].ToString();

                            Produto.marca = Reader["codigomarca"].ToString();
                            Produto.descmarca = Reader["marca"].ToString();

                            Produto.codigocor = Reader["codigocor"].ToString();
                            Produto.cor = Reader["cor"].ToString();

                            Produto.codigotamanho = Reader["codigotamanho"].ToString();
                            Produto.tamanho = Reader["tamanho"].ToString();

                            //Estoque ------------------------------------------------------------------
                            Produto.quantidade = ConverterDecimal(Reader["estoquedisponivel"].ToString());

                            string Sku = Reader["sku"].ToString();
                            
                            if (DadosConfiguracao.Config.CalcularEstoquePeloSKURocky) 
                            {
                                Produto.quantidade = CarregaEstoqueV2(Sku);
                            }
                            //---------------------------------------------------------------------------

                            //Estoque por "Deposito" -- Utilizada quando o Cliente quer Separar os Estoques
                            if (DadosConfiguracao.Config.EnviarEstoqueDeposito.Length > 0)
                            {
                                Produto.quantidade = DAOProdutos.CalculaEstoqueDeposito(Produto.codigoproduto);
                            }
                            //----------------------------------------------------------------------------


                            Produtos.Add(Produto);
                        }

                        Reader.Close();

                        return Produtos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Site");
                        return Produtos;
                    }
                    finally
                    {
                        DBConnectionMySql.FechaConexaoBD(DBMySql);
                    }
                }

                //Converte para Decimal
                decimal ConverterDecimal(string Valor)
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

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Sku - Erro", ex.Message, "Site");
            }
        }

        public static void CopiarPedidosV2()
        {
            try
            {
                var Pedidos = RKConectionWebService.ReceberPedidosListaV2();

                foreach (var Pedido in Pedidos)
                {
                    if (Pedido.status == "3")
                    {
                        PedidosAdd.AddPedidoV2(Pedido);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Versátil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //Ajusta pelo campo Referencia Ecommerce 
        public static void AjustarCodigosProdutosV2()
        {
            try
            {
                var ProdutosEnvio = BuscaProdutosSistema();

                foreach (var Prod in ProdutosEnvio)
                {
                    try
                    {
                        //var Json = TrayWebServices.BuscaListaProdutos(1, Prod.id);
                        var Json = RKConectionWebService.GetProdutoV2(Prod.referenciaecommerce);

                        if (!Json.Equals("Erro"))
                        {
                            var ProdutoSite = JsonConvert.DeserializeObject<RKRetornoProduto>(Json);

                            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                            string Query = "update produtoseservicos SET codigoecommerce = @codigoecommerce, enviadoecommerce = @enviadoecommerce where (codigo = @codigo) and (codigoecommerce is null or codigoecommerce = '')";
                            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                            Comando.Parameters.AddWithValue("@codigoecommerce", ProdutoSite.product.id);
                            Comando.Parameters.AddWithValue("@enviadoecommerce", "1");
                            Comando.Parameters.AddWithValue("@codigo", Prod.id);
                            DBConnectionMySql.AbreConexaoBD(DBMySql);
                            Comando.ExecuteNonQuery();
                            DBConnectionMySql.FechaConexaoBD(DBMySql);
                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Ajustar códigos - Erro", ex.Message, "Rocky");
                    }
                }

                //Busca os Produtos Cadastrados no Sistema - Habilitados para o Ecommerce 
                List<RKProdutos> BuscaProdutosSistema()
                {
                    try
                    {
                        var ListaProdutos = new List<RKProdutos>();

                        MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                        string Query = "select p.codigo, p.referenciaecommerce from produtoseservicos p where p.tipo = 'Produto' and p.situacaoecommerce = '1' and p.site = '1' and p.enviadoecommerce = '0'";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var Produto = new RKProdutos();
                            Produto.id = Reader["codigo"].ToString();
                            Produto.referenciaecommerce = Reader["referenciaecommerce"].ToString();

                            ListaProdutos.Add(Produto);
                        }
                        Reader.Close();

                        return ListaProdutos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Erro na consulta dos produtos cadastrados no sistema", ex.Message, "Site");
                        return new List<RKProdutos>();
                    }
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Ajusta Código dos Produtos", ex.Message, "Tray");
            }
        }

        //Ajusta pelo campo SKU
        public static void AjustarCodigosVaricoesV2()
        {
            try
            {
                var Variacoes = RKConectionWebService.ReceberProdutos_Variacoes();

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                DBConnectionMySql.AbreConexaoBD(DBMySql);

                foreach (var Vari in Variacoes)
                {
                    try
                    {
                        string Query = "update produtoseservicos SET codigovariacao = @codigovariacao, enviadoecommerce = @enviadoecommerce where (sku = @sku and tipo = 'Produto' and variacao = '1') and (sku is not null and sku <> '')";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        Comando.Parameters.AddWithValue("@codigovariacao", Vari.id);
                        Comando.Parameters.AddWithValue("@enviadoecommerce", "1");
                        Comando.Parameters.AddWithValue("@sku", Vari.sku);
                        Comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "SKU: " + Vari.sku + " - Erro", ex.Message, "Site");
                    }
                }

                DBConnectionMySql.FechaConexaoBD(DBMySql);

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Ajusta Código da Variação nos Produtos", ex.Message, "Site");
            }
        }

        //Calcula o Estoque Pelo SKU
        public static decimal CarregaEstoqueV2(string _Sku)
        {
            decimal Estoque = 0;
            var MySqlLocal = new MySqlConnection(DBConnectionMySql.strConnection);
            var Comando = new MySqlCommand("SELECT SUM(e.estoquedisponivel) as saldo FROM tmpestoqueecommerce e WHERE (e.sku = '" + _Sku + "' AND (e.sku IS NOT NULL AND e.sku <> ''));", MySqlLocal);

            DBConnectionMySql.AbreConexaoBD(MySqlLocal);
            var Reader = Comando.ExecuteReader();
            if (Reader.Read())
            {
                Estoque = ConverterDecimal(Reader["saldo"].ToString());
            }

            DBConnectionMySql.FechaConexaoBD(MySqlLocal);
            return Estoque;

            //Converte para Decimal
            decimal ConverterDecimal(string Valor)
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
        }

        //Calcula o estoque TMPESTOQUEECOMMERCE
        public static void CalcularEstoqueEcommerceV2()
        {
            try
            {
                string Cnpj = "";
                var MDB = new List<ConfiguracoesMdb>();

                CarregarEmpresasMdb();//Carrega todas as configurações do MDB

                foreach (var _C in MDB)
                {
                    CarregarEstoque(_C.connString);
                }

                void CarregarEstoque(string _conn)
                {
                    Cnpj = "";
                    var DBMySql = new MySqlConnection(_conn);

                    MySqlCommand Comando = new MySqlCommand("SELECT usuariocnpj FROM configuracoes", DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        if (Reader.Read())
                        {
                            Cnpj = Reader["usuariocnpj"].ToString();
                        }
                    }
                    Reader.Close();

                    Comando = new MySqlCommand("SELECT p.sku, sum(p.estoquedisponivel) as estoquedisponivel FROM produtoseservicos p WHERE p.situacao = 'Ativo' and (p.sku is not null and p.sku <> '') group by p.sku", DBMySql);
                    Comando.CommandTimeout = 3000000;
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    Reader = Comando.ExecuteReader();

                    while (Reader.Read())
                    {
                        InserirEstoque(Reader);
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                }

                void InserirEstoque(MySqlDataReader _Reader)
                {
                    var MySqlLocal = new MySqlConnection(DBConnectionMySql.strConnection);
                    var Comando = new MySqlCommand("replace into tmpestoqueecommerce (cnpj, sku, estoquedisponivel) values (@cnpj, @sku, @estoquedisponivel) ", MySqlLocal);

                    Comando.Parameters.AddWithValue("@cnpj", Cnpj);
                    //Comando.Parameters.AddWithValue("@codigoproduto", _Reader["codigo"].ToString());
                    Comando.Parameters.AddWithValue("@sku", _Reader["sku"].ToString());
                    Comando.Parameters.AddWithValue("@estoquedisponivel", Convert.ToDecimal(_Reader["estoquedisponivel"].ToString()));

                    DBConnectionMySql.AbreConexaoBD(MySqlLocal);
                    Comando.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(MySqlLocal);
                }

                void CarregarEmpresasMdb()
                {
                    var Connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\ERP Versátil\Configurações.mdb");

                    var Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandText = "select * from tblEmpresas";

                    var Reader = Command.ExecuteReader();

                    while (Reader.Read())
                    {
                        var Co = new ConfiguracoesMdb();

                        string Ip = Reader["Servidor"].ToString();
                        string User = Reader["Usuário"].ToString();
                        string Password = Reader["Senha"].ToString();
                        string Database = Reader["Banco de Dados"].ToString();

                        Co.codigo = Reader["Código"].ToString();
                        Co.connString = "server = " + Ip + "; user = " + User + "; database = " + Database + "; password = " + Password + ";";

                        MDB.Add(Co);
                    }

                    Reader.Close();
                    Connection.Close();
                }

            }
            catch (MySqlException ex)
            {
                DAOLogDB.SalvarLogs("", "ERRO - Calcular estoque SKU", ex.Message, "Site");
            }
        }




    }
}
