using GrupoExito.Entities;
using GrupoExito.Entities.Containers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Contracts;
using GrupoExito.Utilities.Contracts.Base;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Contracts.Products;
using Moq;
using System.Collections.Generic;
using System.Net.Http;

namespace GrupoExito.Test.HomeModelTest
{
    public class BaseHomeModelTest
    {
        protected HttpResponseMessage FakeResponseWithStatusCode;
        protected MessagesContainer FakeResultWithMessage;
        protected SearchProductsParameters Parameters;
        protected HomeModel HomeModelBusiness;

        protected Mock<IHomeService> HomeService;
        protected Mock<ICryptographyService> CryptographyService;
        protected Mock<IDiffieHellmanService> DiffieHellmanService;
        protected Mock<IHttpClientService> HttpClientService;
        protected Mock<IJsonService> JsonService;

        public BaseHomeModelTest()
        {
            HomeService = new Mock<IHomeService>();
            CryptographyService = new Mock<ICryptographyService>();
            DiffieHellmanService = new Mock<IDiffieHellmanService>();
            HttpClientService = new Mock<IHttpClientService>();
            JsonService = new Mock<IJsonService>();
            HomeModelBusiness = new HomeModel(HomeService.Object);
            FakeResponseWithStatusCode = new HttpResponseMessage();
            FakeResultWithMessage = new MessagesContainer() { HasErrors = false, Messages = new List<MessageAplication>() };
            Parameters = new SearchProductsParameters();
        }
    }
}
