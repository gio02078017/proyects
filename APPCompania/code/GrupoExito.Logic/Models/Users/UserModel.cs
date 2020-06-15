namespace GrupoExito.Logic.Models.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Parameters.Users;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Users;
    using GrupoExito.Utilities.Resources;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class UserModel
    {
        private IUserService _userService { get; set; }

        public UserModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<SendMessageVerifyUserResponse> SendMessageVerifyUser(VerifyUserParameters parameters)
        {
            return await _userService.SendMessageVerifyUser(parameters);
        }

        public async Task<VerifyUserResponse> VerifyUser(VerifyUserParameters parameters)
        {
            return await _userService.VerifyUser(parameters);
        }

        public async Task<ResponseBase> UpdateCellPhone(UpdateCellPhoneParameters parameters)
        {
            return await _userService.UpdateCellPhone(parameters);
        }

        public async Task<VerifyUserResponse> UpdateVerifyUser(VerifyUserParameters parameters)
        {
            return await _userService.UpdateVerifyUser(parameters);
        }

        public async Task<RegisterCostumerResponse> RegisterCostumer(RegisterCostumerParameters parameters)
        {
            return await _userService.RegisterCostumer(parameters);
        }

        public string ValidateCodeField(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return AppMessages.RequiredFieldsText;
            }

            return string.Empty;
        }

        public string ValidateCellPhoneField(string cellPhone)
        {
            if (cellPhone.Length != 10)
            {
                return AppMessages.MobileNumberLenghtValidationText;
            }

            var mobileOperator = cellPhone.Substring(0, 3);
            var validMobileOperator = Regex.IsMatch(mobileOperator, AppConfigurations.MobilePhoneFormat);

            if (!validMobileOperator)
            {
                return AppMessages.MobileNumberOperatorValidationText;
            }

            return string.Empty;
        }
    }
}
