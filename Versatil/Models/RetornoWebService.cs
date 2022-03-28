using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Models
{
    public class RetornoWebService
    {
        public string Code { get; set; }
        public string Response { get; set; }

        public RetornoWebService()
        {
            Code = "Error";
            Response = "";
        }
    }
}
