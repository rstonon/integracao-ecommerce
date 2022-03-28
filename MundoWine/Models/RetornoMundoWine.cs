using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.MundoWine.Models
{
    public class RetornoMundoWine
    {
        public string referencia { get; set; }
        public string mensagem { get; set; }
        public string mensage { get; set; }
        public string codigo { get; set; }
        public List<RetornoErro> errors { get; set; }

        public RetornoMundoWine()
        {
            referencia = "";
            mensagem = "";
            mensage = "";
            codigo = "";
            errors = new List<RetornoErro>();
        }

    }
}
