using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Models
{
    public class VerColaboradores
    {
        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public string Nomerazaosocial { get; set; }
        public string Nomefantasia { get; set; }
        public string Situacao { get; set; }
        public string Cpfcnpj { get; set; }
        public string Inscricaoestadual { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Codigocidade { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public string Observacao { get; set; }
        public string Codigovendedor { get; set; }
        public string CodigoSistema { get; set; }
        public bool Bloqueado { get; set; }
        public bool EmAtraso { get; set; }
        public string Senha { get; set; }
        public string Empresa { get; set; }
        public string IdEcommerce { get; set; }
        public string Codigotabelapadrao { get; set; }
        public string Datanascimento { get; set; }
        public string Genero { get; set; }
        public string Contato { get; set; }
        public VerColaboradores()
        {
            Codigo = "";
            Tipo = "";
            Nomerazaosocial = "";
            Nomefantasia = "";
            Situacao = "";
            Cpfcnpj = "";
            Inscricaoestadual = "";
            Endereco = "";
            Numero = "";
            Complemento = "";
            Bairro = "";
            Codigocidade = "";
            Telefone = "";
            Celular = "";
            Email = "";
            Cep = "";
            Observacao = "";
            Codigovendedor = "";
            CodigoSistema = "";
            Bloqueado = false;
            EmAtraso = false;
            Senha = "";
            Empresa = "";
            IdEcommerce = "";
            Codigotabelapadrao = "";
            Datanascimento = "1900-01-01";
            Genero = "";
            Contato = "";
        }

    }
}
