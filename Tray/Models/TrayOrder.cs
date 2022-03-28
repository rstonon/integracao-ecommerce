using IntegracaoRockye.Tray.Models.Listar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models
{
    public class TrayOrders
    {
        public TrayOrder Order { get; set; }

        public TrayOrders()
        {
            Order = null;
        }
    }

    public class TrayOrder
    {
        public string status { get; set; }
        public string id { get; set; }
        public DateTime date { get; set; }
        public DateTime hour { get; set; }
        public string customer_id { get; set; }
        public decimal partial_total { get; set; }
        public decimal taxes { get; set; }
        public decimal discount { get; set; }
        public string point_sale { get; set; }
        public string shipment { get; set; }
        public string shipment_value { get; set; }
        public int shipment_id { get; set; }
        //public DateTime shipment_date { get; set; }
        public string delivered { get; set; }
        public string store_note { get; set; }
        public string customer_note { get; set; }
        public string partner_id { get; set; }
        public string discount_coupon { get; set; }
        public decimal payment_method_rate { get; set; }
        public string installment { get; set; }
        public string billing_address { get; set; }
        public string delivery_time { get; set; }
        public string payment_method_id { get; set; }
        public string payment_method { get; set; }
        public string session_id { get; set; }
        public string total { get; set; }
        public DateTime modified { get; set; }
        public string id_quotation { get; set; }
       // public DateTime estimated_delivery_date { get; set; }
        public TrayCustomer Customer { get; set; }
        public List<TrayProductsSoldListar> ProductsSold { get; set; }
        public TrayCoupon coupon { get; set; }

        public TrayOrder()
        {
            status = "";
            id = "";
            date = DateTime.Now;
            hour = DateTime.Now;
            customer_id = "";
            partial_total = 0;
            taxes = 0;
            discount = 0;
            point_sale = "";
            shipment = "";
            shipment_value = "";
            shipment_id = 0;
            //shipment_date = DateTime.Now;
            delivered = "0";
            store_note = "";
            customer_note = "";
            partner_id = "0";
            discount_coupon = "0";
            payment_method_rate = 0;
            installment = "0";
            billing_address = "";
            delivery_time = "";
            payment_method_id = "0";
            payment_method = "";
            session_id = "";
            coupon = null;
            total = "";
            modified = DateTime.Now;
            id_quotation = "0";
            //estimated_delivery_date = DateTime.Now;
            Customer = null;
            ProductsSold = null;
        }



    }
}
