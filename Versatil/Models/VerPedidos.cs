using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Models
{
    public class VerPedidos
    {
        public string Codigo { get; set; }
        public string Codigocolaborador { get; set; }
        public string Codigovendedor { get; set; }
        public string Codigotransportadora { get; set; }
        public string Documento { get; set; }
        public string Status { get; set; }
        public string Observacoes { get; set; }
        public decimal Valorfrete { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Valortotal { get; set; }
        public decimal Totalnf { get; set; }
        public decimal Valordesconto { get; set; }
        public string Pedido { get; set; }
        public string Observacoesnota { get; set; }
        public string Substatus { get; set; }
        public string Pedidoapp { get; set; }
        public string Codigocondicaopagamento { get; set; }
        public string Descricaocondicaopagamento { get; set; }
        public string Tabeladepreco { get; set; }
        public bool Especificardescontonoatendimento{get;set;}
        public string Empresa { get; set; }
        public string CodigoDocumentoaVista { get; set; }
        public string CodigoDocumentoPrazo { get; set; }
        public string ValorEntrada { get; set; }
        public string ValoraPrazo { get; set; }
        public string NumerodeParcelas { get; set; }
        public string pEntrada { get; set; }
        public string pPrazo { get; set; }


        public List<VerItens> Itens { get; set; }
        public List<VerContas> Contas { get; set; }

        public VerPedidos()
        {
            Codigo = "";
            Codigocolaborador = "";
            Codigotransportadora = "";
            Documento = "";
            Status = "";
            Observacoes = "";
            Valorfrete = 0;
            Subtotal = 0;
            Valortotal = 0;
            Totalnf = 0;
            Valordesconto = 0;
            Pedido = "";
            Observacoesnota = "";
            Substatus = "";
            Pedidoapp = "";
            Codigocondicaopagamento = "";
            Tabeladepreco = "";
            Especificardescontonoatendimento = false;
            Descricaocondicaopagamento = "";
            Itens = new List<VerItens>();
            Empresa = "";
            Contas = new List<VerContas>();
        }
    }
}
