using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Funcoes
{
    public static class DAOOcorrenciasContato
    {
        //Busca as Ocorrencias do Contato VIA API APP -  ESPECIFICO APP
        public static List<VerOcorrenciasContatos> BuscaOcorrenciasContato(string _Protocolo)
        {
            try
            {
                List<VerOcorrenciasContatos> ListaOcorrencias = new List<VerOcorrenciasContatos>();

                string Query = "select * from contatosocorrencias where protocolo = '"+_Protocolo+"' and protocolo is not null  and protocolo <> ''";

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                {
                    VerOcorrenciasContatos Ocorrencia = new VerOcorrenciasContatos();

                    Ocorrencia.Codigoocorrencia = Reader["codigoapp"].ToString();
                    Ocorrencia.Codigoapp = Reader["codigo"].ToString();

                    if (string.IsNullOrEmpty(Ocorrencia.Codigoocorrencia))
                    {
                        Ocorrencia.Codigoocorrencia = Ocorrencia.Codigoapp;
                    }

                    Ocorrencia.Datalancamento = Convert.ToDateTime(Reader["data"].ToString());
                    Ocorrencia.Hora = Reader["hora"].ToString();
                    Ocorrencia.Usuariolancamento= Reader["usuario"].ToString();
                    Ocorrencia.Descricaoocorrencia = Reader["ocorrencia"].ToString();
                    Ocorrencia.Usuariolancamento = Reader["usuario"].ToString();
                    Ocorrencia.Protocolo = Reader["protocolo"].ToString();

                    ListaOcorrencias.Add(Ocorrencia);
                }

                Reader.Close();

                DBConnectionMySql.FechaConexaoBD(DBMySql);

                return ListaOcorrencias;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Ocorrências Contato - Erro na consulta de Ocorrências", ex.Message, "APP");
                return new List<VerOcorrenciasContatos>();
            }
        }


        //Cadastra a Ocorrencia do Contato
        public static string CadastrarOcorrenciasContato(VerOcorrenciasContatos Ocorrencia)
        {
            try
            {
                bool Cadastrar = true;
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                string Sql = "select * from contatosocorrencias where (codigo = '"+ Ocorrencia.Codigoapp + "' or codigoapp = '" + Ocorrencia.Codigoapp + "')";

                MySqlCommand Comando = new MySqlCommand(Sql, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                if (Reader.Read())
                {
                    Cadastrar = false;
                    Reader.Close();
                }
                DBConnectionMySql.FechaConexaoBD(DBMySql);

                if (Cadastrar)
                {

                    Sql = "INSERT INTO contatosocorrencias (protocolo, data, hora, usuario, ocorrencia, codigoapp) values (@protocolo, @data, @hora, @usuario, @ocorrencia, @codigoapp)";

                    Comando = new MySqlCommand(Sql, DBMySql);

                    Comando.Parameters.AddWithValue("@protocolo", Ocorrencia.Protocolo);
                    Comando.Parameters.AddWithValue("@data", Ocorrencia.Datalancamento.ToString("yyyy-MM-dd"));
                    Comando.Parameters.AddWithValue("@hora", Ocorrencia.Hora);
                    Comando.Parameters.AddWithValue("@usuario", Ocorrencia.Usuariolancamento);
                    Comando.Parameters.AddWithValue("@ocorrencia", Ocorrencia.Descricaoocorrencia);
                    Comando.Parameters.AddWithValue("@codigoapp", Ocorrencia.Codigoapp);

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
                DAOLogDB.SalvarLogs("", "Ocorrências Contato - Erro no cadastro de Ocorrências do Contato", ex.Message, "APP");
                return "";
            }
        }

    }
}
