using IntegracaoRockye.Macro.Models;
using IntegracaoRockye.Macro.Models.Listar;
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

namespace IntegracaoRockye.Macro
{
    public static class MacroFuncoes
    {
        //Busca os Usuarios no Ecommerce e Cadastra no ERP
        public static void BuscarUsuarios()
        {
            try
            {
                var Json = MacroWebService.getUsuarios();

                if (Json.Equals("Erro"))
                { return; }

                var Clientes = JsonConvert.DeserializeObject<MacroListarUsuarios>(Json);

                foreach (var Cli in Clientes.dados)
                {

                    try
                    {
                        var Colaborador = new VerColaboradores();
                        string Codigo = "";
                        Colaborador.IdEcommerce = Cli.id;

                        if (Cli.razao.Length > 0)
                        {
                            Colaborador.Nomerazaosocial = Cli.razao;
                            Colaborador.Nomefantasia = Cli.fantasia;
                            Colaborador.Cpfcnpj = Cli.cnpj;
                            Colaborador.Inscricaoestadual = "";
                        }
                        else
                        {
                            Codigo = DAOClientes.ConsultaColaborador(Cli.cpf);

                            Colaborador.Nomerazaosocial = Cli.nome;
                            Colaborador.Nomefantasia = Cli.apelido;
                            Colaborador.Cpfcnpj = Cli.cpf;
                            Colaborador.Inscricaoestadual = "";
                        }

                        Colaborador.Email = Cli.email;
                        Colaborador.Observacao = Cli.observacao;

                        // Colaborador.CodigoSistema = Cliente.Codigocolaborador;

                        Colaborador.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;

                        var End = Cli.enderecos.Where(x => x.principal.Equals(1)).First();

                        Colaborador.Endereco = End.endereco;
                        Colaborador.Numero = End.numero;
                        Colaborador.Complemento = End.complemento;
                        Colaborador.Bairro = End.bairro; ;
                        Colaborador.Telefone = End.telefone;
                        Colaborador.Celular = End.celular;
                        Colaborador.Cep = End.cep;


                        Colaborador.Codigocidade = DAOCidades.BuscaCidadeAPP(End.id_cidade).CodigoCidade;

                        if (!string.IsNullOrEmpty(Codigo))
                        {
                            Colaborador.Codigo = Codigo;
                            DAOClientes.UpdateCliente(Colaborador);
                        }
                        else
                        {
                            DAOClientes.CadastrarCliente(Colaborador);
                        }
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Usuários - Erro no cadastro do Usuário " + Cli.nome, ex.Message, "Macro");
                    }
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Usuários - Erro no cadastro de Usuários", ex.Message, "Macro");
            }
        }

        //Envia os Produtos do sistema para o ECommerce
        public static void EnviarProdutos()
        {
            try
            {
                var Produtos = DAOProdutos.GetProdutosMacro();

                foreach (var Prod in Produtos)
                {
                    Prod.variacoes = BuscaVariacoes(Prod.referencia);
                    // Prod.grupos = BuscaGrupos(Prod.CodigoSistema);
                    Prod.especificacoes = BuscaEspecificacoes(Prod.CodigoSistema);
                }

                if (Produtos.Count > 0)
                {
                    var Ret = MacroWebService.postProdutos(Produtos);

                    if (Ret.Equals("ok"))
                    {
                        foreach (var P in Produtos)
                        {
                            string Sql = "update produtoseservicos set enviadoecommerce = @enviadoecommerce where codigo = @codigo";
                            var DBMySqlI = new MySqlConnection(DBConnectionMySql.strConnection);
                            var cmdI = new MySqlCommand(Sql, DBMySqlI);

                            cmdI.Parameters.AddWithValue("@codigo", P.id);
                            cmdI.Parameters.AddWithValue("@enviadoecommerce", "1");

                            DBConnectionMySql.AbreConexaoBD(DBMySqlI);
                            cmdI.CommandTimeout = 0;
                            cmdI.ExecuteNonQuery();
                            DBConnectionMySql.FechaConexaoBD(DBMySqlI);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Produtos - Erro no envio de produtos", ex.Message, "Macro");
            }

            //Busca os Dados das Variações
            List<MacroProdutosVariacoes> BuscaVariacoes(string Referencia)
            {
                var Variacoes = new List<MacroProdutosVariacoes>();

                var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                try
                {
                    string Query = "select p.codigo, p.situacao, p.referenciaecommerce, p.descricao, p.nomecomercial, p.codigoean, p.familia, p.codigocor, p.codigotamanho, p.praticado, p.quantidadeembalagem, p.pesobruto, p.largura, p.altura, p.comprimento, m.codigo as codigomarca, p.estoquedisponivel, p.codigocor2 from produtoseservicos p inner join marcas m on m.codigo = p.codigomarca where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.variacao = '1' and p.referenciaecommerce = '" + Referencia + "'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    while (Reader.Read())
                    {
                        var v = new MacroProdutosVariacoes();

                        v.id_produto_variacao = Reader["codigo"].ToString();
                        v.referencia = Reader["referenciaecommerce"].ToString();
                        v.estoque = Reader["estoquedisponivel"].ToString();

                        v.id_variacao_1 = Reader["familia"].ToString();
                        v.id_variacao_2 = Reader["codigocor"].ToString();
                        v.id_variacao_3 = Reader["codigotamanho"].ToString();
                        v.id_variacao_4 = Reader["codigocor2"].ToString();
                        v.id_variacao_5 = Reader["codigomarca"].ToString();

                        var situacao = Reader["situacao"].ToString();
                        if (situacao.Equals("Ativo"))
                        { v.ativo = 1; }
                        else
                        { v.ativo = 0; }

                        v.ordem = "1";

                        try
                        {
                            v.peso = (Convert.ToDecimal(Reader["pesobruto"].ToString()) * 1000).ToString();
                        }
                        catch
                        {
                            v.peso = "0";
                        }

                        v.ean = Reader["codigoean"].ToString();

                        v.precos = new List<MacroPrecos>();
                        var p = new MacroPrecos();
                        p.id_lista = "1";
                        p.preco = Convert.ToInt32((Convert.ToDecimal(Reader["praticado"].ToString()) * 100));
                        v.precos.Add(p);

                        Variacoes.Add(v);
                    }

                    Reader.Close();
                }
                catch (Exception ex)
                {
                    DAOLogDB.SalvarLogs("", "Produtos Variações - Erro na consulta das variações", ex.Message, "Macro");
                }
                finally
                {
                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                }

                return Variacoes;
            }

            //Busca os Dados das Especificações
            List<MacroEspecificacoes> BuscaEspecificacoes(string CodigoProduto)
            {
                var Especificacoes = new List<MacroEspecificacoes>();
                var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                try
                {
                    string Query = "select * from especificacoesprodutos e where e.codigoproduto = '" + CodigoProduto + "'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    while (Reader.Read())
                    {
                        var e = new MacroEspecificacoes();

                        e.especificacao = Reader["tipo"].ToString();
                        e.valor = Reader["valor"].ToString();

                        Especificacoes.Add(e);
                    }

                    Reader.Close();
                }
                catch (Exception ex)
                {
                    DAOLogDB.SalvarLogs("", "Produtos Especificações - Erro na consulta das Especificações", ex.Message, "Macro");
                }
                finally
                {
                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                }

                return Especificacoes;
            }
        }

        //Envia os Grupos do sistema para o ECommerce
        public static void EnviarGrupos()
        {
            try
            {
                MacroWebService.postGrupos(BuscaGrupos());

                List<MacroGrupos> BuscaGrupos()
                {
                    var Grupos = new List<MacroGrupos>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    try
                    {
                        string Query = "SELECT g.codigo, g.grupo, g.grupomestre FROM grupos g WHERE g.ecommerce = '1'";
                        MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                        DBConnectionMySql.AbreConexaoBD(DBMySql);
                        MySqlDataReader Reader = Comando.ExecuteReader();

                        while (Reader.Read())
                        {
                            var g = new MacroGrupos();

                            g.id = Reader["codigo"].ToString();
                            g.descricao = Reader["grupo"].ToString();
                            g.id_grupo_pai = Reader["grupomestre"].ToString();
                            g.ativo = "1";

                            if (g.id_grupo_pai.Equals("") || g.id_grupo_pai.Equals(null))
                            {
                                g.id_grupo_pai = "0";
                            }

                            Grupos.Add(g);
                        }

                        Reader.Close();
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Grupos Produtos - Erro na consulta dos Grupos do Produto", ex.Message, "Macro");
                    }
                    finally
                    {
                        DBConnectionMySql.FechaConexaoBD(DBMySql);
                    }

                    return Grupos;
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Grupos - Erro no envio de Grupos", ex.Message, "Macro");
            }
        }

        //Envia as Variações SKU dos Produtos
        public static void EnviarVariacoes()
        {
            try
            {
                var Variacoes = new List<MacroVariacoes>();

                var Cores = BuscaCores();
                foreach (var Cor in Cores)
                {
                    Variacoes.Add(Cor);
                }
                var Tamanhos = BuscaTamanhos();
                foreach (var Tamanho in Tamanhos)
                {
                    Variacoes.Add(Tamanho);
                }
                var SubSatatus = BuscaSubStatus();
                foreach (var SubStato in SubSatatus)
                {
                    Variacoes.Add(SubStato);
                }

                var Cores2 = BuscaCores2();
                foreach (var Cor2 in Cores2)
                {
                    Variacoes.Add(Cor2);
                }


                var Marcas = BuscaMarcas();
                foreach (var Marca in Marcas)
                {
                    Variacoes.Add(Marca);
                }


                if (Variacoes.Count > 0)
                {
                    MacroWebService.postVariacoes(Variacoes);
                }

                //Busca as Marcas - Utilizado como Variacao
                List<MacroVariacoes> BuscaMarcas()
                {
                    var _Varicoes = new List<MacroVariacoes>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select * from marcas where codigo <> ''";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Variacao = new MacroVariacoes();

                            Variacao.id = Reader["codigo"].ToString();
                            Variacao.id_tipo = "5";
                            Variacao.descricao = Reader["marca"].ToString();
                            Variacao.ativo = "1";
                            Variacao.indice = "1";
                            Variacao.ordem = "2";

                            _Varicoes.Add(Variacao);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _Varicoes;
                }

                //Busca as Cores - Utilizado como Variacao
                List<MacroVariacoes> BuscaCores2()
                {
                    var _Varicoes = new List<MacroVariacoes>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select * from cores where ecommerce = '1' and codigoadicional = '2'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Variacao = new MacroVariacoes();

                            Variacao.id = Reader["codigo"].ToString();
                            Variacao.id_tipo = "4";
                            Variacao.descricao = Reader["cor"].ToString();
                            Variacao.ativo = "1";
                            Variacao.indice = "1";
                            Variacao.ordem = "2";

                            _Varicoes.Add(Variacao);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _Varicoes;
                }

                //Busca as Cores - Utilizado como Variacao
                List<MacroVariacoes> BuscaCores()
                {
                    var _Varicoes = new List<MacroVariacoes>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select * from cores where ecommerce = '1' and codigoadicional = '1'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Variacao = new MacroVariacoes();

                            Variacao.id = Reader["codigo"].ToString();
                            Variacao.id_tipo = "2";
                            Variacao.descricao = Reader["cor"].ToString();
                            Variacao.ativo = "1";
                            Variacao.indice = "1";
                            Variacao.ordem = "2";

                            _Varicoes.Add(Variacao);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _Varicoes;
                }

                //Busca os Tamanhos - Utilizado como Variacao
                List<MacroVariacoes> BuscaTamanhos()
                {
                    var _Varicoes = new List<MacroVariacoes>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select * from tamanhos where ecommerce = '1'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Variacao = new MacroVariacoes();

                            Variacao.id = Reader["codigo"].ToString();
                            Variacao.id_tipo = "3";
                            Variacao.descricao = Reader["tamanho"].ToString();
                            Variacao.ativo = "1";
                            Variacao.indice = "1";
                            Variacao.ordem = "3";

                            _Varicoes.Add(Variacao);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _Varicoes;
                }

                //Busca o Material/Familia "Sub Status" - Utilizado como Variacao
                List<MacroVariacoes> BuscaSubStatus()
                {
                    var _Varicoes = new List<MacroVariacoes>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select * from complementodecadastros where ecommerce = '1'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Variacao = new MacroVariacoes();

                            Variacao.id = Reader["codigo"].ToString();
                            Variacao.id_tipo = "1";
                            Variacao.descricao = Reader["complemento"].ToString();
                            Variacao.ativo = "1";
                            Variacao.indice = "1";
                            Variacao.ordem = "1";

                            _Varicoes.Add(Variacao);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _Varicoes;
                }

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Variações - Erro no envio de variações", ex.Message, "Macro");
            }
        }


        //Atualiza o Estoque no cadastro do produto
        public static void AtualizarEstoque()
        {
            try
            {
                var Estoque = new List<MacroEstoque>();

                foreach (var Es in BuscaEstoque())
                {
                    Estoque.Add(Es);
                }

                if (Estoque.Count > 0)
                {
                    MacroWebService.EnviarEstoque(Estoque);
                }

                //Busca as Cores - Utilizado como Variacao
                List<MacroEstoque> BuscaEstoque()
                {
                    var _Estoque = new List<MacroEstoque>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select p.codigo, p.estoquedisponivel from produtoseservicos p where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.variacao = '1'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Est = new MacroEstoque();

                            Est.id_produto_variacao = Reader["codigo"].ToString();
                            Est.estoque_real = Convert.ToInt32(Convert.ToDecimal(Reader["estoquedisponivel"].ToString()));

                            _Estoque.Add(Est);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _Estoque;
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Estoque - Erro no envio do Estoque", ex.Message, "Macro");
            }
        }


        //Atualiza o Preço na Variação
        public static void AtualizarPreco()
        {
            try
            {
                var Precos = new List<MacroAtualizarPreco>();

                foreach (var P in BuscaPrecos())
                {
                    Precos.Add(P);
                }

                if (Precos.Count > 0)
                {
                    MacroWebService.AtualizaPreços(Precos);
                }

                //Busca os Preços
                List<MacroAtualizarPreco> BuscaPrecos()
                {
                    var _Precos = new List<MacroAtualizarPreco>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select p.codigo, p.praticado from produtoseservicos p where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.variacao = '1'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Pre = new MacroAtualizarPreco();

                            Pre.id_produto_variacao = Reader["codigo"].ToString();

                            Pre.precos = new List<MacroPrecos>();
                            var p = new MacroPrecos();
                            p.id_lista = "1";
                            p.preco = Convert.ToInt32((Convert.ToDecimal(Reader["praticado"].ToString()) * 100));
                            Pre.precos.Add(p);

                            _Precos.Add(Pre);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _Precos;
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Preços - Erro na atualização dos preços", ex.Message, "Macro");
            }
        }

        //Busca os Pedidos do Ecommerce e Cadastra no ERP
        public static void BuscarPedidos()
        {
            try
            {
                var JsonPedido = MacroWebService.getPedidos();

                if (JsonPedido.Equals("Erro"))
                { return; }

                var Pedidos = JsonConvert.DeserializeObject<MacroListarPedidos>(JsonPedido);

                foreach (var Pedido in Pedidos.dados.Where(x => x.situacao.Equals("Pagamento Confirmado")))
                {
                    var DadosPedido = new VerPedidos();

                    //Busca os Dados do Cliente e Retorna o endereço
                    var Endereco = BuscaDadoseCadastrarCliente(Pedido.id_usuario);

                    DadosPedido.Codigocolaborador = ConsultaColaborador(Pedido.id_usuario);

                    DadosPedido.Documento = "Pedido";
                    DadosPedido.Observacoes = Pedido.observacoes;
                    DadosPedido.Status = "Não Faturado";
                    DadosPedido.Subtotal = Convert.ToDecimal(Pedido.valor_produtos.Replace(".", ","));
                    DadosPedido.Valortotal = Convert.ToDecimal(Pedido.valor_a_pagar.Replace(".", ","));
                    DadosPedido.Totalnf = Convert.ToDecimal(Pedido.valor_a_pagar.Replace(".", ","));
                    DadosPedido.Valorfrete = Convert.ToDecimal(Pedido.valor_frete.Replace(".", ","));
                    DadosPedido.Pedido = "#" + Pedido.id;
                    DadosPedido.Codigovendedor = DadosConfiguracao.Config.CodigoVendedorPadrao;
                    DadosPedido.Substatus = DadosConfiguracao.Config.CodigoSubStatusSite;
                    DadosPedido.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;

                    if (Pedido.frete.Equals("interno"))
                    {
                        DadosPedido.Observacoesnota = "ENDEREÇO ENTREGA: RETIRAR NA LOJA";
                    }
                    else
                    {
                        // = ConsultaEndereco(Pedido.id_usuario, Pedido.id_endereco);
                        var Cidade = ConsultaCidade(Endereco.id_cidade);

                        DadosPedido.Observacoesnota = "ENDEREÇO ENTREGA: \n CIDADE: " + Cidade.Cidade.ToUpper() + " - " + Cidade.Estado.ToUpper() + "; \n " +
                            "CEP: " + Endereco.cep + "; \n " +
                            "ENDEREÇO: " + Endereco.endereco.ToUpper() + ";\n " +
                            "COMPLEMENTO: " + Endereco.complemento.ToUpper() + "; \n " +
                            "BAIRRO: " + Endereco.bairro.ToUpper() + "; \n ";
                    }


                    DadosPedido.Itens = new List<VerItens>();
                    foreach (var Itens in Pedido.itens)
                    {
                        var DadosItens = new VerItens();

                        DadosItens.Codigoproduto = Itens.id_produto_variacao;
                        DadosItens.Produto = "";
                        DadosItens.Quantidade = Convert.ToDecimal(Itens.qtde.Replace(".", ","));
                        DadosItens.Tabelacomdesconto = Convert.ToDecimal(Itens.valor.Replace(".", ","));
                        DadosItens.Totalcomdesconto = (DadosItens.Tabelacomdesconto * DadosItens.Quantidade);

                        DadosPedido.Itens.Add(DadosItens);
                    }

                    DadosPedido.Contas = new List<VerContas>();

                    if (!ConsultaPedido("#" + Pedido.id))
                    {
                        DAOPedidos.CadastrarPedido(DadosPedido);
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

                //Busca a Cidade do Cliente
                MacroCidades ConsultaCidade(string IBGE)
                {
                    var Cidade = new MacroCidades();

                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                    string Query = "select * from cidades where codigoibge = '" + IBGE + "'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        if (Reader.Read())
                        {
                            Cidade.Cidade = Reader["cidade"].ToString();
                            Cidade.Estado = Reader["estado"].ToString();
                            Reader.Close();
                        }
                    }
                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                    return Cidade;
                }

                ////Busca o Enderoço do Cliente
                //MacroEnderecos ConsultaEndereco(string IdCliente, string IdEndereco)
                //{
                //    var JsonCliente = MacroWebService.getUsuarios();
                //    if (JsonCliente.Equals("Erro"))
                //    { return new MacroEnderecos(); }

                //    var Clientes = JsonConvert.DeserializeObject<MacroListarUsuarios>(JsonCliente);
                //    return (Clientes.dados.Where(x => x.id.Equals(IdCliente)).First()).enderecos.Where(e => e.id.Equals(IdEndereco)).First();
                //}

                //Busca o codigo do Cliente
                string ConsultaColaborador(string IdEcommerce)
                {
                     MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string CodigoCliente = "";
                    string Query = "select * from colaboradores where (codigoecommerce = '" + IdEcommerce + "' and codigoecommerce is not null and codigoecommerce <> '')";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        if (Reader.Read())
                        {
                            CodigoCliente = Reader["codigo"].ToString();
                            Reader.Close();
                        }
                    }
                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                    return CodigoCliente;
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Pedidos - Erro no cadastro de Pedidos", ex.Message, "Macro");
            }
        }


        //Busca os dados do Cliente e cadastra no sistema
        public static MacroEnderecos BuscaDadoseCadastrarCliente(string IdEcommerce)
        {
            var JsonCliente = MacroWebService.getDadosUsuario(IdEcommerce);

           // if (JsonCliente.Equals("Erro"))
           // {return ; }

            var Cli = JsonConvert.DeserializeObject<MacroListarUsuarios>(JsonCliente).dados.First();

            try
            {
                var Colaborador = new VerColaboradores();
                string Codigo = "";
                Colaborador.IdEcommerce = Cli.id;

                if (Cli.razao.Length > 0)
                {
                    Colaborador.Nomerazaosocial = Cli.razao;
                    Colaborador.Nomefantasia = Cli.fantasia;
                    Colaborador.Cpfcnpj = Cli.cnpj;
                    Colaborador.Inscricaoestadual = "";
                }
                else
                {
                    Codigo = DAOClientes.ConsultaColaborador(Cli.cpf);

                    Colaborador.Nomerazaosocial = Cli.nome;
                    Colaborador.Nomefantasia = Cli.apelido;
                    Colaborador.Cpfcnpj = Cli.cpf;
                    Colaborador.Inscricaoestadual = "";
                }

                Colaborador.Email = Cli.email;
                Colaborador.Observacao = Cli.observacao;

                // Colaborador.CodigoSistema = Cliente.Codigocolaborador;

                Colaborador.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;

                var End = Cli.enderecos.Where(x => x.principal.Equals(1) && x.ativo.Equals(1)).First();

                Colaborador.Endereco = End.endereco;
                Colaborador.Numero = End.numero;
                Colaborador.Complemento = End.complemento;
                Colaborador.Bairro = End.bairro; ;
                Colaborador.Telefone = End.telefone;
                Colaborador.Celular = End.celular;
                Colaborador.Cep = End.cep;

                Colaborador.Codigocidade = DAOCidades.BuscaCidadeAPP(End.id_cidade).CodigoCidade;

                if (!string.IsNullOrEmpty(Codigo))
                {
                    Colaborador.Codigo = Codigo;
                    DAOClientes.UpdateCliente(Colaborador);
                }
                else
                {
                    DAOClientes.CadastrarCliente(Colaborador);
                }

                //Retorna o endereço principal
                return End;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Usuários - Erro no cadastro do Usuário " + Cli.nome, ex.Message, "Macro");
                return new MacroEnderecos();
            }
        }
    }
}
