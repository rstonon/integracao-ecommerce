using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models.Listar
{
    public class TrayVariantListar
    {
        public TrayVariant Variant { get; set; }

        public TrayVariantListar()
        {
            Variant = null;
        }
    }
    public class TrayVariantsListar : TrayPaginacao
    {

        public List<TrayVariantListar> Variants { get; set; }

        public TrayVariantsListar()
        {
            Variants = null;
        }
    }
}