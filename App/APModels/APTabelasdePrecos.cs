using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.APModels
{
    public class APTabelasdePrecos
    {
        public string Codigo { get; set; }
        public string Cnpj { get; set; }
        public string NomeTabela { get; set; }
        public decimal Desconto { get; set; }
        public string PraticadoPadrao { get; set; }

        public APTabelasdePrecos()
        {
            Codigo = "";
            Cnpj = "";
            NomeTabela = "";
            Desconto = 0;
            PraticadoPadrao = "";
        }
    }
}
