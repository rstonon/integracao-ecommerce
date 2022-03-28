using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKPedidos
    {
        public string id { get; set; }
        public string codigo { get; set; }
        public string copiado_erp { get; set; }
        public string transacao { get; set; }
        public string id_cliente { get; set; }
        public string tipo_cadastro { get; set; }
        public string canal { get; set; }
        public decimal subtotal { get; set; }
        public decimal juros { get; set; }
        public string cupom_desconto { get; set; }
        public string pontos_resgate { get; set; }
        public string valor_pontos_resgate { get; set; }
        public decimal frete { get; set; }
        public decimal total { get; set; }
        public string operacao { get; set; }
        public string status { get; set; }
        public string observacao { get; set; }
        public string total_itens { get; set; }
        public string codigo_frete { get; set; }
        public RKEnderecoEntrega endereco_entrega { get; set; }
        public List<RKItems> items { get; set; }

        public RKPedidos()
        {
            id = "";
            codigo = "";
            copiado_erp = "";
            transacao = "";
            id_cliente = "";
            tipo_cadastro = "";
            canal = "";
            subtotal = 0;
            juros = 0;
            cupom_desconto = "";
            pontos_resgate = "";
            valor_pontos_resgate = "0";
            frete = 0;
            total = 0;
            operacao = "";
            status = "";
            observacao = "";
            total_itens = "0";
            codigo_frete = "";
            endereco_entrega = null;
            items = null;
        }
    }
}
