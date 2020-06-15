using Android.Content;
using Firebase.Iid;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Utilities.Resources;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using WindowsAzure.Messaging;

namespace GrupoExito.Android.Services
{
    public class NotificationHubService : IDisposable
    {
        private NotificationHub notificationHub;

        public Context _context { get; set; }

        public NotificationHubService(Context context)
        {
            _context = context;
        }

        public void Dispose()
        {
            notificationHub.Dispose();
        }

        public void RegisterNotification()
        {
            try
            {
                string token = FirebaseInstanceId.Instance.Token;

                if (notificationHub == null)
                {
                    notificationHub = new NotificationHub(AppConfigurations.NotificationHubName,
                                      AppConfigurations.ListenConnectionString, _context);
                }

                var tags = new List<string>() { };
                tags.Add(AppServiceConfiguration.SiteId);

                if (ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.DocumentNumber))
                {
                    tags.Add(ParametersManager.UserContext.DocumentNumber);
                }

                string cityId = GetCityId();

                if (!string.IsNullOrEmpty(cityId))
                {
                    tags.Add(cityId);
                }

                if (!string.IsNullOrEmpty(token))
                {
                    if (!string.IsNullOrEmpty(DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.FirebaseToken)))
                    {
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.TokenRefreshed, true);
                    }
                    else
                    {
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.TokenCreated, true);
                    }

                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.FirebaseToken, token);
                    UnregisterNotification();

                    notificationHub.Register(token, tags.ToArray());
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.NotificationHubService, ConstantMethodName.RegisterNotification } };
                Crashes.TrackError(exception, properties);
            }
        }

        public void UnregisterNotification()
        {
            try
            {
                if (notificationHub == null)
                {
                    notificationHub = new NotificationHub(AppConfigurations.NotificationHubName,
                                     AppConfigurations.ListenConnectionString, AndroidApplication.Context);
                }

                notificationHub.Unregister();
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.NotificationHubService, ConstantMethodName.UnregisterNotification } };
                Crashes.TrackError(exception, properties);
            }
        }

        private string GetCityId()
        {
            UserContext user = ParametersManager.UserContext;

            if (user != null)
            {
                return user.Address != null ? user.Address.CityId : user.Store != null ? user.Store.CityId : string.Empty;
            }

            return string.Empty;
        }
    }
}