using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray
{
    public class TrayRetorno
    {
        public string message { get; set; }
        public string id { get; set; }
        public string code { get; set; }

        public TrayRetorno()
        {
            message = "";
            id = "";
            code = "";
        }

    }
}
