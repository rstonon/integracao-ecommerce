using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APRotasCidades
    {
        public string codigo { get; set; }
        public string codigocidade { get; set; }
        public string codigorota { get; set; }
        public int ordem { get; set; }

        public APRotasCidades()
        {
            codigo = "";
            codigocidade = "";
            codigorota = "";
            ordem = 0;
        }
    }
}
