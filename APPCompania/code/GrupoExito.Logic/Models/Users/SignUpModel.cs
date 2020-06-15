namespace GrupoExito.Logic.Models.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Users;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class SignUpModel
    {
        private ISignUpService _signUpService { get; set; }
        public SignUpModel(ISignUpService signUpService)
        {
            _signUpService = signUpService;
        }

        public async Task<ClientResponse> RegisterUser(User user)
        {
            return await _signUpService.RegisterUser(user);
        }

        public string ValidateFields(User user)
        {
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) || user.DocumentType <= 0 ||
                string.IsNullOrEmpty(user.DocumentNumber) || string.IsNullOrEmpty(user.DateOfBirth)
                || string.IsNullOrEmpty(user.CellPhone) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) ||
                string.IsNullOrEmpty(user.ConfirmPassword))
            {
                return AppMessages.RequiredFieldsText;
            }
            else
            {
                try
                {
                    DateTime dateOfBirth = StringFormat.ParseFormatDateTime(user.DateOfBirth);

                    if (dateOfBirth > DateTime.Now.AddYears(-18))
                    {
                        return AppMessages.DateOfBirthMessage;
                    }
                }
                catch
                {
                    return AppMessages.DateOfBirthMessage;
                }

                Regex regexString = new Regex(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$");

                if (!regexString.IsMatch(user.FirstName.ToUpper().TrimStart().TrimEnd()))
                {
                    return string.Format(AppMessages.SpecialCharactersMessage, AppMessages.NameText);
                }

                if (!regexString.IsMatch(user.LastName.ToUpper().TrimStart().TrimEnd()))
                {
                    return string.Format(AppMessages.SpecialCharactersMessage, AppMessages.LastNameText);
                }

                if (string.IsNullOrEmpty(user.DocumentNumber) && user.DocumentNumber.Length < 6)
                {
                    return AppMessages.DocumentLengthValidationText;
                }
                else
                {
                    if (user.DocumentType == 4)
                    {
                        Regex regexStringDocumentNumber = new Regex(@"^[a-zA-Z0-9.]+$");

                        if (!regexStringDocumentNumber.IsMatch(user.DocumentNumber))
                        {
                            return string.Format(AppMessages.SpecialCharactersMessage, AppMessages.Documento);
                        }
                    }
                }              
                
                if (!Regex.IsMatch(user.Email, AppConfigurations.EmailRegularExpression))
                {
                    return AppMessages.EmailFormatErrorText;
                }

                if (user.CellPhone.Length != 10)
                {
                    return AppMessages.MobileNumberLenghtValidationText;
                }

                var mobileOperator = user.CellPhone.Substring(0, 3);
                var validMobileOperator = Regex.IsMatch(mobileOperator, AppConfigurations.MobilePhoneFormat);

                if (!validMobileOperator)
                {
                    return AppMessages.MobileNumberOperatorValidationText;
                }

                if (!Regex.IsMatch(user.Email, AppConfigurations.EmailRegularExpression))
                {
                    return AppMessages.EmailFormatErrorText;
                }

                if (user.Password.Length < 8 || user.Password.Length > 25)
                {
                    return AppMessages.PasswordLengthValidationText;
                }

                if (!user.ConfirmPassword.Equals(user.Password))
                {
                    return AppMessages.PasswordConfirmationValidationText;
                }

                if (!user.AcceptTerms)
                {
                    return AppMessages.TermsAndConditionValidationText;
                }
            }

            return string.Empty;
        }
    }
}
