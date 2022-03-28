using IntegracaoRockye.Versatil.Funcoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny
{
    public static class TinyWebServices
    {
        public static string getCliente()
        {
            var ArquivoJson = "";
            //string ArquivoJson = JsonConvert.SerializeObject(Listar); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.tiny.com.br/api2/contato.obter.php");
            //requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "GET";
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
                        DAOLogDB.SalvarLogs("", "Clientes - Erro na consulta do Cliente - WebRequest", Resposta, "Tiny");
                    }
                }

                return "Erro";
            }
        }

        public static string getPedidos()
        {
            var ArquivoJson = "";
            //string ArquivoJson = JsonConvert.SerializeObject(Listar); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.tiny.com.br/api2/pedidos.pesquisa.php");
            //requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "GET";
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
                        DAOLogDB.SalvarLogs("", "Pedidos - Erro na consulta dos Pedidos - WebRequest", Resposta, "Tiny");
                    }
                }

                return "Erro";
            }

        }

        public static string getDadosPedido()
        {
            var ArquivoJson = "";
            //string ArquivoJson = JsonConvert.SerializeObject(Listar); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.tiny.com.br/api2/pedido.obter.php");
            //requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "GET";
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
                        DAOLogDB.SalvarLogs("", "Pedido - Erro na consulta do Pedido - WebRequest", Resposta, "Tiny");
                    }
                }

                return "Erro";
            }

        }

        public static void postEstoque()
        {
            //var dado = new MacroEnviarDados();
            //dado.dados = Variacoes;

            var ArquivoJson = "";
            //string ArquivoJson = JsonConvert.SerializeObject(dado); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.tiny.com.br/api2/produto.atualizar.estoque.php");
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
                        DAOLogDB.SalvarLogs("", "Estoque - Erro no atualizar o estoque - WebRequest", Resposta, "Tiny");
                    }
                }
            }
        }


        //public static void postPraticado()
        //{
        //    //var dado = new MacroEnviarDados();
        //    //dado.dados = Variacoes;

        //    string ArquivoJson = JsonConvert.SerializeObject(dado); //Serealiza a lista de Colaboradores
        //    var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

        //    var requisicaoWeb = WebRequest.CreateHttp("https://api.tiny.com.br/api2/produto.atualizar.estoque.php");
        //    requisicaoWeb.Timeout = 600000;
        //    requisicaoWeb.Method = "POST";
        //    requisicaoWeb.ContentType = "application/json";
        //    requisicaoWeb.Accept = "application/json";
        //    requisicaoWeb.ContentLength = ArquivoEnvio.Length;
        //    requisicaoWeb.UserAgent = "Req_Versatil";

        //    try
        //    {
        //        string Json = "";
        //        //Envia os dados POST
        //        using (var stream = requisicaoWeb.GetRequestStream())
        //        {
        //            stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
        //            stream.Close();
        //        }

        //        //Obtem a resposta do servidor
        //        var httpResponse = requisicaoWeb.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            Json = streamReader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException e)
        //    {
        //        using (WebResponse response = e.Response)
        //        {
        //            HttpWebResponse httpResponse = (HttpWebResponse)response;
        //            using (Stream data = response.GetResponseStream())
        //            using (var reader = new StreamReader(data))
        //            {
        //                string Resposta = reader.ReadToEnd();
        //                reader.Close();
        //                data.Close();
        //                httpResponse.Close();
        //                requisicaoWeb.Abort();
        //                DAOLogDB.SalvarLogs("", "Estoque - Erro no atualizar o estoque - WebRequest", Resposta, "Tiny");
        //            }
        //        }
        //    }
        //}

    }
}
