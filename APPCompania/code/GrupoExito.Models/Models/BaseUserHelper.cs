using System;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Contracts.Generic;

namespace GrupoExito.Models.Models
{
    public class BaseUserHelper : BaseHelper
    {
        protected readonly UserModel userModel;
        protected readonly MyAccountModel accountModel;

        public BaseUserHelper(IDeviceManager deviceManager)
        {
            this.userModel = new UserModel(new UserService(deviceManager));
            this.accountModel = new MyAccountModel(new UserService(deviceManager));
        }

        public async Task<bool> SendVerificationMessage(VerifyUserParameters parameters)
        {
            SendMessageVerifyUserResponse response = await userModel.SendMessageVerifyUser(parameters);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response.MessageSent;
        }

        public async Task<bool> VerifyUserEnd(VerifyUserParameters parameters)
        {
            VerifyUserResponse response = await userModel.VerifyUser(parameters);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response.Verified;
        }

        public async Task<ResponseBase> UpdateUser(User user)
        {
            ResponseBase response = await accountModel.UpdateUser(user);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }
            else
            {
                return response;
            }
        }

        public async Task<ResponseBase> UpdateMobile(UpdateCellPhoneParameters parameters)
        {
            ResponseBase response = await userModel.UpdateCellPhone(parameters);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }
            else
            {
                return response;
            }
        }
    }
}
