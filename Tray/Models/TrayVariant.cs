using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models
{
    public class TrayVariant
    {
        public string id { get; set; }
        public string product_id { get; set; }

        //public string ean { get; set; }
        public string order { get; set; }
        public decimal price { get; set; }
        //public decimal cost_price { get; set; }
        public int stock { get; set; }
        //public string minimum_stock { get; set; }
        public string reference { get; set; }
        public string weight { get; set; }
        public string width { get; set; }
        public string length { get; set; }
        public string height { get; set; }
        public string type_1 { get; set; }
        public string value_1 { get; set; }
        public string type_2 { get; set; }
        public string value_2 { get; set; }

        //public DateTime start_promotion { get; set; }
        //public DateTime end_promotion { get; set; }
        //public decimal promotional_price { get; set; }    
        // public List<TraySku> sku { get; set; }
        
        //public string quantity_sold { get; set; }


        //public string picture_source_1 { get; set; }
        //public string picture_source_2 { get; set; }
        //public string picture_source_3 { get; set; }
        //public string picture_source_4 { get; set; }
        //public string picture_source_5 { get; set; }
        //public string picture_source_6 { get; set; }

        public TrayVariant()
        {
            id = "";
            product_id = "";
            //ean = "";
            order = "0";
            price = 0;
            //cost_price = 0;
            stock = 0;
            //minimum_stock = "0";
            reference = "";
            weight = "0";
            length = "0";
            width = "0";
            height = "0";

            //sku = null;
            //start_promotion = DateTime.Now;
            //end_promotion = DateTime.Now;
            //promotional_price = 0;
            type_1 = "";
            value_1 = "";
            type_2 = "";
            value_2 = "";
            //quantity_sold = "0";
            //picture_source_1 = "";
            //picture_source_2 = "";
            //picture_source_3 = "";
            //picture_source_4 = "";
            //picture_source_5 = "";
            //picture_source_6 = "";
        }

    }
}
