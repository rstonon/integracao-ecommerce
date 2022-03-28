using IntegracaoRockye.Rocky.DB;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Funcoes;
using IntegracaoRockye.Versatil.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Services
{
    public static class VerConectionWebService
    {
        //Consulta Cep do WEB Service do VIACEP
        public static VerConsultaCepViaCep GetCepViaCep(string Cep)
        {
            string Url = "https://viacep.com.br/ws/" + Cep + "/json/";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.UserAgent = "RequisicaoWeb";
            
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string Resposta = streamReader.ReadToEnd();
                    streamReader.Close();
                    requisicaoWeb.Abort();

                    return JsonConvert.DeserializeObject<VerConsultaCepViaCep>(Resposta);
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    //HttpWebResponse httpResponse = (HttpWebResponse)response;
                    //using (Stream data = response.GetResponseStream())
                    //using (var reader = new StreamReader(data))
                   // {
                    //    string Resposta = reader.ReadToEnd();
                    //    reader.Close();
                    //    data.Close();
                    //    httpResponse.Close();
                    //    requisicaoWeb.Abort();

                        return new VerConsultaCepViaCep();
                    //}
                }
            }
        }

        public static VerCidade VerConsultaCepAPIAPP(string Ibge)
        {
            try
            {
                string Url = "http://api.sistemaversatil.com.br/api/cidades/" + Ibge;
                string Json = "";
                var requisicaoWeb = WebRequest.CreateHttp(Url);
                requisicaoWeb.Method = "GET";
                requisicaoWeb.ContentType = "application/json";
                requisicaoWeb.Accept = "application/json";
                requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenApiApp);
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                try
                {
                    using (var resposta = requisicaoWeb.GetResponse())
                    {
                        var streamDados = resposta.GetResponseStream();
                        StreamReader reader = new StreamReader(streamDados, Encoding.Default);
                        Json = reader.ReadToEnd();
                        streamDados.Close();
                        resposta.Close();
                    }

                    return JsonConvert.DeserializeObject<List<VerCidade>>(Json)[0];
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
                            DAOLogDB.SalvarLogs("", "Cidades - Erro na consulta de cidades - WebRequest", Resposta, "Site");
                            return new VerCidade();
                        }
                    }
                }
            }
            catch
            {
                return new VerCidade();
            }

        }
    }
}