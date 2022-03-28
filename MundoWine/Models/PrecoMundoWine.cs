using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.MundoWine.Models
{
    public class PrecoMundoWine
    {
        public string referencia { get; set; }
        public decimal preco { get; set; }

        public PrecoMundoWine()
        {
            referencia = "";
            preco = 0;
        }
    }
}
