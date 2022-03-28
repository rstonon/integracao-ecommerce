using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKItems
    {
        public string id { get; set; }
        public string id_pedido { get; set; }
        public string id_sku { get; set; }
        public string nome { get; set; }
        public decimal qtd { get; set; }
        public decimal valor { get; set; }
        public string valor_desconto { get; set; }
        public string observacao { get; set; }
        public string sku { get; set; }

        public RKItems()
        {
            id = "";
            id_pedido = "";
            id_sku = "";
            nome = "";
            qtd = 0;
            valor = 0;
            valor_desconto = "0";
            observacao = "";
            sku = "";
        }


    }
}
