using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKSku_V2 : RKProdutos
    {
        public string id_produto { get; set; }
        public string codigo { get; set; }
        public string sku { get; set; }
        public int status { get; set; }
        public RKBrand brand { get; set; }

        //[JsonIgnore]
        public List<RKConfigurations> configurations { get; set; }

        public List<RKsku_product_ref> product_ref { get; set; }

        public RKSku_V2()
        {
            id = "";
            id_produto = "";
            codigo = "";
            sku = "";
            status = 1;
            quantidade = 0;
            brand = new RKBrand();
            configurations = new List<RKConfigurations>();
            product_ref = new List<RKsku_product_ref>();
        }
    }
}
