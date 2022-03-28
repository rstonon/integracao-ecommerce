using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Magento
{
    public static class MagentoFuncoes
    {

        public static void AtualizarEstoque()
        {





            List<RKProdutos> GetProdutosEnviados()
            {
                MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);

                try
                {
                    List<RKProdutos> ListaProdutos = new List<RKProdutos>();

                    string Query = "select p.codigo, p.referencia, p.codigoecommerce, p.referenciaecommerce from produtoseservicos p where p.tipo = 'Produto' and p.situacao = 'Ativo' and p.site = '1' and (p.codigoecommerce <> '' and p.codigoecommerce is not null)";
                    MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
                    DBConnectionMySql.AbreConexaoBD(DBMySql);
                    MySqlDataReader Reader = Comando.ExecuteReader();

                    while (Reader.Read())
                    {
                        RKProdutos Produto = new RKProdutos();

                        Produto.id = Reader["codigoecommerce"].ToString();
                        Produto.referencia = Reader["codigo"].ToString();
                        Produto.referenciaecommerce = Reader["referenciaecommerce"].ToString();

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

            decimal ConverterDecimal(string Valor)
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
        }











    }
}
