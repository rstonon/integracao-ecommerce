using IntegracaoRockye.Rocky.DB;
using IntegracaoRockye.Versatil.DB;
using IntegracaoRockye.Versatil.Model;
using IntegracaoRockye.Versatil.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Funcoes
{
    public static class DAOClientes
    {
        public static string ValidaData(string _Data)
        {
            try
            {
                return Convert.ToDateTime(_Data).ToString("yyy-MM-dd");
            }
            catch
            {
                return "1900-01-01";
            }
        }

        //Busca a Lista de Clientes e Transportadoras com CPF ou CNPJ Existente e o Codigo da Cidade e Definido com o IBGE - ESPECIFICO APP
        public static List<VerColaboradores> BuscarClientes()
        {
            List<VerColaboradores> Colaborador = new List<VerColaboradores>();

            string Sql = "SELECT c.codigo, c.tipo, c.nomerazaosocial, c.nomefantasia, c.situacao, c.cpfcnpj, c.inscricaoestadual, c.endereco, c.numero, c.complemento, c.bairro, c.telefone, c.celular, c.email, c.observacoes, c.cep, ci.codigoibge, c.codigoapp, c.bloqueado, c.senha, c.codigovendedorpadrao, c.empresa, c.tabelapadrao, c.datanascimento, c.sexo, c.pessoacontato FROM colaboradores c LEFT JOIN cidades ci ON ci.codigo = c.codigocidade WHERE (c.tipo = 'Cliente' OR c.tipo = 'Transportadora') and c.cpfcnpj <> '' and c.cpfcnpj is not null GROUP BY c.codigo";

            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            MySqlCommand cmd = new MySqlCommand(Sql, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            cmd.CommandTimeout = 0;
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                VerColaboradores Cliente = new VerColaboradores();

                Cliente.Codigo = dr["codigoapp"].ToString();
                Cliente.CodigoSistema = dr["codigo"].ToString();

                if (string.IsNullOrEmpty(Cliente.Codigo))
                {
                    Cliente.Codigo = Cliente.CodigoSistema;
                }

                Cliente.Tipo = dr["tipo"].ToString();
                Cliente.Situacao = dr["situacao"].ToString();
                Cliente.Nomerazaosocial = dr["nomerazaosocial"].ToString();
                Cliente.Nomefantasia = dr["nomefantasia"].ToString();
                Cliente.Cpfcnpj = dr["cpfcnpj"].ToString();
                Cliente.Inscricaoestadual = dr["inscricaoestadual"].ToString();
                Cliente.Endereco = dr["endereco"].ToString();
                Cliente.Numero = dr["numero"].ToString();
                Cliente.Complemento = dr["complemento"].ToString();
                Cliente.Bairro = dr["bairro"].ToString();
                Cliente.Telefone = dr["telefone"].ToString();
                Cliente.Celular = dr["celular"].ToString();
                Cliente.Email = dr["email"].ToString();
                Cliente.Observacao = dr["observacoes"].ToString();
                Cliente.Cep = dr["cep"].ToString();
                Cliente.Codigocidade = dr["codigoibge"].ToString();
                Cliente.Codigovendedor = dr["codigovendedorpadrao"].ToString();

                if (dr["bloqueado"].ToString().Equals("Sim"))
                {
                    Cliente.Bloqueado = true;
                }
                else
                {
                    Cliente.Bloqueado = false;
                }

                Cliente.EmAtraso = ContasAtrasadas(Cliente.CodigoSistema);
                Cliente.Senha = dr["senha"].ToString();
                Cliente.Empresa = dr["empresa"].ToString();
                Cliente.Codigotabelapadrao = dr["tabelapadrao"].ToString();

                try
                {
                    Cliente.Datanascimento = Convert.ToDateTime(dr["datanascimento"].ToString()).ToString("yyy-MM-dd");
                }
                catch
                {
                    Cliente.Datanascimento = "1900-01-01";
                }

                Cliente.Genero = dr["sexo"].ToString();
                Cliente.Contato = dr["pessoacontato"].ToString();

                Colaborador.Add(Cliente);
            }
            dr.Close();
            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return Colaborador;
        }

        //Cadastra o Cliente
        public static string CadastrarCliente(VerColaboradores Cliente)
        {
            string CodigoCliente = UltimosCodigosDB.GetCodigoCliente().ToString();

            string Sql = "insert into colaboradores(codigo, tipo, nomerazaosocial, nomefantasia, situacao, cpfcnpj, inscricaoestadual, endereco, numero, complemento, bairro, telefone, celular, codigocidade, email, cep, observacoes, datacadastro, empresa, codigoapp, codigoecommerce, datanascimento, sexo, pessoacontato) values (@codigo, @tipo, @nomerazaosocial, @nomefantasia, @situacao, @cpfcnpj, @inscricaoestadual, @endereco, @numero, @complemento, @bairro, @telefone, @celular, @codigocidade, @email, @cep, @observacoes, @datacadastro, @empresa, @codigoapp, @codigoecommerce, @datanascimento, @sexo, @pessoacontato)";
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            MySqlCommand Comando = new MySqlCommand(Sql, DBMySql);
            Comando.Parameters.AddWithValue("@codigo", CodigoCliente);
            Comando.Parameters.AddWithValue("@tipo", "Cliente");
            Comando.Parameters.AddWithValue("@nomerazaosocial", Cliente.Nomerazaosocial.ToUpper());
            Comando.Parameters.AddWithValue("@nomefantasia", Cliente.Nomefantasia.ToUpper());
            Comando.Parameters.AddWithValue("@situacao", "Ativo");
            Comando.Parameters.AddWithValue("@cpfcnpj", MascaraCnpjCpf(Cliente.Cpfcnpj));
            Comando.Parameters.AddWithValue("@inscricaoestadual", Cliente.Inscricaoestadual);
            Comando.Parameters.AddWithValue("@endereco", Cliente.Endereco.ToUpper());
            Comando.Parameters.AddWithValue("@numero", Cliente.Numero);
            Comando.Parameters.AddWithValue("@complemento", Cliente.Complemento.ToUpper());
            Comando.Parameters.AddWithValue("@bairro", Cliente.Bairro.ToUpper());
            Comando.Parameters.AddWithValue("@telefone", MascaraTelefone(Cliente.Telefone));
            Comando.Parameters.AddWithValue("@celular", MascaraTelefone(Cliente.Celular));
            Comando.Parameters.AddWithValue("@codigocidade", Cliente.Codigocidade);
            Comando.Parameters.AddWithValue("@email", Cliente.Email);
            Comando.Parameters.AddWithValue("@cep", Cliente.Cep);
            Comando.Parameters.AddWithValue("@observacoes", Cliente.Observacao);
            Comando.Parameters.AddWithValue("@empresa", Cliente.Empresa);
            Comando.Parameters.AddWithValue("@datacadastro", DateTime.Now.Date);

            Comando.Parameters.AddWithValue("@codigoapp", Cliente.CodigoSistema);
            Comando.Parameters.AddWithValue("@codigoecommerce", Cliente.IdEcommerce);
            Comando.Parameters.AddWithValue("@datanascimento", ValidaData(Cliente.Datanascimento));
            Comando.Parameters.AddWithValue("@sexo", ValidaData(Cliente.Genero));
            Comando.Parameters.AddWithValue("@pessoacontato", Cliente.Contato);

            DBConnectionMySql.AbreConexaoBD(DBMySql);
            Comando.ExecuteNonQuery();
            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return CodigoCliente;
        }

        //Update Cliente
        public static void UpdateCliente(VerColaboradores Cliente)
        {
            string Sql = "update colaboradores set codigo = @codigo, tipo = @tipo, nomerazaosocial = @nomerazaosocial, nomefantasia = @nomefantasia, situacao = @situacao, cpfcnpj = @cpfcnpj, inscricaoestadual = @inscricaoestadual, endereco = @endereco, numero = @numero, complemento = @complemento, bairro = @bairro, codigocidade = @codigocidade, telefone = @telefone, email = @email, cep = @cep, celular = @celular, observacoes = @observacoes, codigoecommerce = @codigoecommerce, datanascimento = @datanascimento, sexo = @sexo, pessoacontato = @pessoacontato where codigo = @codigo";
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            MySqlCommand Comando = new MySqlCommand(Sql, DBMySql);
            Comando.Parameters.AddWithValue("@codigo", Cliente.Codigo);
            Comando.Parameters.AddWithValue("@tipo", "Cliente");
            Comando.Parameters.AddWithValue("@nomerazaosocial", Cliente.Nomerazaosocial.ToUpper());
            Comando.Parameters.AddWithValue("@nomefantasia", Cliente.Nomefantasia.ToUpper());
            Comando.Parameters.AddWithValue("@situacao", "Ativo");
            Comando.Parameters.AddWithValue("@cpfcnpj", MascaraCnpjCpf(Cliente.Cpfcnpj));
            Comando.Parameters.AddWithValue("@inscricaoestadual", Cliente.Inscricaoestadual);
            Comando.Parameters.AddWithValue("@endereco", Cliente.Endereco.ToUpper());
            Comando.Parameters.AddWithValue("@numero", Cliente.Numero);
            Comando.Parameters.AddWithValue("@complemento", Cliente.Complemento.ToUpper());
            Comando.Parameters.AddWithValue("@bairro", Cliente.Bairro.ToUpper());
            Comando.Parameters.AddWithValue("@codigocidade", Cliente.Codigocidade);
            Comando.Parameters.AddWithValue("@telefone", MascaraTelefone(Cliente.Telefone));
            Comando.Parameters.AddWithValue("@celular", MascaraTelefone(Cliente.Celular));
            Comando.Parameters.AddWithValue("@email", Cliente.Email);
            Comando.Parameters.AddWithValue("@cep", Cliente.Cep);
            Comando.Parameters.AddWithValue("@observacoes", Cliente.Observacao);

            Comando.Parameters.AddWithValue("@codigoecommerce", Cliente.IdEcommerce);
            Comando.Parameters.AddWithValue("@datanascimento", ValidaData(Cliente.Datanascimento));
            Comando.Parameters.AddWithValue("@sexo", Cliente.Genero);
            Comando.Parameters.AddWithValue("@pessoacontato", Cliente.Contato);

            DBConnectionMySql.AbreConexaoBD(DBMySql);
            Comando.ExecuteNonQuery();
            DBConnectionMySql.FechaConexaoBD(DBMySql);
        }

        //Consulta se ja Existe cadastro desse Cliente
        public static string ConsultaColaborador(string CpfCnpj)
        {
            MySqlConnection DBMySql = new MySqlConnection(DBConnectionMySql.strConnection);
            string CodigoCliente = "";
            string Query = "select * from colaboradores where (cpfcnpj = '" + CpfCnpj + "' or cpfcnpj = '" + MascaraCnpjCpf(CpfCnpj) + "')";
            MySqlCommand Comando = new MySqlCommand(Query, DBMySql);
            DBConnectionMySql.AbreConexaoBD(DBMySql);
            MySqlDataReader Reader = Comando.ExecuteReader();

            if (Reader.Read())
            {
                CodigoCliente = Reader["codigo"].ToString();
                Reader.Close();
            }
            DBConnectionMySql.FechaConexaoBD(DBMySql);

            return CodigoCliente;
        }

        //Mascara CPF/CNPJ
        private static string MascaraCnpjCpf(string valor)
        {
            try
            {
                string pCnpjCpf = "";

                if (valor.Length <= 14)
                {
                    pCnpjCpf = valor.Replace(".", "");
                    pCnpjCpf = pCnpjCpf.Replace("-", "");
                }
                else
                {
                    pCnpjCpf = valor.Replace(".", "");
                    pCnpjCpf = pCnpjCpf.Replace("/", "");
                    pCnpjCpf = pCnpjCpf.Replace("-", "");
                }

                string result = "";
                if (pCnpjCpf.Length == 14)
                {
                    result = pCnpjCpf.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
                }
                if (pCnpjCpf.Length == 11)
                {
                    result = pCnpjCpf.Insert(3, ".").Insert(7, ".").Insert(11, "-");
                }
                if ((pCnpjCpf.Length != 11) && (pCnpjCpf.Length != 14))
                {
                    result = pCnpjCpf;
                }
                return result;
            }
            catch
            {
                return valor;
            }
        }

        //Formata o Telefone
        private static string MascaraTelefone(string _telefone)
        {
            string stelefone = "";
            string resultado = "";
            try
            {
                if (_telefone.Length <= 14)
                {
                    stelefone = _telefone.Replace("(", "");
                    stelefone = stelefone.Replace("-", "");
                    stelefone = stelefone.Replace(")", "");
                }
            }
            catch
            {
                return "";
            }

            if (stelefone.Length >= 10)
            {
                resultado = stelefone.Insert(0, "(").Insert(3, ")").Insert(8, "-");
            }
            else
            {
                resultado = _telefone;
            }
            return resultado;
        }

        //Verifica se Existem contas Atrasadas do Cliente em Aberto
        private static bool ContasAtrasadas(string _Codigo)
        {
            try
            {
                string Numero = "";
                string Sql = "SELECT COUNT(*) AS Numero FROM contas co WHERE co.colaborador = '" + _Codigo + "' AND co.tipodaconta = 'A Receber' AND co.vencimento < '" + DateTime.Now.Date.ToString("yyy-MM-dd") + "' AND co.status = 'Aberta'";

                MySqlConnection DBMySql2 = new MySqlConnection(DBConnectionMySql.strConnection);
                MySqlCommand cmd2 = new MySqlCommand(Sql, DBMySql2);
                DBConnectionMySql.AbreConexaoBD(DBMySql2);
                MySqlDataReader dr2 = cmd2.ExecuteReader();

                if (dr2.Read())
                {
                    Numero = dr2["Numero"].ToString();
                    dr2.Close();
                }

                DBConnectionMySql.FechaConexaoBD(DBMySql2);

                if (Convert.ToInt32(Numero) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
