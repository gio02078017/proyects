using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Interfaces;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Logic.Models.Paymentes;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    public partial class CheckoutCreditCardViewController : UIViewController, ICreditCardController
    {
        #region Attributes
        public event EventHandler<Exception> ErrorHandler;
        protected CreditCardModel _creditCardModel;
        private UIViewControllerBase _viewControllerBase;
        private UIActivityIndicatorView _spinnerAcitivityIndicatorView;
        private UIView _customSpinnerView;
        private CustomActivityIndicatorView _spinnerActivityIndicatorView_;
        private Action startWorkingHandler;
        private Action endWorkingHandler;
        private Action showMessageHandler;
        private Action<ICreditCardController> showInstallmentPickerHandler;
        private Action addCreditCardHandler;
        private List<CreditCard> creditCards;
        private Action<CreditCard> _creditCardSelectedHandler;
        #endregion

        #region Properties
        public Action<CreditCard> CreditCardSelectedHandler { get => _creditCardSelectedHandler; set => _creditCardSelectedHandler = value; }
        public Action StartWorkingHandler { get => startWorkingHandler; set => startWorkingHandler = value; }
        public Action EndWorkingHandler { get => endWorkingHandler; set => endWorkingHandler = value; }
        public Action ShowMessageHandler { get => showMessageHandler; set => showMessageHandler = value; }
        public Action<ICreditCardController> ShowInstallmentPickerHandler { get => showInstallmentPickerHandler; set => showInstallmentPickerHandler = value; }
        public Action AddCreditCardHandler { get => addCreditCardHandler; set => addCreditCardHandler = value; }
        public Action<CreditCard> DeleteHandler { get; set; }
        #endregion

        #region Constructors
        public CheckoutCreditCardViewController() : base("CheckoutCreditCardViewController", null)
        {
            //Default constructor this class
        }

        public CheckoutCreditCardViewController(CreditCardModel creditCardModel,
                                                UIViewControllerBase viewControllerBase,
                                                UIActivityIndicatorView spinnerAcitivityIndicatorView,
                                                UIView customSpinnerView,
                                                ref CustomActivityIndicatorView spinnerActivityIndicatorView_)
        {
            _creditCardModel = creditCardModel;
            _viewControllerBase = viewControllerBase;
            _spinnerAcitivityIndicatorView = spinnerAcitivityIndicatorView;
            _customSpinnerView = customSpinnerView;
            _spinnerActivityIndicatorView_ = spinnerActivityIndicatorView_;
        }
        #endregion

        #region Life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                creditCardsTableView.RegisterNibForCellReuse(CreditCardViewCell.Nib, CreditCardViewCell.Key);
                creditCardsTableView.RegisterNibForCellReuse(CreditCardHeaderCell.Nib, CreditCardHeaderCell.Key);
                HideInstallmentViews();
                this.LoadHandlers();
                this.GetCreditCardsAsync();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CheckoutCreditCardViewController, ConstantMethodName.ViewDidLoad);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (ParametersManager.CreditCardChanges)
            {
                GetCreditCardsAsync();
                ParametersManager.CreditCardChanges = false;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            var deselectItems = tableView.IndexPathsForSelectedRows;
            if (deselectItems != null && deselectItems.Any())
            {
                foreach (var item in deselectItems)
                {
                    tableView.DeselectRow(item, false);
                }
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods
        public async Task<List<CreditCard>> GetCreditCards()
        {
            List<CreditCard> creditCards = new List<CreditCard>();
            try
            {
                StartWorkingHandler?.Invoke();
                CreditCardResponse response = new CreditCardResponse(); //await _creditCardModel.GetCreditCards();

                if (response.Result != null && response.Result.HasErrors)
                {

                }
                else
                {
                    if (response.CreditCards != null && response.CreditCards.Any())
                    {
                        creditCards.AddRange(response.CreditCards);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorHandler?.Invoke(this, exception);
                Util.LogException(exception, ConstantControllersName.CheckoutCreditCardViewController, ConstantMethodName.GetCreditCards);
            }
            finally
            {
                EndWorkingHandler?.Invoke();
            }

            return creditCards;
        }

        public void SetInstallmentTextField(string text)
        {
            //if (!string.IsNullOrEmpty(text))
            //{
                InvokeOnMainThread(() =>
                {
                    installmentTextField.Text = text;
                });
            //}
        }


        private void HideInstallmentViews()
        {
            installmentView.Hidden = true;
            lineView.Hidden = true;
        }

        private void ShowInstallmentView()
        {
            installmentView.Hidden = false;
            lineView.Hidden = false;
        }

        private void LoadHandlers()
        {
            installmentTextField.ShouldBeginEditing = delegate (UITextField field)
            {
                ShowInstallmentPickerHandler?.Invoke(this);
                return false;
            };

            addCardButton.TouchUpInside += (o, e) =>
            {
                AddCreditCardHandler?.Invoke();
            };
        }

        private async Task GetCreditCardsAsync()
        {
            try
            {
                creditCards = await GetCreditCards();

                if (creditCards != null && creditCards.Any())
                {
                    ShowInstallmentView();
                    DrawCreditCards();
                }
                else
                {
                    HideInstallmentViews();
                }
            }
            catch (Exception exception)
            {
                ErrorHandler?.Invoke(this, exception);
                Util.LogException(exception, ConstantControllersName.CheckoutCreditCardViewController, ConstantMethodName.GetCreditCards);
            }
        }

        private void DrawCreditCards()
        {
            int countWithExpireData = 0;
            int countWithoutExpireData = 0;

            foreach (CreditCard card in creditCards)
            {
                if (card.IsNextCaduced)
                {
                    countWithExpireData++;
                }
                else
                {
                    countWithoutExpireData++;
                }
            }
            creditCardsTableViewHeightConstraint.Constant = ((ConstantViewSize.MyCreditCardHeightCell * countWithoutExpireData) + (ConstantViewSize.MyCreditCardHeightCellWithExpireData * countWithExpireData)/* + ConstantViewSize.MyCreditCardHeaderHeightCell*/);
            MyCreditCardTableViewSource source = new MyCreditCardTableViewSource(creditCards, true); ;
            source.RowSelectedHandler = RowSelectedHandler;
            source.DeleteAction = DeleteActionHandler;
            creditCardsTableView.Source = source;
            creditCardsTableView.ReloadData();
        }

        private void RowSelectedHandler(CreditCard card)
        {
            CreditCardSelectedHandler?.Invoke(card);
        }

        private void DeleteActionHandler(CreditCard card)
        {
            DeleteHandler?.Invoke(card);
        }
        #endregion
    }
}

