namespace GrupoExito.DataAgent.Services.Generic
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.Generic;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class TaxesService : BaseApiService, ITaxesService
    {
        public TaxesService(IDeviceManager deviceManager)
            : base(deviceManager)
        {

        }

        public async Task<TaxBagResponse> GetTaxBag()
        {
            var response = await HttpClientBaseService.GetAsync(AppServiceConfiguration.TaxBagEndPoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new TaxBagResponse());
                return (TaxBagResponse)Convert.ChangeType(responseBase, typeof(TaxBagResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new TaxBagResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<TaxBagResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
