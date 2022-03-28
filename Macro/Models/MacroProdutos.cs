using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroProdutos
    {
        public string id { get; set; }
        //public string id_novo { get; set; }
        public string referencia { get; set; }

        [JsonIgnore]
        public string CodigoSistema { get; set; }
        public string descricao_curta { get; set; }
        //public string descricao_longa { get; set; }
        //public string observacao { get; set; }
        //public DateTime criacao { get; set; }
        public DateTime publicacao { get; set; }
        public DateTime expiracao { get; set; }
        public int multiplicador { get; set; }
        public int minimo { get; set; }
        public int lancamento { get; set; }
        //public string sob_encomenda { get; set; }
        public string [] grupos { get; set; }
        //public string [] caracteristicas { get; set; }
        //public List<MacroEspecificacoes> especificacoes { get; set; }
        public List<MacroProdutosVariacoes> variacoes { get; set; }
        public List<MacroEspecificacoes> especificacoes { get; set; }


        public MacroProdutos()
        {
            id = "";
            //id_novo = "";
            referencia = "";
            descricao_curta = "";
            CodigoSistema = "";
            //observacao = "";
            //criacao = DateTime.Now;
            publicacao = DateTime.Now;
            expiracao = DateTime.Now;
            multiplicador = 1;
            lancamento = 0;
            minimo = 1;
            //sob_encomenda = "0";
            grupos = null;
            //caracteristicas = null;
            variacoes = null;
            especificacoes = null;
        }

    }
}
