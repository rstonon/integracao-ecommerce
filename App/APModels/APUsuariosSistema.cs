using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APUsuariosSistema
    {
        public string Codigo { get; set; }
        public string Usuario { get; set; }

        public APUsuariosSistema()
        {
            Codigo = "";
            Usuario = "";
        }
    }
}
