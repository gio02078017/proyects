namespace GrupoExito.Utilities.Contracts.Products
{
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Parameters.InStoreServices;
    using GrupoExito.Entities.Responses.Addresses;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Entities.Responses.Products;
    using System.Threading.Tasks;

    public interface ICheckerPriceService
    {
        Task<CheckerPriceResponse> CheckerPrice(CheckerPriceParameters parameters);
        Task<CitiesResponse> GetCities();
        Task<StoreResponse> GetStores(string cityId);
        Task<CheckerPriceCoverageResponse> CheckerPriceCoverage(CheckerPriceCoverageParameters parameters);
    }
}
