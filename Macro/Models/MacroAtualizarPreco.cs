using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroAtualizarPreco
    {
        public string id_produto_variacao { get; set; }
        public List<MacroPrecos> precos { get; set; }

        public MacroAtualizarPreco()
        {
            id_produto_variacao = "";
            precos = new List<MacroPrecos>();
        }

    }
}
