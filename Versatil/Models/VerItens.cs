using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Models
{
    public class VerItens
    {
        public string Codigo{ get; set; }
        public string Referenciatmp { get; set; }
        public string Atendimento { get; set; }
        public string Codigoproduto { get; set; }
        public string Produto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Tabela { get; set; }
        public decimal Total { get; set; }
        public decimal Tabelacomdesconto { get; set; }
        public decimal Totalcomdesconto { get; set; }
        public string Unidade { get; set; }
        public decimal Vmv { get; set; }
        public decimal FreteItem { get; set; }
        public decimal Totalvmv { get; set; }
        public decimal Cmvunitario { get; set; }
        public string ObservacoesItem { get; set; }
        public decimal ValorDesconto { get; set; }

        public VerItens()
        {
            Codigo = "";
            Referenciatmp = "";
            Atendimento = "";
            Codigoproduto = "";
            Produto = "";
            Quantidade = 0;
            Tabela = 0;
            Total = 0;
            Tabelacomdesconto = 0;
            Totalcomdesconto = 0;
            Unidade = "";
            Vmv = 0;
            Totalvmv = 0;
            Cmvunitario = 0;
            FreteItem = 0;
            ObservacoesItem = "";
            ValorDesconto = 0;
        }
    }
}
