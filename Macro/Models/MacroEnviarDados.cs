using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroEnviarDados:MacroListar
    {
        public object dados { get; set; }

        public MacroEnviarDados()
        {
            dados = null;
        }
    }
}
