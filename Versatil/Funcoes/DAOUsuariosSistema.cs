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
    public static class DAOUsuariosSistema
    {
        //Busca as Usuarios VIA API APP
        public static List<VerUsuariosSistema> BuscaUsuarios()
        {
            try
            {
                List<VerUsuariosSistema> ListaUsuarios = new List<VerUsuariosSistema>();

                string Query = "select * from usuarios where situacaodousuario = 'Ativo'";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                {
                    VerUsuariosSistema Usuario = new VerUsuariosSistema();

                    Usuario.Codigo = Reader["codigo"].ToString();
                    Usuario.Usuario = Reader["usuario"].ToString();

                    ListaUsuarios.Add(Usuario);
                }

                Reader.Close();

                DBConnectionMySql.FechaConexaoBD(DBMySql);

                return ListaUsuarios;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Usuarios Sistema - Erro na consulta de Usuarios", ex.Message, "APP");
                return new List<VerUsuariosSistema>();
            }
        }


    }
}
