namespace GrupoExito.DataAgent.Services.Generic
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.Generic;
    using GrupoExito.Utilities.Contracts;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class DocumentTypesService : BaseApiService, IDocumentTypesService
    {
        public DocumentTypesService(IDeviceManager deviceManager) 
            : base(deviceManager)
        {
        }

        public async Task<DocumentTypeResponse> GetDocumentTypes()
        {
            var response = await HttpClientBaseService.GetAsync(AppServiceConfiguration.DocumentTypesEndPoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new DocumentTypeResponse());
                return (DocumentTypeResponse)Convert.ChangeType(responseBase, typeof(DocumentTypeResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new DocumentTypeResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<DocumentTypeResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
