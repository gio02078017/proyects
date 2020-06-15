using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.iOS.Models;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Cells;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Sources;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.Views.PaymentViews.SummaryViews;
using GrupoExito.Models.Contracts;
using GrupoExito.Models.Enumerations;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    public partial class PurchaseSummaryController : UIViewController
    {
        #region Attributes
        private CustomSpinnerViewController spinnerController;
        private bool isSpinnerAdded = false;
        private PurchaseSummaryViewModel viewModel;
        private PaymentSummaryResponse summaryResponse;
        private CustomSpinnerView customSpinnerView;
        #endregion

        #region Constructors
        public PurchaseSummaryController(PaymentSummaryResponse summaryResponse)
        {
            this.summaryResponse = summaryResponse;
        }
        #endregion

        #region Override Methods
        public override void LoadView()
        {
            base.LoadView();
            var arr = NSBundle.MainBundle.LoadNib(nameof(PurchaseSummaryController), this, null);
            var v = Runtime.GetNSObject<UIView>(arr.ValueAt(0));
            customSpinnerView = CustomSpinnerView.Create();
            View = v;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            viewModel = new PurchaseSummaryViewModel(ParametersManager.UserContext, DeviceManager.Instance)
            {
                Delegate = this
            };

            spinnerController = new CustomSpinnerViewController();

            purchaseSummaryTableView.RegisterNibForCellReuse(PurchaseSummaryViewCell.Nib, PurchaseSummaryViewCell.Key);
            purchaseSummaryTableView.RegisterNibForCellReuse(HeaderPurchaseSummaryViewCell.Nib, HeaderPurchaseSummaryViewCell.Key);

            buyButton.Layer.CornerRadius = Utilities.Constant.ConstantStyle.CornerRadius;

            this.LoadData();
            this.LoadHandlers();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            ConfigureNavigationBar();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(Entities.Constants.Analytic.AnalyticsScreenView.SummaryOfBuy, nameof(PurchaseSummaryController));
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
        #endregion

        #region Methods
        private void ConfigureNavigationBar()
        {
            this.NavigationController.NavigationBar.Hidden = false;
            NavigationHeaderView navigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            navigationView.HiddenCarData();
            navigationView.IsSummaryDisabled = true;
            navigationView.HiddenAccountProfile();
            navigationView.IsAccountEnabled = false;
            navigationView.EnableBackButton(true);
            this.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
        }

        private void LoadHandlers()
        {
            buyButton.TouchUpInside += BuyButtonTouchUpInside;
        }

        private void LoadData()
        {
            //List<PurchaseSummary> purchaseSummaries = JsonService.Deserialize<List<PurchaseSummary>>(AppConfigurations.ResumePurchaseSummary);

            List<PurchaseSummary> items = new List<PurchaseSummary>();

            PurchaseSummary item0 = new PurchaseSummary("Cantidad de productos", ParametersManager.Order.TotalProducts.ToString(), string.Empty);
            items.Add(item0);
            PurchaseSummary item1 = new PurchaseSummary("Dirección de envío", ParametersManager.UserContext.Store != null ? ParametersManager.UserContext.Store.Address : ParametersManager.UserContext.Address.AddressComplete, string.Empty, true, false);
            items.Add(item1);
            PurchaseSummary item2 = new PurchaseSummary("Medio de pago", summaryResponse.PaymentMethodName, string.Empty, true, false);
            items.Add(item2);

            //if (!string.IsNullOrEmpty(ParametersManager.Order.DateSelected) && !string.IsNullOrEmpty(ParametersManager.Order.Schedule))
            //{
            //    PurchaseSummary itemSchedule = new PurchaseSummary("Horario programado", ParametersManager.Order.DateSelected + ", " + ParametersManager.Order.Schedule, string.Empty, true, true);
            //    items.Add(itemSchedule);
            //}
            //else item2.HasContinuousLine = true;

            if (ParametersManager.Order.TypeModality == ConstTypeModality.Express)
            {
                PurchaseSummary itemSchedule = new PurchaseSummary("Horario programado", StringFormat.AddTimeToDateNow(ParametersManager.Order.MinutesPromiseDelivery), string.Empty, true, true);
                items.Add(itemSchedule);
            }
            else if (!string.IsNullOrEmpty(ParametersManager.Order.DateSelected) && !string.IsNullOrEmpty(ParametersManager.Order.Schedule))
            {
                PurchaseSummary itemSchedule = new PurchaseSummary("Horario programado", ParametersManager.Order.DateSelected + ", " + ParametersManager.Order.Schedule, string.Empty, true, true);
                items.Add(itemSchedule);
            }
            else item2.HasContinuousLine = true;

            PurchaseSummary itemSubtotal = new PurchaseSummary("Subtotal", StringFormat.ToPrice(summaryResponse.SubTotal), string.Empty);
            items.Add(itemSubtotal);

            if (AppServiceConfiguration.SiteId.Equals("exito") && summaryResponse.IsPrime && ParametersManager.Order.TypeDispatch == ConstTypeOfDispatch.Delivery)
            {
                PurchaseSummary itemPrime = new PurchaseSummary("Ahorro Prime", ParametersManager.Order.shippingCost, string.Empty);
                items.Add(itemPrime);
            }

            PurchaseSummary itemPoints = new PurchaseSummary("Puntos acumulados", summaryResponse.Points.ToString(), string.Empty);
            items.Add(itemPoints);

            if(!string.IsNullOrEmpty(ParametersManager.Order.shippingCost))
            {
                if (summaryResponse.IsPrime && summaryResponse.CostRemaining != decimal.Parse("0.0") || !summaryResponse.IsPrime && ParametersManager.Order.TypeOfDispatch == ConstTypeOfDispatch.Delivery)
                {
                    PurchaseSummary itemShipment = new PurchaseSummary("Costo de envío", StringFormat.ToPrice(decimal.Parse(ParametersManager.Order.shippingCost)), string.Empty);
                    items.Add(itemShipment);
                }
            }

            PurchaseSummary itemBag = new PurchaseSummary("Impuesto de bolsa", StringFormat.ToPrice(summaryResponse.CountryTax), string.Empty);
            items.Add(itemBag);

            PurchaseSummary itemDiscount = new PurchaseSummary("Descuentos", StringFormat.ToPrice(summaryResponse.Discounts), string.Empty);
            items.Add(itemDiscount);

            PurchaseSummary itemTotal = new PurchaseSummary("Total", StringFormat.ToPrice(summaryResponse.Total), string.Empty);
            items.Add(itemTotal);

            PurchaseSummarySource purchaseSummarySource = new PurchaseSummarySource(items);
            purchaseSummaryTableView.Source = purchaseSummarySource;
            purchaseSummaryTableView.EstimatedRowHeight = 190;
            purchaseSummaryTableView.RowHeight = UITableView.AutomaticDimension;
            purchaseSummaryTableView.ReloadData();
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                var propertyName = e.PropertyName;
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
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(PurchaseSummaryController), "");
            }
        }

        private void ShowSpinner()
        {
            if (!isSpinnerAdded)
            {
                isSpinnerAdded = true;
                View.AddSubview(customSpinnerView);
                customSpinnerView.Frame = NavigationController.View.Frame;
                customSpinnerView.Start();
                View.BringSubviewToFront(customSpinnerView);
                this.NavigationController?.SetNavigationBarHidden(true, false);
            }
        }

        private void HideSpinner()
        {
            if (isSpinnerAdded)
            {
                isSpinnerAdded = false;
                customSpinnerView.Stop();
                customSpinnerView.RemoveFromSuperview();
                View.SendSubviewToBack(customSpinnerView);
                this.NavigationController?.SetNavigationBarHidden(false, false);
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

        #endregion

        #region Events
        private void BuyButtonTouchUpInside(object sender, EventArgs e)
        {
            viewModel.PaymentCommand.Execute(ParametersManager.Order);
            RegisterEventSummaryPayment();
        }

        private static void RegisterEventSummaryPayment()
        {
            FirebaseEventRegistrationService.Instance.SummaryPayment();
            FacebookEventRegistrationService.Instance.AddPaymentInfo(true);
        }
        #endregion
    }

    public partial class PurchaseSummaryController : IPaymentModel
    {
        public void CheckoutSummaryFetched(PaymentSummaryResponse summaryResponse)
        {

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
                Util.LogException(exception, nameof(PurchaseSummaryController), "");
            }
        }

        public void OrderNotValid()
        {

        }

        private void RegisterEvent(PaymentResponse response)
        {
            viewModel.PaidOrder.Total = response.Total;
            viewModel.PaidOrder.SubTotal = response.SubTotal;
            viewModel.PaidOrder.CountryTax = response.CountryTax;
            FirebaseEventRegistrationService.Instance.SuccessPayment(viewModel.PaidOrder);
            FacebookEventRegistrationService.Instance.Purchased((double)response.Total, ParametersManager.Order?.Products);
        }

        public void PaymentFinished(PaymentResponse response)
        {
            try
            {
                InvokeOnMainThread(() =>
                {
                    if (response.FinishProcess)
                    {
                        if (response.StatusCode == null || (response.StatusCode != null && response.StatusCode.Equals(ConstStatusPay.Approved)))
                        {
                            RegisterEvent(response);
                            string deliveryDate = string.IsNullOrEmpty(ParametersManager.Order.Schedule) ? "" : ParametersManager.Order.DateSelected + " entre " + ParametersManager.Order.Schedule;
                            PaymentConfirmationViewController paymentConfirmationViewController = (PaymentConfirmationViewController)this.NavigationController.Storyboard.InstantiateViewController(nameof(PaymentConfirmationViewController));
                            paymentConfirmationViewController.PaymentSummary = response;
                            paymentConfirmationViewController.DeliveryDate = viewModel.Schedule;
                            this.NavigationController.PushViewController(paymentConfirmationViewController, true);
                            ParametersManager.ProductUpdated = true;
                        }
                        else
                        {
                            PaymentErrorViewController paymentErrorViewController = (PaymentErrorViewController)this.NavigationController.Storyboard.InstantiateViewController(nameof(PaymentErrorViewController));
                            paymentErrorViewController.Response = response;
                            this.NavigationController.PushViewController(paymentErrorViewController, true);
                        //var vcs = this.NavigationController.ViewControllers;
                        //vcs[vcs.Count() - 1] = paymentErrorViewController;
                        //this.NavigationController.ViewControllers = vcs;
                    }
                    }
                    else
                    {
                        GenericErrorView errorView = GenericErrorView.Create();
                        errorView.Configure(EnumErrorCode.ErrorServiceUnavailable.ToString(), AppMessages.ErrorServicesUnavailables, (sender, e) =>
                        {
                            errorView.RemoveFromSuperview();
                        });
                        View.AddSubview(errorView);
                    }
                });
            }
            catch (Exception exception)
            {
                Util.LogException(exception, nameof(PurchaseSummaryController), "");
            }
        }

        public void ProductNews(PaymentSummaryResponse summaryResponse)
        {
            try
            {
                ShowChangesAndPrime(summaryResponse);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, nameof(PurchaseSummaryController), "");
            }
        }

        public void ThereIsNotActiveReservation()
        {
            try
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure(EnumErrorCode.ErrorServiceUnavailable.ToString(), AppMessages.ScheduleReservationErroMessage, (senderd, ed) => errorView.RemoveFromSuperview());
                View.AddSubview(errorView);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, nameof(PurchaseSummaryController), "");
            }
        }
    }
}

