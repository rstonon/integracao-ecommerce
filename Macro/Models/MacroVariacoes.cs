using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroVariacoes
    {
        public string id { get; set; }
        public string descricao { get; set; }
        public string id_tipo { get; set; }
        public string indice { get; set; }
        public string ordem { get; set; }
        //public string foto { get; set; }
        public string ativo { get; set; }

        public MacroVariacoes()
        {
            id = "";
            descricao = "";
            id_tipo = "1";
            indice = "1";
            ordem = "1";
            ativo = "0";
        }
    }
}
