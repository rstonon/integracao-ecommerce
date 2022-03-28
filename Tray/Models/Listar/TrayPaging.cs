using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models.Listar
{
    public class TrayPaging
    {
        public int total { get; set; }
        public int page { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public int maxLimit { get; set; }

        public TrayPaging()
        {
            total = 0;
            page = 0;
            offset = 0;
            limit = 0;
            maxLimit = 0;
        }
    }
}
