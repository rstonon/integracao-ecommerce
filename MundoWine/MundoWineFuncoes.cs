using IntegracaoRockye.MundoWine.Models;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Funcoes;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegracaoRockye.MundoWine
{
    public static class MundoWineFuncoes
    {
        //Busca o Estoque e Envia para o Site
        public static void EnviarEstoque()
        {
            try
            {
                var Estoque = BuscaEstoque();
                string RetornoJson = "";

                if (Estoque.Count > 0)
                {
                    RetornoJson = MundoWineWebServices.EnviarEstoque(Estoque);
                }

                if (RetornoJson.Equals("Erro"))
                { return; }

                var Retorno = JsonConvert.DeserializeObject<List<RetornoMundoWine>>(RetornoJson);

                foreach (var r in Retorno)
                {
                    if (!r.mensagem.Equals("Produto atualizado com sucesso."))
                    {
                        DAOLogDB.SalvarLogs("", "Produto: " + r.referencia + " erro na atualização do estoque", r.mensagem, "Site");
                    }
                }

                //Busca o Estoque no Sistema
                List<EstoqueMundoWine> BuscaEstoque()
                {
                    var _Estoque = new List<EstoqueMundoWine>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select p.codigo, p.estoquedisponivel, p.tmpkit, p.tmpcapacidadeproducao from produtoseservicos p where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site = '1'";// and p.codigo = '100300'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Est = new EstoqueMundoWine();

                            Est.referencia = Reader["codigo"].ToString();
                            string esto = Reader["tmpcapacidadeproducao"].ToString();

                            try
                            {
                                if (Convert.ToDecimal(esto) <= 0)
                                {
                                    Est.estoque = 0;
                                }
                                else
                                {
                                    Est.estoque = Convert.ToInt32(Convert.ToDecimal(esto));
                                }
                            }
                            catch
                            {
                                Est.estoque = 0;
                            }

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
                DAOLogDB.SalvarLogs("", "Estoque - Erro no envio do estoque", ex.Message, "Site");
            }

        }

        //Busca o Estoque e Envia para o Site - Somente um produto de teste
        public static void EnviarEstoqueProdutoUnico(string Codigo)
        {
            try
            {
                var Estoque = BuscaEstoque();
                string RetornoJson = "";

                if (Estoque.Count > 0)
                {
                    RetornoJson = MundoWineWebServices.EnviarEstoque(Estoque);
                }

                if (RetornoJson.Equals("Erro"))
                { return; }

                var Retorno = JsonConvert.DeserializeObject<List<RetornoMundoWine>>(RetornoJson);

                foreach (var r in Retorno)
                {
                    if (!r.mensagem.Equals("Produto atualizado com sucesso."))
                    {
                        DAOLogDB.SalvarLogs("", "Produto: " + r.referencia + " erro na atualização do estoque", r.mensagem, "Site");
                    }
                }

                //Busca o Estoque no Sistema
                List<EstoqueMundoWine> BuscaEstoque()
                {
                    var _Estoque = new List<EstoqueMundoWine>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select p.codigo, p.estoquedisponivel, p.tmpkit, p.tmpcapacidadeproducao from produtoseservicos p where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site = '1' and p.codigo = '" + Codigo + "'";// and p.codigo = '100300'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Est = new EstoqueMundoWine();

                            Est.referencia = Reader["codigo"].ToString();
                            string esto = Reader["tmpcapacidadeproducao"].ToString();

                            try
                            {
                                if (Convert.ToDecimal(esto) <= 0)
                                {
                                    Est.estoque = 0;
                                }
                                else
                                {
                                    Est.estoque = Convert.ToInt32(Convert.ToDecimal(esto));
                                }
                            }
                            catch
                            {
                                Est.estoque = 0;
                            }

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
                DAOLogDB.SalvarLogs("", "Estoque - Erro no envio do estoque", ex.Message, "Site");
            }

        }

        //Busca o Praticado e Envia para o Site - Somente um produto de teste
        public static void EnviarPraticadoProdutoUnico(string Codigo)
        {
            try
            {
                var Precos = BuscaPraticado();
                string RetornoJson = "";

                if (Precos.Count > 0)
                {
                    RetornoJson = MundoWineWebServices.EnviarPreco(Precos);
                }

                if (RetornoJson.Equals("Erro"))
                { return; }

                var Retorno = JsonConvert.DeserializeObject<List<RetornoMundoWine>>(RetornoJson);

                foreach (var r in Retorno)
                {
                    if (!r.mensagem.Equals("Produto atualizado com sucesso."))
                    {
                        DAOLogDB.SalvarLogs("", "Produto: " + r.referencia + " erro na atualização do preço", r.mensagem, "Site");
                    }
                }

                //Busca o Estoque no Sistema
                List<PrecoMundoWine> BuscaPraticado()
                {
                    var _Praticado = new List<PrecoMundoWine>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select p.codigo, p.praticado from produtoseservicos p where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site = '1' and p.codigo = '" + Codigo + "'";// and p.codigo = '100300'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Preco = new PrecoMundoWine();

                            Preco.referencia = Reader["codigo"].ToString();
                            string preco = Reader["praticado"].ToString();

                            try
                            {
                                if (Convert.ToDecimal(preco) <= 0)
                                {
                                    Preco.preco = 0;
                                }
                                else
                                {
                                    Preco.preco = Convert.ToDecimal(preco);
                                }
                            }
                            catch
                            {
                            }

                            _Praticado.Add(Preco);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _Praticado;
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Preço - Erro no envio dos preços", ex.Message, "Site");
            }

        }

        //Busca o Praticado e Envia para o Site
        public static void EnviarPraticado()
        {
            try
            {
                var Precos = BuscaPraticado();
                string RetornoJson = "";

                if (Precos.Count > 0)
                {
                    RetornoJson = MundoWineWebServices.EnviarPreco(Precos);
                }

                if (RetornoJson.Equals("Erro"))
                { return; }

                var Retorno = JsonConvert.DeserializeObject<List<RetornoMundoWine>>(RetornoJson);

                foreach (var r in Retorno)
                {
                    if (!r.mensagem.Equals("Produto atualizado com sucesso."))
                    {
                        DAOLogDB.SalvarLogs("", "Produto: " + r.referencia + " erro na atualização do preço", r.mensagem, "Site");
                    }
                }

                //Busca o Estoque no Sistema
                List<PrecoMundoWine> BuscaPraticado()
                {
                    var _Praticado = new List<PrecoMundoWine>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select p.codigo, p.praticado from produtoseservicos p where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site = '1'";// and p.codigo = '100300'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var Preco = new PrecoMundoWine();

                            Preco.referencia = Reader["codigo"].ToString();
                            string preco = Reader["praticado"].ToString();

                            try
                            {
                                if (Convert.ToDecimal(preco) <= 0)
                                {
                                    Preco.preco = 0;
                                }
                                else
                                {
                                    Preco.preco = Convert.ToDecimal(preco);
                                }
                            }
                            catch
                            {
                            }

                            _Praticado.Add(Preco);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _Praticado;
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Preço - Erro no envio dos preços", ex.Message, "Site");
            }

        }

        //Busca as informações da NFe e Envia para o Site
        public static void EnviarNFe()
        {
            try
            {
                var NFe = BuscaNFe();
                string RetornoJson = "";

                if (NFe.Count > 0)
                {
                    RetornoJson = MundoWineWebServices.EnviarNFe(NFe);
                }

                if (RetornoJson.Equals("Erro"))
                { return; }

                try
                {
                    var Retorno = JsonConvert.DeserializeObject<List<RetornoMundoWine>>(RetornoJson);
                    foreach (var r in Retorno)
                    {
                        if (!r.mensagem.Equals("Pedido atualizado."))
                        {
                            DAOLogDB.SalvarLogs("", "NFe: " + r.codigo + " erro na atualização da NFe", r.mensagem, "Site");
                        }
                        else
                        {
                            UpdatePedido(r.codigo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DAOLogDB.SalvarLogs("", "NFe - Erro ao processar retorno", ex.Message + " Retorno Site: " + RetornoJson, "Site");
                }


                //Busca as informações no Sistema
                List<NFeMundoWine> BuscaNFe()
                {
                    var _NFe = new List<NFeMundoWine>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select codigo, nfechavedeacesso, dataes, codigotransportadora, pedido from atendimentos where enviadoecommerce = '0' AND nfechavedeacesso <> '' and documento = 'Nota'";// and p.codigo = '100300'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var NF = new NFeMundoWine();

                            string codigo = "";
                            codigo = Reader["pedido"].ToString();
                            codigo = codigo.Replace("#", "");

                            NF.codigo = Convert.ToInt32(codigo);
                            NF.chave_nf = Reader["nfechavedeacesso"].ToString();
                            NF.data_envio = Convert.ToDateTime(Reader["dataes"].ToString()).ToString("yyyy-MM-dd");
                            NF.transportadora = Convert.ToInt32(Reader["codigotransportadora"].ToString());

                            _NFe.Add(NF);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _NFe;
                }

                void UpdatePedido(string pedido)
                {
                    string Sql = "update atendimentos set enviadoecommerce = @enviado where pedido = @pedido";
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    MySqlCommand Comando = new MySqlCommand(Sql, DBMySql);
                    Comando.Parameters.AddWithValue("@enviado", "1");
                    Comando.Parameters.AddWithValue("@pedido", "#" + pedido);

                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    Comando.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "NFe - Erro no envio das informações da NFe", ex.Message, "Site");
            }

        }

        //Busca as informações da NFe e Envia para o Site - Única
        public static void EnviarNFeUnica(string codigo)
        {
            try
            {
                var NFe = BuscaNFe();
                string RetornoJson = "";

                if (NFe.Count > 0)
                {
                    RetornoJson = MundoWineWebServices.EnviarNFe(NFe);
                }

                if (RetornoJson.Equals("Erro"))
                { return; }

                try
                {
                    var Retorno = JsonConvert.DeserializeObject<List<RetornoMundoWine>>(RetornoJson);
                    foreach (var r in Retorno)
                    {
                        if (!r.mensagem.Equals("Pedido atualizado."))
                        {
                            DAOLogDB.SalvarLogs("", "NFe: " + r.codigo + " erro na atualização da NFe", r.mensagem, "Site");
                        }
                        else
                        {
                            UpdatePedido(r.codigo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DAOLogDB.SalvarLogs("", "NFe - Erro ao processar retorno", ex.Message + " Retorno Site: " + RetornoJson, "Site");
                }


                //Busca as informações no Sistema
                List<NFeMundoWine> BuscaNFe()
                {
                    var _NFe = new List<NFeMundoWine>();

                    var DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    string Query = "select codigo, nfechavedeacesso, dataes, codigotransportadora, pedido from atendimentos where enviadoecommerce = '0' AND nfechavedeacesso <> '' and documento = 'Nota' and p.codigo = '" + codigo + "'";// and p.codigo = '100300'";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            var NF = new NFeMundoWine();

                            string codigo = "";
                            codigo = Reader["pedido"].ToString();
                            codigo = codigo.Replace("#", "");

                            NF.codigo = Convert.ToInt32(codigo);
                            NF.chave_nf = Reader["nfechavedeacesso"].ToString();
                            NF.data_envio = Convert.ToDateTime(Reader["dataes"].ToString()).ToString("yyyy-MM-dd");
                            NF.transportadora = Convert.ToInt32(Reader["codigotransportadora"].ToString());

                            _NFe.Add(NF);
                        }

                        Reader.Close();
                    }

                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    return _NFe;
                }

                void UpdatePedido(string pedido)
                {
                    string Sql = "update atendimentos set enviadoecommerce = @enviado where pedido = @pedido";
                    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                    MySqlCommand Comando = new MySqlCommand(Sql, DBMySql);
                    Comando.Parameters.AddWithValue("@enviado", "1");
                    Comando.Parameters.AddWithValue("@pedido", "#" + pedido);

                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    Comando.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "NFe - Erro no envio das informações da NFe", ex.Message, "Site");
            }

        }

    }
}
