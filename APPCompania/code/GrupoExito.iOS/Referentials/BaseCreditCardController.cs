using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Payments;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.Referentials
{
    public class BaseCreditCardController : UIViewControllerBase
    {
        #region Attributes

        protected CreditCardModel _creditCardModel;

        #endregion

        #region Constructors

        public BaseCreditCardController(IntPtr handle) : base(handle)
        {
            _creditCardModel = new CreditCardModel(new CreditCardService(DeviceManager.Instance));
        }

        #endregion

        #region Life Cycle 

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        #endregion

        #region Methods Async 

        public async Task<List<CreditCard>> GetCreditCardsAsync()
        {
            List<CreditCard> creditCards = new List<CreditCard>();
            try
            {
                StartActivityIndicatorCustom();
                CreditCardResponse response = await _creditCardModel.GetAllCreditCards();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    if (response.CreditCards != null && response.CreditCards.Any())
                    {
                        creditCards.AddRange(response.CreditCards);
                    }
                    else
                    {
                        /*var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.NotCardMessage, UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                        PresentViewController(alertController, true, null);*/
                    }

                    StopActivityIndicatorCustom();
                }
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.BaseCreditCardController, ConstantMethodName.GetCreditCards);
                ShowMessageException(exception);
            }

            return creditCards;
        }

        public async Task<bool> DeleteCreditCard(CreditCard creditCard)
        {
            bool result = true;

            try
            {
                StartActivityIndicatorCustom();
                var response = await _creditCardModel.DeleteCreditCard(creditCard);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    StopActivityIndicatorCustom();
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DeleteCreditCardMessage, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                }
            }
            catch (Exception exception)
            {
                result = false;
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.BaseCreditCardController, ConstantMethodName.DeleteCreditCard);
                ShowMessageException(exception);
            }

            return result;
        }

        #endregion
    }
}
