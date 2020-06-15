using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.Entities;
using GrupoExito.Entities.Containers;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Models.Enumerations;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.Models
{
    public class BaseAddressModel
    {
        protected readonly AddressModel addressModel;

        public BaseAddressModel(IDeviceManager deviceManager)
        {
            this.addressModel = new AddressModel(new AddressService(deviceManager));
        }

        public async Task<bool> ValidateCorrespondence()
        {
            CorrespondenceRespondese response = await addressModel.ValidateCorrespondence();
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }
            else return response.HaveCorreponseAddres;
        }

        public string ValidateMobile(string mobile)
        {
            return addressModel.ValidateFieldCellPhone(mobile);
        }

        public string ValidateCorrespondenceAddress(UserAddress address)
        {
            return addressModel.ValidateFieldsAddressCorrespondence(address);
        }

        public async Task<IList<City>> GetCities(CitiesFilter parameters)
        {
            if (parameters == null) return null;

            IList<City> cities = new List<City>();
            CitiesResponse response = await addressModel.GetCities(parameters);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }
            else { cities = response.Cities; }

            return cities;
        }

        public async Task<bool> SaveAddress(UserAddress address)
        {
            CorrespondenceRespondese response = await addressModel.SaveCorrespondence(address);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }
            else
            {
                if (response.Error)
                {
                    throw CreateNewException(response.Result);
                }
                else
                {
                    return true;
                }
            }
        }

        private Exception CreateNewException(MessagesContainer result)
        {
            Exception e = new Exception();

            if (result.Messages.Any())
            {
                String message = MessagesHelper.GetMessage(result);
                if (!string.IsNullOrEmpty(message))
                {
                    e.Data.Add(nameof(EnumExceptionDataKeys.Message), message);
                }

                e.Data.Add(nameof(EnumExceptionDataKeys.Code), result.Messages[0].Code);
            }

            return e;
        }
    }
}
