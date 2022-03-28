using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.MundoWine.Models
{
    public class NFeMundoWine
    {
        public int codigo { get; set; }
        public string chave_nf { get; set; }
        public string data_envio { get; set; }
        public int transportadora { get; set; }

        public NFeMundoWine()
        {
            codigo = 0;
            chave_nf = "";
            data_envio = "";
            transportadora = 0;
        }
    }
}
