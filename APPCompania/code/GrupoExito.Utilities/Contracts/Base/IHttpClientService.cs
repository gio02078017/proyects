namespace GrupoExito.Utilities.Contracts.Base
{
    using GrupoExito.Entities.Responses.Users;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpClientService
    {
        Task<HttpResponseMessage> DeleteAsync(string serviceUrl);

        Task<HttpResponseMessage> GetAsync(string serviceUrl);

        Task<HttpResponseMessage> PostStringAsync(string serviceUrl, string body);

        Task<HttpResponseMessage> PostAsync<TRequest>(string serviceUrl, TRequest request, Dictionary<string, string> headers);

        Task<HttpResponseMessage> PostAsync<TRequest>(string serviceUrl, TRequest request);

        Task<HttpResponseMessage> PutAsync<TRequest>(string serviceUrl, TRequest request);

        Task<TokenResponse> SaveToken(HttpResponseMessage result);
        Task<HttpResponseMessage> GetAddressAsync(string serviceUrl);
        Task<HttpResponseMessage> ValidateConnected();
    }
}
