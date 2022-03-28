using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models.Listar
{
    public class MacroListarPedidos
    {
        public DateTime data { get; set; }
        public List<MacroPedido> dados { get; set; }

        public MacroListarPedidos()
        {
            data = DateTime.Now;
            dados = null;
        }
    }
}
