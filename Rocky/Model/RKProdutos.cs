using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.Model
{
    public class RKProdutos
    {
        public string id { get; set; }
        public string referencia { get; set; }
        public string nome { get; set; }
        public decimal preco_base { get; set; }
        public decimal preco { get; set; }
        //[JsonIgnore]
        public decimal preco_promocional { get; set; }
        //public string descricao { get; set; }
        public decimal peso { get; set; }
        public string largura { get; set; }
        public string altura { get; set; }
        public string comprimento { get; set; }
        public string multiplos { get; set; }
        public string marca { get; set; }
        public decimal quantidade { get; set; }
        // public string codigosite { get; set; }

        [JsonIgnore]
        public string descmarca { get; set; }
        [JsonIgnore]
        public string referenciaecommerce { get; set; }
        [JsonIgnore]
        public string codigotamanho { get; set; }
        [JsonIgnore]
        public string tamanho { get; set; }
        [JsonIgnore]
        public string codigocor { get; set; }
        [JsonIgnore]
        public string cor { get; set; }
        [JsonIgnore]
        public string codigovariacao { get; set; }

        [JsonIgnore]
        public string codigoproduto{ get; set; }

        public RKProdutos()
        {
            id = "";
            referencia = "";
            nome = "";
            preco_base = 0;
            preco = 0;
            preco_promocional = 0;
            //descricao = "";
            peso = 0;
            largura = "0";
            altura = "0";
            comprimento = "0";
            multiplos = "0";
            marca = "2";
            quantidade = 0;
            //  codigosite = "";

            referenciaecommerce = "";
            codigocor = "";
            cor = "";
            codigotamanho = "";
            tamanho = "";
            codigovariacao = "";
            codigoproduto = "";
        }
    }
}
