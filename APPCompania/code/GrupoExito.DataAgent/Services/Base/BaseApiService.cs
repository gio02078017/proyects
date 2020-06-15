namespace GrupoExito.DataAgent.Services.Base
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Enumerations;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Utilities.Contracts.Base;
    using GrupoExito.Utilities.Contracts.Generic;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class BaseApiService
    {
        private IHttpClientService httpClientBaseService;
        public IHttpClientService HttpClientBaseService { get => httpClientBaseService; set => httpClientBaseService = value; }

        private IDeviceManager deviceManager;
        public IDeviceManager DeviceManager { get => deviceManager; set => deviceManager = value; }

        private ICryptographyService cryptographyService;
        public ICryptographyService CryptographyService { get => cryptographyService; set => cryptographyService = value; }

        public BaseApiService(IDeviceManager deviceManager)
        {
            this.deviceManager = deviceManager;

            if (cryptographyService == null)
                cryptographyService = new CryptographyService();

            if (httpClientBaseService == null)
                httpClientBaseService = new HttpClientService(DeviceManager, CryptographyService);
        }

        public ResponseBase ValidateResponse(HttpResponseMessage response, ResponseBase responseBase)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
            {
                responseBase.Result = GetInternetConnectionError();
                responseBase.HttpResponse = GetInternetConnectionRequestResponse();
                return responseBase;
            }

            responseBase.Result = GetDefaultError();
            responseBase.HttpResponse = GetBadRequestResponse();
            return responseBase;
        }

        public async Task SaveKey()
        {
            ISecurityService securityService = new SecurityService(DeviceManager);
            await securityService.SaveKey();
        }

        public HttpResponseMessage GetNoContentResponse()
        {
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.NoContent };
        }

        public HttpResponseMessage GetBadRequestResponse()
        {
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest};
        }

        public MessagesContainer GetDefaultError()
        {
            return new MessagesContainer
            {
                HasErrors = true,
                Messages = new List<MessageAplication>()
                {
                    new MessageAplication()
                    {
                        IsError = true,
                        Code = EnumErrorCode.UnexpectedErrorMessage.ToString(),
                        Description = Utilities.Resources.AppMessages.UnexpectedErrorMessage
                    }
                }
            };
        }

        public HttpResponseMessage GetInternetConnectionRequestResponse()
        {
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.RequestTimeout };
        }

        public MessagesContainer GetInternetConnectionError()
        {
            return new MessagesContainer
            {
                HasErrors = true,
                Messages = new List<MessageAplication>()
                {
                    new MessageAplication()
                    {
                        IsError = true,
                        Code = EnumErrorCode.InternetErrorMessage.ToString(),
                        Description = Utilities.Resources.AppMessages.InternetErrorMessage
                    }
                }
            };
        }
    }
}
