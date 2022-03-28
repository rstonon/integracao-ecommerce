using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models
{
    public class TraySku
    {
        public string type { get; set; }
        public string value { get; set; }
        //public string image { get; set; }
        //public string image_secure { get; set; }

        public TraySku()
        {
            type = "";
            value = "";
            //image = "";
            //image_secure = "";
        }

    }
}
