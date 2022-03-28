using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models.Listar
{
    public class TrayPaginacao
    {
        public TrayPaging paging { get; set; }
        public List<TraySort> sort { get; set; }
        //public string [] availableFilters { get; set; }
        //public string [] appliedFilters { get; set; }

        public TrayPaginacao()
        {
            paging = null;
            sort = null;
            //availableFilters = null;
            //appliedFilters = null;
        }
    }
}
