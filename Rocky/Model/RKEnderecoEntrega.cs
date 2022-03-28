using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKEnderecoEntrega
    {
        public string cep { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string pais { get; set; }

        public RKEnderecoEntrega()
        {
            cep = "";
            endereco = "";
            numero = "";
            complemento = "";
            bairro = "";
            cidade = "";
            estado = "";
            pais = "";
        }
    }
}
