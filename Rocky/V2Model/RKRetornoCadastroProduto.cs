using IntegracaoRockye.Rocky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Rocky.V2Model
{
    //Retorno do Ecommerce - Cadastro Produto V2.0
    public class RKRetornoCadastroProduto
    {
        public RKProdutos product { get; set; }

        public RKRetornoCadastroProduto()
        {
            product = new RKProdutos();
        }
    }
}
