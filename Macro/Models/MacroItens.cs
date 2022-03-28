using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroItens
    {
        public string id_produto_variacao { get; set; }
        public string qtde { get; set; }
        public string valor { get; set; }
        public string observacoes { get; set; }

        public MacroItens()
        {
            id_produto_variacao = "";
            qtde = "0";
            valor = "0";
            observacoes = "";
        }
    }
}
