﻿using GrupoExito.Entities;
using GrupoExito.Entities.Containers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Contracts;
using GrupoExito.Utilities.Contracts.Base;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Contracts.Products;
using Moq;
using System.Collections.Generic;
using System.Net.Http;

namespace GrupoExito.Test.ProductsModelTest
{
    public class BaseProductsModelTest
    {
        protected HttpResponseMessage FakeResponseWithStatusCode;
        protected MessagesContainer FakeResultWithMessage;

        protected Mock<IProductsService> ProductsService;
        protected Mock<ICryptographyService> CryptographyService;
        protected Mock<IDiffieHellmanService> DiffieHellmanService;
        protected Mock<IHttpClientService> HttpClientService;
        protected Mock<IJsonService> JsonService;
        protected ProductsModel ProductsModel;
        protected SearchProductsParameters Parameters;

        public BaseProductsModelTest()
        {
            ProductsService = new Mock<IProductsService>();
            CryptographyService = new Mock<ICryptographyService>();
            DiffieHellmanService = new Mock<IDiffieHellmanService>();
            HttpClientService = new Mock<IHttpClientService>();
            JsonService = new Mock<IJsonService>();
            ProductsModel = new ProductsModel(ProductsService.Object);
            FakeResultWithMessage = new MessagesContainer() { HasErrors = false, Messages = new List<MessageAplication>() };
            FakeResponseWithStatusCode = new System.Net.Http.HttpResponseMessage();
            Parameters = new SearchProductsParameters();
        }
    }
}
