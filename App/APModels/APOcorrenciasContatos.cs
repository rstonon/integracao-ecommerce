using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APOcorrenciasContatos
    {
        public string Codigoocorrencia { get; set; }
        public string Protocolo { get; set; }
        public DateTime Datalancamento { get; set; }
        public string Hora { get; set; }
        public string Usuariolancamento { get; set; }
        public string Descricaoocorrencia { get; set; }
        public string Codigosistema { get; set; }

        public APOcorrenciasContatos()
        {
            Codigoocorrencia = "";
            Protocolo = "";
            Datalancamento = DateTime.Now.Date;
            Usuariolancamento = "";
            Descricaoocorrencia = "";
            Hora = DateTime.Now.ToString("HH:mm:ss");
            Codigosistema = "";
        }
    }
}
