using IntegracaoRockye.Rocky.Model;
using IntegracaoRockye.Rocky.Model.List;
using IntegracaoRockye.Rocky.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Model;
using IntegracaoRockye.Versatil.Models;
using IntegracaoRockye.Versatil.Funcoes;
using IntegracaoRockye.Rocky.V2Model;

namespace IntegracaoRockye.Rocky
{

    public static class RKConectionWebService
    {
        //Temporario
        public static void DeletarProdutos(string P)
        {
            string Url = "https://api.plataformarocky.com.br/products/" + P;
            var requisicaoWeb = HttpWebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "DELETE";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";


            WebResponse response = requisicaoWeb.GetResponse();
            response.Close();
            requisicaoWeb.Abort();
        }

        ///////////////////////////////  Busca Produtos Api //////////////////////////////////////////////////
        public static List<RKProdutos> BuscarProdutos()
        {
            string Url = "https://api.plataformarocky.com.br/products/allResults";
            string Json = "";
            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
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
                //string ref 
                if (Json != "null" && Json.Length > 0)
                {
                    requisicaoWeb.Abort();
                    var ListadeProdutos = JsonConvert.DeserializeObject<RKProdutosLista>((Json.Replace("\"ref\"", "\"referencia\"")));
                    return ListadeProdutos.products;
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
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na Busca de Produtos - WebRequest", Resposta, "Site");
                        return new List<RKProdutos>();
                    }
                }
            }

