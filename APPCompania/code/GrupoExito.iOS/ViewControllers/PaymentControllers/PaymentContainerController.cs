using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Cells;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Interfaces;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Models;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers;
using GrupoExito.Models.Contracts;
using GrupoExito.Models.Enumerations;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Resources;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    public partial class PaymentContainerController : UIViewController
	{
        private SubtotalViewModel subtotalViewModel;
        private bool isSpinnerAdded = false;
        private CustomSpinnerView customSpinner;
        private PurchaseSummaryViewModel viewModel;
        private UIPickerView pickerView;
        private UIToolbar toolbar;

        private List<CreditCardViewModel> creditCardViewModels;
        private IPayment paymentMethod;

        public PaymentContainerController(IntPtr handle) : base(handle) { }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Payment, nameof(PaymentContainerController));
        }


        public override void ViewDidLoad()
        {            
            base.ViewDidLoad();

            customSpinner = CustomSpinnerView.Create();

            creditCardViewModels = new List<CreditCardViewModel>();
            viewModel = new PurchaseSummaryViewModel(ParametersManager.UserContext, DeviceManager.Instance)
            {
                Delegate = this
            };
            subtotalViewModel = new SubtotalViewModel(string.Empty, string.Empty);
            subtotalViewModel.BagTaxInfoHandler += SubtotalViewModel_BagTaxInfoHandler;;

            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            LoadCreditCardPaymentView();
            ShowCreditCardPayment(true);
            LoadSubtotalView();

            continueButton.Layer.CornerRadius = 8;
            continueButton.TouchUpInside += ContinueButton_TouchUpInside;

            tableView.RegisterNibForCellReuse(PaymentCreditCardViewCell.Nib, PaymentCreditCardViewCell.Key);
            tableView.RegisterNibForCellReuse(PaymentCashViewCell.Nib, PaymentCashViewCell.Key);

            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.EstimatedRowHeight = 35;

            SetHeaderView();

            installmentTextField.Layer.BorderColor = UIColor.Black.CGColor;
            installmentTextField.Layer.BorderWidth = 1;
            installmentTextField.Layer.CornerRadius = 10;

            installmentTextField.Delegate = this;
        }

        bool InstallmentTextField_ShouldBeginEditing(UITextField textField)
        {
            pickerView.Hidden = false;
            return false;
        }

        void SubtotalViewModel_BagTaxInfoHandler(object sender, EventArgs e)
        {
            PopUpInformationView bagTaxInfoView = PopUpInformationView.Create(AppMessages.BagTax, AppMessages.BagTaxDisclaimer);
            this.NavigationController.SetNavigationBarHidden(true, true);
            bagTaxInfoView.Frame = View.Bounds;
            bagTaxInfoView.LayoutIfNeeded();
            View.AddSubview(bagTaxInfoView);

            bagTaxInfoView.AcceptButtonHandler += () =>
            {
                this.NavigationController.SetNavigationBarHidden(false, true);
                bagTaxInfoView.RemoveFromSuperview();
            };
            bagTaxInfoView.CloseButtonHandler += () =>
            {
                this.NavigationController.SetNavigationBarHidden(false, true);
                bagTaxInfoView.RemoveFromSuperview();
            };
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            subtotalViewModel.UpdateContentCommand.Execute(null);

            if(ParametersManager.CreditCardChanges)
            {
                ShowCreditCardPayment(true);
                ParametersManager.CreditCardChanges = false;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        private void LoadSubtotalView()
        {
            SubtotalView subtotalView = SubtotalView.Create();
            subtotalView.Frame = subtotalParentView.Bounds;
            subtotalParentView.AddSubview(subtotalView);
            subtotalView.Setup(subtotalViewModel);
        }

        private void LoadCreditCardPaymentView()
        {
            CreateDuesPicker();

            installmentTextField.InputView = pickerView;
            installmentTextField.InputAccessoryView = toolbar;

            creditCardButton.TouchUpInside += (sender, e) =>
            {
                InvokeOnMainThread(() =>
                {
                    CreditCardViewController viewController = (CreditCardViewController)this.Storyboard.InstantiateViewController(nameof(CreditCardViewController));
                    viewController.HidesBottomBarWhenPushed = true;
                    NavigationController.PushViewController(viewController, true);
                });
            };
            exitoCardButton.TouchUpInside += (sender, e) =>
            {
                InvokeOnMainThread(() =>
            {
                AddCreditCardPaymentezViewController addCreditCardPaymentezViewController = (AddCreditCardPaymentezViewController)Storyboard.InstantiateViewController(nameof(AddCreditCardPaymentezViewController));
                addCreditCardPaymentezViewController.HidesBottomBarWhenPushed = true;
                NavigationController.PushViewController(addCreditCardPaymentezViewController, true);
            });
            };
        }

        private void ShowCreditCardPayment(bool downloadData)
        {
            tableView.Source = new CreditCardSource(creditCardViewModels);
            tableView.ReloadData();

            if (downloadData || viewModel.CreditCards == null)
            {
                viewModel.GetCreditCardsCommand.Execute(null);
            }
        }

        private void ShowSpinner()
        {
            this.StartSpinner(ref isSpinnerAdded, customSpinner);
        }

        private void HideSpinner()
        {
            this.StopSpinner(ref isSpinnerAdded, customSpinner);
        }

        private void CreateCellViewModels()
        {
            creditCardViewModels.Clear();
            foreach (var item in viewModel.CreditCards)
            {
                if (item.TypePayment != null && item.TypePayment == ConstTypePayment.Delivery)
                {
                    CashViewModel cashViewModel = new CashViewModel(item, ConstTypePayment.Delivery);
                    cashViewModel.RowSelected += CashViewModel_RowSelected;
                    cashViewModel.PropertyChanged += CashViewModel_PropertyChanged;
                    creditCardViewModels.Add(cashViewModel);
                }
                else
                {
                    CreditCardViewModel creditCardViewModel = item.Paymentez ? new CreditCardViewModel(item, ConstTypePayment.CreditCardExito) : new CreditCardViewModel(item, ConstTypePayment.CreditCard);
                    creditCardViewModel.RowSelected += CreditCardViewModel_RowSelected;
                    creditCardViewModels.Add(creditCardViewModel);
                }
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                string propertyName = e.PropertyName;
                switch (propertyName)
                {
                    case nameof(viewModel.IsBusy):
                        {
                            InvokeOnMainThread(() =>
                            {
                                if (viewModel.IsBusy && !isSpinnerAdded)
                                    ShowSpinner();
                                else if (!viewModel.IsBusy)
                                    HideSpinner();
                            });
                        }
                        break;
                    case nameof(viewModel.PaymentezCreditCardDeleted):
                        {
                            ParametersManager.CreditCardChanges = true;
                        }
                        break;
                    case nameof(viewModel.CreditCardDeleted):
                        {
                            ParametersManager.CreditCardChanges = true;
                        }
                        break;
                    case nameof(viewModel.CreditCards):
                        {
                            CreateCellViewModels();
                            ShowCreditCardPayment(false);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(SummaryContainerController), "");
            }
        }

        void CashViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                string propertyName = e.PropertyName;
                switch (propertyName)
                {
                    case nameof(CashViewModel.CashOptionSelected):
                        {
                            paymentMethod = new CashPayment(((CashViewModel)sender).CashOptionSelected);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(SummaryContainerController), "");
            }
        }

        private void ShowChangesAndPrime(PaymentSummaryResponse summaryResponse)
        {
            InvokeOnMainThread(() =>
            {
                PaymentNewsViewController paymentNewsViewController = (PaymentNewsViewController)this.Storyboard.InstantiateViewController(nameof(PaymentNewsViewController));
                paymentNewsViewController.SummaryResponse = summaryResponse;
                this.NavigationController.PushViewController(paymentNewsViewController, true);
            });
        }

        private void ShowPurchaseSummary(PaymentSummaryResponse summaryResponse)
        {
            InvokeOnMainThread(() =>
            {
                PurchaseSummaryController purchaseSummaryController = new PurchaseSummaryController(summaryResponse);
                this.NavigationController.PushViewController(purchaseSummaryController, true);
            });
        }

        private void CreateDuesPicker()
        {
            pickerView = new UIPickerView(new CGRect(0, View.Frame.Height - 200, View.Frame.Width, 200));
            PickerViewModel model = new PickerViewModel(24);
            pickerView.Model = model;

            model.ValueChanged += (sender, rowSelected) =>
            {
                if (rowSelected != 0)
                {
                    string text = rowSelected == 1 ? rowSelected.ToString() + " cuota" : rowSelected.ToString() + " cuotas";
                    installmentTextField.Text = text;
                    if (paymentMethod != null) paymentMethod.SetDues((rowSelected + 1).ToString());
                }
                else
                {
                    installmentTextField.Text = string.Empty;
                    paymentMethod?.ClearDues();
                }
            };

            toolbar = new UIToolbar(new CGRect(pickerView.Frame.X, pickerView.Frame.Y, View.Frame.Width, 30));
            toolbar.SizeToFit();
            UIBarButtonItem doneButton = new UIBarButtonItem("Listo", UIBarButtonItemStyle.Done, (sender, e) => View.EndEditing(true));
            UIBarButtonItem flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            toolbar.SetItems(new UIBarButtonItem[] { flexible, doneButton }, false);
            toolbar.UserInteractionEnabled = true;
        }

        void CreditCardViewModel_RowSelected(CreditCardViewModel rowViewModel)
        {
            creditCardViewModels.ForEach((arg) => arg.IsSelected = arg.Equals(rowViewModel));

            if (rowViewModel.CreditCardType.Equals(ConstTypePayment.CreditCard))
            {
                paymentMethod = new CreditCardPayment(rowViewModel.CreditCard);
            }
            else
            {
                paymentMethod = new ExitoCardPayment(rowViewModel.CreditCard);
            }

            ShowInstallmentView(true);
            installmentTextField.Text = string.Empty;
        }

        void CashViewModel_RowSelected(CreditCardViewModel rowViewModel)
        {
            creditCardViewModels.ForEach((arg) => arg.IsSelected = arg.Equals(rowViewModel));

            if(!(paymentMethod is CashPayment))
            {
                paymentMethod = null;
            }

            ShowInstallmentView(false);
        }

        public void ShowInstallmentView(bool show)
        {
            installmentStackView.Hidden = !show;
        }

        void ContinueButton_TouchUpInside(object sender, EventArgs e)
        {
            if (paymentMethod != null)
            {
                string message = paymentMethod.ValidatePayment();
                if (string.IsNullOrEmpty(message))
                {
                    RegisterContinueEvent();
                    viewModel.FetchSummaryCheckoutCommand.Execute(paymentMethod.GetOrder());
                }
                else
                {
                    ShowInformationMessage(string.Empty, message);
                }
            }
            else
            {
                ShowInformationMessage(string.Empty, AppMessages.PaymentMethodMessage);
            }
        }

        private void ShowInformationMessage(string title, string message)
        {
            PopUpInformationView informationView = PopUpInformationView.Create(title, message);
            this.NavigationController.SetNavigationBarHidden(true, true);
            informationView.Frame = View.Bounds;
            informationView.LayoutIfNeeded();
            View.AddSubview(informationView);

            informationView.AcceptButtonHandler += () =>
            {
                this.NavigationController.SetNavigationBarHidden(false, true);
                informationView.RemoveFromSuperview();
            };
            informationView.CloseButtonHandler += () =>
            {
                this.NavigationController.SetNavigationBarHidden(false, true);
                informationView.RemoveFromSuperview();
            };

            //informationView.AcceptButtonHandler += (string text) =>
            //{
            //    informationView.RemoveFromSuperview();
            //};
        }

        private void SetHeaderView()
        {
            UIView headerView = new UIView();
            tableView.TableHeaderView = headerView;

            UILabel title = new UILabel();
            headerView.AddSubview(title);

            var margins = headerView.LayoutMarginsGuide;

            title.Text = "Método de pago:";
            title.Font = UIFont.FromName(ConstantFontSize.LetterTitle, title.Font.PointSize);
            title.TranslatesAutoresizingMaskIntoConstraints = false;
            title.LeadingAnchor.ConstraintEqualTo(margins.LeadingAnchor).Active = true;
            title.TrailingAnchor.ConstraintEqualTo(margins.TrailingAnchor).Active = true;
            title.TopAnchor.ConstraintEqualTo(margins.TopAnchor).Active = true;

            UILabel description = new UILabel();
            headerView.AddSubview(description);

            description.TranslatesAutoresizingMaskIntoConstraints = false;
            description.Text = "Selecciona el método de pago con el que desea pagar";
            description.Font = UIFont.FromName(ConstantFontSize.LetterBody, description.Font.PointSize);
            description.TranslatesAutoresizingMaskIntoConstraints = false;
            description.LeadingAnchor.ConstraintEqualTo(margins.LeadingAnchor).Active = true;
            description.TrailingAnchor.ConstraintEqualTo(margins.TrailingAnchor).Active = true;
            description.TopAnchor.ConstraintEqualTo(title.BottomAnchor, 10).Active = true;
            description.BottomAnchor.ConstraintEqualTo(margins.BottomAnchor).Active = true;
            description.Lines = 0;

            headerView.TranslatesAutoresizingMaskIntoConstraints = false;
            headerView.CenterXAnchor.ConstraintEqualTo(tableView.CenterXAnchor).Active = true;
            headerView.WidthAnchor.ConstraintEqualTo(tableView.WidthAnchor).Active = true;
            headerView.TopAnchor.ConstraintEqualTo(tableView.TopAnchor).Active = true;

            headerView.LayoutIfNeeded();
            tableView.TableHeaderView?.LayoutIfNeeded();
        }

        private void SetFooterView()
        {
            PaymentFooterTableView footerView = PaymentFooterTableView.Create(PaymentFooterTableView.EnumDefaultCashTypeSelection.Cash);
            tableView.TableFooterView = footerView;

            footerView.TranslatesAutoresizingMaskIntoConstraints = false;
            footerView.LeadingAnchor.ConstraintEqualTo(tableView.LeadingAnchor).Active = true;
            footerView.TrailingAnchor.ConstraintEqualTo(tableView.TrailingAnchor).Active = true;
            footerView.BottomAnchor.ConstraintEqualTo(tableView.BottomAnchor).Active = true;

            footerView.LayoutIfNeeded();
            tableView.TableFooterView?.LayoutIfNeeded();
        }

        public void RegisterContinueEvent()
        {
            FirebaseEventRegistrationService.Instance.Payment();
        }
    }

    public class PickerViewModel : UIPickerViewModel
    {
        public event EventHandler<nint> ValueChanged;
        private int _duesNumber { get; set; }

        public PickerViewModel(int duesNumber)
        {
            this._duesNumber = duesNumber;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return _duesNumber + 1;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (row == 0) return "Seleccione";
            else return row.ToString();
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            ValueChanged?.Invoke(this, row);
        }
    }

    public partial class PaymentContainerController : IPaymentModel
    {
        public void CheckoutSummaryFetched(PaymentSummaryResponse summaryResponse)
        {
            try
            {
                if ((summaryResponse.ProductsChanged != null && summaryResponse.ProductsChanged.Any()) ||
                (summaryResponse.ProductsRemoved != null && summaryResponse.ProductsRemoved.Any()) ||
                    (summaryResponse.IsPrime && summaryResponse.CostRemaining > 0))
                {
                    ShowChangesAndPrime(summaryResponse);
                }
                else
                {
                    ShowPurchaseSummary(summaryResponse);
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(PaymentContainerController), "");
            }
        }

        public void HandleError(Exception ex)
        {
            try
            {
                InvokeOnMainThread(() =>
                {
                    GenericErrorView errorView = GenericErrorView.Create();
                    errorView.Configure(ex.Data[nameof(EnumExceptionDataKeys.Code)].ToString(), ex.Data[nameof(EnumExceptionDataKeys.Message)].ToString(), (sender, e) =>
                    {
                        errorView.RemoveFromSuperview();
                    });
                    errorView.Frame = View.Bounds;
                    View.AddSubview(errorView);
                });
            }
            catch (Exception exception)
            {
                Util.LogException(exception, nameof(PaymentContainerController), "");
            }
        }

        public void OrderNotValid()
        {

        }

        public void PaymentFinished(PaymentResponse response)
        {

        }

        public void ProductNews(PaymentSummaryResponse summaryResponse)
        {
            try
            {
                ShowChangesAndPrime(summaryResponse);
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(PaymentContainerController), "");
            }
        }

        public void ThereIsNotActiveReservation()
        {

        }
    }

    public class CreditCardSource : UITableViewSource
    {
        public List<CreditCardViewModel> ViewModels;

        public CreditCardSource(List<CreditCardViewModel> creditCardViewModels)
        {
            ViewModels = creditCardViewModels;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            string cellId = (ViewModels[indexPath.Row].CreditCard.TypePayment != null && ViewModels[indexPath.Row].CreditCard.TypePayment == ConstTypePayment.Delivery) ? 
            PaymentCashViewCell.Key : PaymentCreditCardViewCell.Key;

            BasePaymentCell cell = (BasePaymentCell)tableView.DequeueReusableCell(cellId, indexPath);
            cell.SetData(ViewModels[indexPath.Row]);

            tableView.BeginUpdates();
            tableView.EndUpdates();
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ViewModels.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ViewModels[indexPath.Row].RowSelected.Invoke(ViewModels[indexPath.Row]);

            tableView.BeginUpdates();
            tableView.EndUpdates();
            tableView.ScrollToRow(indexPath, UITableViewScrollPosition.Middle, true);
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.BeginUpdates();
            tableView.EndUpdates();
        }
    }

    public partial class PaymentContainerController : IUITextFieldDelegate
    {
        public override bool CanPerform(Selector action, NSObject withSender)
        {
            return false;
        }

        public override bool CanPaste(NSItemProvider[] itemProviders)
        {
            return false;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            installmentTextField.EndEditing(true);
        }
    }
}
