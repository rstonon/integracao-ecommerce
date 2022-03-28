using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model.List
{
    public class RKProdutosLista
    {
        public List<RKProdutos> products { get; set; }

        public RKProdutosLista()
        {
            products = null;
        }
    }
}
