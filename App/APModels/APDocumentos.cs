using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APDocumentos
    {
        public string CodigoDocumento { get; set; }
        public string Cnpj { get; set; }
        public string DescricaoDocumento { get; set; }
        public decimal ParcelaMinima { get; set; }
        public bool ConsiderarBloqueioAutomatico { get; set; }

        public APDocumentos()
        {
            CodigoDocumento = "";
            Cnpj = "";
            DescricaoDocumento = "";
            ParcelaMinima = 0;
            ConsiderarBloqueioAutomatico = false;
        }

    }
}
