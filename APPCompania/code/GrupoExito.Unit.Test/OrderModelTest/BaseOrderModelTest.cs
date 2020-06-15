using GrupoExito.Entities;
using GrupoExito.Entities.Containers;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Utilities.Contracts;
using GrupoExito.Utilities.Contracts.Addresses;
using GrupoExito.Utilities.Contracts.Base;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Contracts.Orders;
using Moq;
using System.Collections.Generic;
using System.Net.Http;

namespace GrupoExito.Test.OrderModelTest
{
    public class BaseOrderModelTest
    {
        protected HttpResponseMessage FakeResponseWithStatusCode;
        protected MessagesContainer FakeResultWithMessage;
        protected UserCredentials userCredentials;
        protected OrderModel Model;

        protected Mock<IOrderService> orderService;
        protected Mock<ICryptographyService> CryptographyService;
        protected Mock<IDiffieHellmanService> DiffieHellmanService;
        protected Mock<IHttpClientService> HttpClientService;
        protected Mock<IJsonService> JsonService;
        protected Mock<IAddressService> AddressService;

        public BaseOrderModelTest()
        {
            orderService = new Mock<IOrderService>();
            CryptographyService = new Mock<ICryptographyService>();
            DiffieHellmanService = new Mock<IDiffieHellmanService>();
            HttpClientService = new Mock<IHttpClientService>();
            JsonService = new Mock<IJsonService>();
            AddressService = new Mock<IAddressService>();
            Model = new OrderModel(orderService.Object);
            FakeResponseWithStatusCode = new HttpResponseMessage();
            FakeResultWithMessage = new MessagesContainer() { HasErrors = false, Messages = new List<MessageAplication>() };
        }
    }
}
