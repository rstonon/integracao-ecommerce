using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models
{
    public class TrayProduct
    {
        public string id { get; set; }
        //public string ean { get; set; }
        public string name { get; set; }
       // public string ncm { get; set; }
       // public string description { get; set; }
       // public string description_small { get; set; }
        public decimal price { get; set; }

        //public decimal cost_price { get; set; }
        //public decimal promotional_price { get; set; }
        //public DateTime start_promotion { get; set; }
        //public DateTime end_promotion { get; set; }
        //public decimal ipi_value { get; set; }

        public string brand { get; set; }
        public string model { get; set; }
        public int weight { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int stock { get; set; }
        //public string category_id { get; set; }
        public string available { get; set; }
        // public string availability { get; set; }
        //public int availability_days { get; set; }
        public string reference { get; set; }
        public int release { get; set; }

        public string ReferenciaSistema { get; set; }
        //public string [] related_categories { get; set; }
        //public DateTime release_date { get; set; }
        //public string picture_source_1 { get; set; }
        //public string picture_source_2 { get; set; }
        //public string picture_source_3 { get; set; }
        //public string picture_source_4 { get; set; }
        //public string picture_source_5 { get; set; }
        //public string picture_source_6 { get; set; }
        // public object metatag { get; set; }
        // public string virtual_product { get; set; }

        public TrayProduct()
        {
            id = "";
            //ean = "";
            //name = "";
            //ncm = "";
            //description = "";
            //description_small = "";
            price = 0;
            //cost_price = 0;
            //promotional_price = 0;
            //start_promotion = DateTime.Now;
            //end_promotion = DateTime.Now;
            //ipi_value = 0;
            brand = "";
            model = "";
            weight = 0;
            length = 0;
            width = 0;
            height = 0;
            stock = 0;
            //category_id = "";
            available = "0";
            //availability = "";
            //availability_days = 0;
            reference = "";
            release = 0;
            //related_categories = null;
            //release_date = DateTime.Now;
            //picture_source_1 = "";
            //picture_source_2 = "";
            //picture_source_3 = "";
            //picture_source_4 = "";
            //picture_source_5 = "";
            //picture_source_6 = "";
            // metatag = null;
            // virtual_product = "";

            ReferenciaSistema = "";
        }

    }
}
