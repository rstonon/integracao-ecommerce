using IntegracaoRockye.Rocky.DB;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Models;
using IntegracaoRockye.Versatil.Services;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Funcoes
{
    public static class DAOCidades
    {
        //Cadastra a Cidade
        public static string CadastrarCidade(VerCidade Cidade)
        {
            try
            {
                string _Codigocidade = UltimosCodigosDB.GetCodigoCidades().ToString();

                string Sql = "insert into cidades (codigo, cidade, estado, codigoibge, situacao) values (@codigo, @cidade, @estado, @codigoibge, @situacao)";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand Comando = new MySqlCommand(Sql, DBMySql);
                Comando.Parameters.AddWithValue("@codigo", _Codigocidade);
                Comando.Parameters.AddWithValue("@cidade", Cidade.Cidade.ToUpper());
                Comando.Parameters.AddWithValue("@estado", Cidade.Estado.ToUpper());
                Comando.Parameters.AddWithValue("@codigoibge", Cidade.CodigoIbge);
                Comando.Parameters.AddWithValue("@situacao", "Ativa");

                DBConnectionMySql.AbreConexaoBD(DBMySql);
                Comando.ExecuteNonQuery();
                DBConnectionMySql.FechaConexaoBD(DBMySql);

                return _Codigocidade;
            }
            catch
            {
                return "";
            }
        }

        //Busca a Cidade VIA CEP
        public static string BuscaCidadeViaCep(string Cep)
        {
            try
            {
                string CodigoCidade = "";
                var Endereco = VerConectionWebService.GetCepViaCep(Cep);

                string Query = "select c.codigo from cidades c where c.codigoibge = '" + Endereco.Ibge + "'";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                if (Reader.Read())
                {
                    CodigoCidade = Reader["codigo"].ToString();
                    Reader.Close();
                }
                DBConnectionMySql.FechaConexaoBD(DBMySql);

                if (CodigoCidade.Equals(""))
                {
                    VerCidade Cidade = new VerCidade();

                    Cidade.Cidade = Endereco.Localidade;
                    Cidade.CodigoIbge = Endereco.Ibge;
                    Cidade.Estado = Endereco.Uf;

                    CodigoCidade = CadastrarCidade(Cidade);
                }

                return CodigoCidade;
            }
            catch
            {
                return "";
            }
        }

        //Busca a Cidade VIA API APP
        public static VerCidade BuscaCidadeAPP(string Ibge)
        {
            try
            {
                var Cidade = new VerCidade();

                string Query = "select * from cidades where codigoibge = '" + Ibge + "'";
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                if (Reader.Read())
                {
                    Cidade.CodigoCidade = Reader["codigo"].ToString();
                    Cidade.Estado = Reader["estado"].ToString();

                    Reader.Close();
                    return Cidade;
                }
                DBConnectionMySql.FechaConexaoBD(DBMySql);

                var Dados = VerConectionWebService.VerConsultaCepAPIAPP(Ibge);

                if (!Dados.CodigoCidade.Equals(""))
                {
                    Cidade.Cidade = Dados.Cidade;
                    Cidade.CodigoIbge = Dados.CodigoIbge;
                    Cidade.Estado = Dados.Estado;

                    Cidade.CodigoCidade = CadastrarCidade(Cidade);

                    return Cidade;
                }

                return new VerCidade();
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Cidades - Erro na consulta de cidades", ex.Message, "APP");
                return new VerCidade();
            }
        }
    }
}
