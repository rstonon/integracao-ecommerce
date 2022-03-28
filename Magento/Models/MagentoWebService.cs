using IntegracaoRockye.MagentoProducaoServices;
//using IntegracaoRockye.MagentoHLGServices;

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegracaoRockye.Magento.Models
{
    public class MagentoWebService
    {
        private string user;
        private string password;

        private string sessionId;

        public MagentoWebService()
        {
            sessionId = CreateSession();
        }

        public string CreateSession()
        {
            user = "fanes_api";
            password = "!FaN_2020@";

            PortTypeClient port = new PortTypeClient();
            return port.login(user, password);
        }

        public void CloseSession()
        {
            PortTypeClient port = new PortTypeClient();
            port.endSession(sessionId);
        }


        //Cadastra o Produto 
        public int CreateProduct(MagentoProdutos produtos)
        {
            PortTypeClient port = new PortTypeClient();

            var product = new catalogProductCreateEntity();
            product.name = produtos.Nome;
            product.description = produtos.Descricao;
            product.short_description = produtos.DescricaoCurta;
            product.price = produtos.PraticadoEcommerce.ToString();
            product.weight = produtos.Peso; 

            //product.special_from_date = "2020-10-19";
            //product.special_to_date = "2020-10-19";

            product.stock_data = new catalogInventoryStockItemUpdateEntity();
            product.stock_data.qty = produtos.EstoqueDisponivel.ToString();

            //Atributos
            associativeEntity atributesAltura = new associativeEntity();
            atributesAltura.key = "volume_altura";
            atributesAltura.value = produtos.Altura;

            associativeEntity atributesComprimento = new associativeEntity();
            atributesComprimento.key = "volume_comprimento";
            atributesComprimento.value = produtos.Comprimento;

            associativeEntity atributesLargura = new associativeEntity();
            atributesLargura.key = "volume_largura";
            atributesLargura.value = produtos.Largura;

            associativeEntity atributesImage = new associativeEntity();
            atributesImage.key = "image";
            atributesImage.value = produtos.Imagem;

            associativeEntity atributesTamanho = new associativeEntity();
            atributesTamanho.key = "tamanho";
            atributesTamanho.value = produtos.Tamanho;

            associativeEntity atributesMaterial = new associativeEntity();
            atributesMaterial.key = "material";
            atributesMaterial.value = produtos.Material;

            associativeEntity atributesColor = new associativeEntity();
            atributesColor.key = "color";
            atributesColor.value = produtos.Cor;

           // var material = port.catalogProductAttributeInfo(sessionId, "material");
           // var tamanho = port.catalogProductAttributeInfo(sessionId, "tamanho");
           // var color = port.catalogProductAttributeInfo(sessionId, "color");
           // var image = port.catalogProductAttributeInfo(sessionId, "image");


            associativeEntity[] atributes = new associativeEntity[7];
            atributes[0] = atributesAltura;
            atributes[1] = atributesComprimento;
            atributes[1] = atributesComprimento;
            atributes[2] = atributesLargura;
            atributes[3] = atributesImage;
            atributes[4] = atributesTamanho;
            atributes[5] = atributesMaterial;
            atributes[6] = atributesColor;


            product.additional_attributes = new catalogProductAdditionalAttributesEntity
            {
                single_data = atributes,
            };

            product.status = produtos.Status;
            product.visibility = "4";

            //SUB GRUPO/CATEGORIA
            //product.categories = new[] 
            //{
            //    produtos.Categoria,
            //};

            var attributeSets = port.catalogProductAttributeSetList(sessionId);
            var attributeSet = attributeSets.First();

            return port.catalogProductCreate(sessionId, "simple", attributeSet.set_id.ToString(), produtos.Sku, product, "");
        }

        public Boolean UpdateProduct(MagentoProdutos produtos, string CodigoEcommerce)
        {
            PortTypeClient port = new PortTypeClient();

            var product = new catalogProductCreateEntity();
            product.name = produtos.Nome;
            product.description = produtos.Descricao;
            product.short_description = produtos.DescricaoCurta;
            product.price = produtos.PraticadoEcommerce.ToString();
            product.weight = produtos.Peso;

            //product.special_from_date = "2020-10-19";
            //product.special_to_date = "2020-10-19";

            product.stock_data = new catalogInventoryStockItemUpdateEntity();
            product.stock_data.qty = produtos.EstoqueDisponivel.ToString();

            //Atributos
            associativeEntity atributesAltura = new associativeEntity();
            atributesAltura.key = "volume_altura";
            atributesAltura.value = produtos.Altura;

            associativeEntity atributesComprimento = new associativeEntity();
            atributesComprimento.key = "volume_comprimento";
            atributesComprimento.value = produtos.Comprimento;

            associativeEntity atributesLargura = new associativeEntity();
            atributesLargura.key = "volume_largura";
            atributesLargura.value = produtos.Largura;

            associativeEntity atributesImage = new associativeEntity();
            atributesImage.key = "image";
            atributesImage.value = produtos.Imagem;

            associativeEntity atributesTamanho = new associativeEntity();
            atributesTamanho.key = "tamanho";
            atributesTamanho.value = produtos.Tamanho;

            associativeEntity atributesMaterial = new associativeEntity();
            atributesMaterial.key = "material";
            atributesMaterial.value = produtos.Material;

            associativeEntity atributesColor = new associativeEntity();
            atributesColor.key = "color";
            atributesColor.value = produtos.Cor;

            // var material = port.catalogProductAttributeInfo(sessionId, "material");
            // var tamanho = port.catalogProductAttributeInfo(sessionId, "tamanho");
            // var color = port.catalogProductAttributeInfo(sessionId, "color");
            // var image = port.catalogProductAttributeInfo(sessionId, "image");


            associativeEntity[] atributes = new associativeEntity[7];
            atributes[0] = atributesAltura;
            atributes[1] = atributesComprimento;
            atributes[1] = atributesComprimento;
            atributes[2] = atributesLargura;
            atributes[3] = atributesImage;
            atributes[4] = atributesTamanho;
            atributes[5] = atributesMaterial;
            atributes[6] = atributesColor;


            product.additional_attributes = new catalogProductAdditionalAttributesEntity
            {
                single_data = atributes,
            };

            product.status = produtos.Status;
            product.visibility = "4";

            //SUB GRUPO/CATEGORIA
            product.categories = new[]
            {
                produtos.Categoria,
            };

            var attributeSets = port.catalogProductAttributeSetList(sessionId);
            var attributeSet = attributeSets.First();

            return port.catalogProductUpdate(sessionId, CodigoEcommerce, product, "", "");
        }


        //Atualiza o Estoque do Produto
        public int UpdateStock(string productId, decimal qnt)
        {
            PortTypeClient port = new PortTypeClient();
            var stock_data = new catalogInventoryStockItemUpdateEntity();
            stock_data.qty = qnt.ToString();

            return port.catalogInventoryStockItemUpdate(sessionId, productId, stock_data);
        }

        //Atualiza o Praticado do Produto
        public bool UpdatePraticadoProduct(string product_id, string qty_product)
        {
            PortTypeClient port = new PortTypeClient();

            var product = new catalogProductCreateEntity();
            product.price = qty_product;

            return port.catalogProductUpdate(sessionId, product_id, product, "", "");
        }

        //Consulta os Pedidos
        public salesOrderListEntity[] getOrderList()
        {
            PortTypeClient port = new PortTypeClient();
            filters filters = new filters();

            filters.filter = new[]
            {
                    new associativeEntity
                    {
                        key = "status",
                        value = "Processing"
                    }
                };

            filters.complex_filter = new[]
            {
                new complexFilter
                {
                    key = "created_at",
                    value = new associativeEntity
                    {
                        key = "from",
                        value = DateTime.Now.Date.AddDays(-5).ToString("yyyy-MM-dd")
                    }
                }
            };

            return port.salesOrderList(sessionId, filters);
        }

        //Consulta os Dados do Pedido
        public salesOrderEntity getOrderData(string order_id)
        {
            PortTypeClient port = new PortTypeClient();
            return port.salesOrderInfo(sessionId, order_id);
        }

        //Consulta os dados do Cliente
        public customerCustomerEntity getConsumerData(int consumer_id)
        {
            string[] atributes = new string[0];
            PortTypeClient port = new PortTypeClient();
            return port.customerCustomerInfo(sessionId, consumer_id, null);
        }

        //Consulta o Endereço do Cliente
        public customerAddressEntityItem[] getAdressListCustumer(int consumer_id)
        {
            PortTypeClient port = new PortTypeClient();
            return port.customerAddressList(sessionId, consumer_id);
            //return port.customerAddressInfo(sessionId, adress_id);
        }

    }
}
