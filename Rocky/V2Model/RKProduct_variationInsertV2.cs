using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.V2Model
{
    public class RKProduct_variationInsertV2 : RKProduct_variationV2
    {
        public decimal preco_promocional { get; set; }

        public RKProduct_variationInsertV2() 
        {
            preco_promocional = 0;
        }

    }
}
