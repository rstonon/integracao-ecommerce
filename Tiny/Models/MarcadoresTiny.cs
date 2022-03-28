using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class MarcadoresTiny
    {
        public MarcadoresPedidoTiny marcador { get; set; }

        public MarcadoresTiny()
        {
            marcador = new MarcadoresPedidoTiny();
        }
    }
}
