using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class ParcelasTiny
    {
        public ParcelasPedidoTiny parcela { get;set;}

        public ParcelasTiny()
        {
            parcela = new ParcelasPedidoTiny();
        }
    }
}
