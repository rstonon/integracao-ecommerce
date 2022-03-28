using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.APModels
{
    public class APColaboradores
    {
        public string Codigocolaborador { get; set; }
        public string Cnpj { get; set; }
        public string Situacao { get; set; }
        public string Tipocadastro { get; set; }
        public string Cpfcnpj { get; set; }
        public string Razaosocial { get; set; }
        public string Nomefantasia { get; set; }
        public string Ie { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Codigocidade { get; set; }
        public string Cep { get; set; }
        public string Observacao { get; set; }
        public string Codigovendedor { get; set; }
        public string CodigoSistema { get; set; }
        public bool Bloqueado { get; set; }
        public bool EmAtraso { get; set; }
        public string Senha { get; set; }
        public string CodigoTabelaPadrao { get; set; }
        public string DataNascimento { get; set; }
        public string Genero { get; set; }
        public string Contato { get; set; }

        public APColaboradores()
        {
            Codigocolaborador = "";
            Cnpj = "";
            Tipocadastro = "";
            Situacao = "";
            Cpfcnpj = "";
            Razaosocial = "";
            Nomefantasia = "";
            Ie = "";
            Endereco = "";
            Numero = "";
            Complemento = "";
            Bairro = "";
            Telefone = "";
            Celular = "";
            Email = "";
            Codigocidade = "";
            Cep = "";
            Observacao = "";
            Codigovendedor = "";
            CodigoSistema = "";
            Bloqueado = false;
            EmAtraso = false;
            Senha = "";
            CodigoTabelaPadrao = "";
            DataNascimento = "1900-01-01";
            Genero = "";
            Contato = "";
        }
    }
}
