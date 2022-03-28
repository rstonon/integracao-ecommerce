using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.MundoWine.Models
{
    public class RetornoErro
    {
        public string property { get; set; }
        public string pointer { get; set; }
        public string message { get; set; }
        public string constraint { get; set; }
        public string context { get; set; }

        public RetornoErro()
        {
            property = "";
            pointer = "";
            message = "";
            constraint = "";
            context = "";
        }
    }
}
