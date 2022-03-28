using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Models
{
    public class VerCidade
    {
        public string CodigoCidade { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CodigoIbge { get; set; }

        public VerCidade()
        {
            CodigoCidade = "";
            Cidade = "";
            CodigoIbge = "";
            Estado = "";
        }
    }
}
