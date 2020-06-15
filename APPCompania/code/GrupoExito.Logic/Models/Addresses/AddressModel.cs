namespace GrupoExito.Logic.Models.Addresses
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Responses.Addresses;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Utilities.Contracts.Addresses;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class AddressModel
    {
        private IAddressService _addressService { get; set; }

        public AddressModel(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<CitiesResponse> GetCities(CitiesFilter parameters)
        {
            var response = await _addressService.GetCities(parameters);

            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                return response;
            }
            else
            {
                response.Cities.Insert(0, new City { Id = "0", Name = AppMessages.Choose });
                return response;
            }
        }

        public async Task<ResponseBase> AddAddress(UserAddress address)
        {
            return await _addressService.AddAddress(address);
        }

        public async Task<ResponseBase> UpdateAddress(UserAddress address)
        {
            return await _addressService.UpdateAddress(address);
        }

        public async Task<ResponseBase> DeleteAddress(UserAddress address)
        {
            return await _addressService.DeleteAddress(address);
        }

        public async Task<AddressResponse> GetAddress()
        {
            return await _addressService.GetAddress();
        }

        public async Task<CoverageAddressResponse> CoverageAddress(LocationAddress location)
        {
            return await _addressService.CoverageAddress(location);
        }

        public async Task<AddressPredictionResponse> AutoCompleteAddress(string text)
        {
            return await _addressService.AutoCompleteAddress(text);
        }

        public string ValidateFields(UserAddress address)
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(address.CityId) || string.IsNullOrEmpty(address.AddressComplete)
                || string.IsNullOrEmpty(address.CellPhone) || string.IsNullOrEmpty(address.AditionalInformationAddress))
            {
                return AppMessages.RequiredFieldsText;
            }
            else if (string.IsNullOrEmpty(address.CityId))
            {
                return AppMessages.CoverageAddressMessage;
            }

            message = ValidateFieldCellPhone(address.CellPhone);
            return message;
        }

        public City GetCity(string cityName, IList<City> cities)
        {
            if (cities != null)
            {
                City city = cities.Where(x => StringFormat.RemoveDiacritics(x.Name.ToUpper()).
                Equals(StringFormat.RemoveDiacritics(cityName.ToUpper()))).FirstOrDefault();
                string cityId = city != null ? city.Id : string.Empty;
                return city;
            }
            else
            {
                return null;
            }
        }

        public string GetShortCityId(string cityName, IList<City> cities)
        {
            City city = cities.Where(x => StringFormat.RemoveDiacritics(x.Name.ToUpper()).
            Equals(StringFormat.RemoveDiacritics(cityName.ToUpper()))).FirstOrDefault();
            string cityId = city != null ? city.Id.Split('-').FirstOrDefault() : string.Empty;
            return cityId;
        }

        public string GetCityName(string cityId, IList<City> cities)
        {
            City city = cities.Where(x => StringFormat.RemoveDiacritics(x.Id.ToUpper()).
            Equals(StringFormat.RemoveDiacritics(cityId.ToUpper()))).FirstOrDefault();
            string cityName = city != null ? city.Name : string.Empty;
            return cityName;
        }

        public int GetCityPosition(string cityId, IList<City> cities)
        {
            City city = cities.Where(x => StringFormat.RemoveDiacritics(x.Id.ToUpper()).
            Equals(StringFormat.RemoveDiacritics(cityId.ToUpper()))).FirstOrDefault();
            int index = cities.IndexOf(city);
            return index;
        }

        public async Task<StoreResponse> GetStores(StoreParameters parameters)
        {
            return await _addressService.GetStores(parameters);
        }

        public async Task<StoreResponse> GetStores(SearchStoresParameters parameters)
        {
            return await _addressService.GetStores(parameters);
        }

        public async Task<ResponseBase> SetDefaultAddress(UserAddress address)
        {
            return await _addressService.SetDefaultAddress(address);
        }

        public async Task<CorrespondenceRespondese> ValidateCorrespondence()
        {
            return await _addressService.ValidateCorrespondence();
        }

        public async Task<CorrespondenceRespondese> SaveCorrespondence(UserAddress address)
        {
            return await _addressService.SaveCorrespondence(address);
        }

        public async Task<ResponseBase> UpdateDispatchRegion(UpdateDispatchRegionParameters parameters)
        {
            return await _addressService.UpdateDispatchRegion(parameters);
        }

        public string ValidateFieldsAtHome(UserAddress address)
        {
            if (string.IsNullOrEmpty(address.City) || string.IsNullOrEmpty(address.AddressComplete))
            {
                return AppMessages.RequiredFieldsText;
            }
            else if (!string.IsNullOrEmpty(address.City) && string.IsNullOrEmpty(address.CityId))
            {
                return AppMessages.CoverageAddressMessage;
            }

            return string.Empty;
        }

        public string ValidateFieldsInStore(Store store)
        {
            if (string.IsNullOrEmpty(store.Id) || string.IsNullOrEmpty(store.CityId))
            {
                return AppMessages.RequiredFieldsText;
            }

            return string.Empty;
        }

        public string ValidateFieldCellPhone(string cellPhone)
        {
            if (cellPhone.Length != 10)
            {
                return AppMessages.MobileNumberLenghtValidationText;
            }

            var mobileOperator = cellPhone.Substring(0, 3);
            var validMobileOperator = Regex.IsMatch(mobileOperator, AppConfigurations.MobilePhoneFormat);

            if (!validMobileOperator)
            {
                return AppMessages.MobileNumberOperatorValidationText;
            }

            return string.Empty;
        }

        public string ValidateFieldsAddressCorrespondence(UserAddress address)
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(address.CityId) || string.IsNullOrEmpty(address.AddressComplete)
               || string.IsNullOrEmpty(address.CellPhone))
            {
                return AppMessages.RequiredFieldsText;
            }
            else if (string.IsNullOrEmpty(address.CityId))
            {
                return AppMessages.CoverageAddressMessage;
            }

            message = ValidateFieldCellPhone(address.CellPhone);

            return message;
        }
    }
}
