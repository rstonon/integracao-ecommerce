using IntegracaoRockye.Tray.Models;
using IntegracaoRockye.Tray.Models.Listar;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Funcoes;
using IntegracaoRockye.Versatil.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegracaoRockye.Tray
{
    public static class TrayFuncoes
    {
        public static void Auth()
        {
            try
            {
                var Json = TrayWebServices.Auth();

                if (Json.Equals("Erro"))
                { return; }

                var Auth = JsonConvert.DeserializeObject<TrayAuth>(Json);
                DadosConfiguracao.Config.AuthTray = Auth;

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Auth - Erro na autenticação", ex.Message, "Tray");
            }
        }

        public static void AuthRefresh()
        {
            try
            {
                var Json = TrayWebServices.AuthRefresh();

                if (Json.Equals("Erro"))
                { return; }

                var Auth = JsonConvert.DeserializeObject<TrayAuth>(Json);
                DadosConfiguracao.Config.AuthTray = Auth;

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Auth - Erro na autenticação", ex.Message, "Tray");
            }
        }


        //Busca os Produtos do Ecommerce e Ajusta os Codigos do Ecommerce no Sistema
        public static void AjustaCodigoEcommercedoPeodutoSistema()
        {
            try
            {
                var ProdutosEnvio = BuscaProdutosSistema();

                foreach (var Prod in ProdutosEnvio)
                {
                    try
                    {
                        var Json = TrayWebServices.BuscaListaProdutos(1, Prod.id);

                        if (!Json.Equals("Erro"))
                        {
                            var ProdutoSite = JsonConvert.DeserializeObject<TrayProductsListar>(Json).Products.First().Product;

                            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                            string Query = "update produtoseservicos SET codigoecommerce = @codigoecommerce where codigo = @codigo";
                            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                            Comando.Parameters.AddWithValue("@codigoecommerce", ProdutoSite.id);
                            Comando.Parameters.AddWithValue("@codigo", Prod.id);
                            DBConnectionMySql.AbreConexaoBD(DBMySql);
                            Comando.ExecuteNonQuery();
                            DBConnectionMySql.FechaConexaoBD(DBMySql);
                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Envio de Produtos - Erro", ex.Message, "Tray");
                    }
                }

                //Busca os Produtos Cadastrados no Sistema - Habilitados para o Ecommerce 
                List<TrayProduct> BuscaProdutosSistema()
                {
                    try
                    {
                        var ListaProdutos = new List<TrayProduct>();

                        MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                        string Query = "select p.codigo from produtoseservicos p where p.tipo = 'Produto' and p.situacaoecommerce = '1' and p.site = '1'";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var Produto = new TrayProduct();
                            Produto.id = Reader["codigo"].ToString();
                            ListaProdutos.Add(Produto);
                        }
                        Reader.Close();

                        return ListaProdutos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Erro na consulta dos produtos cadastrados no sistema", ex.Message, "Tray");
                        return new List<TrayProduct>();
                    }
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Ajusta Código dos Produtos", ex.Message, "Tray");
            }
        }

        //Busca as Variações no Ecommerce e Ajusta os Codigos da Variação no Sistema
        public static void AjustaCodigoVariacaodoProdutoSistema()
        {
            try
            {
                var ProdutosEnvio = BuscaProdutosSistema();

                foreach (var Prod in ProdutosEnvio)
                {
                    try
                    {
                        var Json = TrayWebServices.BuscaListaVariacoes(1, Prod.id);

                        if (!Json.Equals("Erro"))
                        {
                            var ProdutoSite = JsonConvert.DeserializeObject<TrayVariantsListar>(Json).Variants.First().Variant;

                            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                            string Query = "update produtoseservicos SET codigovariacao = @codigovariacao where codigo = @codigo";
                            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                            Comando.Parameters.AddWithValue("@codigovariacao", ProdutoSite.id);
                            Comando.Parameters.AddWithValue("@codigo", Prod.id);
                            DBConnectionMySql.AbreConexaoBD(DBMySql);
                            Comando.ExecuteNonQuery();
                            DBConnectionMySql.FechaConexaoBD(DBMySql);

                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Ajusta Código da Variação nos Produtos - Erro", ex.Message, "Tray");
                    }
                }

                //Busca os Produtos Cadastrados no Sistema - Habilitados para o Ecommerce 
                List<TrayProduct> BuscaProdutosSistema()
                {
                    try
                    {
                        var ListaProdutos = new List<TrayProduct>();

                        MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                        string Query = "select p.codigo from produtoseservicos p where p.tipo = 'Produto' and p.variacao = '1'";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var Produto = new TrayProduct();
                            Produto.id = Reader["codigo"].ToString();
                            ListaProdutos.Add(Produto);
                        }
                        Reader.Close();

                        return ListaProdutos;
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Erro na consulta dos produtos cadastrados no sistema", ex.Message, "Tray");
                        return new List<TrayProduct>();
                    }
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Ajusta Código da Variação nos Produtos", ex.Message, "Tray");
            }
        }






        //Envia os Produtos
        public static void EnviaProdutos()
        {
            try
            {
                var ProdutosEnvio = DAOProdutos.GetProdutosTray();

                foreach (var Produto in ProdutosEnvio)
                {
                    if (Produto.id.Length == 0)
                    {
                        var Json = TrayWebServices.EnviaProdutos(Produto);

                        if (!Json.Equals("Erro"))
                        {
                            var Retorno = JsonConvert.DeserializeObject<TrayRetorno>(Json);

                            if (Retorno.code.Equals("201"))
                            {
                                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                                string Query = "update produtoseservicos SET codigoecommerce = @codigoecommerce where codigo = @codigo";
                                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                                Comando.Parameters.AddWithValue("@codigoecommerce", Retorno.id);
                                Comando.Parameters.AddWithValue("@codigo", Produto.reference);
                                DBConnectionMySql.AbreConexaoBD(DBMySql);
                                Comando.ExecuteNonQuery();
                                DBConnectionMySql.FechaConexaoBD(DBMySql);
                            }
                        }
                    }
                    else
                    {
                        var Json = TrayWebServices.AtualizaProdutos(Produto);

                        if (!Json.Equals("Erro"))
                        {
                            var Retorno = JsonConvert.DeserializeObject<TrayRetorno>(Json);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Erro no Envio de Produtos", ex.Message, "Tray");
            }
        }

        //Envia as Variações dos Produtos
        public static void EnviaVariacoes()
        {
            try
            {
                var Produtos = DAOProdutos.GetProdutosTray();

                foreach (var Produto in Produtos)
                {
                    var VariacoesEnvio = BuscaVariacoes(Produto.ReferenciaSistema, Produto.reference);

                    foreach (var Vari in VariacoesEnvio)
                    {
                        Vari.product_id = Produto.id;

                        if (Vari.id.Length == 0)
                        {
                            var Json = TrayWebServices.EnviaVariacoes(Vari);

                            if (!Json.Equals("Erro"))
                            {
                                var Retorno = JsonConvert.DeserializeObject<TrayRetorno>(Json);

                                if (Retorno.code.Equals("201"))
                                {
                                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                                    string Query = "update produtoseservicos SET codigovariacao = @codigovariacao where codigo = @codigo";
                                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                                    Comando.Parameters.AddWithValue("@codigovariacao", Retorno.id);
                                    Comando.Parameters.AddWithValue("@codigo", Vari.reference);
                                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                                    Comando.ExecuteNonQuery();
                                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                                }
                                else
                                {
                                    DAOLogDB.SalvarLogs("", "Variações - Erro no envio de variações", "", "Tray");
                                }
                            }
                        }
                        else
                        {
                            var Json = TrayWebServices.AtualizaVariacoes(Vari);

                            if (!Json.Equals("Erro"))
                            {
                                var Retorno = JsonConvert.DeserializeObject<TrayRetorno>(Json);
                            }
                            else
                            {
                                DAOLogDB.SalvarLogs("", "Variações - Erro na atualização de variações", "", "Tray");
                            }
                        }
                    }

                }


                //Busca os Dados das Variações
                List<TrayVariant> BuscaVariacoes(string Referencia, string CodigoProduto)
                {
                    var Variacoes = new List<TrayVariant>();
                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        string VarTipo = "";
                        string Var1 = "";
                        string Var2 = "";

                        string Query = "select p.variacao1, p.variacao2, p.opcaovariacao from produtoseservicos p where p.codigo = '" + CodigoProduto + "'";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        if (Reader.Read())
                        {
                            VarTipo = Reader["opcaovariacao"].ToString();
                            Var1 = Reader["variacao1"].ToString();
                            Var2 = Reader["variacao2"].ToString();
                        }
                        Reader.Close();

                        if (VarTipo.Equals("Sem Variação"))
                        {
                            return new List<TrayVariant>();
                        }

                        int i = 1;
                        Query = "select p.codigo, p.situacao, p.referenciaecommerce, p.descricao, p.nomecomercial, p.codigoean, p.familia, p.codigocor, p.codigotamanho, p.praticado, p.quantidadeembalagem, p.pesobruto, p.largura, p.altura, p.comprimento, m.codigoadicional as codigomarca, p.estoquedisponivel, p.codigovariacao, p.codigoecommerce, co.cor, t.tamanho, m.marca, cl.complemento from produtoseservicos p left join marcas m on m.codigo = p.codigomarca LEFT JOIN cores co ON co.codigo = p.codigocor LEFT JOIN tamanhos t ON t.codigo = p.codigotamanho LEFT JOIN complementodecadastros cl ON cl.codigo = p.familia where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.variacao = '1' and p.referenciaecommerce = '" + Referencia + "'";
                        Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var v = new TrayVariant();

                            v.id = Reader["codigovariacao"].ToString();
                            v.product_id = "";
                            v.reference = Reader["codigo"].ToString();
                            v.stock = Convert.ToInt32(Convert.ToDecimal(Reader["estoquedisponivel"].ToString()));
                            v.price = Convert.ToDecimal(Reader["praticado"].ToString());
                            v.weight = Reader["pesobruto"].ToString();
                            v.length = Reader["comprimento"].ToString();
                            v.width = Reader["largura"].ToString();
                            v.height = Reader["altura"].ToString();

                            var Sku1 = new TraySku();
                            var Sku2 = new TraySku();

                            if (!VarTipo.Equals("Sem Variação"))
                            {
                                if (Var1.Equals("Cor"))
                                {
                                    Sku1.type = "Cor";
                                    Sku1.value = Reader["cor"].ToString();
                                }

                                if (Var1.Equals("Tamanho"))
                                {
                                    Sku1.type = "Tamanho";
                                    Sku1.value = Reader["tamanho"].ToString();
                                }

                                if (Var1.Equals("Familía"))
                                {
                                    Sku1.type = "Modelo";
                                    Sku1.value = Reader["complemento"].ToString();
                                }

                                if (Var1.Equals("Marca"))
                                {
                                    Sku1.type = "Marca";
                                    Sku1.value = Reader["marca"].ToString();
                                }

                                if (Var1.Equals("Largura"))
                                {
                                    Sku1.type = "Largura";
                                    Sku1.value = Reader["largura"].ToString();
                                }

                                if (Var1.Equals("Comprimento"))
                                {
                                    Sku1.type = "Comprimento";
                                    Sku1.value = Reader["comprimento"].ToString();
                                }

                                if (Var1.Equals("Altura"))
                                {
                                    Sku1.type = "Altura";
                                    Sku1.value = Reader["Altura"].ToString();
                                }

                            }

                            if (VarTipo.Equals("Variação Dupla"))
                            {
                                if (Var2.Equals("Cor"))
                                {
                                    Sku2.type = "Cor";
                                    Sku2.value = Reader["cor"].ToString();
                                }

                                if (Var2.Equals("Tamanho"))
                                {
                                    Sku2.type = "Tamanho";
                                    Sku2.value = Reader["tamanho"].ToString();
                                }

                                if (Var2.Equals("Familía"))
                                {
                                    Sku2.type = "Modelo";
                                    Sku2.value = Reader["complemento"].ToString();
                                }

                                if (Var2.Equals("Marca"))
                                {
                                    Sku2.type = "Marca";
                                    Sku2.value = Reader["marca"].ToString();
                                }

                                if (Var2.Equals("Largura"))
                                {
                                    Sku2.type = "Largura";
                                    Sku2.value = Reader["largura"].ToString();
                                }

                                if (Var2.Equals("Comprimento"))
                                {
                                    Sku2.type = "Comprimento";
                                    Sku2.value = Reader["comprimento"].ToString();
                                }

                                if (Var2.Equals("Altura"))
                                {
                                    Sku2.type = "Altura";
                                    Sku2.value = Reader["altura"].ToString();
                                }

                            }

                            v.type_1 = Sku1.type;
                            v.value_1 = Sku1.value;

                            v.type_2 = Sku2.type;
                            v.value_2 = Sku2.value;


                            v.order = i.ToString();

                            Variacoes.Add(v);
                            i++;
                        }

                        Reader.Close();
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos Variações - Erro na consulta das variações", ex.Message, "Tray");
                    }
                    finally
                    {
                        DBConnectionMySql.FechaConexaoBD(DBMySql);
                    }

                    return Variacoes;
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Variações - Erro no envio de variações", ex.Message, "Tray");
            }
        }

        //Busca os Pedidos
        public static void BuscaPedidos()
        {
            try
            {
                string DataFiltro = DateTime.Now.Date.AddDays(-5).ToString("yyy-MM-dd");

                var Json = TrayWebServices.BuscarPedidos(1, DataFiltro);

                if (Json.Equals("Erro"))
                { return; }

                var PedidosLista = JsonConvert.DeserializeObject<TrayOrdersListar>(Json);

                float Registros = PedidosLista.paging.total;
                float NumerodePaginas = (Registros / 50);

                for (int i = 0; i < NumerodePaginas; i++)
                {
                    var JsonPedido = TrayWebServices.BuscarPedidos((i + 1), DataFiltro);

                    if (JsonPedido.Equals("Erro"))
                    { return; }

                    var Pedidos = JsonConvert.DeserializeObject<TrayOrdersListar>(JsonPedido);

                    foreach (var lista in Pedidos.Orders.Where(x => x.Order.status.Equals("PAGAMENTO CONFIRMADO") || x.Order.status.Equals("AGUARDANDO ENVIO") || x.Order.status.Equals("A ENVIAR") || x.Order.status.Equals("A ENVIAR YAPAY")))
                    {
                        try
                        {
                            bool Validacao = true;
                            var JsonDadosPedido = TrayWebServices.BuscarDadosdoPedidos(lista.Order.id);

                            if (JsonDadosPedido.Equals("Erro"))
                            { return; }

                            var Pedido = JsonConvert.DeserializeObject<TrayOrders>(JsonDadosPedido);

                            var DadosPedido = new VerPedidos();
                            DadosPedido.Codigocolaborador = ConsultaColaborador(Pedido.Order.Customer);

                            DadosPedido.Documento = "Pedido";
                            DadosPedido.Observacoes = Pedido.Order.store_note + " \n " + Pedido.Order.customer_note;
                            DadosPedido.Subtotal = Convert.ToDecimal(Pedido.Order.partial_total.ToString().Replace(".", ","));
                            DadosPedido.Valortotal = Convert.ToDecimal(Pedido.Order.total.Replace(".", ","));
                            DadosPedido.Totalnf = Convert.ToDecimal(Pedido.Order.total.Replace(".", ","));

                            try
                            {
                                DadosPedido.Valordesconto = Convert.ToDecimal(Pedido.Order.coupon.discount.Replace(".", ","));
                                DadosPedido.Especificardescontonoatendimento = true;
                            }
                            catch
                            {
                                DadosPedido.Valordesconto = 0;
                            }

                            DadosPedido.Valorfrete = Convert.ToDecimal(Pedido.Order.shipment_value.Replace(".", ","));
                            DadosPedido.Pedido = "#" + Pedido.Order.id;
                            DadosPedido.Codigovendedor = DadosConfiguracao.Config.CodigoVendedorPadrao;
                            DadosPedido.Substatus = DadosConfiguracao.Config.CodigoSubStatusSite;
                            DadosPedido.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;
                            DadosPedido.Status = "Não Faturado";

                            //if (Pedido.Order.shipment.Equals("Motoboy- Para cidade de Erechim, RS"))
                           // {
                            //    DadosPedido.Observacoesnota = "ENDEREÇO ENTREGA: RETIRAR NA LOJA";
                            //}
                           // else
                            //{
                                //var Endereco = ConsultaEndereco(Pedido.id_usuario, Pedido.id_endereco);
                                //var Cidade = ConsultaCidade(Endereco.id_cidade);

                                DadosPedido.Observacoesnota = "ENDEREÇO ENTREGA: \n CIDADE: " + Pedido.Order.Customer.city.ToUpper() + " - " + Pedido.Order.Customer.state.ToUpper() + "; \n " +
                                    "CEP: " + Pedido.Order.Customer.zip_code + "; \n " +
                                    "ENDEREÇO: " + Pedido.Order.Customer.address.ToUpper() + " " + Pedido.Order.Customer.number + ";\n " +
                                    "COMPLEMENTO: " + Pedido.Order.Customer.complement.ToUpper() + "; \n " +
                                    "BAIRRO: " + Pedido.Order.Customer.neighborhood.ToUpper() + "; \n ";
                            //}

                            try
                            {
                                DadosPedido.Observacoes += " - " + Pedido.Order.shipment;
                            }
                            catch { }

                            DadosPedido.Itens = new List<VerItens>();
                            foreach (var Itens in Pedido.Order.ProductsSold)
                            {
                                decimal PerDesc = 0;
                                var DadosItens = new VerItens();

                                var CodigoProd = ConsultaCodigoProduto(Itens.ProductsSold.reference.ToString());

                                if (CodigoProd == "")
                                {
                                    Validacao = false;
                                    DAOLogDB.SalvarLogs(Itens.ProductsSold.order_id.ToString(), "Erro Pedido: " + Itens.ProductsSold.order_id, "Cadastro do produto/variação não encontrado id: " + Itens.ProductsSold.id.ToString() + " - Nome site: " + Itens.ProductsSold.original_name, "Tray");
                                }

                                DadosItens.Codigoproduto = CodigoProd;
                                DadosItens.Produto = "";
                                DadosItens.Quantidade = Convert.ToDecimal(Itens.ProductsSold.quantity);

                                try
                                {
                                    PerDesc = DAOPedidos.CalularPercentualDesconto(DadosPedido.Valordesconto, DadosPedido.Subtotal);
                                }
                                catch
                                {
                                    PerDesc = 0;
                                }

                                DadosItens.Tabelacomdesconto = Convert.ToDecimal(Itens.ProductsSold.price) - (Convert.ToDecimal(Itens.ProductsSold.price) * (PerDesc / 100));
                                DadosItens.Totalcomdesconto = (DadosItens.Tabelacomdesconto * DadosItens.Quantidade);

                                DadosPedido.Itens.Add(DadosItens);
                            }

                            DadosPedido.Contas = new List<VerContas>();

                            if (!ConsultaPedido("#" + Pedido.Order.id) && Validacao == true)
                            {
                                DAOPedidos.CadastrarPedido(DadosPedido);
                            }
                        }
                        catch (Exception ex)
                        {
                            DAOLogDB.SalvarLogs(lista.Order.id, "Erro Pedido - " + lista.Order.id, ex.Message, "Tray");
                        }
                    }

                }

                //Consulta se o Peidido ja foi cadastrado
                bool ConsultaPedido(string IdPedido)
                {
                    bool Retorno = false;
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select codigo from atendimentos where pedido = '" + IdPedido + "'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        Retorno = true;
                    }
                    else
                    {
                        Retorno = false;
                    }
                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                    return Retorno;
                }

                //Busca o codigo do Cliente
                string ConsultaColaborador(TrayCustomer Cliente)
                {
                    var Colaborador = new VerColaboradores();
                    string Codigo = "";
                    Colaborador.IdEcommerce = Cliente.id;

                    if (Cliente.cnpj.Length > 0)
                    {
                        Codigo = DAOClientes.ConsultaColaborador(Cliente.cnpj);

                        Colaborador.Nomerazaosocial = Cliente.company_name;
                        Colaborador.Nomefantasia = Cliente.name;
                        Colaborador.Cpfcnpj = Cliente.cnpj;
                        Colaborador.Inscricaoestadual = Cliente.state_inscription;
                    }
                    else
                    {
                        Codigo = DAOClientes.ConsultaColaborador(Cliente.cpf);

                        Colaborador.Nomerazaosocial = Cliente.name;
                        Colaborador.Nomefantasia = "";
                        Colaborador.Cpfcnpj = Cliente.cpf;
                        Colaborador.Inscricaoestadual = "";
                    }

                    Colaborador.Email = Cliente.email;
                    Colaborador.Observacao = "";


                    Colaborador.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;


                    Colaborador.Endereco = Cliente.address;
                    Colaborador.Numero = Cliente.number;
                    Colaborador.Complemento = Cliente.complement;
                    Colaborador.Bairro = Cliente.neighborhood;
                    Colaborador.Datanascimento = Cliente.birth_date;

                    try
                    {
                        if (Cliente.phone.Length > 5)
                        {
                            Colaborador.Telefone = Cliente.phone;
                            Colaborador.Celular = Cliente.cellphone;
                        }
                        else
                        {
                            Colaborador.Telefone = Cliente.cellphone;
                            Colaborador.Celular = "";
                        }
                    }
                    catch { }

                    Colaborador.Cep = Cliente.zip_code;
                    Colaborador.Codigocidade = DAOCidades.BuscaCidadeViaCep(Cliente.zip_code);

                    if (!string.IsNullOrEmpty(Codigo))
                    {
                        Colaborador.Codigo = Codigo;
                        DAOClientes.UpdateCliente(Colaborador);
                    }
                    else
                    {
                        Colaborador.Codigo = DAOClientes.CadastrarCliente(Colaborador);
                    }

                    return Colaborador.Codigo;
                }

                string ConsultaCodigoProduto(string CodigoReferencia)
                {
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Codigo = "";
                    string Query = "select * from produtoseservicos where (codigo = '" + CodigoReferencia + "')";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.Read())
                    {
                        Codigo = Reader["codigo"].ToString();
                        Reader.Close();
                    }
                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return Codigo;
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Erro nos Pedidos", ex.Message, "Tray");
            }
        }

    }
}
