using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroUsuarios
    {
        public string id { get; set; }
        public string nome { get; set; }
        public string apelido { get; set; }
        public string email { get; set; }
        public string sexo { get; set; }
        public DateTime aniversario { get; set; }
        public string razao { get; set; }
        public string fantasia { get; set; }
        public string cpf { get; set; }
        public string cnpj { get; set; }
        public string rg { get; set; }
        public int id_status { get; set; }
        public string id_lista { get; set; }
        public string [] id_distribuidores { get; set; }
        public decimal desconto { get; set; }
        public string observacao { get; set; }
        public List<MacroEnderecos> enderecos { get; set; }

        public MacroUsuarios()
        {
            id = "";
            nome = "";
            apelido = "";
            email = "";
            sexo = "";
            aniversario = DateTime.Now.Date;
            razao = "";
            fantasia = "";
            cpf = "";
            cnpj = "";
            rg = "";
            id_status = 0;
            id_lista = "";
            id_distribuidores = null;
            desconto = 0;
            observacao = "";
            enderecos = null;
        }

    }
}
