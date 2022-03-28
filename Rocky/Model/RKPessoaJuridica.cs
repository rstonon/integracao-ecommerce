using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKPessoaJuridica
    {
        public string id { get; set; }
        public string razaosocial { get; set; }
        public string nomefantasia { get; set; }
        public string ie { get; set; }
        public string cnpj { get; set; }
        public string categoria { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string nome_responsavel { get; set; }
        public string email_responsavel { get; set; }
        public string cpf_responsavel { get; set; }
        public DateTime data_cadastro { get; set; }

        public RKPessoaJuridica()
        {
            id = "";
            razaosocial = "";
            nomefantasia = "";
            ie = "";
            cnpj = "";
            categoria = "";
            telefone = "";
            celular = "";
            endereco = "";
            numero = "";
            complemento = "";
            bairro = "";
            cep = "";
            cidade = "";
            estado = "";
            nome_responsavel = "";
            email_responsavel = "";
            cpf_responsavel = "";
            data_cadastro = DateTime.Now.Date;
        }
    }
}
