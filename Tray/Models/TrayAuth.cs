using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models
{
    public class TrayAuth
    {
        public string code { get; set; }
        public string message { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime date_expiration_access_token { get; set; }
        public DateTime date_expiration_refresh_token { get; set; }
        public DateTime date_activated { get; set; }
        public string api_host { get; set; }
        public string store_id { get; set; }

        public TrayAuth()
        {
            code = "";
            message = "";
            access_token = "";
            refresh_token = "";
            date_expiration_access_token = DateTime.Now;
            date_expiration_refresh_token = DateTime.Now;
            date_activated = DateTime.Now;
            api_host = "";
            store_id = "";
        }

    }
}
