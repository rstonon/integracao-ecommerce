using IntegracaoRockye.Rocky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.V2Model
{
    public class product_variation
    {
        public string id { get; set; }
        public string id_produto { get; set; }
        public string codigo { get; set; }
        public string sku { get; set; }
        public int status { get; set; }
        public string nome { get; set; }
        public decimal preco_base { get; set; }
        public decimal preco { get; set; }
        public decimal preco_promocional { get; set; }
        public decimal peso { get; set; }
        public string largura { get; set; }
        public string altura { get; set; }
        public string comprimento { get; set; }
        public string multiplos { get; set; }
        public decimal quantidade { get; set; }
        public List<RKsku_product_ref> product_ref { get; set; }
    }

    public class RKRetornoCadastroVariacaoProduto
    {
        public product_variation product_variation { get; set; }

        public RKRetornoCadastroVariacaoProduto()
        {
            product_variation = null;
        }
    }
}
