using IntegracaoRockye.Rocky.Model;
using IntegracaoRockye.Rocky.DB;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Funcoes;
using IntegracaoRockye.Versatil.Models;

namespace IntegracaoRockye.Rocky.DB
{
    public static class PedidosAdd
    {
        public static void AddPedido(RKPedidos Pedido)
        {
            try
            {
                string CodigoCliente = "";

                //Verifica o Cliente 
                if (Pedido.tipo_cadastro.Equals("b2c"))
                {
                    var Dados = RKConectionWebService.ReceberClientePF(Pedido.id_cliente);
                    CodigoCliente = DAOClientes.ConsultaColaborador(Dados.cpf);
                    Dados.cidade = DAOCidades.BuscaCidadeViaCep(Dados.cep);

                    VerColaboradores Cliente = new VerColaboradores();
                    Cliente.Codigo = CodigoCliente;
                    Cliente.Situacao = "Ativo";
                    Cliente.Tipo = "Cliente";
                    Cliente.Nomerazaosocial = Dados.nome;
                    Cliente.Nomefantasia = "";
                    Cliente.Cpfcnpj = Dados.cpf;
                    Cliente.Inscricaoestadual = "";
                    Cliente.Endereco = Dados.endereco;
                    Cliente.Numero = Dados.numero;
                    Cliente.Complemento = Dados.complemento;
                    Cliente.Bairro = Dados.bairro;
                    Cliente.Codigocidade = Dados.cidade;
                    Cliente.Telefone = Dados.celular;
                    Cliente.Celular = "";
                    Cliente.Email = Dados.email;
                    Cliente.Cep = Dados.cep;
                    Cliente.Observacao = "";

                    if (CodigoCliente.Equals(""))
                    {
                        CodigoCliente = DAOClientes.CadastrarCliente(Cliente);
                    }
                    else
                    {
                        DAOClientes.UpdateCliente(Cliente);
                    }
                }
                else
                {
                    var Dados = RKConectionWebService.ReceberClientePJ(Pedido.id_cliente);
                    CodigoCliente = DAOClientes.ConsultaColaborador(Dados.cnpj);
                    Dados.cidade = DAOCidades.BuscaCidadeViaCep(Dados.cep);

                    VerColaboradores Cliente = new VerColaboradores();
                    Cliente.Codigo = "";
                    Cliente.Situacao = "Ativo";
                    Cliente.Tipo = "Cliente";
                    Cliente.Nomerazaosocial = Dados.razaosocial;
                    Cliente.Nomefantasia = Dados.nomefantasia;
                    Cliente.Cpfcnpj = Dados.cnpj;
                    Cliente.Inscricaoestadual = Dados.ie;
                    Cliente.Endereco = Dados.endereco;
                    Cliente.Numero = Dados.numero;
                    Cliente.Complemento = Dados.complemento;
                    Cliente.Bairro = Dados.bairro;
                    Cliente.Codigocidade = Dados.cidade;
                    Cliente.Telefone = Dados.celular;
                    Cliente.Celular = "";
                    Cliente.Email = Dados.email_responsavel;
                    Cliente.Cep = Dados.cep;
                    Cliente.Observacao = "Responsável: " + Dados.nome_responsavel;

                    if (CodigoCliente.Equals(""))
                    {
                        CodigoCliente = DAOClientes.CadastrarCliente(Cliente);
                    }
                    else
                    {
                        DAOClientes.UpdateCliente(Cliente);
                    }
                }

                //Verifica se o Pedido ja Existe
                MySqlConnection DBMySqlItens = new MySqlConnection(DBConnectionMySql.strConnection);
                string CodigoPedido = "";
                string Query = "SELECT a.codigo FROM atendimentos a WHERE a.pedido ='#" + Pedido.codigo + "'";
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
                    DadosPedido.Observacoes = Pedido.observacao;
                    DadosPedido.Status = "Não Faturado";
                    DadosPedido.Subtotal = Pedido.subtotal;
                    DadosPedido.Valortotal = Pedido.total;
                    DadosPedido.Valorfrete = Pedido.frete;
                    DadosPedido.Totalnf = Pedido.total;
                    DadosPedido.Pedido = "#" + Pedido.codigo;
                    DadosPedido.Codigovendedor = DadosConfiguracao.Config.CodigoVendedorPadrao;
                    DadosPedido.Substatus = DadosConfiguracao.Config.CodigoSubStatusSite;
                    DadosPedido.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;

                    if (Pedido.codigo_frete.Equals("IN-STORE"))
                    {
                        DadosPedido.Observacoesnota = "ENDEREÇO ENTREGA: RETIRAR NA LOJA";
                    }
                    else
                    {
                        DadosPedido.Observacoesnota = "ENDEREÇO ENTREGA: \n CIDADE: " + Pedido.endereco_entrega.cidade.ToUpper() + " - " + Pedido.endereco_entrega.estado.ToUpper() + "; \n " +
                            "CEP: " + Pedido.endereco_entrega.cep + "; \n " +
                            "ENDEREÇO: " + Pedido.endereco_entrega.endereco.ToUpper() + ";\n " +
                            "COMPLEMENTO: " + Pedido.endereco_entrega.complemento.ToUpper() + "; \n " +
                            "BAIRRO: " + Pedido.endereco_entrega.bairro.ToUpper() + "; \n ";
                    }

                    DadosPedido.Itens = new List<VerItens>();

                    foreach (var Itens in Pedido.items)
                    {
                        VerItens DadosItens = new VerItens();

                        DadosItens.Codigoproduto = Itens.sku;
                        DadosItens.Produto = Itens.nome;
                        DadosItens.Quantidade = Itens.qtd;
                        DadosItens.Tabelacomdesconto = Itens.valor;
                        DadosItens.Totalcomdesconto = (Itens.valor * Itens.qtd);

                        DadosPedido.Itens.Add(DadosItens);
                    }

                    DadosPedido.Contas = new List<VerContas>();

                    DAOPedidos.CadastrarPedido(DadosPedido);
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs(Pedido.id, "Pedidos - Erro no cadastro de pedido", ex.Message, "Site");
            }
        }

