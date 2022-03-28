using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroPedido
    {
        public string id { get; set; }
        public string id_usuario { get; set; }
        public DateTime data { get; set; }
        public string valor_produtos { get; set; }
        public string valor_frete { get; set; }
        public string valor_descontos { get; set; }
        public string valor_a_pagar { get; set; }
        public string pedido_desconto { get; set; }
        public string frete { get; set; }
        public string id_endereco { get; set; }
        public string id_lista { get; set; }
        public string indice { get; set; }
        public string [] id_distribuidores { get; set; }
        public string situacao { get; set; }
        public string observacoes { get; set; }
        public DateTime data_atualizacao { get; set; }
        public List<MacroItens> itens { get; set; }

        public MacroPedido()
        {
            id = "";
            id_usuario = "";
            data = DateTime.Now;
            valor_produtos = "0";
            valor_frete = "0";
            valor_descontos = "0";
            valor_a_pagar = "0";
            pedido_desconto = "0";
            frete = "0";
            id_endereco = "";
            id_lista = "";
            indice = "";
            situacao = "";
            observacoes = "";
            data_atualizacao = DateTime.Now;
            itens = null;
        }

    }
}
