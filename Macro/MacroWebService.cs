using IntegracaoRockye.Macro.Models;
using IntegracaoRockye.Macro.Models.Listar;
using IntegracaoRockye.Versatil.Funcoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace IntegracaoRockye.Macro
{
    public static class MacroWebService
    {
        //Busca a lista de Usuarios
        public static string getUsuarios()
        {
            var Listar = new MacroListar();
            Listar.data = DateTime.Now.AddDays(-4).ToString();

            string ArquivoJson = JsonConvert.SerializeObject(Listar); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.emacro.com.br/usuarios/listar");
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
                        DAOLogDB.SalvarLogs("", "Usuários - Erro na consulta de usuários - WebRequest", Resposta, "Macro");
                    }
                }

                return "Erro";
            }
        }

        //Busca os Dados do Usuario
        public static string getDadosUsuario(string _Id)
        {
            var Listar = new MacroDadosCliente();
            Listar.id = _Id;

            string ArquivoJson = JsonConvert.SerializeObject(Listar); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.emacro.com.br/usuarios/listar");
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
                        DAOLogDB.SalvarLogs("", "Usuários - Erro na consulta de usuários - WebRequest", Resposta, "Macro");
                    }
                }

                return "Erro";
            }
        }

        //Envia os produtos
        public static string postProdutos(List<MacroProdutos> Produtos)
        {
            var dado = new MacroEnviarDados();
            dado.dados = Produtos;

            string ArquivoJson = JsonConvert.SerializeObject(dado); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.emacro.com.br/produtos/criar");
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

                return "ok";
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
                        DAOLogDB.SalvarLogs("", "Produtos - Erro no Envio dos Produtos - WebRequest", Resposta, "Macro");
                    }
                }

                return "erro";
            }
        }

        //Envia as Variações
        public static void postVariacoes(List<MacroVariacoes> Variacoes)
        {
            var dado = new MacroEnviarDados();
            dado.dados = Variacoes;

            string ArquivoJson = JsonConvert.SerializeObject(dado); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.emacro.com.br/variacoes/criar");
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
                        DAOLogDB.SalvarLogs("", "Variações - Erro no envio de variações - WebRequest", Resposta, "Macro");
                    }
                }
            }
        }

        //Envia os Grupos
        public static void postGrupos(List<MacroGrupos> Grupos)
        {
            var dado = new MacroEnviarDados();
            dado.dados = Grupos;

            string ArquivoJson = JsonConvert.SerializeObject(dado); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.emacro.com.br/grupos/criar");
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
                        DAOLogDB.SalvarLogs("", "Grupos - Erro no envio dos Grupos - WebRequest", Resposta, "Macro");
                    }
                }
            }
        }

        //Busca a lista de Pedidos
        public static string getPedidos()
        {
            var Listar = new MacroListar();
            Listar.data = DateTime.Now.AddDays(-7).ToString("yyy-MM-dd H:mm:ss");

            string ArquivoJson = JsonConvert.SerializeObject(Listar); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.emacro.com.br/pedidos/listar");
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
                        DAOLogDB.SalvarLogs("", "Pedidos - Erro na consulta de pedidos - WebRequest", Resposta + "  Date - " + Listar.data, "Macro");
                    }
                }

                return "Erro";
            }
        }

        //Envia o Estoque para o Ecommerce
        public static void EnviarEstoque(List<MacroEstoque> Estoque)
        {
            var dado = new MacroEnviarDados();
            dado.dados = Estoque;

            string ArquivoJson = JsonConvert.SerializeObject(dado); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("https://api.emacro.com.br/estoques/editar");
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
                        DAOLogDB.SalvarLogs("", "Estoque - Erro na atualização do estoque - WebRequest", Resposta, "Macro");
                    }
                }
            }
        }


        //Envia os Preços para o Ecommerce
        public static void AtualizaPreços(List<MacroAtualizarPreco> Precos)
        {
            var dado = new MacroEnviarDados();
            dado.dados = Precos;

            string ArquivoJson = JsonConvert.SerializeObject(dado); //Serealiza a lista de Colaboradores
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson); //Converte o arquivo para Byte

            var requisicaoWeb = WebRequest.CreateHttp("http://api.emacro.com.br/precos/criar");
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
                        DAOLogDB.SalvarLogs("", "Preços - Erro na atualização de preços - WebRequest", Resposta, "Macro");
                    }
                }
            }
        }

    }
}
