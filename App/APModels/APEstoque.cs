using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APEstoque
    {
        public string CodigoProduto { get; set; }
        public string CodigoVendedor { get; set; }
        public decimal Estoque { get; set; }


        public APEstoque()
        {
            CodigoProduto = "";
            CodigoVendedor = "";
            Estoque = 0;
        }

    }
}
