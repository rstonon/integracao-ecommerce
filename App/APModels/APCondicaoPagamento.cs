using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.APModels
{
    public class APCondicaoPagamento
    {
        public string CodigoCondicao { get; set; }
        public string Cnpj { get; set; }
        public string DescricaoCondicao { get; set; }
        public decimal Desconto { get; set; }

        public decimal PercentualEntrada { get; set; }
        public decimal ParcelasPrazo { get; set; }
        public string CodigoDocumentoPrazo { get; set; }
        public string CodigoDocumentoVista { get; set; }
        public string Intervalo { get; set; }
        public string DiaPadrao { get; set; }

        public APCondicaoPagamento()
        {
            CodigoCondicao = "";
            Cnpj = "";
            DescricaoCondicao = "";
            Desconto = 0;

            PercentualEntrada = 0;
            ParcelasPrazo = 0;
            CodigoDocumentoPrazo = "";
            CodigoDocumentoVista = "";
            Intervalo = "";
            DiaPadrao = "";
        }
    }
}
