using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tiny.Models
{
    public class PedidoTiny
    {
        public string id { get; set; }
        public string numero { get; set; }
        public string data_pedido { get; set; }
        
        //public string data_prevista { get; set; }
        //public string data_faturamento { get; set; }

        public ContatoTiny cliente { get; set; }
        public List<ItensTiny> itens { get; set; }
        public List<ParcelasTiny> parcelas { get; set; }
        public List<MarcadoresTiny> marcadores { get; set; }

        public string condicao_pagamento { get; set; }
        public string forma_pagamento { get; set; }
        public string meio_pagamento { get; set; }
        public string nome_transportador { get; set; }
        public string frete_por_conta { get; set; }
        public decimal valor_frete { get; set; }
        public decimal valor_desconto { get; set; }
        public decimal total_produtos { get; set; }
        public decimal total_pedido { get; set; }
        public string numero_ordem_compra { get; set; }
        public string deposito { get; set; }
        public string forma_envio { get; set; }
        public string forma_frete { get; set; }
        public string situacao { get; set; }
        public string obs { get; set; }
        public string id_vendedor { get; set; }
        public string nome_vendedor { get; set; }
        public string codigo_rastreamento { get; set; }

        public PedidoTiny()
        {
            id = "";
            numero = "";
            data_pedido = "";
            //data_prevista = DateTime.Now;
            //data_faturamento = DateTime.Now;
            cliente = new ContatoTiny();
            itens = new List<ItensTiny>();
            parcelas = new List<ParcelasTiny>();
            marcadores = new List<MarcadoresTiny>();
            condicao_pagamento = "";
            forma_pagamento = "";
            meio_pagamento = "";
            nome_transportador = "";
            frete_por_conta = "";
            valor_frete = 0;
            valor_desconto = 0;
            total_produtos = 0;
            total_pedido = 0;
            numero_ordem_compra = "";
            deposito = "";
            forma_envio = "";
            forma_frete = "";
            situacao = "";
            obs = "";
            id_vendedor = "";
            nome_vendedor = "";
            codigo_rastreamento = "";
        }
    }
}
