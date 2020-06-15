using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class MyCreditCardViewController : BaseCreditCardController
    {
        #region Attributes
        public IList<CreditCard> creditCards { get; set; }
        #endregion

        #region Constructors
        public MyCreditCardViewController(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        #endregion

        #region Life Cycle 

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.MyCards, nameof(MyCreditCardViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {               
                this.LoadExternalViews();
                this.LoadCorners();
                this.LoadHandlers();
                this.GetCreditCards();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCreditCardViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
                if (ParametersManager.ContainChanges)
                {
                    GetCreditCards();
                    myCreditCardTableView.ReloadData();
                    ParametersManager.ContainChanges = false;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCreditCardViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 
        private void LoadCorners()

        {
            customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadFonts()
        {
            //titleMyCreditCardLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyAddressTitle);
            //messageMyCreditCardLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyAddressMessageSubtitle);
        }

        private void LoadExternalViews()
        {
            myCreditCardTableView.RegisterNibForCellReuse(CreditCardHeaderCell.Nib, CreditCardHeaderCell.Key);
            myCreditCardTableView.RegisterNibForCellReuse(CreditCardViewCell.Nib, CreditCardViewCell.Key);
            myCreditCardTableView.RegisterNibForCellReuse(NotCreditCardViewCell.Nib, NotCreditCardViewCell.Key);
            LoadNavigationView(this.NavigationController.NavigationBar);
            LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
            SpinnerActivityIndicatorViewFromBase = spinnerAcitivityIndicatorView;
            CustomSpinnerViewFromBase = customSpinnerView;
        }

        private void LoadHandlers()
        {
            addCreditCardButton.TouchUpInside += AddCreditCardUpInside;
            addCreditCardExito.TouchUpInside += AddCreditCardExitoTouchUpInside;
        }

        private void GetCreditCards()
        {
            try
            {
                InvokeOnMainThread(async () =>
                {
                    creditCards = await GetCreditCardsAsync() as List<CreditCard>;
                    DrawCreditCards();
                });
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCreditCardViewController, ConstantMethodName.GetAddressAsync);
                ShowMessageException(exception);
            }
        }




        private void DrawCreditCards()
        {
            MyCreditCardTableViewSource source = new MyCreditCardTableViewSource(creditCards, false);
            myCreditCardTableView.Source = source;
            source.ActiveCaduceWarningAction += SourceCaduceCreditCard;
            source.DeleteAction += SourceDeleteAction;
            myCreditCardTableView.RowHeight = UITableView.AutomaticDimension;
            myCreditCardTableView.ReloadData();
        }

        public async Task DeleteCreditCardAsync(CreditCard creditCard)
        {
            if (creditCard.Paymentez)
            {
                await DeleteCreditCardPaymentz(creditCard);
            }
            else
            {
                await DeleteCreditCard(creditCard);
            }
        }

        public async Task<bool> DeleteCreditCard(CreditCard creditCard)
        {
            bool result = true;
            try
            {
                spinnerAcitivityIndicatorView.StartAnimating();
                _spinnerActivityIndicatorView.Image.StartAnimating();
                _spinnerActivityIndicatorView.Message.Text = string.Empty;
                customSpinnerView.Hidden = false;

                var response = await _creditCardModel.DeleteCreditCard(creditCard);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    this.PresentViewController(alertController, true, null);
                }
                else
                {
                    spinnerAcitivityIndicatorView.StopAnimating();
                    spinnerAcitivityIndicatorView.HidesWhenStopped = true;
                    spinnerAcitivityIndicatorView.Hidden = true;
                    _spinnerActivityIndicatorView.Image.StopAnimating();
                    customSpinnerView.Hidden = true;
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DeleteCreditCardMessage, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    this.PresentViewController(alertController, true, null);
                    creditCards.Remove(creditCard);
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
                    myCreditCardTableView.ReloadData();
                }
            }
            catch (Exception)
            {
                result = false;
                spinnerAcitivityIndicatorView.StopAnimating();
                spinnerAcitivityIndicatorView.HidesWhenStopped = true;
                spinnerAcitivityIndicatorView.Hidden = true;
                _spinnerActivityIndicatorView.Image.StopAnimating();
                customSpinnerView.Hidden = true;
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantControllersName.BaseCreditCardController, ConstantMethodName.DeleteCreditCard } };
            }
            finally
            {
                spinnerAcitivityIndicatorView.StopAnimating();
                spinnerAcitivityIndicatorView.HidesWhenStopped = true;
                spinnerAcitivityIndicatorView.Hidden = true;
                _spinnerActivityIndicatorView.Image.StopAnimating();
            }
            return result;
        }


        public async Task<bool> DeleteCreditCardPaymentz(CreditCard creditCard)
        {
            bool result = true;
            try
            {
                spinnerAcitivityIndicatorView.StartAnimating();
                _spinnerActivityIndicatorView.Image.StartAnimating();
                _spinnerActivityIndicatorView.Message.Text = string.Empty;
                customSpinnerView.Hidden = false;

                var response = await _creditCardModel.DeleteCreditCardPaymentez(creditCard);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    this.PresentViewController(alertController, true, null);
                }
                else
                {
                    spinnerAcitivityIndicatorView.StopAnimating();
                    spinnerAcitivityIndicatorView.HidesWhenStopped = true;
                    spinnerAcitivityIndicatorView.Hidden = true;
                    _spinnerActivityIndicatorView.Image.StopAnimating();
                    customSpinnerView.Hidden = true;
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DeleteCreditCardMessage, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    this.PresentViewController(alertController, true, null);
                    creditCards.Remove(creditCard);
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
                    myCreditCardTableView.ReloadData();
                }
            }
            catch (Exception)
            {
                result = false;
                spinnerAcitivityIndicatorView.StopAnimating();
                spinnerAcitivityIndicatorView.HidesWhenStopped = true;
                spinnerAcitivityIndicatorView.Hidden = true;
                _spinnerActivityIndicatorView.Image.StopAnimating();
                customSpinnerView.Hidden = true;
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantControllersName.BaseCreditCardController, ConstantMethodName.DeleteCreditCard } };
            }
            finally
            {
                spinnerAcitivityIndicatorView.StopAnimating();
                spinnerAcitivityIndicatorView.HidesWhenStopped = true;
                spinnerAcitivityIndicatorView.Hidden = true;
                _spinnerActivityIndicatorView.Image.StopAnimating();
            }
            return result;
        }


        #endregion

        #region Events 

        private void SourceCaduceCreditCard(object sender, EventArgs e)
        {
            NSIndexPath indexPath = (NSIndexPath)sender;
            NSIndexPath[] nSIndexPaths = new NSIndexPath[1];
            nSIndexPaths[0] = indexPath;
            creditCards[indexPath.Row].Selected = true;
            myCreditCardTableView.ReloadRows(nSIndexPaths, UITableViewRowAnimation.Automatic);
        }

        //private void SourceDeleteAction(object sender, EventArgs e)
        private void SourceDeleteAction(CreditCard creditCard)
        {
            //NSIndexPath indexPath = (NSIndexPath)sender;

            MessageConfirmView messageConfirmView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageConfirmView, Self, null).GetItem<MessageConfirmView>(0);
            CGRect messageConfirmViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, customSpinnerView.Frame.Size.Height);
            messageConfirmView_.Frame = messageConfirmViewFrame;
            customSpinnerView.AddSubview(messageConfirmView_);
            _spinnerActivityIndicatorView.Hidden = true;
            spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.3f);
            spinnerAcitivityIndicatorView.StartAnimating();
            customSpinnerView.Hidden = false;
            customSpinnerView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            messageConfirmView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
            messageConfirmView_.Close.Hidden = false;
            messageConfirmView_.Title.Hidden = true;
            messageConfirmView_.Negation.TintColor = UIColor.Black;
            messageConfirmView_.Afirmation.TintColor = UIColor.Black;
            messageConfirmView_.Message.Text = AppMessages.DeletedCreditCardValidation;
            Util.SetConstraint(customSpinnerView, ConstantViewSize.customSpinnerViewHeightDefault, ConstantViewSize.messageCustomViewHeightDefault);
            messageConfirmView_.Negation.TouchUpInside += (object sender1, EventArgs e1) =>
            {
                spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                spinnerAcitivityIndicatorView.StopAnimating();
                messageConfirmView_.RemoveFromSuperview();
                customSpinnerView.BackgroundColor = UIColor.Clear;
                customSpinnerView.Hidden = true;
                Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
            };

            messageConfirmView_.Afirmation.TouchUpInside += (object sender2, EventArgs e2) =>
            {
                spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                spinnerAcitivityIndicatorView.StopAnimating();
                messageConfirmView_.RemoveFromSuperview();
                customSpinnerView.BackgroundColor = UIColor.Clear;
                _spinnerActivityIndicatorView.Hidden = false;
                //DeleteCreditCardAsync(creditCards[indexPath.Row]); 
                DeleteCreditCardAsync(creditCard);
                Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
            };

            messageConfirmView_.Close.TouchUpInside += (object sender1, EventArgs e1) =>
            {
                spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                spinnerAcitivityIndicatorView.StopAnimating();
                messageConfirmView_.RemoveFromSuperview();
                customSpinnerView.BackgroundColor = UIColor.Clear;
                customSpinnerView.Hidden = true;
                Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
            };
        }

        private void AddCreditCardUpInside(object sender, EventArgs e)
        {
            CreditCardViewController creditCardViewController_ = (CreditCardViewController)Storyboard.InstantiateViewController(ConstantControllersName.CreditCardViewController);
            creditCardViewController_.HidesBottomBarWhenPushed = true;
            NavigationController.PushViewController(creditCardViewController_, true);
        }

        private void AddCreditCardExitoTouchUpInside(object sender, EventArgs e)
        {
            CreditCardViewController creditCardViewController_ = (CreditCardViewController)Storyboard.InstantiateViewController(ConstantControllersName.CreditCardViewController);
            creditCardViewController_.HidesBottomBarWhenPushed = true;
            NavigationController.PushViewController(creditCardViewController_, true);
        }
        #endregion
    }
}

