namespace GrupoExito.Logic.Models.InStoreServices
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Resources;
    using System.Threading.Tasks;

    public class InsuranceModel
    {
        private IInsuranceService _insuranceService { get; set; }

        public InsuranceModel(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        public async Task<SoatResponse> GetSoat(Soat soat)
        {
            return await _insuranceService.GetSoat(soat);
        }

        public string ValidateFields(Soat userCredentials)
        {
            if (string.IsNullOrEmpty(userCredentials.DocumentNumber))
            {
                return AppMessages.RequiredFieldsText;
            }

            if (string.IsNullOrEmpty(userCredentials.DocumentType))
            {
                return AppMessages.RequiredFieldsText;
            }

            if (string.IsNullOrEmpty(userCredentials.LicensePlate))
            {
                return AppMessages.RequiredFieldsText;
            }

            return string.Empty;
        }
    }
}