            return new List<RKProdutos>();
        }

        //////////////////////////////  Atualiza Produtos Ecommerce //////////////////////////////////////////
        public static string AtualizarProdutos(RKProdutos product)
        {
            string Url = "https://api.plataformarocky.com.br/products/update/" + product.id;

            var Json = new
            {
                product
            };

            string ArquivoJson = (JsonConvert.SerializeObject(Json)).Replace("[", "").Replace("]", "").Replace(",\"referencia\":\"\"", null);
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson);

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "PUT";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            using (var stream = requisicaoWeb.GetRequestStream())
            {
                stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                stream.Close();
            }

            try
            {
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string Resposta = streamReader.ReadToEnd();
                    requisicaoWeb.Abort();
                    return Resposta;
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
                        DAOLogDB.SalvarLogs("", "Produtos - Erro no Atualizar o Produto - WebRequest", Resposta, "Site");
                        return Resposta;
                    }
                }
            }
        }

        //////////////////////////////  Envia Produtos Ecommerce /////////////////////////////////////////////
        public static string EnviarProdutos(RKProdutos product)
        {
            string Url = "https://api.plataformarocky.com.br/products";

            var Json = new
            {
                product
            };

            string ArquivoJson = (JsonConvert.SerializeObject(Json)).Replace("[", "").Replace("]", "").Replace("referencia", "ref");
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson);

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            using (var stream = requisicaoWeb.GetRequestStream())
            {
                stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                stream.Close();
            }

            try
            {
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string Resposta = streamReader.ReadToEnd();
                    requisicaoWeb.Abort();
                    return Resposta;
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

                        DAOLogDB.SalvarLogs("", "Produtos - Erro no Envio de Produtos - WebRequest", Resposta + " - Produto " + product.referencia, "Site");

                        return Resposta;
                    }
                }
            }
        }

        /////////////////////////////  Busca os Pedidos do  Ecommerce  ///////////////////////////////////////
        public static List<RKPedidos> ReceberPedidosLista()
        {
            DateTime DataInicial = DateTime.Now.Date.AddDays(-4);

            string Url = "https://api.plataformarocky.com.br/orders/filter/" + DataInicial.ToString("dd-MM-yyy") + "/" + DateTime.Now.Date.AddDays(1).ToString("dd-MM-yyy");
            string Json = "";
            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
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

                if (Json != "null" && Json.Length > 0)
                {
                    requisicaoWeb.Abort();
                    var ListadePedidos = JsonConvert.DeserializeObject<RKPedidosLista>(Json);
                    return ListadePedidos.orders;
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

                        DAOLogDB.SalvarLogs("", "Pedidos - Erro na Busca de Pedidos - WebRequest", Resposta, "Site");
                        return new List<RKPedidos>();
                    }
                }
            }

            return new List<RKPedidos>();
        }

        /////////////////////////////  Busca os Dados do Cliente PF do Ecommerce  /////////////////////////////
        public static RKPessoaFisica ReceberClientePF(string CodigoCliente)
        {
            string Url = "https://api.plataformarocky.com.br/clients/" + CodigoCliente;
            string Json = "";
            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
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

                if (Json != "null" && Json.Length > 0)
                {
                    requisicaoWeb.Abort();
                    var Cliente = JsonConvert.DeserializeObject<RKClientePFLista>(Json);
                    return Cliente.client;
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
                        DAOLogDB.SalvarLogs("", "Cliente - Erro na Busca de Dados do Cliente - WebRequest", Resposta, "Site");
                    }
                }
            }

            return new RKPessoaFisica();
        }

        /////////////////////////////  Busca os Dados do Cliente PJ do Ecommerce  /////////////////////////////
        public static RKPessoaJuridica ReceberClientePJ(string CodigoCliente)
        {
            string Url = "https://api.plataformarocky.com.br/companies/" + CodigoCliente;
            string Json = "";
            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
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

                if (Json != "null" && Json.Length > 0)
                {
                    requisicaoWeb.Abort();
                    var ClientePJ = JsonConvert.DeserializeObject<RKClientePJLista>(Json);
                    return ClientePJ.company;
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
                        DAOLogDB.SalvarLogs("", "Cliente - Erro na Busca de Dados do Cliente - WebRequest", Resposta, "Site");
                    }
                }
            }

            return new RKPessoaJuridica();
        }

        /////////////////////////////////  Recebe a Lista de SKU  /////////////////////////////////////////////
        public static List<RKSku> ReceberProdutos_Variacoes()
        {
            string Url = "https://api.plataformarocky.com.br/products/variations/allResults";
            string Json = "";
            var requisicaoWeb2 = WebRequest.CreateHttp(Url);
            requisicaoWeb2.Method = "GET";
            requisicaoWeb2.ContentType = "application/json";
            requisicaoWeb2.Accept = "application/json";
            requisicaoWeb2.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb2.UserAgent = "RequisicaoWebDemo";

            try
            {
                using (var resposta = requisicaoWeb2.GetResponse())
                {
                    var streamDados = resposta.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados, Encoding.Default);
                    Json = reader.ReadToEnd();
                    streamDados.Close();
                    resposta.Close();
                }

                if (Json != "null" && Json.Length > 0)
                {
                    requisicaoWeb2.Abort();
                    var ListadeVariacoes = JsonConvert.DeserializeObject<RKProdutos_VariacoesLista>(Json.Replace("\"ref\"", "\"referencia\""));
                    return ListadeVariacoes.product_variations;
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
                        requisicaoWeb2.Abort();
                        DAOLogDB.SalvarLogs("", "Sku - Erro na Busca da Lista de Sku - WebRequest", Resposta, "Site");
                    }
                }
            }
            return new List<RKSku>();
        }

        //////////////////////////////    Envia as Variações SKU  ////////////////////////////////////////////
        public static string EnviarProdutos_Variacoes(RKSku product_variation)
        {
            string Url = "https://api.plataformarocky.com.br/products/" + product_variation.id_produto + "/variations";

            var Json = new
            {
                product_variation
            };

            string ArquivoJson = (JsonConvert.SerializeObject(Json)).Replace("[", "").Replace("]", "");
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson);

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            //Envia os dados POST
            Stream dataStream = requisicaoWeb.GetRequestStream();
            dataStream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
            dataStream.Close();

            try
            {
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string Resposta = streamReader.ReadToEnd();
                    requisicaoWeb.Abort();
                    return Resposta;
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
                        DAOLogDB.SalvarLogs("", "Sku - Erro no envio do Sku - Referência: " + product_variation.referencia + " - ID: " + product_variation.id_produto + " - WebRequest", Resposta, "Site");
                        return Resposta;
                    }
                }
            }
        }

        ////////////////////////////   Atualiza as Variações SKU   //////////////////////////////////////////
        public static string AtualizarProdutos_Variacoes(RKSku product_variation)
        {
            string Url = "https://api.plataformarocky.com.br/products/" + product_variation.id_produto + "/variations/" + product_variation.id;

            var Json = new
            {
                product_variation
            };

            string ArquivoJson = (JsonConvert.SerializeObject(Json)).Replace("[", "").Replace("]", "");
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson);

            var requisicaoWeb1 = WebRequest.CreateHttp(Url);
            requisicaoWeb1.Method = "PUT";
            requisicaoWeb1.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb1.ContentType = "application/json";
            requisicaoWeb1.Accept = "application/json";
            requisicaoWeb1.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb1.UserAgent = "RequisicaoWebDemo";

            //Envia os dados POST
            Stream dataStream = requisicaoWeb1.GetRequestStream();
            dataStream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
            dataStream.Close();

            try
            {
                var httpResponse = requisicaoWeb1.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string Resposta = streamReader.ReadToEnd();
                    requisicaoWeb1.Abort();
                    return Resposta;
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
                        requisicaoWeb1.Abort();
                        DAOLogDB.SalvarLogs("", "Sku - Erro na atualização do Sku - Referência: " + product_variation.referencia + " - ID: " + product_variation.id_produto + " - WebRequest", Resposta, "Site");
                        return Resposta;
                    }
                }
            }
        }




        ////////////////////////////////////       Versão 2.0      ///////////////////////////////////////////
        ///
        public static RetornoWebService EnviarProdutosV2(RKProdutos product)
        {
            var Retorno = new RetornoWebService();
            string Url = "https://api.plataformarocky.com.br/products";

            var Json = new
            {
                product
            };

            string ArquivoJson = (JsonConvert.SerializeObject(Json)).Replace("[", "").Replace("]", "").Replace("referencia", "ref");
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson);

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            using (var stream = requisicaoWeb.GetRequestStream())
            {
                stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                stream.Close();
            }

            try
            {
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Retorno.Response = streamReader.ReadToEnd();
                    Retorno.Code = "200";
                    requisicaoWeb.Abort();
                    return Retorno;
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

                        DAOLogDB.SalvarLogs("", "Produtos - Erro no Envio de Produtos - WebRequest", Resposta + " - Produto " + product.referencia + " JSON: " + ArquivoJson, "Site");
                        Retorno.Response = Resposta;
                        Retorno.Code = "Error";

                        return Retorno;
                    }
                }
            }
        }

        public static RetornoWebService UpdateProdutosV2(RKProdutosUpdate product)
        {
            var Retorno = new RetornoWebService();
            string Url = "https://api.plataformarocky.com.br/products/" + product.id;

            var Json = new
            {
                product
            };

            string ArquivoJson = (JsonConvert.SerializeObject(Json)).Replace("[", "").Replace("]", "").Replace("referencia", "ref");
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson);

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "PUT";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            using (var stream = requisicaoWeb.GetRequestStream())
            {
                stream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
                stream.Close();
            }

            try
            {
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Retorno.Response = streamReader.ReadToEnd();
                    Retorno.Code = "200";
                    requisicaoWeb.Abort();
                    return Retorno;
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

                        DAOLogDB.SalvarLogs("", "Produtos - Erro no Update de Produtos - WebRequest", Resposta + " - Produto " + product.id + " JSON: " + ArquivoJson, "Site");
                        Retorno.Response = Resposta;
                        Retorno.Code = "Error";

                        return Retorno;
                    }
                }
            }
        } 

        public static RetornoWebService EnviarProdutos_VariacoesV2(RKProduct_variationInsertV2 product_variation)
        {
            var Retorno = new RetornoWebService();
            string Url = "https://api.plataformarocky.com.br/products/" + product_variation.id_produto + "/variations";

            var Json = new
            {
                product_variation
            };

            string ArquivoJson = (JsonConvert.SerializeObject(Json));
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson);

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            //Envia os dados POST
            Stream dataStream = requisicaoWeb.GetRequestStream();
            dataStream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
            dataStream.Close();

            try
            {
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Retorno.Response = streamReader.ReadToEnd();
                    Retorno.Code = "200";
                    requisicaoWeb.Abort();
                    return Retorno;
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
                        Retorno.Response = reader.ReadToEnd();
                        Retorno.Code = "Error";
                        reader.Close();
                        data.Close();
                        httpResponse.Close();
                        requisicaoWeb.Abort();
                        DAOLogDB.SalvarLogs("", "Sku - Erro no envio do Sku - Referência: " + product_variation.product_ref[0].referencia + " - ID: " + product_variation.id_produto + " - WebRequest", Retorno.Response + " JSON: " + ArquivoJson, "Site");
                        return Retorno;
                    }
                }
            }
        }

        public static RetornoWebService UpdateProdutos_VariacoesV2(RKProduct_variationV2 product_variation)
        {
            var Retorno = new RetornoWebService();
            string Url = "https://api.plataformarocky.com.br/products/" + product_variation.id_produto + "/variations/" + product_variation.id;

            var Json = new
            {
                product_variation
            };

            string ArquivoJson = (JsonConvert.SerializeObject(Json));
            var ArquivoEnvio = Encoding.UTF8.GetBytes(ArquivoJson);

            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "PUT";
            requisicaoWeb.ContentLength = ArquivoEnvio.Length;
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            //Envia os dados POST
            Stream dataStream = requisicaoWeb.GetRequestStream();
            dataStream.Write(ArquivoEnvio, 0, ArquivoEnvio.Length);
            dataStream.Close();

            try
            {
                var httpResponse = requisicaoWeb.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Retorno.Response = streamReader.ReadToEnd();
                    Retorno.Code = "200";
                    requisicaoWeb.Abort();
                    return Retorno;
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
                        Retorno.Response = reader.ReadToEnd();
                        Retorno.Code = "Error";
                        reader.Close();
                        data.Close();
                        httpResponse.Close();
                        requisicaoWeb.Abort();
                        DAOLogDB.SalvarLogs("", "Sku - Erro no update do Sku - Referência: " + product_variation.product_ref[0].referencia + " - ID: " + product_variation.id_produto + " - WebRequest", Retorno.Response + " JSON: " + ArquivoJson, "Site");
                        return Retorno;
                    }
                }
            }
        }


        public static List<RKPedidos> ReceberPedidosListaV2()
        {
            DateTime DataInicial = DateTime.Now.Date.AddDays(-4);

            string Url = "https://api.plataformarocky.com.br/orders/filter/" + DataInicial.ToString("dd-MM-yyy") + "/" + DateTime.Now.Date.AddDays(1).ToString("dd-MM-yyy");
            string Json = "";
            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
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

                if (Json != "null" && Json.Length > 0)
                {
                    requisicaoWeb.Abort();
                    var ListadePedidos = JsonConvert.DeserializeObject<RKPedidosLista>(Json);
                    return ListadePedidos.orders;
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

                        DAOLogDB.SalvarLogs("", "Pedidos - Erro na Busca de Pedidos - WebRequest", Resposta, "Site");
                        return new List<RKPedidos>();
                    }
                }
            }

            return new List<RKPedidos>();
        }

        //Busca os dados do produto pela refrencia 
        public static string GetProdutoV2(string _Referencia)
        {
            string Url = "https://api.plataformarocky.com.br/products/ref/" + _Referencia;
            string Json = "";
            var requisicaoWeb = WebRequest.CreateHttp(Url);
            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.Accept = "application/json";
            requisicaoWeb.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
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
                //string ref 
                if (Json != "null" && Json.Length > 0)
                {
                    requisicaoWeb.Abort();
                    return Json.Replace("\"ref\"", "\"referencia\"");
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
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na Busca de Produtos - WebRequest", Resposta + " JSON: " + Json, "Site");
                        return "Erro";
                    }
                }
            }

            return "Erro";
        }

        //Busca todas as variações
        public static List<RKSku> GetVariacoesAllV2()
        {
            string Url = "https://api.plataformarocky.com.br/products/variations/allResults";
            string Json = "";
            var requisicaoWeb2 = WebRequest.CreateHttp(Url);
            requisicaoWeb2.Method = "GET";
            requisicaoWeb2.ContentType = "application/json";
            requisicaoWeb2.Accept = "application/json";
            requisicaoWeb2.Headers.Add("Authorization", "Bearer " + DadosConfiguracao.Config.TokenRocky);
            requisicaoWeb2.UserAgent = "RequisicaoWebDemo";

            try
            {
                using (var resposta = requisicaoWeb2.GetResponse())
                {
                    var streamDados = resposta.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados, Encoding.Default);
                    Json = reader.ReadToEnd();
                    streamDados.Close();
                    resposta.Close();
                }

                if (Json != "null" && Json.Length > 0)
                {
                    requisicaoWeb2.Abort();
                    var ListadeVariacoes = JsonConvert.DeserializeObject<RKProdutos_VariacoesLista>(Json.Replace("\"ref\"", "\"referencia\""));
                    return ListadeVariacoes.product_variations;
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
                        requisicaoWeb2.Abort();
                        DAOLogDB.SalvarLogs("", "Sku - Erro na Busca da Lista de Sku - WebRequest", Resposta + " JSON: " + Json, "Site");
                    }
                }
            }
            return new List<RKSku>();
        }
    }
}