using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Models
{
    public class VerProdutos
    {
        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public string Situacao { get; set; }
        public string Descricao { get; set; }
        public string Referencia { get; set; }
        public string Unidade { get; set; }
        public string Codigoean { get; set; }
        public decimal Valormercadoria { get; set; }
        public decimal Customercadoria { get; set; }
        public decimal Custorealmercadoria { get; set; }
        public decimal Praticado { get; set; }
        public decimal Praticado2 { get; set; }
        public decimal Praticado3 { get; set; }
        public decimal Estoquedisponivel { get; set; }
        public decimal Descontomaximo { get; set; }
        public string Observacoes { get; set; }
        public string Codigomarca { get; set; }
        public string Codigoadicionalmarca { get; set; }
        public string Marca { get; set; }
        public string Codigogrupo { get; set; }
        public string Grupo { get; set; }
        public decimal Vmv { get; set; }
        public decimal Totalvmv { get; set; }
        public decimal Cmvunitario { get; set; }
        public string Cor { get; set; }
        public string Tamanho { get; set; }
        public string Especificacoestecnicas { get; set; }
        public string Aplicacao { get; set; }
        public DateTime DataImagem { get; set; }
        public decimal CustoReal { get; set; }
        public decimal pComissao { get; set; }
        public int N { get; set; }

        public VerProdutos()
        {
            Codigo = "";
            Tipo = "Produto";
            Situacao = "Ativo";
            Descricao = "";
            Referencia = "";
            Unidade = "";
            Codigoean = "";
            Valormercadoria = 0;
            Praticado = 0;
            Praticado2 = 0;
            Praticado3 = 0;
            Estoquedisponivel = 0;
            Descontomaximo = 100;
            Observacoes = "";
            Codigomarca = "";
            Marca = "";
            Codigoadicionalmarca = "";
            Codigogrupo = "";
            Grupo = "";
            Cor = "";
            Tamanho = "";
            Especificacoestecnicas = "";
            Aplicacao = "";
            Custorealmercadoria = 0;
            Customercadoria = 0;

            Vmv = 0;
            Totalvmv = 0;
            Cmvunitario = 0;
            CustoReal = 0;
            pComissao = 0;

            N = 0;
            DataImagem = DateTime.Now;
        }
    }
}
