namespace GrupoExito.Entities.Responses.Users
{
    using GrupoExito.Entities.Responses.Base;
    using System.Collections.Generic;

    public class NotificationsResponse : ResponseBase
    {
        public NotificationsResponse()
        {
            Notifications = new List<AppNotification>();
        }

        public IList<AppNotification> Notifications { get; set; }
    }
}
