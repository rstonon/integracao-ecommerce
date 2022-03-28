using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.APModels
{
    public class APPedidos
    {
        public string Codigo { get; set; }
        public string Cnpj { get; set; }
        public string CodigoColaborador { get; set; }
        public string CodigoTransportadora { get; set; }
        public string Documento { get; set; }
        public string DataCadastro { get; set; }
        public string HoraCadastro { get; set; }
        public string Observacoes { get; set; }
        public string CodigoVendedor { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public string Status { get; set; }
        public string Operacao { get; set; }
        public string CodigoCondicaoPagamento { get; set; }
        public string CodigoTabelaPreco { get; set; }
        public bool EspecificarDescontoAtendimento { get; set; }

        public string CodigoDocumentoaVista { get; set; }
        public string CodigoDocumentoPrazo { get; set; }
        public string ValorEntrada { get; set; }
        public string ValoraPrazo { get; set; }
        public string NumerodeParcelas { get; set; }
        public string pEntrada { get; set; }
        public string pPrazo { get; set; }



        public List<APItensPedido> ItensPedido { get; set; }
        public List<APContas> ContasPedido { get; set; }

        public APPedidos()
        {
            Codigo = "";
            Cnpj = "";
            CodigoColaborador = "";
            CodigoTransportadora = "";
            Documento = "";
            DataCadastro = "";
            HoraCadastro = "";
            Observacoes = "";
            CodigoVendedor = "";
            SubTotal = 0;
            ValorTotal = 0;
            ValorDesconto = 0;
            Status = "";
            Operacao = "";
            CodigoCondicaoPagamento = "";
            CodigoTabelaPreco = "";
            EspecificarDescontoAtendimento = false;
            ItensPedido = null;
            ContasPedido = null;
            ValorFrete = 0;

            pPrazo = "0";
            pEntrada = "0";
            NumerodeParcelas = "0";
            ValoraPrazo = "0";
            ValorEntrada = "0";
            CodigoDocumentoaVista = "";
            CodigoDocumentoPrazo = "";
        }
    }
}
