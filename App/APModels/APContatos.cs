using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APContatos
    {
        public string Codigocontato { get; set; }
        public string Codigoocorrencia { get; set; }
        public string Codigocolaborador { get; set; }
        public string Formadecontato { get; set; }
        public string Tipodecontato { get; set; }
        public string Statusdocontato { get; set; }
        public string Usuario { get; set; }
        public DateTime Datalanacamento { get; set; }
        public string Horalancamento { get; set; }
        public string Atendimento { get; set; }
        public string Dataconclusao { get; set; }
        public string Horaconclusao { get; set; }
        public string Problema { get; set; }
        public string Diagnostico { get; set; }
        public Boolean Urgente { get; set; }
        public string Codigosistema { get; set; }
        public DateTime Dataalteracao { get; set; }
        public string Horaalteracao { get; set; }
        public string Usuariolancamento { get; set; }
        public string Protocolo { get; set; }

        public List<APOcorrenciasContatos> OcorrenciasContato { get; set; }

        public APContatos()
        {
            Codigocontato = "";
            Codigoocorrencia = "";
            Codigocolaborador = "";
            Formadecontato = "";
            Tipodecontato = "";
            Statusdocontato = "";
            Usuario = "";
            Datalanacamento = DateTime.Now.Date;
            Horalancamento = DateTime.Now.ToString();
            Atendimento = "";
            //Dataconclusao = "";
            //Horaconclusao = "";
            Problema = "";
            Diagnostico = "";
            Urgente = false;
            Codigosistema = "";
            //Dataalteracao = DateTime.Now.Date;
            //Horaalteracao = DateTime.Now.ToString("HH:mm:ss");
            Usuariolancamento = "";
            Protocolo = "";

            OcorrenciasContato = new List<APOcorrenciasContatos>();
        }
    }
}
