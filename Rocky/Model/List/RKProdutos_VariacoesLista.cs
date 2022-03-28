using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model.List
{
    public class RKProdutos_VariacoesLista
    {
        public List<RKSku> product_variations { get; set; }

        public RKProdutos_VariacoesLista()
        {
            product_variations = null;
        }
    }
}
