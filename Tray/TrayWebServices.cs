using IntegracaoRockye.Tray.Models;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Funcoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegracaoRockye.Tray
{
    public static class TrayWebServices
    {
        //public static string URL = "https://trayparceiros.commercesuite.com.br";
        public static string URL = "https://dankana.commercesuite.com.br";

        //Solicita as Credenciais de Autenticação
        public static string Auth()
        {
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("consumer_key", DadosConfiguracao.Config.Consumer_keyTray);
            queryParameters.Add("consumer_secret", DadosConfiguracao.Config.Consumer_secretTray);
            queryParameters.Add("code", DadosConfiguracao.Config.CodeTray);

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }

            string postString = String.Join("&", items.ToArray());
            var ArquivoEnvio = Encoding.UTF8.GetBytes(postString); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp(URL + "/web_api/auth");
            requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentType = "application/x-www-form-urlencoded";
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
                        DAOLogDB.SalvarLogs("", "Auth - Erro na Solicitação de Autenticação - WebRequest", Resposta, "Tray");
                    }
                }

                return "Erro";
            }    
        }

        //Solicita a Atualização da Credencial
        public static string AuthRefresh()
        {
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("refresh_token", DadosConfiguracao.Config.AuthTray.refresh_token);

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }

            string argsString = String.Join("&", items.ToArray());


            WebRequest requisicaoWeb = WebRequest.Create(URL + "/web_api/auth" + "?" + argsString);
            requisicaoWeb.Credentials = CredentialCache.DefaultCredentials;


            try
            {
                string Json = "";
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
                        DAOLogDB.SalvarLogs("", "Auth - Erro na Solicitação de Autenticação - WebRequest", Resposta, "Tray");
                    }
                }

                return "Erro";
            }
        }




        //Busca os dados do Produto pelo Filtro Referencia onde enviamos o codigo do produto
        public static string BuscaListaProdutos(int Page, string ReferenciaProduto)
        {
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("access_token", DadosConfiguracao.Config.AuthTray.access_token);
            queryParameters.Add("limit", "50");
            queryParameters.Add("page", Page.ToString());
            queryParameters.Add("reference", ReferenciaProduto);

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }

            string postString = String.Join("&", items.ToArray());
            //var ArquivoEnvio = Encoding.UTF8.GetBytes(postString); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp(URL + "/web_api/products" + "?" + postString);
            requisicaoWeb.Timeout = 600000;
            //requisicaoWeb.Method = "POST";
            //requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.UserAgent = "Req_Versatil";
            requisicaoWeb.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                string Json = "";
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
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na Consulta de Dados - WebRequest", Resposta, "Tray");
                    }
                }

                return "Erro";
            }
        }

        //Busca os dados do Produto pelo Filtro Referencia onde enviamos o codigo do produto
        public static string BuscaListaVariacoes(int Page, string ReferenciaProduto)
        {
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("access_token", DadosConfiguracao.Config.AuthTray.access_token);
            queryParameters.Add("limit", "50");
            queryParameters.Add("page", Page.ToString());
            queryParameters.Add("reference", ReferenciaProduto);

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }

            string postString = String.Join("&", items.ToArray());
            //var ArquivoEnvio = Encoding.UTF8.GetBytes(postString); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp(URL + "/web_api/products/variants" + "?" + postString);
            requisicaoWeb.Timeout = 600000;
            //requisicaoWeb.Method = "POST";
            //requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.UserAgent = "Req_Versatil";
            requisicaoWeb.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                string Json = "";
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
                        DAOLogDB.SalvarLogs("", "Variações - Erro na Consulta de Dados - WebRequest", Resposta, "Tray");
                    }
                }

                return "Erro";
            }
        }





        //Envia os Produtos para o Ecommerce
        public static string EnviaProdutos(TrayProduct Product)
        {
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("access_token", DadosConfiguracao.Config.AuthTray.access_token);

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }

            string argsString = String.Join("&", items.ToArray());

            object JsonEnvio = new
            {
                Product
            };

            string ArquivoJson = JsonConvert.SerializeObject(JsonEnvio); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp(URL + "/web_api/products?" + argsString);


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
                        DAOLogDB.SalvarLogs("", "Produtos - Erro no Insert do Produto " + Product.reference + "- WebRequest", Resposta + " - JSON: " + ArquivoJson, "Tray");
                        return "Erro";
                    }
                }
            }
        }

        //Atualiza os Produtos do Ecommerce
        public static string AtualizaProdutos(TrayProduct Product)
        {
            
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("access_token", DadosConfiguracao.Config.AuthTray.access_token);
            List<string> items = new List<string>();
            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }
            string argsString = String.Join("&", items.ToArray());
            string Url = URL + "/web_api/products/" + Product.id + "?" + argsString;

            object JsonEnvio = new
            {
                Product
            };

            string ArquivoJson = JsonConvert.SerializeObject(JsonEnvio); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "PUT";
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
                        DAOLogDB.SalvarLogs("", "Produtos - Erro no Update do Produto " + Product.reference + "- WebRequest", Resposta + " - JSON: " + ArquivoJson, "Tray");
                        return "Erro";
                    }
                }
            }
        }

        


        //Envia as Variações para o Ecommerce
        public static string EnviaVariacoes(TrayVariant Variant)
        {
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("access_token", DadosConfiguracao.Config.AuthTray.access_token);

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }

            string argsString = String.Join("&", items.ToArray());

            object JsonEnvio = new
            {
                Variant
            };

            string ArquivoJson = JsonConvert.SerializeObject(JsonEnvio); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson.Replace(",\"type_1\":\"\",\"value_1\":\"\"", "").Replace(",\"type_2\":\"\",\"value_2\":\"\"", "")); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp(URL + "/web_api/products/variants?" + argsString);
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
                        DAOLogDB.SalvarLogs("", "Variação - Erro no Insert da Variação - Código Produto: " + Variant.reference + "- WebRequest", Resposta + " - JSON: " + ArquivoJson, "Tray");
                        return "Erro";
                    }
                }
            }
        }

        //Envia as Variações para o Ecommerce
        public static string AtualizaVariacoes(TrayVariant Variant)
        {
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("access_token", DadosConfiguracao.Config.AuthTray.access_token);

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }

            string argsString = String.Join("&", items.ToArray());

            object JsonEnvio = new
            {
                Variant
            };
            //,"type_2":"","value_2":""
            string ArquivoJson = JsonConvert.SerializeObject(JsonEnvio); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson.Replace(",\"type_1\":\"\",\"value_1\":\"\"", "").Replace(",\"type_2\":\"\",\"value_2\":\"\"", "")); //Converte o arquivo para Byte


            var requisicaoWeb = WebRequest.CreateHttp(URL + "/web_api/products/variants/" + Variant.id + "?" + argsString);
            requisicaoWeb.Timeout = 600000;
            requisicaoWeb.Method = "PUT";
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
                        DAOLogDB.SalvarLogs("", "Variação - Erro no Update da Variação - Código Produto: " + Variant.reference + "- WebRequest", Resposta + " - JSON: " + ArquivoJson, "Tray");
                        return "Erro";
                    }
                }
            }
        }




        //Busca os Pedidos do Ecommerce
        public static string BuscarPedidos(int Page, string Data)
        {
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("access_token", DadosConfiguracao.Config.AuthTray.access_token);
            queryParameters.Add("limit", "50");
            queryParameters.Add("page", Page.ToString());
            queryParameters.Add("date", Data);

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }

            string postString = String.Join("&", items.ToArray());
            //var ArquivoEnvio = Encoding.UTF8.GetBytes(postString); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp(URL + "/web_api/orders" + "?" + postString);
            requisicaoWeb.Timeout = 600000;
            //requisicaoWeb.Method = "POST";
            //requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.UserAgent = "Req_Versatil";
            requisicaoWeb.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                string Json = "";
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
                        DAOLogDB.SalvarLogs("", "Pedidos - Erro na Consulta de Pedidos - WebRequest", Resposta, "Tray");
                    }
                }

                return "Erro";
            }
        }

        //Busca os Dados do Pedido
        public static string BuscarDadosdoPedidos(string id_pedido)
        {
            NameValueCollection queryParameters = new NameValueCollection();
            queryParameters.Add("access_token", DadosConfiguracao.Config.AuthTray.access_token);

            List<string> items = new List<string>();

            foreach (String name in queryParameters)
            {
                items.Add(String.Concat(name, "=", queryParameters[name]));
            }

            string postString = String.Join("&", items.ToArray());

            var requisicaoWeb = WebRequest.CreateHttp(URL + "/web_api/orders/" + id_pedido + "/complete?" + postString);
            requisicaoWeb.Timeout = 600000;
            //requisicaoWeb.Method = "POST";
            //requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.UserAgent = "Req_Versatil";
            requisicaoWeb.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                string Json = "";
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
                        DAOLogDB.SalvarLogs("", "Pedido - Erro na Consulta de Dados do Pedidos - WebRequest", Resposta, "Tray");
                    }
                }

                return "Erro";
            }
        }


    }
}
