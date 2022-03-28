using IntegracaoRockye.Rocky.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.V2Model
{
    //SKU V2.0
    public class RKProduct_variationV2
    {
        public string id { get; set; }
        public string id_produto {get;set;}
        public string codigo { get; set; }
        public string sku { get; set; }
        public int status { get; set; }
        public string nome { get; set; }
        public decimal preco_base { get; set; }
        public decimal preco { get; set; }
        //[JsonIgnore]
        //public decimal preco_promocional { get; set; }
        public decimal peso { get; set; }
        public string largura { get; set; }
        public string altura { get; set; }
        public string comprimento { get; set; }
        public string multiplos { get; set; }
        public decimal quantidade { get; set; }
        public string[] configurations { get; set; }
        public RKBrand brand { get; set; }
        public List<RKsku_product_ref> product_ref { get; set; }

        public RKProduct_variationV2()
        {
            id_produto = "";
            codigo = "";
            sku = "";
            status = 1;
            nome = "";
            preco_base = 0;
            preco = 0;
            //preco_promocional = 0;
            peso = 0;
            largura = "";
            altura = "";
            comprimento = "";
            multiplos = "1";
            quantidade = 0;
            configurations = null;
            product_ref = null;
            brand = null;
        }
    }
}
