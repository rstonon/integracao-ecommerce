using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models.Listar
{
    public class TrayProductListar
    {
        public TrayProduct Product { get; set; }

        public TrayProductListar()
        {
            Product = null;
        }
    }

    public class TrayProductsListar:TrayPaginacao 
    {    
        public List<TrayProductListar> Products { get; set; }

        public TrayProductsListar()
        {
            Products = null;
        }

    }
}
