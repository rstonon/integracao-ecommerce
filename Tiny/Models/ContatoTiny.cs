using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class ContatoTiny
    {
        public string id { get; set; }
        public string codigo { get; set; }
        public string nome { get; set; }
        public string fantasia { get; set; }
        public string cpf_cnpj { get; set; }
        public string ie { get; set; }
        public string rg { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string contatos { get; set; }
        public string fone { get; set; }
        public string celular { get; set; }
        public string email { get; set; }

        public ContatoTiny()
        {
            id = "";
            codigo = "";
            nome = "";
            fantasia = "";
            cpf_cnpj = "";
            ie = "";
            rg = "";
            endereco = "";
            numero = "";
            complemento = "";
            bairro = "";
            cep = "";
            cidade = "";
            uf = "";
            contatos = "";
            fone = "";
            celular = "";
            email = "";
        }
    }
}
