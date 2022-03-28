using IntegracaoRockye.App.APModels;
using IntegracaoRockye.Macro.Models;
using IntegracaoRockye.Rocky.Model;
using IntegracaoRockye.Tray.Models;
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
    public static class DAOProdutos
    {
        public static decimal ConverterDecimal(string Valor)
        {
            try
            {
                return Convert.ToDecimal(Valor);
            }
            catch
            {
                return 0;
            }
        }
        public static DateTime ConverterDateTime(string Valor)
        {
            try
            {
                return Convert.ToDateTime(Valor);
            }
            catch
            {
                return DateTime.Now; ;
            }
        }


        //Busca Produtos para Enviar para o APP VERSATIL
        public static List<VerProdutos> BuscaProdutos()
        {
            List<VerProdutos> ListaProdutos = new List<VerProdutos>();

            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            MySqlCommand set = new MySqlCommand("set net_write_timeout=999999; set net_read_timeout=999999", DBMySql); // Setting tiimeout on mysqlServer
            set.ExecuteNonQuery();
            int numOfRecordsUpdated = set.ExecuteNonQuery();
            DBConnectionMySql.FechaConexaoBD(DBMySql);



            string Sql = "select @'rownum':= @'rownum' + 1 as n, p.codigo, p.tipo, p.situacao, p.descricao, p.valormercadoria, p.praticado, p.referencia, p.unidade, p.codigoean, p.praticado2, p.praticado3, p.estoquedisponivel, p.descontomaximo, m.marca, g.grupo, cr.cor, t.tamanho, p.observacoes, p.especificacoestecnicas, p.aplicacao, p.customercadoria, p.custoreal from produtoseservicos p LEFT JOIN marcas m ON m.codigo = p.codigomarca LEFT JOIN grupos g on g.codigo = p.codigogrupo LEFT JOIN cores cr ON cr.codigo = p.codigocor LEFT JOIN tamanhos t ON t.codigo = p.codigotamanho, (SELECT @'rownum':= 0) r where p.tipo = 'Produto' GROUP BY codigo;";
            MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                VerProdutos Produto = new VerProdutos();

                Produto.Codigo = dr["codigo"].ToString();
                Produto.Tipo = dr["tipo"].ToString();
                Produto.Situacao = dr["situacao"].ToString();
                Produto.Descricao = dr["descricao"].ToString();
                Produto.Valormercadoria = ConverterDecimal(dr["valormercadoria"].ToString());
                Produto.Praticado = ConverterDecimal(dr["praticado"].ToString());
                Produto.Referencia = dr["referencia"].ToString();
                Produto.Unidade = dr["unidade"].ToString();
                Produto.Codigoean = dr["codigoean"].ToString();
                Produto.Praticado2 = ConverterDecimal(dr["praticado2"].ToString());
                Produto.Praticado3 = ConverterDecimal(dr["praticado3"].ToString());
                Produto.Estoquedisponivel = ConverterDecimal(dr["estoquedisponivel"].ToString());
                Produto.Descontomaximo = ConverterDecimal(dr["descontomaximo"].ToString());
                Produto.Grupo = dr["grupo"].ToString();
                Produto.Marca = dr["marca"].ToString();
                Produto.Observacoes = dr["observacoes"].ToString();
                Produto.Tamanho = dr["tamanho"].ToString();
                Produto.Cor = dr["cor"].ToString();
                Produto.Especificacoestecnicas = dr["especificacoestecnicas"].ToString();
                Produto.Aplicacao = dr["aplicacao"].ToString();
                Produto.Customercadoria = ConverterDecimal(dr["customercadoria"].ToString());
                Produto.Custorealmercadoria = ConverterDecimal(dr["custoreal"].ToString());
                Produto.DataImagem = ConverterDateTime("2020-01-01"); //Desativado

                Produto.N = Convert.ToInt32(dr["n"].ToString());

                ListaProdutos.Add(Produto);
            }
            dr.Close();
            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return ListaProdutos;
        }

        //Busca os Produtos para Enviar para o Site ROCKY
        public static List<RKProdutos> GetProdutosCadastro()
        {
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

            try
            {
                List<RKProdutos> ListaProdutos = new List<RKProdutos>();

                string Query = "select p.codigo, p.referencia, p.descricao, p.nomecomercial, p.praticado, p.pesobruto, p.quantidadeembalagem, p.largura, p.altura, p.comprimento, m.codigoadicional as codigomarca, p.estoquedisponivel from produtoseservicos p inner join marcas m on m.codigo = p.codigomarca where p.tipo = 'Produto' and p.situacao = 'Ativo' and m.codigoadicional is not null and p.site = '1'";
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                {
                    RKProdutos Produto = new RKProdutos();

                    Produto.id = Reader["codigo"].ToString();

                    if (!string.IsNullOrEmpty(Reader["nomecomercial"].ToString()))
                    {
                        Produto.nome = Reader["nomecomercial"].ToString();
                    }
                    else
                    {
                        Produto.nome = Reader["descricao"].ToString();
                    }

                    Produto.referencia = Produto.id;
                    //Produto.descricao = "<p>" + Produto.nome + "</p>";
                    Produto.preco_base = ConverterDecimal(Reader["praticado"].ToString());
                    Produto.preco = Produto.preco_base;
                    Produto.quantidade = ConverterDecimal(Reader["estoquedisponivel"].ToString());
                    Produto.preco_promocional = 0;
                    Produto.peso = ConverterDecimal(Reader["pesobruto"].ToString()) * 1000;
                    Produto.comprimento = ConverterDecimal(Reader["comprimento"].ToString()).ToString();
                    Produto.altura = ConverterDecimal(Reader["altura"].ToString()).ToString();
                    Produto.largura = ConverterDecimal(Reader["largura"].ToString()).ToString();
                    Produto.multiplos = Reader["quantidadeembalagem"].ToString();
                    Produto.marca = Reader["codigomarca"].ToString();

                    ListaProdutos.Add(Produto);
                }

                Reader.Close();

                return ListaProdutos;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Site");
                return new List<RKProdutos>();
            }
            finally
            {
                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }
        }


        //Busca os Produtos para Enviar pro Ecommerce da MACRO
        public static List<MacroProdutos> GetProdutosMacro()
        {
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

            try
            {
                var ListaProdutos = new List<MacroProdutos>();

                string Query = "select p.codigo, p.referenciaecommerce, p.descricao, p.minimo, p.multiplicador, p.codigogrupo, p.codigosubgrupo, p.datapublicacao, p.dataexpiracao, p.lancamentoecommerce, p.nomecomercial, p.praticado, p.quantidadeembalagem, p.pesobruto, p.largura, p.altura, p.comprimento, m.codigoadicional as codigomarca, p.estoquedisponivel from produtoseservicos p inner join marcas m on m.codigo = p.codigomarca where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site = '1' and p.enviadoecommerce = '0'";
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                {
                    var Produto = new MacroProdutos();
                    
                    try
                    {
                        Produto.id = Reader["codigo"].ToString();
                        Produto.CodigoSistema = Reader["codigo"].ToString();
                        //Produto.id_novo = "";

                        if (!string.IsNullOrEmpty(Reader["nomecomercial"].ToString()))
                        { Produto.descricao_curta = Reader["nomecomercial"].ToString(); }
                        else { Produto.descricao_curta = Reader["descricao"].ToString(); }

                        Produto.referencia = Reader["referenciaecommerce"].ToString();


                        var GruposAd = GruposAdicionais(Produto.id);
                        int TamanhoArray = GruposAd.Count + 2;
                        Produto.grupos = new string[TamanhoArray];

                        Produto.grupos[0] = Reader["codigogrupo"].ToString();
                        Produto.grupos[1] = Reader["codigosubgrupo"].ToString();

                        int i = 2;
                        foreach (var g in GruposAd)
                        {
                            Produto.grupos[i] = g.id;
                            i++;
                        }

                        try { Produto.multiplicador = Convert.ToInt32(Reader["multiplicador"].ToString()); }
                        catch { Produto.multiplicador = 1; }

                        try { Produto.minimo = Convert.ToInt32(Reader["minimo"].ToString()); }
                        catch { Produto.minimo = 1; }

                        Produto.lancamento = Convert.ToInt32(Convert.ToBoolean(Reader["lancamentoecommerce"].ToString()));

                        Produto.publicacao = Convert.ToDateTime(Reader["datapublicacao"].ToString());
                        Produto.expiracao = Convert.ToDateTime(Reader["dataexpiracao"].ToString());

                        ListaProdutos.Add(Produto);
                    }
                    catch (Exception ex) 
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos - Produto: " + Produto.id, ex.Message, "Macro");
                    }
                }

                Reader.Close();

                return ListaProdutos;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Macro");
                return new List<MacroProdutos>();
            }
            finally
            {
                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }



            ///Carrega os Grupos Adicionais
            List<MacroGrupos> GruposAdicionais(string _codProduto)
            {
                MySqlConnection DBMySql1 = new MySqlConnection(DBConnectionMySql.strConnection);

                try
                {
                    var Grupos = new List<MacroGrupos>();  
                    string Query1 = "SELECT e.codigogrupo FROM ecommercegruposprodutos e INNER JOIN grupos g ON g.codigo = e.codigogrupo WHERE g.ecommerce = '1' AND e.codigoproduto = '" + _codProduto + "'";
                    MySqlCommand Comando1 = new MySqlCommand(Query1, DBMySql1);
                    DBConnectionMySql.AbreConexaoBD(DBMySql1);
                    MySqlDataReader Reader1 = Comando1.ExecuteReader();

                    while (Reader1.Read())
                    {
                        var G = new MacroGrupos();
                        G.id = Reader1["codigogrupo"].ToString();
                        Grupos.Add(G);
                    }

                    return Grupos;

                }
                catch (Exception ex)
                {
                    DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de Grupos Adicionais", ex.Message, "Macro");
                    return new List<MacroGrupos>();
                }
                finally
                {
                    DBConnectionMySql.FechaConexaoBD(DBMySql1);
                }

            }
        }


        //Busca os Produtos para Enviar pro Ecommerce da TRAY
        public static List<TrayProduct> GetProdutosTray()
        {
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

            try
            {
                var ListaProdutos = new List<TrayProduct>();

                string Query = "select p.codigo, p.codigoecommerce, p.referenciaecommerce, p.descricao, p.nomecomercial, m.marca, p.praticado, p.quantidadeembalagem, p.pesobruto, p.largura, p.altura, p.comprimento, m.codigoadicional as codigomarca, p.estoquedisponivel, co.complemento, p.situacaoecommerce, p.lancamentoecommerce, p.datapublicacao from produtoseservicos p left join marcas m on m.codigo = p.codigomarca left join complementodecadastros co on co.codigo = p.familia where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site = '1'";
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                while (Reader.Read())
                {
                    var Produto = new TrayProduct();

                    try
                    {
                        Produto.id = Reader["codigoecommerce"].ToString();

                        Produto.ReferenciaSistema = Reader["referenciaecommerce"].ToString(); //Somente para Controle Interno 

                        if (!string.IsNullOrEmpty(Reader["nomecomercial"].ToString()))
                        { Produto.name = Reader["nomecomercial"].ToString(); }
                        else { Produto.name = Reader["descricao"].ToString(); }

                        Produto.reference = Reader["codigo"].ToString();

                        Produto.price = Convert.ToDecimal(Reader["praticado"].ToString());
                        Produto.brand = Reader["marca"].ToString();
                        Produto.model = Reader["complemento"].ToString();

                        Produto.weight = Convert.ToInt32(Convert.ToDecimal(Reader["pesobruto"].ToString()) * 1000);
                        Produto.length = Convert.ToInt32(Reader["comprimento"].ToString());
                        Produto.width = Convert.ToInt32(Reader["largura"].ToString());
                        Produto.height = Convert.ToInt32(Reader["altura"].ToString());

                        try
                        {
                            // Produto.release_date = Convert.ToDateTime(Reader["datapublicacao"].ToString());
                        }
                        catch
                        {
                            // Produto.release_date = DateTime.Now.Date;
                        }

                        try
                        {
                            Produto.stock = Convert.ToInt32(Convert.ToDecimal(Reader["estoquedisponivel"].ToString()));
                        }
                        catch
                        {
                            Produto.stock = 0;
                        }

                        Produto.length = Convert.ToInt32(Reader["comprimento"].ToString());

                        try
                        {
                            Produto.available = Convert.ToInt32(Convert.ToBoolean(Reader["situacaoecommerce"].ToString())).ToString();
                        }
                        catch
                        {
                            Produto.available = "0";
                        }

                        try
                        {
                            Produto.release = Convert.ToInt32(Convert.ToBoolean(Reader["lancamentoecommerce"].ToString()));
                        }
                        catch
                        {
                            Produto.release = 0;
                        }


                        ListaProdutos.Add(Produto);
                    }
                    catch (Exception ex)
                    {
                        DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta do produto - " + Produto.reference, ex.Message, "Tray");
                    }
                }

                Reader.Close();

                return ListaProdutos;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Tray");
                return new List<TrayProduct>();
            }
            finally
            {
                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }
        }


        //Busca os Produtos para Atualizar o Estoque do Site ROCKY
        public static RKProdutos GetProdutosCadastroEstoque(string Codigo)
        {
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            RKProdutos Produto = new RKProdutos();
            try
            {
                string Query = "select p.codigo, p.referencia, p.descricao, p.nomecomercial, p.praticado, p.quantidadeembalagem, p.pesobruto, p.largura, p.altura, p.comprimento, m.codigoadicional as codigomarca, p.estoquedisponivel from produtoseservicos p inner join marcas m on m.codigo = p.codigomarca and p.codigo = '" + Codigo + "'";
                MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                MySqlDataReader Reader = Comando.ExecuteReader();

                if (Reader.Read())
                {
                    Produto.id = Reader["codigo"].ToString();

                    if (!string.IsNullOrEmpty(Reader["nomecomercial"].ToString()))
                    {
                        Produto.nome = Reader["nomecomercial"].ToString();
                    }
                    else
                    {
                        Produto.nome = Reader["descricao"].ToString();
                    }

                    Produto.referencia = Produto.id;
                    //Produto.descricao = "<p>" + Produto.nome + "</p>";
                    Produto.preco_base = ConverterDecimal(Reader["praticado"].ToString());
                    Produto.preco = Produto.preco_base;
                    Produto.quantidade = ConverterDecimal(Reader["estoquedisponivel"].ToString());
                    Produto.preco_promocional = 0;
                    Produto.peso = ConverterDecimal(Reader["pesobruto"].ToString()) * 1000;
                    Produto.comprimento = ConverterDecimal(Reader["comprimento"].ToString()).ToString();
                    Produto.altura = ConverterDecimal(Reader["altura"].ToString()).ToString();
                    Produto.largura = ConverterDecimal(Reader["largura"].ToString()).ToString();
                    Produto.multiplos = Reader["quantidadeembalagem"].ToString();
                    Produto.marca = Reader["codigomarca"].ToString();
                }

                Reader.Close();

                return Produto;
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Produtos - Erro na consulta de produtos", ex.Message, "Site");
                return Produto;
            }
            finally
            {
                DBConnectionMySql.FechaConexaoBD(DBMySql);
            }
        }

        //Busca os Dados que faltam do Produto para Adicionar o Item no Pedido GERAL
        public static VerProdutos GetDadosProdutosCadastro(string Codigo)
        {
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

            string Query = "select p.codigo, p.praticado, p.unidade, p.descricao, p.valormercadoria, p.customercadoria, p.referencia, co.cor, t.tamanho, p.custoreal, p.comissao from produtoseservicos p LEFT JOIN cores co ON co.codigo = p.codigocor LEFT JOIN tamanhos t ON t.codigo = p.codigotamanho where p.codigo = '" + Codigo + "'";
            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            MySqlDataReader Reader = Comando.ExecuteReader();

            if (Reader.Read())
            {
                VerProdutos Produto = new VerProdutos();

                Produto.Codigo = Reader["codigo"].ToString();
                Produto.Unidade = Reader["unidade"].ToString();
                Produto.Descricao = Reader["descricao"].ToString();
                Produto.Referencia = Reader["referencia"].ToString();
                Produto.Valormercadoria = ConverterDecimal(Reader["valormercadoria"].ToString());
                Produto.Cmvunitario = ConverterDecimal(Reader["customercadoria"].ToString());
                Produto.Praticado = ConverterDecimal(Reader["praticado"].ToString());
                Produto.Cor = Reader["cor"].ToString();
                Produto.Tamanho = Reader["tamanho"].ToString();
                Produto.CustoReal = ConverterDecimal(Reader["custoreal"].ToString());
                Produto.pComissao = ConverterDecimal(Reader["comissao"].ToString());

                Reader.Close();

                return Produto;
            }

            return new VerProdutos();
        }

        //Calcula o Estoque Disponivel por Empresa e Sala na Tabela Temporaria "tmpestoqueempresasapp" - APP
        public static void CalculaEstoqueDisponivel()
        {
            try
            {
                var EstoqueLista = new List<APEstoque>();

                string Sql = "select p.codigo from produtoseservicos P WHERE p.situacao = 'Ativo'";

                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
                DBConnectionMySql.AbreConexaoBD(DBMySql);
                cmd.CommandTimeout = 20000;
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    APEstoque Estoque = new APEstoque();
                    Estoque.CodigoProduto = dr["codigo"].ToString();

                    EstoqueLista.Add(Estoque);
                }
                dr.Close();
                DBConnectionMySql.FechaConexaoBD(DBMySql);


                foreach (var Produto in EstoqueLista)
                {

                    Sql = "" +
                          "SELECT pp.codigo, mm.empresa," +
                          "((SELECT IFNULL(SUM(m.qnt),0) AS Entradas FROM movimentoestoque m INNER join produtoseservicos p ON p.codigo = m.codigoproduto WHERE m.tipomovimentacao  = 'Entrada' AND m.codigoproduto = pp.codigo AND m.empresa = mm.empresa) " +
                          " - " +
                          "(SELECT IFNULL(SUM(m.qnt),0) AS Saidas FROM movimentoestoque m INNER join produtoseservicos p ON p.codigo = m.codigoproduto WHERE m.tipomovimentacao  = 'Saida' AND m.codigoproduto = pp.codigo AND m.empresa = mm.empresa) " +
                          " - " +
                          "(SELECT IFNULL(SUM(i.quantidade),0) FROM atendimentos a INNER JOIN itensatendimentos i ON i.atendimento = a.codigo WHERE (a.documento = 'Pedido' OR a.documento = 'Condicional' OR a.documento = 'Nota' OR a.documento = 'Garantia') AND a.status = 'Não Faturado' AND i.codigoproduto = pp.codigo AND a.empresa = mm.empresa)) " +
                          " AS Estoque " +
                          "FROM produtoseservicos pp INNER JOIN movimentoestoque mm ON mm.codigoproduto = pp.codigo where mm.codigoproduto = '" + Produto.CodigoProduto + "' and mm.empresa IS NOT NULL group by pp.codigo, mm.empresa";


                    cmd = new MySqlCommand(Sql, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    cmd.CommandTimeout = 0;
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        APEstoque Estoque = new APEstoque();

                        Estoque.CodigoProduto = dr["codigo"].ToString();
                        Estoque.CodigoVendedor = dr["empresa"].ToString();
                        Estoque.Estoque = ConverterDecimal(dr["Estoque"].ToString());
                        Atualiza(Estoque);
                    }
                    dr.Close();
                    DBConnectionMySql.FechaConexaoBD(DBMySql);
                }


                void Atualiza(APEstoque _Estoque)
                {
                    Sql = "replace into tmpestoqueempresasapp (produto, empresa, estoquedisponivel) values (@produto, @empresa, @estoquedisponivel)";
                    var DBMySqlI = new MySqlConnection(DBConnectionMySql.strConnection);
                    var cmdI = new MySqlCommand(Sql, DBMySqlI);

                    cmdI.Parameters.AddWithValue("@produto", _Estoque.CodigoProduto);
                    cmdI.Parameters.AddWithValue("@empresa", _Estoque.CodigoVendedor);
                    cmdI.Parameters.AddWithValue("@estoquedisponivel", _Estoque.Estoque);

                    DBConnectionMySql.AbreConexaoBD(DBMySqlI);
                    cmdI.CommandTimeout = 0;
                    cmdI.ExecuteNonQuery();
                    DBConnectionMySql.FechaConexaoBD(DBMySqlI);
                }
            }
            catch (Exception ex)
            {
                DAOLogDB.SalvarLogs("", "Estoque - Erro no ajuste de estoque", ex.Message, "APP");
            }
        }

        //Calcula o estoque por "Deposito" -----
        public static decimal CalculaEstoqueDeposito(string CodProd) 
        {
            string Sql = "SELECT pp.codigo, mm.estoque, mm.empresa," +
                " ((SELECT IFNULL(SUM(m.qnt),0) AS Entradas FROM movimentoestoque m INNER join produtoseservicos p ON p.codigo = m.codigoproduto WHERE m.tipomovimentacao  = 'Entrada' AND m.codigoproduto = pp.codigo AND m.estoque = mm.estoque AND m.empresa = mm.empresa) " +
                "- (SELECT IFNULL(SUM(m.qnt),0) AS Saidas FROM movimentoestoque m INNER join produtoseservicos p ON p.codigo = m.codigoproduto WHERE m.tipomovimentacao  = 'Saida' AND m.codigoproduto = pp.codigo AND m.estoque = mm.estoque AND m.empresa = mm.empresa))  AS estoqueproduto " +
                "FROM produtoseservicos pp INNER JOIN movimentoestoque mm ON mm.codigoproduto = pp.codigo where mm.codigoproduto = '" + CodProd + "' AND (mm.estoque = '" + DadosConfiguracao.Config.EnviarEstoqueDeposito + "' AND mm.estoque IS NOT NULL) group by pp.codigo, mm.estoque";
            string Estoque = "0";

            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            cmd.CommandTimeout = 20000;
            MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Estoque = dr["estoqueproduto"].ToString();
            }
            dr.Close();
            DBConnectionMySql.FechaConexaoBD(DBMySql);

           return ConverterDecimal(Estoque);
        }


    }
}
