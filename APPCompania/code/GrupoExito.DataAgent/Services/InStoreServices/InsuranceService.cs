namespace GrupoExito.DataAgent.Services.InStoreServices
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class InsuranceService : BaseApiService, IInsuranceService
    {
        public InsuranceService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<SoatResponse> GetSoat(Soat soat)
        {
            string endpoint = AppServiceConfiguration.GetSoatEndPoint;
            soat.LicensePlate = soat.LicensePlate.ToUpper();
            var response = await HttpClientBaseService.PostAsync(endpoint, soat);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new SoatResponse());
                return (SoatResponse)Convert.ChangeType(responseBase, typeof(SoatResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new SoatResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<SoatResponse>(data);
            result.HttpResponse = response;
            return result;
        }
    }
}
