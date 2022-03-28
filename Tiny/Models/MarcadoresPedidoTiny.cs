using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class MarcadoresPedidoTiny
    {
        public string id { get; set; }
        public string descricao { get; set; }
        public string cor { get; set; }

        public MarcadoresPedidoTiny()
        {
            id = "";
            descricao = "";
            cor = "";
        }
    }
}
