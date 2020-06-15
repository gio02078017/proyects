namespace GrupoExito.DataAgent.Services.Base
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Base;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class HttpClientService : IHttpClientService
    {
        private ICryptographyService _cryptographyService;
        private IDeviceManager _deviceManager;

        private string ServiceUrl { get; set; }
        private string ServiceKey { get; set; }
        private string SiteId { get; set; }
        private string DeviceId { get; set; }

        public HttpClientService(IDeviceManager deviceManager,
            ICryptographyService cryptographyService)
        {
            _deviceManager = deviceManager;
            _cryptographyService = cryptographyService;

            ServiceUrl = AppServiceConfiguration.ApiUrl;

            ServiceKey = _cryptographyService.Decrypt(AppServiceConfiguration.ApiKey, AppServiceConfiguration.InnerStrength);
            SiteId = AppServiceConfiguration.SiteId;
            DeviceId = _deviceManager.GetDeviceId();
        }

        public async Task<HttpResponseMessage> PostAsync<TRequest>(string serviceUrl, TRequest request, Dictionary<string, string> headers = null)
        {
            try
            {
                if (await _deviceManager.IsNetworkAvailable())
                {
                    using (var client = new HttpClient())
                    {
                        string bodyRequest;

                        bodyRequest = JsonService.Serialize(request);

                        if (headers != null)
                        {
                            foreach (var item in headers)
                            {
                                client.DefaultRequestHeaders.Add(item.Key, item.Value);
                            }
                        }

                        return await client.PostAsync(serviceUrl, new StringContent(bodyRequest, System.Text.Encoding.UTF8, "application/json"));
                    }
                }
                else
                {
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.GatewayTimeout, RequestMessage = new HttpRequestMessage { } };
                }
            }
            catch
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            }
        }

        public async Task<HttpResponseMessage> PostAsync<TRequest>(string serviceUrl, TRequest request)
        {
            try
            {
                if (await _deviceManager.IsNetworkAvailable())
                {
                    string url = this.ServiceUrl + serviceUrl;

                    using (var client = new HttpClient())
                    {
                        string bodyRequest;
                        bodyRequest = JsonService.Serialize(request);
                        var result = await this.SetAuthorizationHeader(client);
                        client.Timeout.Add(new TimeSpan(0, 0, 20));
                        if (result)
                        {
                            return await client.PostAsync(url, new StringContent(bodyRequest, System.Text.Encoding.UTF8, "application/json"));
                        }
                        else
                        {
                            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
                        }
                    }
                }
                else
                {
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.RequestTimeout };
                }
            }
            catch
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            }
        }

        public async Task<HttpResponseMessage> PutAsync<TRequest>(string serviceUrl, TRequest request)
        {
            try
            {
                if (await _deviceManager.IsNetworkAvailable())
                {
                    string url = this.ServiceUrl + serviceUrl;

                    using (var client = new HttpClient())
                    {
                        var bodyRequest = JsonService.Serialize(request);
                        var result = await this.SetAuthorizationHeader(client);

                        if (result)
                        {
                            return await client.PutAsync(url, new StringContent(bodyRequest, System.Text.Encoding.UTF8, "application/json"));
                        }
                        else
                        {
                            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
                        }
                    }
                }
                else
                {
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.RequestTimeout };
                }
            }
            catch
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string serviceUrl)
        {
            try
            {
                if (await _deviceManager.IsNetworkAvailable())
                {
                    string url = this.ServiceUrl + serviceUrl;

                    using (var client = new HttpClient())
                    {
                        var result = await this.SetAuthorizationHeader(client);

                        if (result)
                        {
                            return await client.GetAsync(new Uri(url));
                        }
                        else
                        {
                            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
                        }
                    }
                }
                else
                {
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.RequestTimeout };
                }
            }
            catch
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            }
        }

        public async Task<HttpResponseMessage> GetAddressAsync(string serviceUrl)
        {
            try
            {
                if (await _deviceManager.IsNetworkAvailable())
                {
                    using (var client = new HttpClient())
                    {
                        var result = await this.SetAuthorizationHeader(client);

                        if (result)
                        {
                            return await client.GetAsync(new Uri(serviceUrl));
                        }
                        else
                        {
                            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
                        }
                    }
                }
                else
                {
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.RequestTimeout };
                }
            }
            catch
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string serviceUrl)
        {
            try
            {
                if (await _deviceManager.IsNetworkAvailable())
                {
                    string url = this.ServiceUrl + serviceUrl;

                    using (var client = new HttpClient())
                    {
                        var result = await this.SetAuthorizationHeader(client);

                        if (result)
                        {
                            return await client.DeleteAsync(url);
                        }
                        else
                        {
                            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
                        }
                    }
                }
                else
                {
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.RequestTimeout };
                }
            }
            catch
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            }
        }

        public async Task<HttpResponseMessage> PostStringAsync(string serviceUrl, string body)
        {
            try
            {
                if (await _deviceManager.IsNetworkAvailable())
                {
                    using (var client = new HttpClient())
                    {
                        var result = await this.SetAuthorizationHeader(client);

                        if (result)
                        {
                            return await client.PostAsync(serviceUrl, new StringContent(body, System.Text.Encoding.UTF8));
                        }
                        else
                        {
                            return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
                        }
                    }
                }
                else
                {
                    return new HttpResponseMessage { StatusCode = HttpStatusCode.RequestTimeout };
                }
            }
            catch
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            }
        }

        public async Task<TokenResponse> GetToken()
        {
            try
            {
                bool expirationTimeExists = _deviceManager.ValidateAccessPreference(ConstPreferenceKeys.ExpirationTime);
                bool userIdentified = _deviceManager.ValidateAccessPreference(ConstPreferenceKeys.User);
                bool fileTokenExists = _deviceManager.ValidateAccessPreference(ConstPreferenceKeys.Token);

                var expirationTime = expirationTimeExists ? JsonService.Deserialize<DateTime>(_deviceManager.GetAccessPreference(ConstPreferenceKeys.ExpirationTime)) : default(DateTime);

                if (expirationTime.ToLocalTime() <= DateTime.Now || !fileTokenExists)
                {
                    string url = string.Empty;
                    var headers = new Dictionary<string, string>
                    {
                        { ConstPreferenceKeys.SiteId, SiteId },
                        { ConstPreferenceKeys.DeviceId, DeviceId },
                        { ConstPreferenceKeys.ApiKey, ServiceKey }
                    };

                    if (fileTokenExists)
                    {
                        var token = JsonService.Deserialize<Token>(_deviceManager.GetAccessPreference(ConstPreferenceKeys.Token));

                        HttpResponseMessage result = null;
                        UserContext user = userIdentified ?
                            JsonService.Deserialize<UserContext>(_deviceManager.GetAccessPreference(ConstPreferenceKeys.User)) : null;


                        if (userIdentified && !user.IsAnonymous)
                        {
                            url = ServiceUrl + AppServiceConfiguration.GetRefreshTokenEndpoint;

                            var request = new
                            {
                                documentType = user.DocumentType,
                                documentNumber = user.DocumentNumber,
                                refreshToken = token.RefreshToken,
                                clientId = user.Id
                            };

                            result = await PostAsync(url, request, headers);
                        }
                        else
                        {
                            url = ServiceUrl + AppServiceConfiguration.GetAnonymousRefreshTokenEndpoint;
                            result = await PostAsync(url, token, headers);
                        }

                        string data = await result.Content.ReadAsStringAsync();
                        return await SaveToken(result);
                    }
                    else
                    {
                        url = ServiceUrl + AppServiceConfiguration.GetAnonymousTokenEndpoint;

                        var request = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("grant_type", "password")
                        });

                        var result = await PostAsync(url, request, headers);
                        return await SaveToken(result);
                    }
                }
                else
                {
                    var response = new TokenResponse
                    {
                        Token = JsonService.Deserialize<Token>(_deviceManager.GetAccessPreference(ConstPreferenceKeys.Token)),
                        HttpResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK }
                    };

                    return response;
                }
            }
            catch
            {
                return new TokenResponse { HttpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest } };
            }
        }

        public async Task<TokenResponse> SaveToken(HttpResponseMessage result)
        {
            try
            {
                if (result.IsSuccessStatusCode)
                {
                    var response = new TokenResponse { HttpResponse = result };
                    response.Token = await JsonService.GetSerializedResponse<Token>(result);
                    var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    date = date.AddSeconds(response.Token.ExpiresIn);
                    _deviceManager.SaveAccessPreference(ConstPreferenceKeys.ExpirationTime, JsonService.Serialize(date));
                    _deviceManager.SaveAccessPreference(ConstPreferenceKeys.Token, JsonService.Serialize(response.Token));
                    return response;
                }
                else
                {
                    return new TokenResponse { HttpResponse = result };
                }
            }
            catch
            {
                return new TokenResponse { HttpResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest } };
            }
        }

        private async Task<bool> SetAuthorizationHeader(HttpClient client)
        {
            var tokenResponse = await this.GetToken();
            var token = tokenResponse.Token;

            if (token == null)
                return false;

            client.DefaultRequestHeaders.Add(ConstPreferenceKeys.SiteId, SiteId);
            client.DefaultRequestHeaders.Add(ConstPreferenceKeys.DeviceId, DeviceId);
            client.DefaultRequestHeaders.Add(ConstPreferenceKeys.ApiKey, ServiceKey);
            client.DefaultRequestHeaders.Add(ConstPreferenceKeys.Authorization, $"Bearer {token.AccessToken}");

            return true;
        }

        public async Task<HttpResponseMessage> ValidateConnected()
        {
            if (!await _deviceManager.IsNetworkAvailable())
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.RequestTimeout };
            }

            return null;
        }
    }
}
