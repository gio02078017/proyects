namespace GrupoExito.Logic.Models.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Parameters.InStoreServices;
    using GrupoExito.Entities.Responses.Addresses;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Products;
    using GrupoExito.Utilities.Resources;
    using System.Threading.Tasks;

    public class CheckerPriceModel
    {
        private ICheckerPriceService _checkerPriceService { get; set; }

        public CheckerPriceModel(ICheckerPriceService checkerPriceService)
        {
            this._checkerPriceService = checkerPriceService;
        }

        public async Task<CheckerPriceResponse> CheckerPrice(CheckerPriceParameters parameters)
        {
            return await _checkerPriceService.CheckerPrice(parameters);
        }

        public async Task<CitiesResponse> GetCities()
        {
            var response = await _checkerPriceService.GetCities();

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

        public async Task<StoreResponse> GetStores(string cityId)
        {
            var response = await _checkerPriceService.GetStores(cityId);

            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                return response;
            }
            else
            {
                response.Stores.Insert(0, new Store { Id = "0", DependencyName = AppMessages.Choose });
                return response;
            }
        }

        public async Task<CheckerPriceCoverageResponse> CheckerPriceCoverage(CheckerPriceCoverageParameters parameters)
        {
            return await _checkerPriceService.CheckerPriceCoverage(parameters);
        }
    }
}
