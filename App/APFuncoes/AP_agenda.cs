using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegracaoRockye.Versatil.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using IntegracaoRockye.App.APModels;
using IntegracaoRockye.Versatil.APModels;
using IntegracaoRockye.Versatil.DB;
using System.Data;

namespace IntegracaoRockye.App.APFuncoes
{
    public static class AP_agenda
    {

        public static List<APContatos> BuscarContatos()
        {
            DateTime DataSinc = DateTime.Now.AddDays(-60);

            string Query = "SELECT * FROM contatos c WHERE ((c.appdatasincronizadoem IS NULL) OR (c.dataalteracao = c.appdatasincronizadoem AND c.horaalteracao > c.apphorasincronizadoem) OR (c.dataalteracao > c.appdatasincronizadoem) OR (c.dataalteracao IS null AND appdatasincronizadoem IS null)) AND (c.data > '" + DataSinc.ToString("yyyy-MM-dd") + "' and c.data > '2020-10-27')";

            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);

            var Adapter = new MySqlDataAdapter(Comando);
            DataTable Obj = new DataTable();
            Adapter.Fill(Obj);
            DBConnectionMySql.FechaConexaoBD(DBMySql);

            var Contatos = (from p in Obj.AsEnumerable()
                            select new APContatos()
                            {
                                Codigocontato = string.IsNullOrEmpty(p.Field<string>("codigoapp")) ? p.Field<int?>("codigo").ToString() : p.Field<string>("codigoapp"),
                                Codigosistema = p.Field<int?>("codigo").ToString(),
                                Codigoocorrencia = string.IsNullOrEmpty(p.Field<string>("tipoocorrencia")) ? "" : p.Field<string>("tipoocorrencia"),
                                Formadecontato = string.IsNullOrEmpty(p.Field<string>("formadecontato")) ? "" : p.Field<string>("formadecontato"),
                                Tipodecontato = string.IsNullOrEmpty(p.Field<string>("tipodecontato")) ? "" : p.Field<string>("tipodecontato"),
                                Statusdocontato = string.IsNullOrEmpty(p.Field<string>("statusdocontato")) ? "" : p.Field<string>("statusdocontato"),
                                Usuario = string.IsNullOrEmpty(p.Field<string>("usuario")) ? "" : p.Field<string>("usuario"),
                                Horalancamento = (p.Field<TimeSpan?>("hora")).ToString(),
                                Datalanacamento = Convert.ToDateTime(p.Field<DateTime?>("data")),
                                Problema = string.IsNullOrEmpty(p.Field<string>("ocorrencia")) ? "" : p.Field<string>("ocorrencia"),
                                Diagnostico = string.IsNullOrEmpty(p.Field<string>("diagnostico")) ? "" : p.Field<string>("diagnostico"),
                                Codigocolaborador = string.IsNullOrEmpty(p.Field<string>("colaborador")) ? "" : p.Field<string>("colaborador"),
                                Atendimento = string.IsNullOrEmpty(p.Field<string>("atendimento")) ? "" : p.Field<string>("atendimento"),
                                Usuariolancamento = string.IsNullOrEmpty(p.Field<string>("usuariolancamento")) ? "" : p.Field<string>("usuariolancamento"),
                                Dataconclusao = string.IsNullOrEmpty(p.Field<DateTime?>("dataconclusao").ToString()) ? null : p.Field<DateTime>("dataconclusao").ToString("yyyy-MM-dd"),
                                Horaconclusao = p.Field<TimeSpan?>("horaconclusao").ToString(),
                                Dataalteracao = Convert.ToDateTime(p.Field<DateTime?>("dataalteracao")),
                                Horaalteracao = p.Field<TimeSpan?>("horaalteracao").ToString(),
                                Urgente = Convert.ToBoolean(p.Field<bool?>("urgente")),
                                Protocolo = string.IsNullOrEmpty(p.Field<string>("protocolo")) ? "" : p.Field<string>("protocolo"),
                            }).ToList();

            return Contatos;
        }

        public static List<APOcorrenciasContatos> BuscarOcorrencias(string _protocolo)
        {
            var Ocorrencias = new List<APOcorrenciasContatos>();

            string Query = "SELECT * FROM contatosocorrencias o where o.protocolo = '" + _protocolo + "'";

            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);

            MySqlDataReader dr = Comando.ExecuteReader();

            while (dr.Read())
            {
                var Ocorr = new APOcorrenciasContatos();

                Ocorr.Codigoocorrencia = 
                    string.IsNullOrEmpty(dr["codigoapp"].ToString()) ? dr["codigo"].ToString() : dr["codigoapp"].ToString();

                Ocorr.Protocolo = dr["protocolo"].ToString();
                Ocorr.Datalancamento = Convert.ToDateTime(dr["data"].ToString());
                Ocorr.Usuariolancamento = dr["usuario"].ToString();
                Ocorr.Hora = dr["hora"].ToString();
                Ocorr.Descricaoocorrencia = dr["ocorrencia"].ToString();
                Ocorr.Codigosistema = dr["codigo"].ToString();


                Ocorrencias.Add(Ocorr);
            }
            dr.Close();
            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return Ocorrencias;
        }

    }
}
