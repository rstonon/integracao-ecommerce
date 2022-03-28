using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegracaoRockye.Versatil.Funcoes
{
    public static class DAOContatos
    {
        //Busca as Ocorrencias do Contato VIA API APP - ESPECIFICO APP
        public static List<VerContatos> BuscaContato()
        {
            try
            {
                List<VerContatos> ListaContatos = new List<VerContatos>();

                DateTime DataSinc = DateTime.Now.AddDays(-120);

                string Query = "SELECT * FROM contatos c WHERE ((c.appdatasincronizadoem IS NULL) OR (c.dataalteracao = c.appdatasincronizadoem AND c.horaalteracao > c.apphorasincronizadoem) OR (c.dataalteracao > c.appdatasincronizadoem) OR (c.dataalteracao IS null)) and c.data > '" + DataSinc.ToString("yyyy-MM-dd") + "'";

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                {
                    try
                    {
                        VerContatos Contato = new VerContatos();

                        Contato.Codigocontato = Reader["codigoapp"].ToString();
                        Contato.Codigosistema = Reader["codigo"].ToString();

                        if (string.IsNullOrEmpty(Contato.Codigocontato))
                        {
                            Contato.Codigocontato = Contato.Codigosistema;
                        }

                        Contato.Codigoocorrencia = Reader["tipoocorrencia"].ToString();
                        Contato.Formadecontato = Reader["formadecontato"].ToString();
                        Contato.Tipodecontato = Reader["tipodecontato"].ToString();
                        Contato.Statusdocontato = Reader["statusdocontato"].ToString();
                        Contato.Usuario = Reader["usuario"].ToString();
                        Contato.Horalancamento = Reader["hora"].ToString();
                        Contato.Datalanacamento = Convert.ToDateTime(Reader["data"].ToString());
                        Contato.Problema = Reader["ocorrencia"].ToString();
                        Contato.Diagnostico = Reader["diagnostico"].ToString();
                        Contato.Codigocolaborador = Reader["colaborador"].ToString();
                        Contato.Atendimento = Reader["atendimento"].ToString();
                        Contato.Usuariolancamento = Reader["usuariolancamento"].ToString();

                        try
                        {
                            Contato.Dataconclusao = Convert.ToDateTime(Reader["dataconclusao"]).ToString("yyyy-MM-dd");

                        }
                        catch
                        {
                            Contato.Dataconclusao = null;
                        }

                        Contato.Horaconclusao = Reader["horaconclusao"].ToString();

                        try
                        {
                            Contato.Dataalteracao = Convert.ToDateTime(Reader["dataalteracao"].ToString());
                            Contato.Horaalteracao = Reader["horaalteracao"].ToString();
                        }
                        catch
                        {
                            Contato.Dataalteracao = Contato.Dataalteracao;
                            Contato.Horaalteracao = "00:00:00";
                        }

                        try
                        {
                            Contato.Urgente = Convert.ToBoolean(Reader["urgente"].ToString());
                        }
                        catch
                        {
                            Contato.Urgente = false;
                        }

                        Contato.Protocolo = Reader["protocolo"].ToString();

                        ListaContatos.Add(Contato);
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Contatos - Erro na consulta de Contatos 5", ex.Message, "APP");
                    }
                }

                Reader.Close();

                DBConnectionMySql.FechaConexaoBD(DBMySql);

                return ListaContatos;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Contatos - Erro na consulta de Contatos", ex.Message, "APP");
                return new List<VerContatos>();
            }
        }

        //Verifica se Ja existe um acadstro com o mesmo atendimento
        public static bool ValidaCadastro(VerContatos Contato)
        {
            string Verif = "";
            if (Contato.Atendimento == "")
            {
                return true;
            }

            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

            string Query = "select count(*) as quantidade from contatos c where c.atendimento = 'AP" + Contato.Atendimento + "' and c.atendimento <> '' and c.atendimento is not null";
            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            MySqlDataReader Reader = Comando.ExecuteReader();

            if (Reader.Read())
            {
                Verif = Reader["quantidade"].ToString();
                Reader.Close();
            }
            DBConnectionMySql.FechaConexaoBD(DBMySql);

            if (Verif.Equals("0"))
            {
                return true;
            }

            string Sql = "update contatos set  statusdocontato = @statusdocontato where atendimento = @atendimento and atendimento <> '' and atendimento is not null";

            Comando = new MySqlCommand(Sql, DBMySql);
            Comando.Parameters.AddWithValue("@statusdocontato", Contato.Statusdocontato);
            Comando.Parameters.AddWithValue("@atendimento", "AP" + Contato.Atendimento);

            DBConnectionMySql.AbreConexaoBD(DBMySql);
            Comando.ExecuteNonQuery();
            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return false;
        }

        //Cadastra a Ocorrencia do Contato
        public static string CadastrarContato(VerContatos Contato)
        {
            try
            {
                bool Cadastra = true;


                if (ValidaCadastro(Contato) == false)
                {
                    return "OK";
                }

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                string Query = "select Count(*) as quantidade from contatos where (codigoapp =  '" + Contato.Codigosistema + "' or codigo =  '" + Contato.Codigocontato + "')";
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                if (Reader.Read())
                {
                    string Verif = Reader["quantidade"].ToString();

                    if (!Verif.Equals("0"))
                    {
                        Cadastra = false;
                    }

                    Reader.Close();
                }
                DBConnectionMySql.FechaConexaoBD(DBMySql);

                if (Cadastra)
                {

                    string Sql = "INSERT INTO contatos (formadecontato, tipodecontato, tipoocorrencia, statusdocontato, usuario, data, hora, ocorrencia, diagnostico, colaborador, protocolo, atendimento, usuariolancamento, dataconclusao, horaconclusao, urgente, dataalteracao, horaalteracao, codigoapp)" +
                        "values (@formadecontato, @tipodecontato, @tipoocorrencia, @statusdocontato, @usuario, @data, @hora, @ocorrencia, @diagnostico, @colaborador, @protocolo, @atendimento, @usuariolancamento, @dataconclusao, @horaconclusao, @urgente, @dataalteracao, @horaalteracao, @codigoapp)";

                    Comando = new MySqlCommand(Sql, DBMySql);

                    Comando.Parameters.AddWithValue("@formadecontato", Contato.Formadecontato);
                    Comando.Parameters.AddWithValue("@tipodecontato", Contato.Tipodecontato);     //.Datalancamento.ToString("yyyy-MM-dd"));
                    Comando.Parameters.AddWithValue("@tipoocorrencia", Contato.Codigoocorrencia);
                    Comando.Parameters.AddWithValue("@statusdocontato", Contato.Statusdocontato);
                    Comando.Parameters.AddWithValue("@usuario", Contato.Usuario);
                    Comando.Parameters.AddWithValue("@data", Contato.Datalanacamento);
                    Comando.Parameters.AddWithValue("@hora", Contato.Horalancamento);
                    Comando.Parameters.AddWithValue("@ocorrencia", Contato.Problema);
                    Comando.Parameters.AddWithValue("@diagnostico", Contato.Diagnostico);
                    Comando.Parameters.AddWithValue("@colaborador", Contato.Codigocolaborador);
                    Comando.Parameters.AddWithValue("@protocolo", Contato.Protocolo);

                    if (Contato.Atendimento != "")
                        Comando.Parameters.AddWithValue("@atendimento", "AP" + Contato.Atendimento);
                    else
                        Comando.Parameters.AddWithValue("@atendimento", "");

                    Comando.Parameters.AddWithValue("@usuariolancamento", Contato.Usuariolancamento);
                    Comando.Parameters.AddWithValue("@dataconclusao", Contato.Dataconclusao);
                    Comando.Parameters.AddWithValue("@horaconclusao", Contato.Horaconclusao);
                    Comando.Parameters.AddWithValue("@dataalteracao", Contato.Dataalteracao);
                    Comando.Parameters.AddWithValue("@horaalteracao", Contato.Horaalteracao);
                    Comando.Parameters.AddWithValue("@urgente", Contato.Urgente);
                    Comando.Parameters.AddWithValue("@codigoapp", Contato.Codigosistema);

                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    Comando.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(DBMySql);

                    if (Comando.LastInsertedId != 0)
                    {
                        Comando.Parameters.Add(new MySqlParameter("ultimoId", Comando.LastInsertedId));
                    }

                    return Comando.Parameters["@ultimoId"].Value.ToString();
                }

                return "";
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Contatos - Erro no cadastro de Contatos - WebRequest", ex.Message, "APP");
                return "";
            }
        }

        //Atualiza a Ocorrencia do Contato
        public static bool UpdateContato(VerContatos Contato)
        {
            try
            {
                bool Atualiza = true;
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                string Query = "select count(*) as quantidade from contatos where codigo =  '" + Contato.Codigocontato + "' and (dataalteracao >= '" + Contato.Dataalteracao.ToString("yyyy-MM-dd") + "' and horaalteracao > '" + Contato.Horaalteracao + "')";
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                if (Reader.Read())
                {
                    string Verif = Reader["quantidade"].ToString();

                    if (!Verif.Equals("0"))
                    {
                        Atualiza = false;
                    }
                }
                Reader.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);

                if (Atualiza)
                {
                    string Sql = "update contatos set formadecontato = @formadecontato, tipodecontato = @tipodecontato, statusdocontato = @statusdocontato, usuario = @usuario, data = @data, hora = @hora, ocorrencia = @ocorrencia, diagnostico = @diagnostico, colaborador = @colaborador, protocolo = @protocolo, atendimento = @atendimento, usuariolancamento = @usuariolancamento, dataconclusao = @dataconclusao, horaconclusao = @horaconclusao, urgente = @urgente, dataalteracao = @dataalteracao, horaalteracao = @horaalteracao, codigoapp = @codigoapp, tipoocorrencia = @tipoocorrencia where (codigoapp = @codigoapp or codigo = @codigo) and ((dataalteracao = @dataalteracao and horaalteracao < @horaalteracao) or (dataalteracao < @dataalteracao) or (dataalteracao is null))";

                    Comando = new MySqlCommand(Sql, DBMySql);

                    Comando.Parameters.AddWithValue("@formadecontato", Contato.Formadecontato);
                    Comando.Parameters.AddWithValue("@tipodecontato", Contato.Tipodecontato);
                    Comando.Parameters.AddWithValue("@tipoocorrencia", Contato.Codigoocorrencia);
                    Comando.Parameters.AddWithValue("@statusdocontato", Contato.Statusdocontato);
                    Comando.Parameters.AddWithValue("@usuario", Contato.Usuario);
                    Comando.Parameters.AddWithValue("@data", Contato.Datalanacamento);
                    Comando.Parameters.AddWithValue("@hora", Contato.Horalancamento);
                    Comando.Parameters.AddWithValue("@ocorrencia", Contato.Problema);
                    Comando.Parameters.AddWithValue("@diagnostico", Contato.Diagnostico);
                    Comando.Parameters.AddWithValue("@colaborador", Contato.Codigocolaborador);
                    Comando.Parameters.AddWithValue("@protocolo", Contato.Protocolo);
                    Comando.Parameters.AddWithValue("@atendimento", "AP" + Contato.Atendimento);
                    Comando.Parameters.AddWithValue("@usuariolancamento", Contato.Usuariolancamento);
                    Comando.Parameters.AddWithValue("@dataconclusao", Contato.Dataconclusao);
                    Comando.Parameters.AddWithValue("@horaconclusao", Contato.Horaconclusao);
                    Comando.Parameters.AddWithValue("@urgente", Contato.Urgente);
                    Comando.Parameters.AddWithValue("@dataalteracao", Contato.Dataalteracao);
                    Comando.Parameters.AddWithValue("@horaalteracao", Contato.Horaalteracao);
                    Comando.Parameters.AddWithValue("@codigoapp", Contato.Codigosistema);
                    Comando.Parameters.AddWithValue("@codigo", Contato.Codigocontato);

                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    Comando.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                }

                return true;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Contatos - Erro no cadastro de Contatos - WebRequest", ex.Message, "APP");
                return false;
            }
        }

        //Altera a Data de Sincronização no sistema
        public static void UpdateDataSincronizacao(DateTime Date)
        {
            try
            {
                string Hora = Date.ToString("HH:mm:ss");

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                string Query = "update contatos c SET c.appdatasincronizadoem = @data, c.apphorasincronizadoem = @hora";// where ((c.statusdocontato <> 'Concluído' and c.statusdocontato <> 'Cancelado')) AND ((c.dataalteracao = c.appdatasincronizadoem AND c.horaalteracao > c.apphorasincronizadoem) OR (c.dataalteracao > c.appdatasincronizadoem) OR (c.appdatasincronizadoem IS NULL));";
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                Comando.Parameters.AddWithValue("@data", Date.Date.ToString("yyy-MM-dd"));
                Comando.Parameters.AddWithValue("@hora", Hora);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                Comando.ExecuteNonQuery();
                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
        }
    }
}
