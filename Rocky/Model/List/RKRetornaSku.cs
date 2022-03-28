using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model.List
{
    public class RKRetornaSku
    {
        public RKSku product_variation { get; set; }

        public RKRetornaSku()
        {
            product_variation = null;
        }
    }
}
