using Android.App;
using Android.Content.PM;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Base
{
    [Activity(Label = "Mis tarjetas", ScreenOrientation = ScreenOrientation.Portrait)]
    public class BaseCreditCardActivity : BaseActivity
    {
        #region Properties

        private CreditCardModel creditCardModel;

        public CreditCardModel CreditCardModel
        {
            get { return creditCardModel; }
            set { creditCardModel = value; }
        }

        #endregion
     
        public async Task<bool> DeleteCreditCard(CreditCard creditCard)
        {
            bool result = true;

            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var response = await CreditCardModel.DeleteCreditCard(creditCard);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        result = false;
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    });
                }              
            }
            catch (Exception exception)
            {
                result = false;
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseAddressesActivity, ConstantMethodName.GetAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }

            return result;
        }

        public async Task<bool> DeleteCreditCardPaymentez(CreditCard creditCard)
        {
            bool result = true;

            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var response = await CreditCardModel.DeleteCreditCardPaymentez(creditCard);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        result = false;
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    });
                }
            }
            catch (Exception exception)
            {
                result = false;
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseAddressesActivity, ConstantMethodName.GetAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }

            return result;
        }
    }
}