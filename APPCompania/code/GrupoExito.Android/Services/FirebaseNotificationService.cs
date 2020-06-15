using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Messaging;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.InStoreServices;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Utilities.Resources;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;
using android = Android;

namespace GrupoExito.Android.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            if (message.GetNotification() != null)
            {
                SendNotification(message.GetNotification().Body, message.Data);
            }
            else
            {
                SendNotification(message.Data.Values.First(), message.Data);
            }
        }

        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            if (data.Keys.Contains(AppConfigurations.TicketNameDataValue))
            {
                if (data.Keys.Contains(AppConfigurations.SlotDisplayName))
                {
                    DeviceManager.Instance.SaveAccessPreference(ConstantPreference.SlotDisplayName, data[AppConfigurations.SlotDisplayName]);
                }

                if (AndroidApplication.CurrentActivity.GetType() == typeof(CashDrawerTurnActivity) &&
                    !ParametersManager.IsInBackground)
                {
                    ((CashDrawerTurnActivity)AndroidApplication.CurrentActivity).RefreshData();
                }
                else
                {
                    var intent = new Intent(this, typeof(CashDrawerTurnActivity));
                    DrawNotification(messageBody, intent);
                }
            }
            else
            {
                var intent = new Intent(this, typeof(MainActivity));
                DrawNotification(messageBody, intent);
            }
        }

        private void DrawNotification(string messageBody, Intent intent)
        {
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            Bitmap largeIcon = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.icono_circular);
            ICharSequence notificationChannelName = new Java.Lang.String(AppConfigurations.AppNotificationTitle);
            NotificationChannel mChannel = Build.VERSION.SdkInt >= android.OS.BuildVersionCodes.O ? new NotificationChannel(AppServiceConfiguration.SiteId, notificationChannelName, NotificationImportance.High) : null;

            var notificationBuilder = new NotificationCompat.Builder(this)
             .SetContentTitle(AppConfigurations.AppNotificationTitle)
             .SetContentText(AddNameUserToTextNotification(messageBody))
             .SetContentIntent(pendingIntent)
             .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
             .SetStyle(new NotificationCompat.BigTextStyle()
             .BigText(AddNameUserToTextNotification(messageBody)))
             .SetLargeIcon(largeIcon)
             .SetAutoCancel(true)
             .SetVibrate(new long[] { 1000, 1000, 1000, 1000, 1000 })
             .SetLights(Color.Yellow, 3000, 3000);

            notificationBuilder.SetSmallIcon(GetNotificationIcon(notificationBuilder));

            var notificationManager = NotificationManager.FromContext(this);

            if (mChannel != null)
            {
                notificationBuilder.SetChannelId(AppServiceConfiguration.SiteId);
                notificationManager.CreateNotificationChannel(mChannel);
            }

            notificationManager.Notify(0, notificationBuilder.Build());
        }

        private string AddNameUserToTextNotification(string messageBody)
        {
            if (ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.FirstName))
            {
                return ParametersManager.UserContext.FirstName + ", " + messageBody;
            }
            else
            {
                return messageBody;
            }
        }

        private int GetNotificationIcon(NotificationCompat.Builder notificationBuilder)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                notificationBuilder.SetColor(Resource.Color.colorPrimary);
                return Resource.Mipmap.icono_push;
            }

            return Resource.Mipmap.icono;
        }
    }
}