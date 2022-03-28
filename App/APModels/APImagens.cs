using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.App.APModels
{
    public class APImagens
    {
        public string NomeImagem { get; set; }
        public string Extensao { get; set; }
        public byte[] ImagemByte { get; set; }
        public string ImageBase64 { get; set; }

        public APImagens()
        {
            NomeImagem = "";
            Extensao = "";
            ImageBase64 = "";
            ImagemByte = null;
        }
    }
}
