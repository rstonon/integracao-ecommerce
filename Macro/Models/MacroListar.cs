using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroListar:MacroCredenciais
    {
        public string data { get; set; }

        public MacroListar()
        {
            data = DateTime.Now.Date.ToString();
        }

    }
}
