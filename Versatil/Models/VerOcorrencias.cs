using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Models
{
    public class VerOcorrencias
    {
        public string Codigo { get; set; }
        public string Ocorrencia { get; set; }
        public string Codigoapp { get; set; }

        public VerOcorrencias()
        {
            Codigo = "";
            Ocorrencia = "";
            Codigoapp = "";
        }
    }
}
