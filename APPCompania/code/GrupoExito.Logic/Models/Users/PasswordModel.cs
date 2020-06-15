namespace GrupoExito.Logic.Models.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Utilities.Contracts.Users;
    using GrupoExito.Utilities.Resources;
    using System.Threading.Tasks;

    public class PasswordModel
    {
        private IPasswordService _passwordService { get; set; }

        public PasswordModel(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public async Task<ResponseBase> ResetPassword(string email)
        {
            return await _passwordService.ResetPassword(email);
        }

        public string ValidateFieldsResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return AppMessages.RequiredFieldsText;
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(email, AppConfigurations.EmailRegularExpression))
                {
                    return AppMessages.EmailFormatErrorText;
                }
            }

            return string.Empty;
        }

        public string ValidateFieldsChangePassword(UserCredentials userCredentials)
        {
            if (string.IsNullOrEmpty(userCredentials.OldPassword) || string.IsNullOrEmpty(userCredentials.NewPassword)
                || string.IsNullOrEmpty(userCredentials.ConfirmPassword))
            {
                return AppMessages.RequiredFieldsText;
            }
            else
            {
                if (userCredentials.NewPassword.Length < 8 || userCredentials.NewPassword.Length > 25)
                {
                    return AppMessages.PasswordLengthValidationText;
                }

                if (!userCredentials.NewPassword.Equals(userCredentials.ConfirmPassword))
                {
                    return AppMessages.PasswordConfirmationValidationText;
                }
            }

            return string.Empty;
        }

        public async Task<ResponseBase> ChangePassword(UserCredentials userCredentials)
        {
            return await _passwordService.ChangePassword(userCredentials).ConfigureAwait(false);
        }
    }
}
