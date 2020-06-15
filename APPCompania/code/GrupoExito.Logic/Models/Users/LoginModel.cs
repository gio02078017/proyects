namespace GrupoExito.Logic.Models.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Users;
    using GrupoExito.Utilities.Resources;
    using System.Threading.Tasks;

    public class LoginModel
    {
        private ILoginService _loginService { get; set; }

        public LoginModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<LoginResponse> Login(UserCredentials userCredentials)
        {
            return await _loginService.Login(userCredentials);
        }      

        public string ValidateFields(UserCredentials userCredentials)
        {
            if (string.IsNullOrEmpty(userCredentials.Email) || string.IsNullOrEmpty(userCredentials.Password))
            {
                return AppMessages.RequiredFieldsText;
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(userCredentials.Email, AppConfigurations.EmailRegularExpression))
                {
                    return AppMessages.EmailFormatErrorText;
                }
            }

            return string.Empty;
        }
    }
}
