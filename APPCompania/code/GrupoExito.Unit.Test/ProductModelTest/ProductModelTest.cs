using GrupoExito.Entities;
using GrupoExito.Entities.Responses;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.ProductModelTest
{
    public class ProductModelTest : BaseProductModelTest
    {
        [Fact]
        public async Task GetProductSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            var product = new Product() { Id = "000000000", Name = "Any" };

            ProductResponse response = new ProductResponse()
            {
                Product = product,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Parameters = new ProductParameters()
            {
                ProductId = "000000000"
            };

            ProductsService.Setup(_ => _.GetProduct(Parameters)).ReturnsAsync(response);
            var actual = await ProductModel.GetProduct(Parameters);

            Assert.True(actual.Product != null);
            Assert.Equal(actual.Product.Id, Parameters.ProductId);
        }

        [Fact]
        public async Task GetProductFailed()
        {            
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;

            ProductResponse response = new ProductResponse()
            {
                Product = null,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            ProductsService.Setup(_ => _.GetProduct(Parameters)).ReturnsAsync(response);
            var actual = await ProductModel.GetProduct(Parameters);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Product == null);
            ProductsService.VerifyAll();
        }
    }
}
