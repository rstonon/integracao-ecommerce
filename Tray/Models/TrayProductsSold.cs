using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models
{
    public class TrayProductsSold
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int order_id { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public decimal original_price { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public string reference { get; set; }
        public int variant_id { get; set; }
        public string additional_information { get; set; }

        public TrayProductsSold()
        {
            id = 0;
            product_id = 0;
            order_id = 0;
            name = "";
            original_name = "";
            original_price = 0;
            price = 0;
            quantity = 0;
            reference = "";
            variant_id = 0;
            additional_information = "";
        }
    }
}
