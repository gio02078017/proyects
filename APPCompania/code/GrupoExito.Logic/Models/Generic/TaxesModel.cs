namespace GrupoExito.Logic.Models.Generic
{
    using GrupoExito.Entities.Responses.Generic;
    using GrupoExito.Utilities.Contracts.Generic;
    using System.Threading.Tasks;

    public class TaxesModel
    {
        private ITaxesService _taxesService { get; set; }

        public TaxesModel(ITaxesService taxesService)
        {
            _taxesService = taxesService;
        }

        public async Task<TaxBagResponse> GetTaxBag()
        {
            return await _taxesService.GetTaxBag().ConfigureAwait(false);
        }
    }
}
