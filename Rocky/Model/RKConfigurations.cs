using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKConfigurations
    {
        public string id { get; set; }
        public string item { get; set; }
        public string sigla { get; set; }
        public RKVariation variation { get; set; }
        public RKConfigurations()
        {
            id = "";
            item = "";
            sigla = "";
            variation = null;
        }


    }
}
