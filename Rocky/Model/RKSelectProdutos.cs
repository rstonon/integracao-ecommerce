using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKSelectProdutos
    {
        public string codigo { get; set; }
        public string unidade { get; set; }
        public string referencia { get; set; }
        public decimal praticado { get; set; }
        public decimal valormercadoria { get; set; }
        public decimal customercadoria { get; set; }

        public RKSelectProdutos()
        {
            codigo = "";
            unidade = "";
            referencia = "";
            praticado = 0;
            valormercadoria = 0;
            customercadoria = 0;
        }
    }
}
