using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APTabelasPrecosPersonalizada
    {
        public string Cnpj { get; set; }
        public string CodigoProduto { get; set; }
        public string CodigoTabela{ get; set; }
        public decimal Valor { get; set; }

        public APTabelasPrecosPersonalizada()
        {
            Cnpj = "";
            CodigoProduto = "";
            CodigoTabela = "";
            Valor = 0;
        }

    }
}
