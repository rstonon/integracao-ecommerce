using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroEspecificacoes
    {
        public string especificacao { get; set; }
        public string valor { get; set; }

        public MacroEspecificacoes()
        {
            especificacao = "";
            valor = "";
        }

    }
}
