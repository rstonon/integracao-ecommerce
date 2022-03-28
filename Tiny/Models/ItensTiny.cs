using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class ItensTiny
    {
        public ItensPedidoTiny item { get; set; }

        public ItensTiny()
        {
            item = new ItensPedidoTiny();
        }
    }
}
