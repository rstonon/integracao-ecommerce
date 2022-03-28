using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class RetornoContatoTiny: RetornoTiny
    {
        public ContatoTiny contato { get; set; }

        public RetornoContatoTiny()
        {
            contato = new ContatoTiny();
        }
    }
}
