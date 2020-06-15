//namespace GrupoExito.Test.HomeModelTest
//{
//    using GrupoExito.Entities;
//    using GrupoExito.Entities.Responses;
//    using Moq;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using Xunit;

//    public class HomeModelTest : BaseHomeModelTest
//    {
//        [Fact]
//        public async Task GetCustomerProductsSucessfull()
//        {
//            //Arrange
//            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
//            FakeResultWithMessage.Messages.Add(new MessageAplication() { Code = "Any", Description = "Any", IsError = false });

//            var customerProducts = new List<Product>()
//            {
//                new Product() { Id = "Any", Name = "Any" }
//            };

//            HomeResponse response = new HomeResponse()
//            {
//                CustomerProducts = customerProducts,
//                DiscountProducts = new List<Product>(),
//                HttpResponse = FakeResponseWithStatusCode,
//                Result = FakeResultWithMessage
//            };

//            HomeService.Setup(_ => _.HomeProducts(Parameters)).ReturnsAsync(response);

//            //Action
//            var actual = await HomeModelBusiness.HomeProducts(Parameters);

//            //Assert
//            Assert.True(actual.CustomerProducts.Any());
//            HomeService.VerifyAll();
//        }

//        [Fact]
//        public async Task GetCustomerProductsFailed()
//        {
//            //Arrange
//            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;

//            HomeResponse response = new HomeResponse()
//            {
//                CustomerProducts = new List<Product>(),
//                DiscountProducts = new List<Product>(),
//                HttpResponse = FakeResponseWithStatusCode,
//                Result = FakeResultWithMessage
//            };

//            HomeService.Setup(_ => _.HomeProducts(Parameters)).ReturnsAsync(response);
//            var actual = await HomeModelBusiness.HomeProducts(Parameters);

//            Assert.True(actual.CustomerProducts.Count == 0);
//            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
//        }

//        [Fact]
//        public async Task GetDiscountProductsSucessfull()
//        {
//            //Arrange
//            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

//            var discountProducts = new List<Product>()
//            {
//                new Product() { Id = "Any", Name = "Any" }
//            };

//            HomeResponse response = new HomeResponse()
//            {
//                CustomerProducts = new List<Product>(),
//                DiscountProducts = discountProducts,
//                HttpResponse = FakeResponseWithStatusCode,
//                Result = FakeResultWithMessage
//            };

//            HomeService.Setup(_ => _.HomeProducts(Parameters)).ReturnsAsync(response);

//            //Action
//            var actual = await HomeModelBusiness.HomeProducts(Parameters);

//            // Assert
//            Assert.True(actual.DiscountProducts.Any());
//            HomeService.VerifyAll();
//        }

//        [Fact]
//        public async Task GetDiscountProductsFailed()
//        {
//            //Arrange
//            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
//            FakeResultWithMessage.Messages.Add(new MessageAplication() { Code = "Any", Description = "Any", IsError = true });
//            var customerProducts = new List<Product>();
//            customerProducts.Add(new Product() { Id = "Any", Name = "Any" });

//            HomeResponse response = new HomeResponse()
//            {
//                CustomerProducts = new List<Product>(),
//                DiscountProducts = new List<Product>(),
//                HttpResponse = FakeResponseWithStatusCode,
//                Result = FakeResultWithMessage
//            };

//            HomeService.Setup(_ => _.HomeProducts(Parameters)).ReturnsAsync(response);

//            //Action
//            var actual = await HomeModelBusiness.HomeProducts(Parameters);

//            //Assert
//            Assert.True(actual.DiscountProducts.Count == 0);
//            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
//            HomeService.VerifyAll();
//        }
//    }
//}
