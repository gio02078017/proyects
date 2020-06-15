using Android.App;
using Android.Content;
using Firebase.Iid;

namespace GrupoExito.Android.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseRegistrationService : FirebaseInstanceIdService
    {
        private NotificationHubService _notificationHubService { get; set; }

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            SendRegistrationToServer(refreshedToken);
        }

        void SendRegistrationToServer(string token)
        {
            _notificationHubService = new NotificationHubService(this);
            _notificationHubService.UnregisterNotification();
            _notificationHubService.RegisterNotification();
        }
    }
}