        //Versão 2.0
        public static void AddPedidoV2(RKPedidos Pedido)
        {
            try
            {
                string CodigoCliente = "";

                //Verifica o Cliente 
                if (Pedido.tipo_cadastro.Equals("b2c"))
                {
                    var Dados = RKConectionWebService.ReceberClientePF(Pedido.id_cliente);
                    CodigoCliente = DAOClientes.ConsultaColaborador(Dados.cpf);
                    Dados.cidade = DAOCidades.BuscaCidadeViaCep(Dados.cep);

                    VerColaboradores Cliente = new VerColaboradores();
                    Cliente.Codigo = CodigoCliente;
                    Cliente.Situacao = "Ativo";
                    Cliente.Tipo = "Cliente";
                    Cliente.Nomerazaosocial = Dados.nome;
                    Cliente.Nomefantasia = "";
                    Cliente.Cpfcnpj = Dados.cpf;
                    Cliente.Inscricaoestadual = "";
                    Cliente.Endereco = Dados.endereco;
                    Cliente.Numero = Dados.numero;
                    Cliente.Complemento = Dados.complemento;
                    Cliente.Bairro = Dados.bairro;
                    Cliente.Codigocidade = Dados.cidade;
                    Cliente.Telefone = Dados.celular;
                    Cliente.Celular = "";
                    Cliente.Email = Dados.email;
                    Cliente.Cep = Dados.cep;
                    Cliente.Observacao = "";
                    //Cliente.Datanascimento = 

                    if (CodigoCliente.Equals(""))
                    {
                        CodigoCliente = DAOClientes.CadastrarCliente(Cliente);
                    }
                    else
                    {
                        DAOClientes.UpdateCliente(Cliente);
                    }
                }
                else
                {
                    var Dados = RKConectionWebService.ReceberClientePJ(Pedido.id_cliente);
                    CodigoCliente = DAOClientes.ConsultaColaborador(Dados.cnpj);
                    Dados.cidade = DAOCidades.BuscaCidadeViaCep(Dados.cep);

                    VerColaboradores Cliente = new VerColaboradores();
                    Cliente.Codigo = "";
                    Cliente.Situacao = "Ativo";
                    Cliente.Tipo = "Cliente";
                    Cliente.Nomerazaosocial = Dados.razaosocial;
                    Cliente.Nomefantasia = Dados.nomefantasia;
                    Cliente.Cpfcnpj = Dados.cnpj;
                    Cliente.Inscricaoestadual = Dados.ie;
                    Cliente.Endereco = Dados.endereco;
                    Cliente.Numero = Dados.numero;
                    Cliente.Complemento = Dados.complemento;
                    Cliente.Bairro = Dados.bairro;
                    Cliente.Codigocidade = Dados.cidade;
                    Cliente.Telefone = Dados.celular;
                    Cliente.Celular = "";
                    Cliente.Email = Dados.email_responsavel;
                    Cliente.Cep = Dados.cep;
                    Cliente.Observacao = "Responsável: " + Dados.nome_responsavel;

                    if (CodigoCliente.Equals(""))
                    {
                        CodigoCliente = DAOClientes.CadastrarCliente(Cliente);
                    }
                    else
                    {
                        DAOClientes.UpdateCliente(Cliente);
                    }
                }

                //Verifica se o Pedido ja Existe
                MySqlConnection DBMySqlItens = new MySqlConnection(DBConnectionMySql.strConnection);
                string CodigoPedido = "";
                string Query = "SELECT a.codigo FROM atendimentos a WHERE a.pedido ='#" + Pedido.codigo + "'";
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
                    DadosPedido.Observacoes = Pedido.observacao;
                    DadosPedido.Subtotal = Pedido.subtotal;
                    DadosPedido.Valortotal = Pedido.total;
                    DadosPedido.Status = "Não Faturado";
                    DadosPedido.Valorfrete = Pedido.frete;
                    DadosPedido.Totalnf = Pedido.total;
                    DadosPedido.Pedido = "#" + Pedido.codigo;
                    DadosPedido.Codigovendedor = DadosConfiguracao.Config.CodigoVendedorPadrao;
                    DadosPedido.Substatus = DadosConfiguracao.Config.CodigoSubStatusSite;
                    DadosPedido.Empresa = DadosConfiguracao.Config.CodigoConfiguracao;

                    if (Pedido.codigo_frete.Equals("IN-STORE"))
                    {
                        DadosPedido.Observacoesnota = "ENDEREÇO ENTREGA: RETIRAR NA LOJA";
                    }
                    else
                    {
                        DadosPedido.Observacoesnota = "ENDEREÇO ENTREGA: \n CIDADE: " + Pedido.endereco_entrega.cidade.ToUpper() + " - " + Pedido.endereco_entrega.estado.ToUpper() + "; \n " +
                            "CEP: " + Pedido.endereco_entrega.cep + "; \n " +
                            "ENDEREÇO: " + Pedido.endereco_entrega.endereco.ToUpper() + ";\n " +
                            "COMPLEMENTO: " + Pedido.endereco_entrega.complemento.ToUpper() + "; \n " +
                            "BAIRRO: " + Pedido.endereco_entrega.bairro.ToUpper() + "; \n ";
                    }

                    DadosPedido.Itens = new List<VerItens>();

                    foreach (var Itens in Pedido.items)
                    {
                        VerItens DadosItens = new VerItens();

                        DadosItens.Codigoproduto = BuscaCodigoProduto(Itens.id_sku);
                        DadosItens.Produto = Itens.nome;
                        DadosItens.Quantidade = Itens.qtd;
                        DadosItens.Tabelacomdesconto = Itens.valor;
                        DadosItens.Totalcomdesconto = (Itens.valor * Itens.qtd);

                        DadosPedido.Itens.Add(DadosItens);
                    }

                    DadosPedido.Contas = new List<VerContas>();

                    DAOPedidos.CadastrarPedido(DadosPedido);
                }

                string BuscaCodigoProduto(string _codVariacao)
                {
                    var DBMySqlPro = new MySqlConnection(DBConnectionMySql.strConnection);
                    string CodigoProduto = "";
                    string QueryP = "SELECT p.codigo FROM produtoseservicos p WHERE p.codigovariacao ='" + _codVariacao + "'";
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

            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs(Pedido.id, "Pedidos - Erro no cadastro de pedido", ex.Message, "Site");
            }
        }
    }
}
