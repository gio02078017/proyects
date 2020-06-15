using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Payments;
using GrupoExito.Entities.Constants;
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
using GrupoExito.Utilities.Helpers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    public partial class PaymentezCreditCardViewController : UIViewController, ICreditCardController
    {
        #region Attributes
        private IList<CreditCard> creditCards;
        private CreditCardModel _creditCardModel;
        private UIViewControllerBase _viewControllerBase;
        private UIActivityIndicatorView _spinnerAcitivityIndicatorView;
        private UIView _customSpinnerView;
        private CustomActivityIndicatorView _spinnerActivityIndicatorView_;
        private Action _addPaymentezCreditCardHandler;
        private Action<CreditCard> _creditCardSelectedHandler;
        private Action _addPaymentezMastercardCreditCardHandler;
        private Action<ICreditCardController> _showInstallmentPickerHandler;
        private Action _startWorkingHandler;
        private Action _endWorkingHandler;
        #endregion

        #region Properties
        public event EventHandler<Exception> ErrorHandler;
        public Action AddPaymentezCreditCardHandler { get => _addPaymentezCreditCardHandler; set => _addPaymentezCreditCardHandler = value; }
        public Action<CreditCard> CreditCardSelectedHandler { get => _creditCardSelectedHandler; set => _creditCardSelectedHandler = value; }
        public Action AddPaymentezMastercardCreditCardHandler { get => _addPaymentezMastercardCreditCardHandler; set => _addPaymentezMastercardCreditCardHandler = value; }
        public Action<ICreditCardController> ShowInstallmentPickerHandler { get => _showInstallmentPickerHandler; set => _showInstallmentPickerHandler = value; }
        public Action StartWorkingHandler { get => _startWorkingHandler; set => _startWorkingHandler = value; }
        public Action EndWorkingHandler { get => _endWorkingHandler; set => _endWorkingHandler = value; }
        public Action<CreditCard> DeleteHandler { get; set; }
        #endregion

        #region Constructors
        public PaymentezCreditCardViewController() : base("PaymentezCreditCardViewController", null) 
        {
            //Default constructor this class 
        }

        public PaymentezCreditCardViewController(CreditCardModel creditCardModel,
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
                _creditCardModel = new CreditCardModel(new CreditCardService(DeviceManager.Instance));
                this.SetSubviews();
                this.LoadHandlers();
                this.GetCreditCardsAsync();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentezCreditCardViewController, ConstantMethodName.ViewDidLoad);
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

        private void SetSubviews()
        {
            tableView.RegisterNibForCellReuse(CreditCardViewCell.Nib, CreditCardViewCell.Key);
            tableView.RegisterNibForCellReuse(CreditCardHeaderCell.Nib, CreditCardHeaderCell.Key);
            headerView.Hidden = true;
            HideInstallmentViews();
        }

        private void LoadHandlers()
        {
            addPaymentezCreditCardButton.TouchUpInside += (o, e) =>
            {
                AddPaymentezCreditCardHandler?.Invoke();
            };

            addPaymentezMastercardCreditCardButton.TouchUpInside += (o, e) =>
            {
                AddPaymentezMastercardCreditCardHandler?.Invoke();
            };

            installmentTextField.ShouldBeginEditing = delegate (UITextField field)
            {
                ShowInstallmentPickerHandler?.Invoke(this);

                return false;
            };
        }

        private async Task GetCreditCardsAsync()
        {
            try
            {
                creditCards = await GetCreditCards();

                if (creditCards != null && creditCards.Count > 0)
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
            }
        }

        private async Task<List<CreditCard>> GetCreditCards()
        {
            List<CreditCard> Cards = new List<CreditCard>();

            try
            {
                StartWorkingHandler?.Invoke();
                CreditCardResponse response = new CreditCardResponse(); //await _creditCardModel.GetCreditCardsPaymentez();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                    }
                }
                else
                {
                    if (response.CreditCards != null && response.CreditCards.Count > 0)
                    {
                        Cards.AddRange(response.CreditCards);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorHandler?.Invoke(this, exception);
            }
            finally
            {
                EndWorkingHandler?.Invoke();
            }

            return Cards;
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

        private void DrawCreditCards()
        {
            int countWithExpireData = 0;
            int countWithoutExpireData = 0;

            foreach (CreditCard card in creditCards)
            {
                if (card.IsNextCaduced && !card.Type.Equals(ConstCreditCardType.Exito))
                {
                    countWithExpireData++;
                }
                else
                {
                    countWithoutExpireData++;
                }
            }

            tableViewHeightConstraint.Constant = ((ConstantViewSize.MyCreditCardHeightCell * countWithoutExpireData) + (ConstantViewSize.MyCreditCardHeightCellWithExpireData * countWithExpireData)/* + ConstantViewSize.MyCreditCardHeaderHeightCell*/);
            MyCreditCardTableViewSource source = new MyCreditCardTableViewSource(creditCards, true); ;
            source.RowSelectedHandler = RowSelectedHandler;
            source.DeleteAction = DeleteActionHandler;
            tableView.Source = source;
            tableView.ReloadData();
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

