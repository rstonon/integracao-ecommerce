using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APOcorrencias
    {
        public string Codigoocorrencia { get; set; }
        public string Cnpj { get; set; }
        public string Ocorrencia { get; set; }

        public APOcorrencias()
        {
            Codigoocorrencia = "";
            Cnpj = "";
            Ocorrencia = "";
        }
    }
}
