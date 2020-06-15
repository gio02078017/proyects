using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.Entities;
using GrupoExito.Models.Contracts;
using GrupoExito.Models.Models;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class AddressCorrespondenceViewModel : BaseViewModel
    {
        public ICorrespondenceAddressModel Delegate { get; set; }

        public Command GetCitiesCommand { get; set; }
        public Command SaveAddressCommand { get; set; }

        private BaseAddressModel baseAddressModel;

        private IList<City> cities;

        public AddressCorrespondenceViewModel(IDeviceManager deviceManager)
        {
            baseAddressModel = new BaseAddressModel(deviceManager);
            cities = new List<City>();

            GetCitiesCommand = new Command(async () => await ExecuteGetCitiesCommand());
            SaveAddressCommand = new Command<UserAddress>(async (userAddress) => await ExecuteSaveAddressCommand(userAddress));
        }

        public string ValidateMobile(string mobile)
        {
            return baseAddressModel.ValidateMobile(mobile);
        }

        public string ValidateAddress(UserAddress userAddress)
        {
            return baseAddressModel.ValidateCorrespondenceAddress(userAddress);
        }

        protected async Task ExecuteGetCitiesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (!cities.Any())
                {
                    CitiesFilter parameters = new CitiesFilter() { HomeDelivery = "true", Pickup = "false" };
                    cities = await baseAddressModel.GetCities(parameters);
                }

                Delegate?.CitiesFetched(cities);
            }
            catch (Exception ex)
            {
                Delegate?.HandleError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async Task ExecuteSaveAddressCommand(UserAddress address)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await baseAddressModel.SaveAddress(address);
                Delegate?.AddressSaved();
            }
            catch (Exception ex)
            {
                Delegate?.HandleError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
