using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKSku:RKProdutos
    {
        public string id_produto { get; set; }
        public string sku { get; set; }
        public int status { get; set; }
        public List<RKsku_product_ref> product_ref { get; set; }

        public RKSku()
        {
            id = "";
            id_produto = "";
            sku = "";
            status = 1;
            quantidade = 0;
            product_ref = null;
        }
    }
}
