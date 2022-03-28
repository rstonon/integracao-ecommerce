using IntegracaoRockye.Rocky.DB;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.DB
{
    public static class UltimosCodigosDB
    {
        public static int GetCodigoCliente()
        {
            int CodigoCliente = 0;
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            string Query = "select u.colaboradores from ultimoscodigos u where u.empresa ='"+ DadosConfiguracao.Config.CodigoConfiguracao + "'";
            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            MySqlDataReader Reader = Comando.ExecuteReader();

            if (Reader.Read())
            {
                CodigoCliente = Convert.ToInt32(Reader["colaboradores"].ToString());
                Reader.Close();
            }

            if (CodigoCliente > 0)
            {
                CodigoCliente = (CodigoCliente + 1);
                Query = "update ultimoscodigos set colaboradores = '" + CodigoCliente + "' where empresa ='" + DadosConfiguracao.Config.CodigoConfiguracao + "'";
                Comando = new MySqlCommand(Query, DBMySql);
                Comando.ExecuteNonQuery();
            }

            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return CodigoCliente;
        }

        public static int GetCodigoAtendimentos()
        {
            int CodigoAtendimento = 0;
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            string Query = "select u.atendimentos from ultimoscodigos u where u.empresa ='" + DadosConfiguracao.Config.CodigoConfiguracao + "'";
            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            MySqlDataReader Reader = Comando.ExecuteReader();

            if (Reader.Read())
            {
                CodigoAtendimento = Convert.ToInt32(Reader["atendimentos"].ToString());
                Reader.Close();
            }

            if (CodigoAtendimento > 0)
            {
                CodigoAtendimento = (CodigoAtendimento + 1);
                Query = "update ultimoscodigos set atendimentos = '" + CodigoAtendimento + "' where empresa ='" + DadosConfiguracao.Config.CodigoConfiguracao + "'";
                Comando = new MySqlCommand(Query, DBMySql);
                Comando.ExecuteNonQuery();
            }

            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return CodigoAtendimento;
        }


        //Utilizado no campo Item do Itens atendimento - Codigo Principal é Auto Increment
        public static int GetCodigoItensAtendimentos()
        {
            int CodigoItensAtendimento = 0;
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            string Query = "select u.itensatendimentos from ultimoscodigos u where u.empresa ='" + DadosConfiguracao.Config.CodigoConfiguracao + "'";
            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            MySqlDataReader Reader = Comando.ExecuteReader();

            if (Reader.Read())
            {
                CodigoItensAtendimento = Convert.ToInt32(Reader["itensatendimentos"].ToString());
                Reader.Close();
            }

            if (CodigoItensAtendimento > 0)
            {
                CodigoItensAtendimento = (CodigoItensAtendimento + 1);
                Query = "update ultimoscodigos set itensatendimentos = '" + CodigoItensAtendimento + "' where empresa ='" + DadosConfiguracao.Config.CodigoConfiguracao + "'";
                Comando = new MySqlCommand(Query, DBMySql);
                Comando.ExecuteNonQuery();
            }

            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return CodigoItensAtendimento;
        }



        public static int GetCodigoCidades()
        {
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            int CodigoCidades = 0;
            string Query = "select u.cidades from ultimoscodigos u where u.empresa ='" + DadosConfiguracao.Config.CodigoConfiguracao + "'";
            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            MySqlDataReader Reader = Comando.ExecuteReader();

            if (Reader.Read())
            {
                CodigoCidades = Convert.ToInt32(Reader["cidades"].ToString());
                Reader.Close();
            }

            if (CodigoCidades > 0)
            {
                CodigoCidades = (CodigoCidades + 1);
                Query = "update ultimoscodigos set cidades = '" + CodigoCidades + "' where empresa ='" + DadosConfiguracao.Config.CodigoConfiguracao + "'";
                Comando = new MySqlCommand(Query, DBMySql);
                Comando.ExecuteNonQuery();
            }

            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return CodigoCidades;
        }

        //public static int GetCodigoContas()
        //{
        //    MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
        //    int CodigoContas = 0;
        //    string Query = "select u.contas from ultimoscodigos u where u.empresa ='" + DadosConfiguracao.Config.CodigoConfiguracao + "'";
        //    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
        //    DBConnectionMySql.AbreConexaoBD(DBMySql);
        //    MySqlDataReader Reader = Comando.ExecuteReader();

        //    if (Reader.Read())
        //    {
        //        CodigoContas = Convert.ToInt32(Reader["contas"].ToString());
        //        Reader.Close();
        //    }

        //    if (CodigoContas > 0)
        //    {
        //        CodigoContas = (CodigoContas + 1);
        //        Query = "update ultimoscodigos set contas = '" + CodigoContas + "' where empresa ='" + DadosConfiguracao.Config.CodigoConfiguracao + "'";
        //        Comando = new MySqlCommand(Query, DBMySql);
        //        Comando.ExecuteNonQuery();
        //    }

        //    DBConnectionMySql.FechaConexaoBD(DBMySql);

        //    return CodigoContas;
        //}


    }
}
