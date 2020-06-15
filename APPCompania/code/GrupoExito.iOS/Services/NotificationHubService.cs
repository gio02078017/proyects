using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Resources;
using Microsoft.AppCenter.Crashes;
using WindowsAzure.Messaging;

namespace GrupoExito.iOS.Services
{
    public class NotificationHubService : IDisposable
    {
        #region Attributes

        private SBNotificationHub _notificationHub { get; set; }
        private NSData _token { get; set; }

        #endregion

        #region Properties 
        #endregion

        #region Constructors

        public NotificationHubService(NSData deviceToken)
        {
            this._token = deviceToken;
        }

        public void Dispose()
        {
            _notificationHub.Dispose();
        }

        #endregion

        #region Methods

        public void RegisterNotification()
        {
            try
            {
                _notificationHub = new SBNotificationHub(AppConfigurations.ListenConnectionString, AppConfigurations.NotificationHubName);
                _notificationHub.UnregisterAllAsync(_token, (error) =>
                {
                    if (error != null)
                    {
                        return;
                    }

                    string splitTags = AppServiceConfiguration.SiteId;
                    string cityId = GetCityId();

                    if (ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.DocumentNumber))
                    {
                        splitTags += "," + ParametersManager.UserContext.DocumentNumber;
                    }

                    if (!string.IsNullOrEmpty(cityId))
                    {
                        splitTags += "," + cityId;
                    }

                    string[] tagsArray = splitTags.Split(',');
                    NSSet tags = new NSSet(tagsArray);
                    _notificationHub.RegisterNativeAsync(_token, tags, (errorCallback) =>
                    {
                    });
                });

            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantControllersName.NotificationHubService, ConstantMethodName.RegisterNotification } };
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

        #endregion
    }
}
