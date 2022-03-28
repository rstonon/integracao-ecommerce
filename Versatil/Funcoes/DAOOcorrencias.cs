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
    public static class DAOOcorrencias
    {
        //Busca as Ocorrencias VIA API APP
        public static List<VerOcorrencias> BuscaOcorrencias()
        {
            try
            {
                List<VerOcorrencias> ListaOcorrencias = new List<VerOcorrencias>();

                string Query = "select * from ocorrencias";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                {
                    VerOcorrencias Ocorrencia = new VerOcorrencias();

                    Ocorrencia.Codigo = Reader["codigo"].ToString();
                    Ocorrencia.Ocorrencia = Reader["ocorrencia"].ToString();

                    ListaOcorrencias.Add(Ocorrencia);
                }

                Reader.Close();

                DBConnectionMySql.FechaConexaoBD(DBMySql);

                return ListaOcorrencias;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Cidades - Erro na consulta de cidades", ex.Message, "APP");
                return new List<VerOcorrencias>();
            }
        }

    }
}
