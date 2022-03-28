using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class ItensPedidoTiny
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public decimal quantidade { get; set; }
        public decimal valor_unitario { get; set; }

        public ItensPedidoTiny()
        {
            codigo = "";
            descricao = "";
            unidade = "";
            quantidade = 0;
            valor_unitario = 0;
        }
    }
}
