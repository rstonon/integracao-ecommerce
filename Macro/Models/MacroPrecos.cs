using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroPrecos
    {
        public string id_lista {get;set;}
        public int preco {get;set;}

        public MacroPrecos()
        {
            id_lista = "";
            preco = 0;
        }

    }
}
