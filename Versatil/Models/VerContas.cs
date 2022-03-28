using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Models
{
    public class VerContas
    {
        public string CodigoConta { get; set; }
        public string Cnpj { get; set; }
        public string CodigoColaborador { get; set; }
        public string NumeroDocumento { get; set; }
        public string Status { get; set; }
        public string CodigoDocumento { get; set; }
        public string CodigoTitular { get; set; }
        public string CodigoAtendimento { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string DataQuitacao { get; set; }
        public decimal ValorInicial { get; set; }
        public decimal ValorQuitado { get; set; }
        public decimal ValorSaldo { get; set; }

        public VerContas()
        {
            CodigoConta = "";
            Cnpj = "";
            CodigoColaborador = "";
            NumeroDocumento = "";
            DataEmissao = DateTime.Now.Date;
            DataVencimento = DateTime.Now.Date;
            Status = "";
            CodigoDocumento = "";
            CodigoTitular = "";
            ValorInicial = 0;
            ValorQuitado = 0;
            ValorSaldo = 0;
        }

    }
}
