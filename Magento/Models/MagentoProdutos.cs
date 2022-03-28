using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Magento.Models
{
    public class MagentoProdutos
    {
        public string Codigo { get; set; }
        public string CodigoEcommerce { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string DescricaoCurta { get; set; }
        public string Status { get; set; }
        public string Visivil { get; set; }
        public string Peso { get; set; }
        public string Altura { get; set; }
        public string Largura { get; set; }
        public string Comprimento { get; set; }
        public string Sku { get; set; }
        public string Tamanho { get; set; }
        public string Cor { get; set; }
        public string Material { get; set; }
        public string Categoria { get; set; }
        public string Imagem { get; set; }

        public decimal EstoqueDisponivel { get; set; }
        public decimal PraticadoEcommerce { get; set; }

        public MagentoProdutos()
        {
            Codigo = "";
            CodigoEcommerce = "";

            Nome = "";
            Descricao = "";
            DescricaoCurta = "";
            Status = "1";
            Visivil = "4";
            Peso = "0";
            Altura = "0";
            Largura = "0";
            Comprimento = "";
            Sku = "";
            Tamanho = "";
            Cor = "";
            Material = "";
            Categoria = "";
            Imagem = "";

            EstoqueDisponivel = 0;
            PraticadoEcommerce = 0;
        }


    }
}
