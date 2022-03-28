using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class RetornoTiny
    {
        public string status_processamento { get; set; }
        public string status { get; set; }
        public string codigo_erro { get; set; }
        public List<ErrosTiny> erros { get; set; }

        public RetornoTiny()
        {
            status_processamento = "";
            status = "";
            codigo_erro = "";
            erros = new List<ErrosTiny>();
        }
    }
}
