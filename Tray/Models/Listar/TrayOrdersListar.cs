using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models.Listar
{

    public class TrayOrderListar
    {
        public TrayOrder Order { get; set; }

        public TrayOrderListar()
        {
            Order = null;
        }
    }


    public class TrayOrdersListar:TrayPaginacao
    {
        public List<TrayOrderListar> Orders { get; set; }

        public TrayOrdersListar()
        {
            Orders = null;
        }
    }
}
