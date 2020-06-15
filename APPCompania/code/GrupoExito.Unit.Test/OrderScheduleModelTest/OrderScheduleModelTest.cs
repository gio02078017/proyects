using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses;
using GrupoExito.Entities.Responses.Orders;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.OrderScheduleModelTest
{
    public class OrderScheduleModelTest : BaseOrderScheduleModelTest
    {
        [Fact]
        public async Task GetGetOrderScheduleSuccessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            OrderScheduleParameters parameters = new OrderScheduleParameters();
            IList<ScheduleDays> Schedules = new List<ScheduleDays>
            {
                new ScheduleDays { Id = 1 }
            };

            var response = new OrderScheduleResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Schedules = Schedules
            };

            OrderScheduleService.Setup(_ => _.GetOrderSchedule(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetOrderSchedule(parameters);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
            Assert.True(actual.Schedules.Any());
        }

        [Fact]
        public async Task GetGetOrderScheduleFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            OrderScheduleParameters parameters = new OrderScheduleParameters();
            IList<ScheduleDays> Schedules = new List<ScheduleDays>();
            var response = new OrderScheduleResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Schedules = Schedules
            };

            OrderScheduleService.Setup(_ => _.GetOrderSchedule(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetOrderSchedule(parameters);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
            Assert.False(actual.Schedules.Any());
        }

        [Fact]
        public async Task ScheduleReservationSuccessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            ScheduleReservationParameters parameters = new ScheduleReservationParameters() { OrderId = "123456" };

            var response = new ScheduleReservationResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            OrderScheduleService.Setup(_ => _.ScheduleReservation(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.ScheduleReservation(parameters);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task ScheduleReservationFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            ScheduleReservationParameters parameters = new ScheduleReservationParameters() { OrderId = "123456" };

            var response = new ScheduleReservationResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            OrderScheduleService.Setup(_ => _.ScheduleReservation(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.ScheduleReservation(parameters);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task ValidateScheduleReservationSuccessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            ScheduleReservationParameters parameters = new ScheduleReservationParameters() { OrderId = "123456" };

            var response = new ValidateScheduleReservationResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            OrderScheduleService.Setup(_ => _.ValidateScheduleReservation(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.ValidateScheduleReservation(parameters);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task ValidateScheduleReservationFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            ScheduleReservationParameters parameters = new ScheduleReservationParameters() { OrderId = "123456" };

            var response = new ValidateScheduleReservationResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            OrderScheduleService.Setup(_ => _.ValidateScheduleReservation(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.ValidateScheduleReservation(parameters);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }
    }
}
