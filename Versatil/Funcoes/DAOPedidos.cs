using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Model;
using IntegracaoRockye.Versatil.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Funcoes
{
    public static class DAOPedidos
    {
        //Cadastra Pedido no ERP Versátil
        public static void CadastrarPedido(VerPedidos Pedido)
        {
            MySqlConnection DBMySqlItens = new MySqlConnection(DBConnectionMySql.strConnection);
            DBConnectionMySql.AbreConexaoBD(DBMySqlItens);
            MySqlTransaction Transaction = DBMySqlItens.BeginTransaction();

            try
            {
                string CodigoAtendimento = UltimosCodigosDB.GetCodigoAtendimentos().ToString();
                //CodigoAtendimento = "790";
                
                //ITENS
                int NumeroItem = 0;
                foreach (var Itens in Pedido.Itens)
                {
                    NumeroItem++;
                    var DadosProduto = DAOProdutos.GetDadosProdutosCadastro(Itens.Codigoproduto);
                    if (Itens.Produto.Length < 5)
                    { Itens.Produto = DadosProduto.Descricao; }

                    string CodigoItenAtendimento = UltimosCodigosDB.GetCodigoItensAtendimentos().ToString();
                    string Sqli = "insert into itensatendimentos(item, referenciatmp, atendimento, codigoproduto, produto, quantidade, tabela, total, tabelacomdesconto, totalcomdesconto, unidade, vmv, totalvmv, cmvunitario, observacoes, tmpcor, tmptamanho, freteitem, custorealunitario, pcomissaoitem, comissaoitem) values (@item, @referenciatmp, @atendimento, @codigoproduto, @produto, @quantidade, @tabela, @total, @tabelacomdesconto, @totalcomdesconto, @unidade, @vmv, @totalvmv, @cmvunitario, @observacoes, @tmpcor, @tmptamanho, @freteitem, @custorealunitario, @pcomissaoitem, @comissaoitem)";

                    MySqlCommand ComandoI = new MySqlCommand(Sqli, DBMySqlItens);
                    ComandoI.Transaction = Transaction;

                    ComandoI.Parameters.AddWithValue("@item", CodigoItenAtendimento);
                    //ComandoI.Parameters.AddWithValue("@item", NumeroItem);
                    ComandoI.Parameters.AddWithValue("@referenciatmp", DadosProduto.Referencia);
                    ComandoI.Parameters.AddWithValue("@atendimento", CodigoAtendimento);
                    ComandoI.Parameters.AddWithValue("@codigoproduto", Itens.Codigoproduto);
                    ComandoI.Parameters.AddWithValue("@produto", DadosProduto.Descricao);
                    ComandoI.Parameters.AddWithValue("@quantidade", Itens.Quantidade);
                    ComandoI.Parameters.AddWithValue("@freteitem", Itens.FreteItem);

                    if (DadosConfiguracao.Config.CodigoSubStatusSite.Equals(Pedido.Substatus) && DadosConfiguracao.Config.EcommerceMagento.Equals(true))
                    {
                        ComandoI.Parameters.AddWithValue("@tabela", Itens.Tabela);
                        ComandoI.Parameters.AddWithValue("@total", (Itens.Tabela * Itens.Quantidade));
                    }
                    else
                    {
                        ComandoI.Parameters.AddWithValue("@tabela", DadosProduto.Praticado);
                        ComandoI.Parameters.AddWithValue("@total", (DadosProduto.Praticado * Itens.Quantidade));
                    }

                    ComandoI.Parameters.AddWithValue("@tabelacomdesconto", Itens.Tabelacomdesconto);
                    ComandoI.Parameters.AddWithValue("@totalcomdesconto", (Itens.Tabelacomdesconto * Itens.Quantidade));
                    ComandoI.Parameters.AddWithValue("@unidade", DadosProduto.Unidade);
                    ComandoI.Parameters.AddWithValue("@vmv", DadosProduto.Valormercadoria);
                    ComandoI.Parameters.AddWithValue("@totalvmv", (DadosProduto.Valormercadoria * Itens.Quantidade));
                    ComandoI.Parameters.AddWithValue("@cmvunitario", DadosProduto.Cmvunitario);
                    ComandoI.Parameters.AddWithValue("@observacoes", Itens.ObservacoesItem);
                    ComandoI.Parameters.AddWithValue("@tmpcor", DadosProduto.Cor);
                    ComandoI.Parameters.AddWithValue("@tmptamanho", DadosProduto.Tamanho);

                    ComandoI.Parameters.AddWithValue("@custorealunitario", DadosProduto.CustoReal);
                    ComandoI.Parameters.AddWithValue("@pcomissaoitem", DadosProduto.pComissao);
                    ComandoI.Parameters.AddWithValue("@comissaoitem", ((Itens.Tabelacomdesconto * Itens.Quantidade) * (DadosProduto.pComissao / 100)));

                    ComandoI.ExecuteNonQuery();

                    if (Pedido.Status.Equals("Faturado"))
                    {
                        string SqlE = "insert into movimentoestoque(codigoproduto, documento, tipomovimentacao, qnt, data, codigocolaborador, atendimento, empresa, usuario) values (@codigoproduto, @documento, @tipomovimentacao, @qnt, @data, @codigocolaborador, @atendimento, @empresa, @usuario)";
                        MySqlCommand ComandoE = new MySqlCommand(SqlE, DBMySqlItens);
                        ComandoE.Transaction = Transaction;

                        ComandoE.Parameters.AddWithValue("@codigoproduto", Itens.Codigoproduto);
                        ComandoE.Parameters.AddWithValue("@documento", CodigoAtendimento);
                        ComandoE.Parameters.AddWithValue("@tipomovimentacao", "Saída");
                        ComandoE.Parameters.AddWithValue("@qnt", Itens.Quantidade);
                        ComandoE.Parameters.AddWithValue("@data", DateTime.Now.ToString("yyyy-MM-dd"));
                        ComandoE.Parameters.AddWithValue("@codigocolaborador", Pedido.Codigocolaborador);
                        ComandoE.Parameters.AddWithValue("@atendimento", CodigoAtendimento);
                        ComandoE.Parameters.AddWithValue("@empresa", Pedido.Empresa);
                        ComandoE.Parameters.AddWithValue("@usuario", "APP");

                        //DBConnectionMySql.AbreConexaoBD(DBMySqlItens);
                        ComandoE.ExecuteNonQuery();
                        //DBConnectionMySql.FechaConexaoBD(DBMySqlItens);
                    }

                }

                //CONTAS
                int i = 1;
                foreach (var Conta in Pedido.Contas)
                {
                    //string CodigoConta = UltimosCodigosDB.GetCodigoContas().ToString();
                    string SqlC = "insert into contas(emissao, horaemissao, colaborador, numerodocumento, vencimento, parcela, atendimento, tipodocumento, status, valorinicial, valorquitacao, multa, valorjuros, valordesconto, total, saldo, valorprodutos, percentualcomissao, valorcomissao, tipodaconta, cartorio, outras, empresa, valorservicos, pedido, titular, mora, competencia, codigoapp, atendimentoapp, origem, diadasemanavencimento) values (@emissao, @horaemissao, @colaborador, @numerodocumento, @vencimento, @parcela, @atendimento, @tipodocumento, @status, @valorinicial, @valorquitacao, @multa, @valorjuros, @valordesconto, @total, @saldo, @valorprodutos, @percentualcomissao, @valorcomissao, @tipodaconta, @cartorio, @outras, @empresa, @valorservicos, @pedido, @titular, @mora, @competencia, @codigoapp, @atendimentoapp, @origem, @diadasemanavencimento)";
                    MySqlCommand ComandoC = new MySqlCommand(SqlC, DBMySqlItens);
                    ComandoC.Transaction = Transaction;

                    //ComandoC.Parameters.AddWithValue("@codigo", CodigoConta);
                    ComandoC.Parameters.AddWithValue("@emissao", Conta.DataEmissao);
                    ComandoC.Parameters.AddWithValue("@horaemissao", DateTime.Now.ToString("HH:MM:ss"));
                    ComandoC.Parameters.AddWithValue("@colaborador", Pedido.Codigocolaborador);
                    ComandoC.Parameters.AddWithValue("@numerodocumento", Conta.NumeroDocumento);
                    ComandoC.Parameters.AddWithValue("@vencimento", Conta.DataVencimento);
                    ComandoC.Parameters.AddWithValue("@parcela", i + " de " + Pedido.Contas.Count());
                    ComandoC.Parameters.AddWithValue("@atendimento", CodigoAtendimento);
                    ComandoC.Parameters.AddWithValue("@tipodocumento", Conta.CodigoDocumento);
                    ComandoC.Parameters.AddWithValue("@status", Conta.Status);
                    ComandoC.Parameters.AddWithValue("@valorinicial", Conta.ValorInicial);
                    ComandoC.Parameters.AddWithValue("@valorquitacao", Conta.ValorQuitado);
                    ComandoC.Parameters.AddWithValue("@multa", 0);
                    ComandoC.Parameters.AddWithValue("@valorjuros", 0);
                    ComandoC.Parameters.AddWithValue("@valordesconto", 0);
                    ComandoC.Parameters.AddWithValue("@total", Conta.ValorSaldo);
                    ComandoC.Parameters.AddWithValue("@saldo", Conta.ValorSaldo);
                    ComandoC.Parameters.AddWithValue("@valorprodutos", 0);
                    ComandoC.Parameters.AddWithValue("@percentualcomissao", 0);
                    ComandoC.Parameters.AddWithValue("@valorcomissao", 0);
                    ComandoC.Parameters.AddWithValue("@tipodaconta", "A Receber");
                    ComandoC.Parameters.AddWithValue("@cartorio", 0);
                    ComandoC.Parameters.AddWithValue("@outras", 0);
                    ComandoC.Parameters.AddWithValue("@empresa", Pedido.Empresa);
                    ComandoC.Parameters.AddWithValue("@valorservicos", 0);
                    ComandoC.Parameters.AddWithValue("@pedido", "");
                    ComandoC.Parameters.AddWithValue("@titular", Conta.CodigoTitular);
                    ComandoC.Parameters.AddWithValue("@mora", 0);
                    ComandoC.Parameters.AddWithValue("@competencia", DateTime.Now.ToString("yyy-MM-dd"));
                    ComandoC.Parameters.AddWithValue("@codigoapp", Conta.CodigoConta);
                    ComandoC.Parameters.AddWithValue("@atendimentoapp", Conta.CodigoAtendimento);
                    ComandoC.Parameters.AddWithValue("@origem", "Venda");

                    string DiaSemana = "";
                    try
                    {
                        int Dia = ((int)Conta.DataVencimento.DayOfWeek);
                        string[] nome_dia = new string[7];
                        nome_dia[0] = "Domingo";
                        nome_dia[1] = "Segunda-feira";
                        nome_dia[2] = "Terca-feira";
                        nome_dia[3] = "Quarta-feira";
                        nome_dia[4] = "Quinta-feira";
                        nome_dia[5] = "Sexta-feira";
                        nome_dia[6] = "Sabado";

                        DiaSemana = nome_dia[Dia];
                    }
                    catch
                    {
                        DiaSemana = "";
                    }

                    ComandoC.Parameters.AddWithValue("@diadasemanavencimento", DiaSemana);

                    //DBConnectionMySql.AbreConexaoBD(DBMySqlItens);
                    ComandoC.ExecuteNonQuery();
                    //DBConnectionMySql.FechaConexaoBD(DBMySqlItens);

                    i++;
                }

                //ATENDIMENTO
                string Sql = "";
                if (Pedido.Status.Equals("Faturado"))
                {
                    Sql = "insert into atendimentos(codigo, codigocolaborador, documento, operacao, status, datacadastro, horacadastro, observacoes,subtotal, valortotal, totalnf, pedido, empresa, observacoesnota, substatus, contadebito, contacredito, codigotipodocumentoavista, codigotipodocumento, codigovendedor, pedidoapp, codigocondicaopagamento, codigotransportadora, valordesconto, tabelapadrao, especificardescontonanota, descricaocondicaopagamento, totalprodutos, totalservicos, valorfrete, percentualdesconto, percentualentrada, valorentrada, valorprazo, numeroparcelas, datafaturamento) values (@codigo, @codigocolaborador, @documento, @operacao, @status, @datacadastro, @horacadastro, @observacoes , @subtotal, @valortotal, @totalnf, @pedido, @empresa, @observacoesnota, @substatus, @contadebito, @contacredito, @codigotipodocumentoavista, @codigotipodocumento, @codigovendedor, @pedidoapp, @codigocondicaopagamento, @codigotransportadora, @valordesconto, @tabelapadrao, @especificardescontonanota, @descricaocondicaopagamento, @totalprodutos, @totalservicos, @valorfrete, @percentualdesconto, @percentualentrada, @valorentrada, @valorprazo, @numeroparcelas, @datafaturamento)";
                }
                else
                {
                    Sql = "insert into atendimentos(codigo, codigocolaborador, documento, operacao, status, datacadastro, horacadastro, observacoes,subtotal, valortotal, totalnf, pedido, empresa, observacoesnota, substatus, contadebito, contacredito, codigotipodocumentoavista, codigotipodocumento, codigovendedor, pedidoapp, codigocondicaopagamento, codigotransportadora, valordesconto, tabelapadrao, especificardescontonanota, descricaocondicaopagamento, totalprodutos, totalservicos, valorfrete, percentualdesconto, percentualentrada, valorentrada, valorprazo, numeroparcelas) values (@codigo, @codigocolaborador, @documento, @operacao, @status, @datacadastro, @horacadastro, @observacoes , @subtotal, @valortotal, @totalnf, @pedido, @empresa, @observacoesnota, @substatus, @contadebito, @contacredito, @codigotipodocumentoavista, @codigotipodocumento, @codigovendedor, @pedidoapp, @codigocondicaopagamento, @codigotransportadora, @valordesconto, @tabelapadrao, @especificardescontonanota, @descricaocondicaopagamento, @totalprodutos, @totalservicos, @valorfrete, @percentualdesconto, @percentualentrada, @valorentrada, @valorprazo, @numeroparcelas)";
                }

                MySqlCommand Comando = new MySqlCommand(Sql, DBMySqlItens);
                Comando.Transaction = Transaction;

                Comando.Parameters.AddWithValue("@codigo", CodigoAtendimento);
                Comando.Parameters.AddWithValue("@codigocolaborador", Pedido.Codigocolaborador);
                Comando.Parameters.AddWithValue("@documento", Pedido.Documento);
                Comando.Parameters.AddWithValue("@operacao", "Venda");
                Comando.Parameters.AddWithValue("@status", Pedido.Status);
                Comando.Parameters.AddWithValue("@datacadastro", DateTime.Now.Date);
                Comando.Parameters.AddWithValue("@horacadastro", DateTime.Now.ToString("HH:mm:ss"));
                Comando.Parameters.AddWithValue("@observacoes", Pedido.Observacoes);
                Comando.Parameters.AddWithValue("@valorfrete", ValidaValores(Pedido.Valorfrete.ToString()));
                Comando.Parameters.AddWithValue("@subtotal", Pedido.Subtotal);
                Comando.Parameters.AddWithValue("@valortotal", Pedido.Valortotal);
                Comando.Parameters.AddWithValue("@totalnf", Pedido.Totalnf);
                Comando.Parameters.AddWithValue("@pedido", (Pedido.Pedido));
                Comando.Parameters.AddWithValue("@valordesconto", Pedido.Valordesconto);
                Comando.Parameters.AddWithValue("@percentualdesconto", CalularPercentualDesconto(Pedido.Valordesconto, Pedido.Valortotal));
                Comando.Parameters.AddWithValue("@empresa", Pedido.Empresa);
                Comando.Parameters.AddWithValue("@contadebito", DadosConfiguracao.Config.CodigoContaDebito);
                Comando.Parameters.AddWithValue("@contacredito", DadosConfiguracao.Config.CodigoContaCredito);
                Comando.Parameters.AddWithValue("@codigovendedor", Pedido.Codigovendedor);
                Comando.Parameters.AddWithValue("@observacoesnota", Pedido.Observacoesnota);
                Comando.Parameters.AddWithValue("@substatus", Pedido.Substatus);
                Comando.Parameters.AddWithValue("@pedidoapp", Pedido.Pedidoapp);
                Comando.Parameters.AddWithValue("@codigocondicaopagamento", Pedido.Codigocondicaopagamento);
                Comando.Parameters.AddWithValue("@tabelapadrao", Pedido.Tabeladepreco);
                Comando.Parameters.AddWithValue("@codigotransportadora", Pedido.Codigotransportadora);
                Comando.Parameters.AddWithValue("@especificardescontonanota", Convert.ToInt32(Pedido.Especificardescontonoatendimento));
                Comando.Parameters.AddWithValue("@descricaocondicaopagamento", Pedido.Descricaocondicaopagamento);
                Comando.Parameters.AddWithValue("@totalprodutos", Pedido.Subtotal);
                Comando.Parameters.AddWithValue("@totalservicos", "0");

                if (Pedido.Status.Equals("Faturado"))
                {
                    Comando.Parameters.AddWithValue("@datafaturamento", DateTime.Now.Date.ToString("yyy-MM-dd"));
                }

                if (ValidaString(Pedido.CodigoDocumentoaVista).Equals(""))
                {
                    Comando.Parameters.AddWithValue("@codigotipodocumentoavista", DadosConfiguracao.Config.CodigoDocumentoaVista);
                }
                else
                {
                    Comando.Parameters.AddWithValue("@codigotipodocumentoavista", ValidaString(Pedido.CodigoDocumentoaVista));
                }

                if (ValidaString(Pedido.CodigoDocumentoPrazo).Equals(""))
                {
                    Comando.Parameters.AddWithValue("@codigotipodocumento", DadosConfiguracao.Config.CodigoDocumentoaPrazo);
                }
                else
                {
                    Comando.Parameters.AddWithValue("@codigotipodocumento", ValidaString(Pedido.CodigoDocumentoPrazo));
                }

                Comando.Parameters.AddWithValue("@percentualentrada", ValidaValores(Pedido.pEntrada));
                Comando.Parameters.AddWithValue("@valorentrada", ValidaValores(Pedido.ValorEntrada));
                Comando.Parameters.AddWithValue("@valorprazo", ValidaValores(Pedido.ValoraPrazo));
                Comando.Parameters.AddWithValue("@numeroparcelas", ValidaString(Pedido.NumerodeParcelas));
 
                Comando.ExecuteNonQuery();


                //LOG
                MySqlCommand ComandoL = new MySqlCommand("Insert into arquivodeeventos (usuario, data, hora, operacao, atendimento, colaborador) values (@usuario, @data, @hora, @operacao, @atendimento, @colaborador)", DBMySqlItens);
                ComandoL.Transaction = Transaction;

                var Data = DateTime.Now;
                ComandoL.Parameters.AddWithValue("@usuario", "Aplicativo");
                ComandoL.Parameters.AddWithValue("@data", Data.ToString("yyyy-MM-dd"));
                ComandoL.Parameters.AddWithValue("@hora", Data.ToString("HH:mm:ss"));
                ComandoL.Parameters.AddWithValue("@operacao", "Criou Atendimento");
                ComandoL.Parameters.AddWithValue("@atendimento", CodigoAtendimento);
                ComandoL.Parameters.AddWithValue("@colaborador", Pedido.Codigocolaborador);

                ComandoL.ExecuteNonQuery();



                Transaction.Commit();
                DBConnectionMySql.FechaConexaoBD(DBMySqlItens);
            }
            catch(Exception ex)
            {
                Transaction.Rollback();
                throw new Exception(ex.ToString());
            }

            //Validação of null
            string ValidaString(string Valor)
            {
                try
                {
                    if (Valor.Equals(""))
                    { }
                    return Valor;
                }
                catch
                {
                    return "";
                }
            }

            string ValidaValores(string Valor)
            {
                try
                {
                    if (Valor.Equals(""))
                    { }
                    return Valor.Replace(",",".");
                }
                catch
                {
                    return "0";
                }
            }

        }

        //Calcula o percentual de Desconto
        public static decimal CalularPercentualDesconto(decimal ValorDesc, decimal ValorTot)
        {
            try
            {
                return (ValorDesc / ValorTot) * 100;
            }
            catch 
            {
                return 0;
            }
        }

        //Busca Nome da Condição de Pagamento
        public static string BuscarNomeCondicao(string _CodigoCondicao)
        {
            try
            {
                string NomeCondicao = "";

                string Sql = "SELECT c.condicao FROM condicoespagamento c WHERE c.codigo = '"+ _CodigoCondicao + "'";

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    NomeCondicao = dr["condicao"].ToString();
                    dr.Close();
                }
               
                DBConnectionMySql.FechaConexaoBD(DBMySql);

                return NomeCondicao;
            }
            catch
            {
                return "";
            }
        }
    }
}
