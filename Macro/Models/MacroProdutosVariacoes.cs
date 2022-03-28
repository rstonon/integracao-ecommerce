using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroProdutosVariacoes
    {
        public string id_produto_variacao { get; set; }
        public string referencia { get; set; }
        public string id_variacao_1 { get; set; }
        public string id_variacao_2 { get; set; }
        public string id_variacao_3 { get; set; }
        public string id_variacao_4 { get; set; }
        public string id_variacao_5 { get; set; }
        public string estoque { get; set; }
        public string peso { get; set; }
        public string ean { get; set; }
        public string ordem { get; set; }
        public int ativo { get; set; }
        public List<MacroPrecos> precos { get; set; }


        public MacroProdutosVariacoes()
        {
            id_produto_variacao = "0";
            referencia = "";
            id_variacao_1 = "";
            id_variacao_2 = "";
            id_variacao_3 = "";
            id_variacao_4 = "";
            id_variacao_5 = "";
            estoque = "0";
            peso = "0";
            ean = "";
            ordem = "1";
            ativo = 0;
            precos = null;
        }

    }
}
