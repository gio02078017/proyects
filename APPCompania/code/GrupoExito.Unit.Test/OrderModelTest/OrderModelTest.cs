using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Orders;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.OrderModelTest
{
    public class OrderModelTest : BaseOrderModelTest
    {
        [Fact]
        public async Task GetOrderSuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });


            var response = new OrderResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                ActiveOrderId = "1234"
            };

            orderService.Setup(_ => _.GetOrder()).ReturnsAsync(response);

            // Action
            var actual = await Model.GetOrder();


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
            Assert.True(actual.ActiveOrderId == response.ActiveOrderId);
        }

        [Fact]
        public async Task GetOrderFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.HasErrors = true;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });


            var response = new OrderResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage

            };

            orderService.Setup(_ => _.GetOrder()).ReturnsAsync(response);

            // Action
            var actual = await Model.GetOrder();

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task GetOrdersSuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            OrderParameters parameters = new OrderParameters();

            var orders = new List<OrderInformation>
            {
                new OrderInformation() { HomeDelivery = new List<Order>() }
            };
            var response = new OrdersResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Orders = orders
            };

            orderService.Setup(_ => _.GetOrders(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetOrders(parameters);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
            Assert.True(actual.Orders.Any());
        }

        [Fact]
        public async Task GetOrdersFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.HasErrors = true;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            OrderParameters parameters = new OrderParameters();

            var response = new OrdersResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Orders = new List<OrderInformation>()
            };

            orderService.Setup(_ => _.GetOrders(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetOrders(parameters);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
            Assert.False(actual.Orders.Any());
        }

        [Fact]
        public async Task GetHistoricalOrdersSuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            OrderParameters parameters = new OrderParameters();

            var orders = new List<Order>
            {
                new Order() { Id = "" }
            };
            var response = new HistoricalOrdersResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Orders = orders
            };

            orderService.Setup(_ => _.GetHistoricalOrders(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetHistoricalOrders(parameters);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
            Assert.True(actual.Orders.Any());
        }

        [Fact]
        public async Task GetHistoricalOrdersFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.HasErrors = true;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            OrderParameters parameters = new OrderParameters();

            var response = new HistoricalOrdersResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Orders = new List<Order>()
            };

            orderService.Setup(_ => _.GetHistoricalOrders(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetHistoricalOrders(parameters);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
            Assert.False(actual.Orders.Any());
        }

        [Fact]
        public async Task GetHistoricalOrderSuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });


            var response = new OrderDetailResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Order order = new Order() { Id = "any" };

            orderService.Setup(_ => _.GetHistoricalOrder(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetHistoricalOrder(order);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task GetHistoricalOrderFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });

            var response = new OrderDetailResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage

            };

            Order order = new Order() { Id = "any" };

            orderService.Setup(_ => _.GetHistoricalOrder(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetHistoricalOrder(order);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetSummarySuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            Order order = new Order()
            {
            };

            var response = new SummaryResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Summary = new Summary() { Id = "Any" }
            };

            orderService.Setup(_ => _.GetSummary(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetSummary(order);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
            Assert.True(actual.Summary.Id == response.Summary.Id);
        }

        [Fact]
        public async Task GetSummaryFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });

            Order order = new Order()
            {
            };

            var response = new SummaryResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            orderService.Setup(_ => _.GetSummary(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetSummary(order);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task FlushCarSuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            Order order = new Order()
            {
            };

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            orderService.Setup(_ => _.FlushCar(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.FlushCar(order);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task FlushCarFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });

            FakeResultWithMessage.HasErrors = true;

            Order order = new Order()
            {
            };

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            orderService.Setup(_ => _.FlushCar(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.FlushCar(order);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task UpdateOrderNoteSuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UpdateNoteOrderParameters order = new UpdateNoteOrderParameters()
            {
            };

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            orderService.Setup(_ => _.UpdateOrderNote(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.UpdateOrderNote(order);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task UpdateOrderNoteFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UpdateNoteOrderParameters order = new UpdateNoteOrderParameters()
            {
            };

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            orderService.Setup(_ => _.UpdateOrderNote(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.UpdateOrderNote(order);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }       

        [Fact]
        public async Task GetTrackingOrderSuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });


            List<Tracking> TrackingOrders = new List<Tracking>
            {
                new Tracking() { Status = true }
            };

            var response = new TrackingOrderResponse()
            {
                TrackingOrders = TrackingOrders,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
            };

            Order order = new Order { Id = "any" };

            orderService.Setup(_ => _.GetTrackingOrder(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetTrackingOrder(order);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
            Assert.True(actual.TrackingOrders.Any());
        }

        [Fact]
        public async Task GetTrackingOrderFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.HasErrors = true;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            var response = new TrackingOrderResponse()
            {
                TrackingOrders = new List<Tracking>(),
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
            };

            Order order = new Order { Id = "any" };

            orderService.Setup(_ => _.GetTrackingOrder(order)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetTrackingOrder(order);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
            Assert.False(actual.TrackingOrders.Any());
        }
    }
}
