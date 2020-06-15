using GrupoExito.Utilities.Contracts.Analytic;

namespace GrupoExito.iOS.Utilities.Analytic
{
    public class FacebookRegistrationEventsService_Deprecated : IAnalyticsRegistrationEvent
    {
        #region Attributes
        private static FacebookRegistrationEventsService_Deprecated instance;
        #endregion

        #region Properties
        public static FacebookRegistrationEventsService_Deprecated Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FacebookRegistrationEventsService_Deprecated();
                }

                return instance;
            }
        }
        #endregion

        #region Methods
        //public void RegisterEvent(string eventName, Dictionary<string, string> parameters)
        //{
        //    NSMutableDictionary<NSString, NSString> Eventparameters = new NSMutableDictionary<NSString, NSString>();

        //    foreach (var item in parameters)
        //    {
        //        Eventparameters.Add((NSString)item.Key, NSObject.FromObject(item.Value));
        //    }

        //    AppEvents.LogEvent(eventName, Eventparameters);
        //}

        //private NSMutableDictionary<NSString, NSObject> FormatParameters(Dictionary<string, object> parameters)
        //{
        //    NSMutableDictionary<NSString, NSObject> eventParameters = new NSMutableDictionary<NSString, NSObject>();

        //    foreach (var item in parameters)
        //    {
        //        eventParameters.Add((NSString)item.Key, NSObject.FromObject(item.Value));
        //    }

        //    return eventParameters;
        //}

        //public void LoginSuccessful()
        //{
        //    if (ParametersManager.UserContext != null)
        //    {
        //        var parameters = new Dictionary<string, object>
        //        {
        //            { AppEventParameterName.ContentID, ParametersManager.UserContext.Id }
        //        };
        //        AppEvents.LogEvent(AppEventName.Subscribe, FormatParameters(parameters));
        //    }
        //}

        //public void LoginForgotPassword()
        //{
        //}

        //public void RegisterEventWithUserId(string eventName)
        //{
        //    if (ParametersManager.UserContext != null)
        //    {
        //        var parameters = new Dictionary<string, object>
        //        {
        //            { AnalyticsParameter.UserId, ParametersManager.UserContext.Id }
        //        };
        //        AppEvents.LogEvent(eventName, FormatParameters(parameters));
        //    }
        //}

        //public void RegisterEvent(string eventName)
        //{
        //    var parameters = new Dictionary<string, object>();
        //    AppEvents.LogEvent(eventName, FormatParameters(parameters));
        //}

        //public void RegisterEventShippingRelated(string eventName)
        //{
        //}

        //public void RegisterEventPickUpRelated(string eventName, string city)
        //{
        //}

        //public void RegisterEventWithUserIdAndLaunchedFrom(string EventName, string launchedFrom)
        //{
        //}

        //public void SignUp()
        //{
        //    if (ParametersManager.UserContext != null)
        //    {
        //        var parameters = new Dictionary<string, object>
        //        {
        //            { AppEventParameterName.ContentID, ParametersManager.UserContext.Id },
        //            { AppEventParameterName.Description, ParametersManager.UserContext.DateOfBirth },
        //            { AppEventParameterName.Content, ParametersManager.UserContext.CellPhone }
        //        };
        //        AppEvents.LogEvent(AppEventName.CompletedRegistration, FormatParameters(parameters));
        //    }
        //}

        //public void RecoveryPassword()
        //{
        //}

        //public void HomeCart()
        //{
        //}

        //public void WithoutCoverage(string city, string address)
        //{
        //}

        //public void RegisterEventSetDefaultAddressRelated(string eventName)
        //{
        //}

        //public void RegisterEventProductRelated(string eventName, Product product, string launchedFrom)
        //{
        //}

        //public void CategorySelected(string category)
        //{
        //}

        #endregion
    }
}
