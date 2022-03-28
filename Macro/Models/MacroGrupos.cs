using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroGrupos
    {
        public string id { get; set; }
        public string id_grupo_pai { get; set; }
        public string descricao { get; set; }
        public string ativo { get; set; }

        public MacroGrupos()
        {
            id = "";
            id_grupo_pai = "0";
            descricao = "";
            ativo = "0";
        }

    }
}
