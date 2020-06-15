namespace GrupoExito.Logic.Models.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Users;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class MyAccountModel
    {
        private IUserService _userService { get; set; }

        public MyAccountModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserResponse> GetUser()
        {
            return await _userService.GetUser();
        }

        public MyAccount GetMyAccount()
        {
            MyAccount myAccount = new MyAccount();
            var menu = JsonService.Deserialize<List<MenuItem>>(AppConfigurations.MenuMyAccountSource);

            if (AppServiceConfiguration.SiteId.Equals("carulla"))
            {
                var stikersMenuItem = menu.Where(x => x.ActionName.Equals(ConstMenuMyAccount.MyStickers)).FirstOrDefault();
                menu.Remove(stikersMenuItem);

                var primeMenuItem = menu.Where(x => x.ActionName.Equals(ConstMenuMyAccount.Prime)).FirstOrDefault();
                menu.Remove(primeMenuItem);
            }

            myAccount.Menu = menu;
            return myAccount;
        }

        public async Task<ResponseBase> UpdateUser(User user)
        {
            return await _userService.UpdateUser(user);
        }

        public async Task<UserTypeResponse> GetUserType()
        {
            return await _userService.GetUserType();
        }

        public string ValidateFields(User user)
        {
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) ||
                string.IsNullOrEmpty(user.DateOfBirth) || string.IsNullOrEmpty(user.CellPhone))
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

                if (!user.AcceptTerms)
                {
                    return AppMessages.TermsAndConditionValidationText;
                }
            }

            return string.Empty;
        }
    }
}
