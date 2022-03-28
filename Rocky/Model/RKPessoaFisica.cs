using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKPessoaFisica
    {
        public string id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string cpf { get; set; }
        public string celular { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string genero { get; set; }
        public DateTime data_nasc { get; set; }

        public RKPessoaFisica()
        {
            id = "";
            nome = "";
            email = "";
            cpf = "";
            celular = "";
            endereco = "";
            numero = "";
            complemento = "";
            bairro = "";
            cep = "";
            cidade = "";
            estado = "";
            genero = "";
            data_nasc = DateTime.Now.Date;
        }
    }
}
