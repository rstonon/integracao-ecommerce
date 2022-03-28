using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class ParcelasPedidoTiny
    {
        public string dias { get; set; }
        public DateTime data { get; set; }
        public decimal valor { get; set; }
        public string obs { get; set; }

        public ParcelasPedidoTiny()
        {
            dias = "0";
            data = DateTime.Now;
            valor = 0;
            obs = "";
        }
    }
}
