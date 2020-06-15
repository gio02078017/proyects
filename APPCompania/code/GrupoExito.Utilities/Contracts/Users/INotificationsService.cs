﻿namespace GrupoExito.Utilities.Contracts.Users
{
    using GrupoExito.Entities.Responses.Users;
    using System.Threading.Tasks;

    public interface INotificationsService
    {
        Task<NotificationsResponse> GetNotifications();
    }
}
