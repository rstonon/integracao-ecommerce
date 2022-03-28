using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models.Listar
{
    public class MacroDadosCliente:MacroCredenciais
    {
        public string id { get; set; }

        public MacroDadosCliente()
        {
            id = "";
        }

    }
}
