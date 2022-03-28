using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model.List
{
    public class RKPedidosLista
    {
        public List<RKPedidos> orders { get; set; }

        public RKPedidosLista()
        {
            orders = null;
        }
    }
}
