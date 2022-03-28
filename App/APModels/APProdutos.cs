using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegracaoRockye.Versatil.APModels
{
    public class APProdutos
    {
        public string Codigo { get; set; }
        public string Cnpj { get; set; }
        public string Tipo { get; set; }
        public string Situacao { get; set; }
        public string Descricao { get; set; }
        public string Referencia { get; set; }
        public string Unidade { get; set; }
        public string CodigoEan { get; set; }
        public decimal ValorMercadoria { get; set; }
        public decimal Praticado { get; set; }
        public decimal Praticado2 { get; set; }
        public decimal Praticado3 { get; set; }
        public decimal EstoqueDisponivel { get; set; }
        public decimal DescontoMaximo { get; set; }
        public string Observacoes { get; set; }
        public string Marca { get; set; }
        public string Grupo { get; set; }
        public decimal Vmv { get; set; }
        public decimal TotalVmv { get; set; }
        public decimal CmvUnitario { get; set; }
        public string Cor { get; set; }
        public string Tamanho { get; set; }
        public string Especificacoestecnicas { get; set; }
        public string Aplicacao { get; set; }
        public decimal Customercadoria { get; set; }
        public decimal Custorealmercadoria { get; set; }
        public DateTime DataImagem { get; set; }


        public APProdutos()
        {
            Codigo = "";
            Cnpj = "";
            Tipo = "Produto";
            Situacao = "Ativo";
            Descricao = "";
            Referencia = "";
            Unidade = "";
            CodigoEan = "";
            ValorMercadoria = 0;
            Praticado = 0;
            Praticado2 = 0;
            Praticado3 = 0;
            EstoqueDisponivel = 0;
            DescontoMaximo = 100;
            Observacoes = "";
            Marca = "";
            Grupo = "";
            Cor = "";
            Tamanho = "";
            Especificacoestecnicas = "";
            Aplicacao = "";
            Customercadoria = 0;
            Custorealmercadoria = 0;

            Vmv = 0;
            TotalVmv = 0;
            CmvUnitario = 0;
            DataImagem = DateTime.Now;
        }
    }
}
