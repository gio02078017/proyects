using System;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Utilities;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    public class FilappController
    {
        private static FilappController instance;
        private CashDrawerTurnModel cashDrawerTurnModel;
        private string firebaseToken;
        public Action FirebaseTokenRefreshed { get; set; }

        public static FilappController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FilappController();
                }

                return instance;
            }
        }

        private FilappController()
        {
            cashDrawerTurnModel = new CashDrawerTurnModel(new CashDrawerTurnService(DeviceManager.Instance));
        }

        internal void SetFirebaseToken(string newToken)
        {
            if (!string.IsNullOrEmpty(newToken))
            {
                firebaseToken = newToken;
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.FirebaseToken, firebaseToken);
                FirebaseTokenRefreshed?.Invoke();
            }
        }

        internal async Task<bool> CheckFilappRegistration()
        {
            bool result = false;
            if (!string.IsNullOrEmpty(firebaseToken))
            {
                DeviceResponse getMobileResponse = await GetMobileId();
                if (!string.IsNullOrEmpty(getMobileResponse.MobileId))
                {
                    SaveMobileId(getMobileResponse.MobileId);
                    DeviceResponse updateResponse = await UpdateDevicePushToken(firebaseToken);
                    result = updateResponse.Success;
                }
                else
                {
                    bool deviceNotFound = CheckErrorType(getMobileResponse, AppMessages.deviceNotFound);

                    if (deviceNotFound)
                    {
                        DeviceResponse registerResponse = await RegisterDevice(firebaseToken);
                        if (registerResponse.Success)
                        {
                            SaveMobileId(getMobileResponse.MobileId);
                            DeviceResponse updateResponse = await UpdateDevicePushToken(firebaseToken);
                            result = updateResponse.Success;
                        }
                        else
                        {

                        }
                    }
                }
            }

            return result;
        }

        internal async Task<DeviceResponse> GetMobileId()
        {
            AppDevice appDevice = new AppDevice
            {
                DeviceId = DeviceManager.Instance.GetDeviceId()
            };
            DeviceResponse responseGetMobile = await cashDrawerTurnModel.GetMobileId(appDevice);
            return responseGetMobile;
        }

        internal void SaveMobileId(string mobileId)
        {
            if(!string.IsNullOrEmpty(mobileId))
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.MobileId, mobileId);
        }

        internal async Task<DeviceResponse> RegisterDevice(string firebaseToken)
        {
            AppDevice appDevice = new AppDevice
            {
                DeviceId = DeviceManager.Instance.GetDeviceId(),
                TokenNotificationPush = firebaseToken
            };
            DeviceResponse responseDevice = await cashDrawerTurnModel.Device(appDevice);
            return responseDevice;
        }

        internal async Task<DeviceResponse> UpdateDevicePushToken(string firebaseToken)
        {
            AppDevice appDevice = new AppDevice
            {
                DeviceId = DeviceManager.Instance.GetDeviceId(),
                TokenNotificationPush = firebaseToken
            };
            DeviceResponse responseGetMobile = await cashDrawerTurnModel.UpdateDevice(appDevice);
            return responseGetMobile;
        }

        internal bool CheckErrorType(DeviceResponse response, string errorMessage)
        {
            bool result = false;
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                if (response.Result.Messages.Any())
                {
                    if (response.Result.Messages[0].Description.Equals(errorMessage))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
