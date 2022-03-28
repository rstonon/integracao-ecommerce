using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APConfirmaOcorrencia
    {
        public string CodigoApp { get; set; }
        public string CodigoSistema { get; set; }
        public APConfirmaOcorrencia()
        {
            CodigoApp = "";
            CodigoSistema = "";
        }
    }

    public class APConfirmaOperacao
    {
        public string CodigoApp { get; set; }
        public string CodigoSistema { get; set; }
        public string DataSincronizacao { get; set; }
        public string HoraSincronizacao { get; set; }
        public List<APConfirmaOcorrencia> Ocorrencias { get; set; }

        public APConfirmaOperacao()
        {
            CodigoApp = "";
            CodigoSistema = "";
            DataSincronizacao = DateTime.Now.ToString("yyyy-MM-dd");
            HoraSincronizacao = DateTime.Now.ToString("HH:mm:ss");
            Ocorrencias = new List<APConfirmaOcorrencia>();
        }

    }
}
