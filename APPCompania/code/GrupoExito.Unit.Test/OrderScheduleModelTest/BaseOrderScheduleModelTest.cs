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

namespace GrupoExito.Test.OrderScheduleModelTest
{
    public class BaseOrderScheduleModelTest
    {
        protected HttpResponseMessage FakeResponseWithStatusCode;
        protected MessagesContainer FakeResultWithMessage;
        protected OrderScheduleModel Model;

        protected Mock<IOrderScheduleService> OrderScheduleService;
        protected Mock<ICryptographyService> CryptographyService;
        protected Mock<IDiffieHellmanService> DiffieHellmanService;
        protected Mock<IHttpClientService> HttpClientService;
        protected Mock<IJsonService> JsonService;
        protected Mock<IAddressService> AddressService;

        public BaseOrderScheduleModelTest()
        {
            OrderScheduleService = new Mock<IOrderScheduleService>();
            CryptographyService = new Mock<ICryptographyService>();
            DiffieHellmanService = new Mock<IDiffieHellmanService>();
            HttpClientService = new Mock<IHttpClientService>();
            JsonService = new Mock<IJsonService>();
            AddressService = new Mock<IAddressService>();
            Model = new OrderScheduleModel(OrderScheduleService.Object);
            FakeResponseWithStatusCode = new HttpResponseMessage();
            FakeResultWithMessage = new MessagesContainer() { HasErrors = false, Messages = new List<MessageAplication>() };
        }
    }
}
