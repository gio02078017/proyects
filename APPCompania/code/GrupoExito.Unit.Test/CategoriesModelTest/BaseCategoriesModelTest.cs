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

namespace GrupoExito.Test.CategoriesModelTest
{
    public class BaseCategoriesModelTest
    {
        protected HttpResponseMessage FakeResponseWithStatusCode;
        protected MessagesContainer FakeResultWithMessage;

        protected Mock<ICategoriesService> CategoriesService;
        protected Mock<ICryptographyService> CryptographyService;
        protected Mock<IDiffieHellmanService> DiffieHellmanService;
        protected Mock<IHttpClientService> HttpClientService;
        protected Mock<IJsonService> JsonService;
        protected CategoriesModel Model;

        public BaseCategoriesModelTest()
        {
            CategoriesService = new Mock<ICategoriesService>();
            CryptographyService = new Mock<ICryptographyService>();
            DiffieHellmanService = new Mock<IDiffieHellmanService>();
            HttpClientService = new Mock<IHttpClientService>();
            JsonService = new Mock<IJsonService>();
            Model = new CategoriesModel(CategoriesService.Object);
            FakeResponseWithStatusCode = new HttpResponseMessage();
            FakeResultWithMessage = new MessagesContainer() { HasErrors = false, Messages = new List<MessageAplication>() };
        }
    }
}
