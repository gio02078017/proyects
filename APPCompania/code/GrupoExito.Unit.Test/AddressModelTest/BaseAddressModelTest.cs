using GrupoExito.Entities;
using GrupoExito.Entities.Containers;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Utilities.Contracts;
using GrupoExito.Utilities.Contracts.Addresses;
using GrupoExito.Utilities.Contracts.Base;
using GrupoExito.Utilities.Contracts.Generic;
using Moq;
using System.Collections.Generic;
using System.Net.Http;

namespace GrupoExito.Test.AddressModelTest
{
    public class BaseAddressModelTest
    {
        protected HttpResponseMessage FakeResponseWithStatusCode;
        protected MessagesContainer FakeResultWithMessage;

        protected Mock<IAddressService> AddressService;
        protected Mock<ICryptographyService> CryptographyService;
        protected Mock<IDiffieHellmanService> DiffieHellmanService;
        protected Mock<IHttpClientService> HttpClientService;
        protected Mock<IJsonService> JsonService;
        protected AddressModel Model;
        public BaseAddressModelTest()
        {
            AddressService = new Mock<IAddressService>();
            CryptographyService = new Mock<ICryptographyService>();
            DiffieHellmanService = new Mock<IDiffieHellmanService>();
            HttpClientService = new Mock<IHttpClientService>();
            JsonService = new Mock<IJsonService>();
            Model = new AddressModel(AddressService.Object);
            FakeResponseWithStatusCode = new HttpResponseMessage();
            FakeResultWithMessage = new MessagesContainer() { HasErrors = false, Messages = new List<MessageAplication>() };
        }
    }
}
