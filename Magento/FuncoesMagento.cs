using IntegracaoRockye.Magento.Models;

using IntegracaoRockye.MagentoProducaoServices;
//using IntegracaoRockye.MagentoHLGServices;

using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Funcoes;
using IntegracaoRockye.Versatil.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Magento
{
    public class FuncoesMagento
    {

        private MagentoWebService WSMagento;

        public FuncoesMagento()
        {
            WSMagento = new MagentoWebService();
        }

        public void CloseSession()
        {
            WSMagento.CloseSession();
        }


        public void AtualizarEstoque()
        {
            try
            {
                var estoque = GetProdutos();

                foreach (var produto in estoque)
                {
                    try
                    {
                        var Retono = WSMagento.UpdateStock(produto.CodigoEcommerce, produto.EstoqueDisponivel);

                        if (Retono != 1)
                        {
                            DAOLogDB.SalvarLogs("", "Web Service - Erro no Update do estoque do produto código: " + produto.Codigo, "O WS retornou o código: " + Retono.ToString(), "Magento");
                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Web Service - Erro no Update do do estoque do produto código: " + produto.Codigo, ex.Message, "Magento");
                    }
                }

                List<MagentoProdutos> GetProdutos()
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        var Produtos = new List<MagentoProdutos>();

                        // string Query = "select p.codigo, p.codigoecommerce, p.referenciaecommerce, p.referencia, p.estoquedisponivel from produtoseservicos p where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site2 = '1' and p.enviadoecommerce = '1' and (p.codigoecommerce <> '' and p.codigoecommerce is not null)";
                        string Query = "select e.produto as codigo, sum(e.estoquedisponivel) as estoquedisponivel, p.codigoecommerce from tmpestoqueempresasapp e left join produtoseservicos p on p.codigo = e.produto where (e.empresa = '300004898' or e.empresa = '1') and p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site2 = '1' and p.enviadoecommerce = '1' and(p.codigoecommerce <> '' and p.codigoecommerce is not null) group by e.produto";
                        
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var Produto = new MagentoProdutos();

                            Produto.Codigo = Reader["codigo"].ToString();
                            Produto.CodigoEcommerce = Reader["codigoecommerce"].ToString();
                            Produto.EstoqueDisponivel = ConverterDecimal(Reader["estoquedisponivel"].ToString());

                            Produtos.Add(Produto);
                        }

                        Reader.Close();

                        return Produtos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Magento");
                        return new List<MagentoProdutos>();
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
                DAOLogDB.SalvarLogs("", "Função - AtualizarEstoque()", ex.Message, "Magento");
            }
        }

        public void AtualizarPraticado()
        {
            try
            {
                var estoque = GetProdutos();

                foreach (var produto in estoque)
                {
                    try
                    {
                        if (produto.PraticadoEcommerce > 0)
                        {
                            var praticado = produto.PraticadoEcommerce.ToString().Replace(",", ".");
                            var Retono = WSMagento.UpdatePraticadoProduct(produto.CodigoEcommerce, praticado);

                            if (Retono == false)
                            {
                                DAOLogDB.SalvarLogs("", "Web Service - Erro no Update do praticado do produto código: " + produto.Codigo, "O WS não conseguiu realizar o Update ", "Magento");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Web Service - Erro no Update do praticado do produto código: " + produto.Codigo, ex.Message, "Magento");
                    }
                }

                List<MagentoProdutos> GetProdutos()
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        var Produtos = new List<MagentoProdutos>();

                        string Query = "select p.codigo, p.codigoecommerce, p.praticadoecommerce from produtoseservicos p where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site2 = '1' and p.enviadoecommerce = '1' and (p.codigoecommerce <> '' and p.codigoecommerce is not null) and p.praticadoecommerce > 0";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var Produto = new MagentoProdutos();

                            Produto.Codigo = Reader["codigo"].ToString();
                            Produto.CodigoEcommerce = Reader["codigoecommerce"].ToString();
                            Produto.PraticadoEcommerce = ConverterDecimal(Reader["praticadoecommerce"].ToString());

                            if (Produto.PraticadoEcommerce > 0)
                            {
                                Produtos.Add(Produto);
                            }
                        }

                        Reader.Close();

                        return Produtos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Magento");
                        return new List<MagentoProdutos>();
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
                DAOLogDB.SalvarLogs("", "Função - AtualizarPraticado()", ex.Message, "Magento");
            }
        }

        public void CopiarPedidos()
        {
            try
            {
                var pedidosList = WSMagento.getOrderList();

                foreach (var p in pedidosList)
                {
                    var pedido = WSMagento.getOrderData(p.increment_id);
                    InsertPedido(pedido);
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Copiar Pedidos - Erro na consulta de Pedidos", ex.Message, "Magento");
            }
        }

        private void InsertPedido(salesOrderEntity order)
        {
            try
            {
                string CodigoCliente = "";

                //Verifica o Cliente
                var customer = WSMagento.getConsumerData(Convert.ToInt32(order.customer_id));
                var adress = WSMagento.getAdressListCustumer(customer.customer_id).First();

                var EndEntrega = order.shipping_address.street.Split('\n');
                var Endereco = adress.street.Split('\n');

                CodigoCliente = DAOClientes.ConsultaColaborador(customer.taxvat);
                var CodigoCidade = DAOCidades.BuscaCidadeViaCep(order.shipping_address.postcode);

                VerColaboradores Cliente = new VerColaboradores();
                Cliente.Codigo = CodigoCliente;
                Cliente.Situacao = "Ativo";
                Cliente.Tipo = "Cliente";
                Cliente.Nomerazaosocial = customer.firstname + " " + customer.lastname;
                Cliente.Nomefantasia = "";
                Cliente.Cpfcnpj = customer.taxvat;
                Cliente.Inscricaoestadual = "";

                Cliente.Endereco = Endereco[0];
                Cliente.Numero = Endereco[1];
                Cliente.Complemento = Endereco[2];
                Cliente.Bairro = Endereco[3];

                Cliente.Codigocidade = CodigoCidade;
                Cliente.Telefone = adress.telephone;
                //Cliente.Celular = "";
                Cliente.Email = customer.email;
                Cliente.Cep = order.shipping_address.postcode;
                //Cliente.Observacao = "";
                //Cliente.Datanascimento = 

                if (CodigoCliente.Equals(""))
                {
                    CodigoCliente = DAOClientes.CadastrarCliente(Cliente);
                }
                else
                {
                    DAOClientes.UpdateCliente(Cliente);
                }

                //Verifica se o Pedido ja Existe
                MySqlConnection DBMySqlItens = new MySqlConnection(DBConnectionMySql.strConnection);
                string CodigoPedido = "";
                string Query = "SELECT a.codigo FROM atendimentos a WHERE a.pedido ='#" + order.increment_id + "'";
                MySqlCommand Comando0 = new MySqlCommand(Query, DBMySqlItens);
                DBConnectionMySql.AbreConexaoBD(DBMySqlItens);
                MySqlDataReader Reader = Comando0.ExecuteReader();

                if (Reader.Read())
                {
                    CodigoPedido = Reader["codigo"].ToString();
                    Reader.Close();
                }
                DBConnectionMySql.FechaConexaoBD(DBMySqlItens);


                if (CodigoPedido.Equals(""))
                {
                    VerPedidos DadosPedido = new VerPedidos();

                    DadosPedido.Codigocolaborador = CodigoCliente;
                    DadosPedido.Documento = "Pedido";
                    DadosPedido.Observacoes = order.shipping_description;
                    DadosPedido.Subtotal = ConverterDecimal(order.base_subtotal);
                    DadosPedido.Valortotal = ConverterDecimal(order.grand_total);
                    DadosPedido.Status = "Não Faturado";
                    DadosPedido.Valorfrete = ConverterDecimal(order.shipping_amount);
                    DadosPedido.Valordesconto = ConverterDecimal(order.discount_amount);

                    if (DadosPedido.Valordesconto < 0)
                    {
                        DadosPedido.Especificardescontonoatendimento = true;
                        DadosPedido.Valordesconto = DadosPedido.Valordesconto * -1;
                    }

                    DadosPedido.Totalnf = ConverterDecimal(order.grand_total);
                    DadosPedido.Pedido = "#" + order.increment_id;
                    DadosPedido.Codigovendedor = "10000118"; //DadosConfiguracao.Config.CodigoVendedorPadrao;

                    DadosPedido.Substatus = DadosConfiguracao.Config.CodigoSubStatusSite;
                    DadosPedido.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;
                    //DadosPedido.Itens = "0"

                    DadosPedido.Observacoesnota = "ENDEREÇO ENTREGA: \n CIDADE: " + order.shipping_address.city.ToUpper() + " - " + order.shipping_address.region.ToUpper() + "; \n " +
                        "CEP: " + order.shipping_address.postcode + "; \n " +
                        "ENDEREÇO: " + EndEntrega[0].ToUpper() + ", " + EndEntrega[1] + ";\n " +
                        "COMPLEMENTO: " + EndEntrega[2].ToUpper() + "; \n " +
                        "BAIRRO: " + EndEntrega[3].ToUpper() + ";\n " +
                        "Telefone: " + order.shipping_address.telephone + "; \n ";


                    DadosPedido.Itens = new List<VerItens>();

                    foreach (var Itens in order.items)
                    {
                        VerItens DadosItens = new VerItens();

                        DadosItens.Codigoproduto = BuscaCodigoProduto(Itens.product_id);
                        DadosItens.Produto = Itens.name;
                        DadosItens.Quantidade = ConverterDecimal(Itens.qty_ordered);
                        DadosItens.ValorDesconto = ConverterDecimal(Itens.discount_amount);

                        DadosItens.Tabela = ConverterDecimal(Itens.original_price);
                        DadosItens.Total = DadosItens.Quantidade * DadosItens.Tabela;

                        if (DadosItens.ValorDesconto < 0)
                        {
                            DadosItens.ValorDesconto = DadosItens.ValorDesconto * -1;
                        }

                        DadosItens.Totalcomdesconto = ConverterDecimal(Itens.row_total) - DadosItens.ValorDesconto;
                        DadosItens.Tabelacomdesconto = DadosItens.Totalcomdesconto / DadosItens.Quantidade;

                        if (!DadosItens.Codigoproduto.Equals(""))
                        {
                            DadosPedido.Itens.Add(DadosItens);
                        }
                    }

                    DadosPedido.Contas = new List<VerContas>();
                    DAOPedidos.CadastrarPedido(DadosPedido);
                }

                string BuscaCodigoProduto(string codigoecommerce)
                {
                    var DBMySqlPro = new MySqlConnection(DBConnectionMySql.strConnection);
                    string CodigoProduto = "";
                    string QueryP = "SELECT p.codigo FROM produtoseservicos p WHERE p.codigoecommerce ='" + codigoecommerce + "'";
                    MySqlCommand ComandoP = new MySqlCommand(QueryP, DBMySqlPro);
                    DBConnectionMySql.AbreConexaoBD(DBMySqlPro);
                    MySqlDataReader ReaderP = ComandoP.ExecuteReader();

                    if (ReaderP.HasRows)
                    {
                        if (ReaderP.Read())
                        {
                            CodigoProduto = ReaderP["codigo"].ToString();
                            ReaderP.Close();
                        }
                    }
                    DBConnectionMySql.FechaConexaoBD(DBMySqlPro);

                    return CodigoProduto;
                }

                decimal ConverterDecimal(string Valor)
                {
                    try
                    {
                        Valor = Valor.Replace(".", ",");
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
                DAOLogDB.SalvarLogs(order.increment_id, "Pedidos - Erro no cadastro de pedido", ex.Message, "Magento");
            }
        }

        public void EnviarProdutos()
        {
            try
            {
                var produtos = GetProdutos();

                foreach (var produto in produtos)
                {
                    try
                    {
                        var CodigoEcommerce = WSMagento.CreateProduct(produto);
                        AlteraStatusEnviado(produto.Codigo, CodigoEcommerce);
                    }
                    catch(Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Envio de Produtos - Erro no envio dos Produtos - Produto Código:" + produto.Codigo, ex.Message, "Magento");
                    }
                }

                void AlteraStatusEnviado(string _CodProd, int _CodEcommerce)
                {

                    string Sql = "update produtoseservicos set enviadoecommerce = @enviadoecommerce, codigoecommerce = @codigoecommerce where codigo = @codigo";
                    var DBMySqlI = new MySqlConnection(DBConnectionMySql.strConnection);
                    var cmdI = new MySqlCommand(Sql, DBMySqlI);

                    cmdI.Parameters.AddWithValue("@codigo", _CodProd);
                    cmdI.Parameters.AddWithValue("@enviadoecommerce", "1");
                    cmdI.Parameters.AddWithValue("@codigoecommerce", _CodEcommerce);

                    DBConnectionMySql.AbreConexaoBD(DBMySqlI);
                    cmdI.CommandTimeout = 0;
                    cmdI.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(DBMySqlI);
                }

                List<MagentoProdutos> GetProdutos()
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        var Produtos = new List<MagentoProdutos>();

                        string Query = "select p.codigo, p.codigoecommerce, p.nomecomercial, p.observacoes2 AS descricao, p.pesobruto, p.largura, p.altura, p.comprimento, co.codigoadicional AS cor, t.codigoadicional AS tamanho, cp.complemento2 AS material, g.codigoadicional as categoria, p.estoquedisponivel, p.sku, p.praticadoecommerce, p.caminhoimagemecommerce as imagem from produtoseservicos p " +
                            "left join complementodecadastros cp on cp.codigo = p.familia " +
                            "left join cores co on co.codigo = p.codigocor " +
                            "LEFT JOIN tamanhos t ON t.codigo = p.codigotamanho " +
                            "LEFT JOIN grupos g ON g.codigo = p.codigogrupo " +
                            "where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site2 = '1' and p.enviadoecommerce = '0' and (p.codigoecommerce = '' or p.codigoecommerce is null) AND p.praticadoecommerce > 0";

                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var Produto = new MagentoProdutos();

                            Produto.Codigo = Reader["codigo"].ToString();
                            Produto.CodigoEcommerce = Reader["codigoecommerce"].ToString();

                            Produto.Sku = Reader["sku"].ToString();

                            Produto.Nome = Reader["nomecomercial"].ToString();
                            Produto.Descricao = Reader["descricao"].ToString();
                            Produto.DescricaoCurta = Reader["descricao"].ToString();

                            Produto.Peso = Reader["pesobruto"].ToString();
                            Produto.Comprimento = Reader["comprimento"].ToString();
                            Produto.Largura = Reader["largura"].ToString();
                            Produto.Altura = Reader["altura"].ToString();
                            Produto.Cor = Reader["cor"].ToString();
                            Produto.Material = Reader["material"].ToString();
                            Produto.Tamanho = Reader["tamanho"].ToString();

                            Produto.Categoria = Reader["categoria"].ToString();
                            Produto.Imagem = Reader["imagem"].ToString();

                            Produto.PraticadoEcommerce = ConverterDecimal(Reader["praticadoecommerce"].ToString());
                            Produto.EstoqueDisponivel = ConverterDecimal(Reader["estoquedisponivel"].ToString());

                            Produtos.Add(Produto);
                        }

                        Reader.Close();

                        return Produtos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Magento");
                        return new List<MagentoProdutos>();
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
                DAOLogDB.SalvarLogs("", "Envio de Produtos - Erro na função EnviarProdutos()", ex.Message, "Magento");
            }
        }


        //Não é utilizado ainda
        public void UpdateProdutos()
        {
            try
            {
                var produtos = GetProdutos();

                foreach (var produto in produtos)
                {
                    try
                    {
                        var update = WSMagento.UpdateProduct(produto, produto.CodigoEcommerce);

                        if (update.Equals(false))
                        {
                            DAOLogDB.SalvarLogs("", "Update de Produtos - Erro no update do Produto - Produto Código:" + produto.Codigo, "", "Magento");
                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Update de Produtos - Erro no envio dos Produtos - Produto Código:" + produto.Codigo, ex.Message, "Magento");
                    }
                }

                List<MagentoProdutos> GetProdutos()
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        var Produtos = new List<MagentoProdutos>();

                        string Query = "select p.codigo, p.codigoecommerce, p.nomecomercial, p.observacoes2 AS descricao, p.pesobruto, p.largura, p.altura, p.comprimento, co.codigoadicional AS cor, t.codigoadicional AS tamanho, cp.complemento2 AS material, g.codigoadicional as categoria, p.estoquedisponivel, p.sku, p.praticadoecommerce, p.caminhoimagemecommerce as imagem from produtoseservicos p " +
                            "left join complementodecadastros cp on cp.codigo = p.familia " +
                            "left join cores co on co.codigo = p.codigocor " +
                            "LEFT JOIN tamanhos t ON t.codigo = p.codigotamanho " +
                            "LEFT JOIN grupos g ON g.codigo = p.codigogrupo " +
                            "where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site2 = '1' and p.enviadoecommerce = '1' and (p.codigoecommerce <> '' and p.codigoecommerce is not null) AND p.praticadoecommerce > 0";

                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var Produto = new MagentoProdutos();

                            Produto.Codigo = Reader["codigo"].ToString();
                            Produto.CodigoEcommerce = Reader["codigoecommerce"].ToString();

                            Produto.Sku = Reader["sku"].ToString();

                            Produto.Nome = Reader["nomecomercial"].ToString();
                            Produto.Descricao = Reader["descricao"].ToString();
                            Produto.DescricaoCurta = Reader["descricao"].ToString();

                            Produto.Peso = Reader["pesobruto"].ToString();
                            Produto.Comprimento = Reader["comprimento"].ToString();
                            Produto.Largura = Reader["largura"].ToString();
                            Produto.Cor = Reader["cor"].ToString();
                            Produto.Material = Reader["material"].ToString();
                            Produto.Tamanho = Reader["tamanho"].ToString();

                            Produto.Categoria = Reader["categoria"].ToString();
                            Produto.Imagem = Reader["imagem"].ToString();

                            Produto.PraticadoEcommerce = ConverterDecimal(Reader["praticadoecommerce"].ToString());
                            Produto.EstoqueDisponivel = ConverterDecimal(Reader["estoquedisponivel"].ToString());

                            Produtos.Add(Produto);
                        }

                        Reader.Close();

                        return Produtos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Magento");
                        return new List<MagentoProdutos>();
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
                DAOLogDB.SalvarLogs("", "Update de Produtos - Erro na função UpdateProdutos()", ex.Message, "Magento");
            }
        }

    }
}
