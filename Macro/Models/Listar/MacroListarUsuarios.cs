using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models.Listar
{
    public class MacroListarUsuarios
    {
        public DateTime data { get; set; }
        public List<MacroUsuarios> dados { get; set; }

        public MacroListarUsuarios()
        {
            data = DateTime.Now;
            dados = null;
        }

    }
}
