using IntegracaoRockye.Rocky.DB;
using IntegracaoRockye.Versatil.DB;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Funcoes
{
    public static class DAOLogDB
    {
        //Salva os Logs de Erro
        public static void SalvarLogs(string NumeroPedido, string Obs, string ErroSistema, string Sistema)
        {
            try
            {
                string Sql = "insert into logsincronizacao (numeropedido, data, hora ,obs, errosistema, sistema) values (@numeropedido, @data, @hora, @obs, @errosistema, @sistema)";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand ComandoI = new MySqlCommand(Sql, DBMySql);
                ComandoI.Parameters.AddWithValue("@numeropedido", NumeroPedido);
                ComandoI.Parameters.AddWithValue("@data", DateTime.Now.Date);
                ComandoI.Parameters.AddWithValue("@hora", DateTime.Now.ToString("H:mm:ss"));
                ComandoI.Parameters.AddWithValue("@obs", Obs);
                ComandoI.Parameters.AddWithValue("@errosistema", ErroSistema);
                ComandoI.Parameters.AddWithValue("@sistema", Sistema);

                DBConnectionMySql.AbreConexaoBD(DBMySql);
                ComandoI.ExecuteNonQuery();
                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }
            catch { }
        }
    }
}
