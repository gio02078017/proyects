namespace GrupoExito.Utilities.Contracts.Addresses
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Responses.Addresses;
    using GrupoExito.Entities.Responses.Base;
    using System.Threading.Tasks;

    public interface IAddressService
    {
        Task<CitiesResponse> GetCities(CitiesFilter parameters);

        Task<CoverageAddressResponse> CoverageAddress(LocationAddress location);

        Task<ResponseBase> AddAddress(UserAddress address);

        Task<AddressResponse> GetAddress();

        Task<AddressPredictionResponse> AutoCompleteAddress(string text);

        Task<ResponseBase> UpdateAddress(UserAddress address);

        Task<ResponseBase> DeleteAddress(UserAddress address);

        Task<StoreResponse> GetStores(StoreParameters parameters);
        Task<StoreResponse> GetStores(SearchStoresParameters parameters);

        Task<ResponseBase> SetDefaultAddress(UserAddress address);

        Task<CorrespondenceRespondese> ValidateCorrespondence();

        Task<CorrespondenceRespondese> SaveCorrespondence(UserAddress address);

        Task<ResponseBase> UpdateDispatchRegion(UpdateDispatchRegionParameters parameters);
    }
}
