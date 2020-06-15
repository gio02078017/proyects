using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.iOS.Referentials
{
    public class BaseAddressController : UIViewControllerBase
    {
        #region Attributes
        protected AddressModel _addressModel;
        #endregion

        #region Constructors 
        public BaseAddressController(IntPtr handle) : base(handle)
        {
            _addressModel = new AddressModel(new AddressService(DeviceManager.Instance));
        }
        #endregion

        #region Methods Async 
        public async Task<IList<UserAddress>> GetAddresses()
        {
            IList<UserAddress> address = new List<UserAddress>();
            try
            {
                if (_spinnerActivityIndicatorView != null)
                {
                    StartActivityIndicatorCustom();
                }
                AddressResponse response = await _addressModel.GetAddress();
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        if (_spinnerActivityIndicatorView != null)
                        {
                            if (!EnumErrorCode.UnknownError.ToString().Equals(response.Result.Messages[0].Code.ToString()))
                            {
                                string message = MessagesHelper.GetMessage(response.Result);
                                StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                            }
                        }
                    }
                }
                else
                {
                    if (response.Addresses != null && response.Addresses.Any())
                    {
                        if (_spinnerActivityIndicatorView != null)
                        {
                            _spinnerActivityIndicatorView.CodeMesage = string.Empty;
                        }
                        address = response.Addresses;
                    }
                }
            }
            catch (Exception exception)
            {
                if (_spinnerActivityIndicatorView != null)
                {
                    StopActivityIndicatorCustom();
                }
                Util.LogException(exception, ConstantControllersName.BaseAddressController, ConstantMethodName.GetAddress);
                ShowMessageException(exception);
            }
            return address;
        }

        public async Task DeleteAddress(UserAddress address)
        {
            try
            {
                var response = await _addressModel.DeleteAddress(address);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseAddressController, ConstantMethodName.DeleteAddress);
                ShowMessageException(exception);
            }
        }

        protected async Task<IList<City>> LoadCitiesAddresses()
        {
            IList<City> cities = new List<City>();

            try
            {
                CitiesFilter parameters = new CitiesFilter() { HomeDelivery = "true", Pickup = "false" };
                var response = await _addressModel.GetCities(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    cities = response.Cities;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseAddressController, ConstantMethodName.LoadCitiesAddresses);
                ShowMessageException(exception);
            }

            return cities;
        }

        public async Task<IList<City>> LoadCitiesStore()
        {
            IList<City> cities = new List<City>();

            try
            {
                CitiesFilter parameters = new CitiesFilter() { HomeDelivery = "true", Pickup = "true" };
                var response = await _addressModel.GetCities(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    cities = response.Cities;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseAddressController, ConstantMethodName.LoadCitiesStore);
                ShowMessageException(exception);
            }

            return cities;
        }

        public async Task<IList<Store>> LoadStore(StoreParameters parameters)
        {
            IList<Store> stores = new List<Store>();

            try
            {
                var response = await _addressModel.GetStores(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    stores = response.Stores;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseAddressController, ConstantMethodName.LoadStore);
                ShowMessageException(exception);
            }

            return stores;
        }

        public async Task<IList<String>> AutoCompleteAddress(string address)
        {
            List<string> predictions = new List<string>();

            try
            {
                var response = await _addressModel.AutoCompleteAddress(address);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    predictions = response.Predictions.ToList()
                        .Where(x => x.Description.ToLower().Contains(AppConfigurations.CountryGeolocation))
                        .Select(x => x.Description).ToList();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseAddressController, ConstantMethodName.AutoCompleteAddress);
                ShowMessageException(exception);
            }

            return predictions;
        }

        public async Task<ResponseBase> UpdateDispatchRegion(UpdateDispatchRegionParameters parameters)
        {
            ResponseBase response = await _addressModel.UpdateDispatchRegion(parameters);
            return response;
        }

        #endregion
    }
}
