using GrupoExito.Entities;
using GrupoExito.Entities.Containers;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Contracts;
using GrupoExito.Utilities.Contracts.Addresses;
using GrupoExito.Utilities.Contracts.Base;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Contracts.Users;
using Moq;
using System.Collections.Generic;
using System.Net.Http;

namespace GrupoExito.Test.MyAccountModelTest
{
    public class BaseMyAccountModelTest 
    {
        protected HttpResponseMessage FakeResponseWithStatusCode;
        protected MessagesContainer FakeResultWithMessage;
        protected MyAccountModel Model;
        protected Mock<IUserService> userService;
        protected Mock<ICryptographyService> CryptographyService;
        protected Mock<IDiffieHellmanService> DiffieHellmanService;
        protected Mock<IHttpClientService> HttpClientService;
        protected Mock<IJsonService> JsonService;
        protected Mock<IDocumentTypesService> documentTypesService;
        protected Mock<IAddressService> AddressService;

        public BaseMyAccountModelTest()
        {
            documentTypesService = new Mock<IDocumentTypesService>();
            userService = new Mock<IUserService>();
            CryptographyService = new Mock<ICryptographyService>();
            DiffieHellmanService = new Mock<IDiffieHellmanService>();
            HttpClientService = new Mock<IHttpClientService>();
            JsonService = new Mock<IJsonService>();
            AddressService = new Mock<IAddressService>();
            Model = new MyAccountModel(userService.Object);
            FakeResponseWithStatusCode = new HttpResponseMessage();
            FakeResultWithMessage = new MessagesContainer() { HasErrors = false, Messages = new List<MessageAplication>() };
        }
    }
}
