using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class EstoqueTiny
    {
        public string idProduto { get; set; }
        public string tipo { get; set; }
        public DateTime data { get; set; }
        public decimal quantidade { get; set; }
        public decimal precoUnitario { get; set; }
        public string observacoes { get; set; }
        public string deposito { get; set; }

        public EstoqueTiny()
        {
            idProduto = "";
            tipo = "";
            data = DateTime.Now;
            quantidade = 0;
            precoUnitario = 0;
            observacoes = "";
            deposito = "";
        }

    }
}
