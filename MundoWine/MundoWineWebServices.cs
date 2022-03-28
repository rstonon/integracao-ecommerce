using IntegracaoRockye.MundoWine.Models;
using IntegracaoRockye.Versatil.Funcoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.MundoWine
{
    public static class MundoWineWebServices
    {
        static String username = "versatil";
        static String password = "versatil";
        static String encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

        //Envia para o Site o Estoque dos Produtos
        public static string EnviarEstoque(List<EstoqueMundoWine> Estoque)
        {
            string ArquivoJson = JsonConvert.SerializeObject(Estoque); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://www.mundowine.com.br/api/produtos/estoque");
            requisicaoWeb.Headers.Add("Authorization", "Basic " + encoded);
            requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.UserAgent = "Req_Versatil";

            try
            {
                string Json = "";
                //Envia os dados POST
                using (var stream = requisicaoWeb.GetRequestStream())
                {
                    stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                    stream.Close();
                }

                //Obtem a resposta do servidor
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Json = streamReader.ReadToEnd();
                }

                return Json;
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string Resposta = reader.ReadToEnd();
                        reader.Close();
                        data.Close();
                        httpResponse.Close();
                        requisicaoWeb.Abort();
                        DAOLogDB.SalvarLogs("", "Estoque - Erro no envio do estoque - WebRequest", Resposta, "Site");
                        return "Erro";
                    }
                }
            }
        }

        //Envia para o Site o Preco dos Produtos
        public static string EnviarPreco(List<PrecoMundoWine> Preco)
        {
            string ArquivoJson = JsonConvert.SerializeObject(Preco); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://www.mundowine.com.br/api/produtos/preco");
            requisicaoWeb.Headers.Add("Authorization", "Basic " + encoded);
            requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.UserAgent = "Req_Versatil";

            try
            {
                string Json = "";
                //Envia os dados POST
                using (var stream = requisicaoWeb.GetRequestStream())
                {
                    stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                    stream.Close();
                }

                //Obtem a resposta do servidor
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Json = streamReader.ReadToEnd();
                }

                return Json;
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string Resposta = reader.ReadToEnd();
                        reader.Close();
                        data.Close();
                        httpResponse.Close();
                        requisicaoWeb.Abort();
                        DAOLogDB.SalvarLogs("", "Preços - Erro no envio de preços - WebRequest", Resposta, "Site");
                        return "Erro";
                    }
                }
            }
        }

        //Envia para o Site as Informações das NFes
        public static string EnviarNFe(List<NFeMundoWine> NF)
        {
            string ArquivoJson = JsonConvert.SerializeObject(NF); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://www.mundowine.com.br/api/pedidos/notas");
            requisicaoWeb.Headers.Add("Authorization", "Basic " + encoded);
            requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.UserAgent = "Req_Versatil";

            try
            {
                string Json = "";
                //Envia os dados POST
                using (var stream = requisicaoWeb.GetRequestStream())
                {
                    stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                    stream.Close();
                }

                //Obtem a resposta do servidor
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Json = streamReader.ReadToEnd();
                }

                return Json;
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string Resposta = reader.ReadToEnd();
                        reader.Close();
                        data.Close();
                        httpResponse.Close();
                        requisicaoWeb.Abort();
                        DAOLogDB.SalvarLogs("", "NFe - Erro no envio das informações da NFe - WebRequest", Resposta, "Site");
                        return "Erro";
                    }
                }
            }
        }

    }
}
