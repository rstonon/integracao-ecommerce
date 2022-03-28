using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.MundoWine.Models
{
    public class EstoqueMundoWine
    {
        public string referencia { get; set; }
        public int estoque { get; set; }

        public EstoqueMundoWine()
        {
            referencia = "";
            estoque = 0;
        }
    }
}
