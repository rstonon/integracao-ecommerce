using IntegracaoRockye.Tray.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Versatil.Model
{
    public class VerConfiguracao
    {
        public string CodigoConfiguracao { get; set; }
        public string CodigoSubStatusSite { get; set; }
        public string CodigoSubStatusApp { get; set; }
        public string CnpjEmpresa { get; set; }
        public string CodigoContaDebito { get; set; }
        public string CodigoContaCredito { get; set; }
        public string CodigoDocumentoaVista { get; set; }
        public string CodigoDocumentoaPrazo { get; set; }
        public string CodigoVendedorPadrao { get; set; }
        public bool SincronizarAgendaApp { get; set; }
        public bool SincronizarProdutosApp { get; set; }
        public bool SincronizarHistoricoFinanceiroApp { get; set; }

        public bool UtilizarCodigoVendedorComoEmpresaAPP { get; set; }
        public bool IntegracaoVendedorEmpresa { get; set; }
        public bool EnviarProdutosdaEmpresa { get; set; }
        public bool EnviarClientesdaEmpresa { get; set; }
        public string DiretorioImagensProdutos { get; set; }
        public string VersaoRocky { get; set; }
        public bool CalcularEstoquePeloSKURocky { get; set; }
        public bool Ajustarcodigoecommerce { get; set; }
        public string PraticadoPadraoEcommerce { get; set; }
        public bool UstilizarSequenciacomoCodigoRocky { get; set; }
        public string EnviarEstoqueDeposito { get; set; }

        public string TokenMacro { get; set; }
        public string TokenRocky { get; set; }
        public string TokenApiApp { get; set; }


        public string Consumer_keyTray { get; set; }
        public string Consumer_secretTray { get; set; }
        public string CodeTray { get; set; }

        public TrayAuth AuthTray { get; set; }

        public bool EcommerceMagento { get; set; }

    }
}
