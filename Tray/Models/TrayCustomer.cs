using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models
{
    public class TrayCustomer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string rg { get; set; }
        public string cpf { get; set; }
        public string phone { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public int type { get; set; }
        public string observation { get; set; }
        public string cnpj { get; set; }
        public string company_name { get; set; }
        public string state_inscription { get; set; }
        public string blocked { get; set; }
        public string address { get; set; }
        public string zip_code { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
        public string neighborhood { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        //public List<> CustomerAddresses { get; set; }
        public string birth_date { get; set; }

        public TrayCustomer()
        {
            id = "";
            name = "";
            rg = "";
            cpf = "";
            phone = "";
            cellphone = "";
            email = "";
            token = "";
            type = 0;
            observation = "";
            cnpj = "";
            company_name = "";
            state_inscription = "";
            blocked = "";
            address = "";
            zip_code = "";
            number = "";
            complement = "";
            neighborhood = "";
            city = "";
            state = "";
            country = "";
            birth_date = "";
            //CustomerAddresses = null;
        }
    }
}
