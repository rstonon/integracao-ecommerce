using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroEstoque
    {
        public string id_produto_variacao { get; set; }
       public int estoque_real { get; set; }

        public MacroEstoque()
        {
            id_produto_variacao = "";
            estoque_real = 0;
        }
    }
}
