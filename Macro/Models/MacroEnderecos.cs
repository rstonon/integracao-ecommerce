using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroEnderecos
    {
        public string id { get; set; }
        public string nome { get; set; }
        public string id_cidade { get; set; }
        public string endereco { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string complemento { get; set; }
        public int principal { get; set; }
        public int ativo { get; set; }

        public MacroEnderecos()
        {
            id = "";
            nome = "";
            id_cidade = "";
            endereco = "";
            telefone = "";
            celular = "";
            numero = "";
            bairro = "";
            cep = "";
            complemento = "";
            principal = 0;
            ativo = 0;
        }



    }
}
