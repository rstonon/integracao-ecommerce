using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class RetornoPedidoTiny : RetornoTiny
    {
        public PedidoTiny pedido {get;set;}

        public RetornoPedidoTiny()
        {
            pedido = new PedidoTiny();
        }
    }
}
