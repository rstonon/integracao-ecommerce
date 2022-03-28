using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.APModels
{
    public class APItensPedido
    {
        public string CodigoItem { get; set; }
        public string CodigoAtendimento { get; set; }
        public string CodigoProduto { get; set; }
        public string DescricaoItem { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Praticado { get; set; }
        public decimal Valortotal { get; set; }
        public string ObservacoesItem { get; set; }


        public APItensPedido()
        {
            CodigoItem = "";
            CodigoAtendimento = "";
            CodigoProduto = "";
            DescricaoItem = "";
            Quantidade = 0;
            Praticado = 0;
            Valortotal = 0;
            ObservacoesItem = "";

        }
    }
}
