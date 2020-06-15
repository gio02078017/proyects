namespace GrupoExito.DataAgent.Services.Users
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Parameters.Users;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Users;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class UserService : BaseApiService, IUserService
    {
        public UserService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<UserResponse> GetUser()
        {
            string endpoint = AppServiceConfiguration.GetClientEndPoint;
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new UserResponse());
                return (UserResponse)Convert.ChangeType(responseBase, typeof(UserResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new UserResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<UserResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> UpdateUser(User user)
        {
            string endpoint = AppServiceConfiguration.UpdateUserEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, user);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<UserTypeResponse> GetUserType()
        {
            string endpoint = AppServiceConfiguration.GetUserTypeEndPoint;
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new UserTypeResponse());
                return (UserTypeResponse)Convert.ChangeType(responseBase, typeof(UserTypeResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new UserTypeResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<UserTypeResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<SendMessageVerifyUserResponse> SendMessageVerifyUser(VerifyUserParameters parameters)
        {
            string endpoint = AppServiceConfiguration.SendMessageVerifyUserEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new SendMessageVerifyUserResponse());
                return (SendMessageVerifyUserResponse)Convert.ChangeType(responseBase, typeof(SendMessageVerifyUserResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new SendMessageVerifyUserResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<SendMessageVerifyUserResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<VerifyUserResponse> VerifyUser(VerifyUserParameters parameters)
        {
            string endpoint = AppServiceConfiguration.VerifyUserEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new VerifyUserResponse());
                return (VerifyUserResponse)Convert.ChangeType(responseBase, typeof(VerifyUserResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new VerifyUserResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<VerifyUserResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> UpdateCellPhone(UpdateCellPhoneParameters parameters)
        {
            string endpoint = AppServiceConfiguration.UpdateCellPhoneEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<VerifyUserResponse> UpdateVerifyUser(VerifyUserParameters parameters)
        {
            string endpoint = AppServiceConfiguration.UpdateVerifyUserEndPoint;
            var response = await HttpClientBaseService.PutAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new VerifyUserResponse());
                return (VerifyUserResponse)Convert.ChangeType(responseBase, typeof(VerifyUserResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new VerifyUserResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<VerifyUserResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<RegisterCostumerResponse> RegisterCostumer(RegisterCostumerParameters parameters)
        {
            string endpoint = AppServiceConfiguration.RegisterCostumerEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new RegisterCostumerResponse());
                return (RegisterCostumerResponse)Convert.ChangeType(responseBase, typeof(RegisterCostumerResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new RegisterCostumerResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<RegisterCostumerResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
