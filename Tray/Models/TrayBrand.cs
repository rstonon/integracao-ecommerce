using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models
{
    public class TrayBrand
    {
        public string id { get; set; }
        public string brand { get; set; }
        //public string slug { get; set; }

        public TrayBrand()
        {
            id = "";
            brand = "";
            //slug = "";
        }
    }
}
