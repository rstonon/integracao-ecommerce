using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.DB
{
    public static class DBConnectionMySql
    {
        public static string strConnection = "";

        public static void AbreConexaoBD(MySqlConnection _DBMySql)
        {
            if (_DBMySql.State == ConnectionState.Closed)
            {
                _DBMySql.Open();
            }
        }

        //fecha a conexao com o banco de dados
        public static void FechaConexaoBD(MySqlConnection _DBMySql)
        {
            if (_DBMySql.State == ConnectionState.Open)
            {
                _DBMySql.Close();
            }
        }

    }
}
