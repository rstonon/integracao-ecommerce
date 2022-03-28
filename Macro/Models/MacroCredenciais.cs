using IntegracaoRockye.Versatil.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Macro.Models
{
    public class MacroCredenciais
    {
        public string versao { get; set; }
        public string chave { get; set; }

        public MacroCredenciais()
        {
            versao = "1";
            chave = DadosConfiguracao.Config.TokenMacro;
        }

    }
}
