using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroCidades
    {
        public string CodigoCidade { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string IBGE { get; set; }

        public MacroCidades()
        {
            CodigoCidade = "";
            Cidade = "";
            Estado = "";
            IBGE = "";
        }
    }
}
