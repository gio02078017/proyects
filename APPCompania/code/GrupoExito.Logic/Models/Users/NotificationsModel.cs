namespace GrupoExito.Logic.Models.Users
{
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Users;
    using System.Threading.Tasks;

    public class NotificationsModel
    {
        private INotificationsService _notificationsService { get; set; }

        public NotificationsModel(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        public async Task<NotificationsResponse> GetNotifications()
        {
            return await _notificationsService.GetNotifications();
        }
    }
}
