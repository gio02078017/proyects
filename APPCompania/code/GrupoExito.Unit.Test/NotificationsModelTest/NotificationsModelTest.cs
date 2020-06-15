using GrupoExito.Entities;
using GrupoExito.Entities.Responses.Users;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.NotificationsModelTest
{
    public class NotificationsModelTest : BaseNotificationsModelTest
    {
        [Fact]
        public async Task GetNotificationsSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            List<AppNotification> notifications = new List<AppNotification>
            {
                new AppNotification() { IdContent = 01 }
            };
            NotificationsResponse response = new NotificationsResponse()
            {
                Notifications = notifications,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetNotifications()).ReturnsAsync(response);
            var actual = await Model.GetNotifications();

            Assert.True(actual.Notifications.Any());
            Service.VerifyAll();
        }

        [Fact]
        public async Task GetNotificationsFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            List<AppNotification> notifications = new List<AppNotification>();

            NotificationsResponse response = new NotificationsResponse()
            {
                Notifications = notifications,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetNotifications()).ReturnsAsync(response);
            var actual = await Model.GetNotifications();

            Assert.False(actual.Notifications.Any());
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }
    }
}
