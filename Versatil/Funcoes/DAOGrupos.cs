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
    public  static class DAOGrupos
    {
        //Busca os Grupos 
        public static List<VerGrupos> BuscaGrupos()
        {
            try
            {
                List<VerGrupos> ListaGrupos = new List<VerGrupos>();

                string Query = "select * from grupos where tipo = 'Grupo de Produtos'";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                {
                    VerGrupos Grupo = new VerGrupos();

                    Grupo.CodigoGrupo = Reader["codigo"].ToString();
                    Grupo.NomeGrupo = Reader["grupo"].ToString();

                    ListaGrupos.Add(Grupo);
                }

                Reader.Close();

                DBConnectionMySql.FechaConexaoBD(DBMySql);

                return ListaGrupos;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Grupos - Erro na consulta de grupos", ex.Message, "APP");
                return new List<VerGrupos>();
            }
        }

    }
}
