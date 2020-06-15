using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Views;
using GrupoExito.iOS.ViewControllers.PaymentControllers;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Models.Contracts;
using GrupoExito.Models.Enumerations;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers
{
    public partial class ScheduleContainerController : UIViewController
    {
        private SubtotalViewModel subtotalViewModel;
        private List<ScheduleHourViewModel> scheduleHourViewModels;
        private ScheduleViewModel viewModel;
        private ScheduleSource source;
        private EmptyListView emptyView;
        private CustomSpinnerView customSpinnerView;
        private bool isSpinnerAdded = false;
        private nint daySelected = 0;
        private bool isExpressOptionSelected = false;
        private ScheduleHours HourSelected;

        private nint scheduleTypeOption = -1;

        private ShipmentTypeView shipmentType1View;
        private ShipmentTypeView shipmentType2View;

        public ScheduleContainerController(IntPtr handle) : base(handle) { }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.OrderSchedule, nameof(ScheduleContainerController));
        }

        public override void ViewDidLoad()
        {            
            base.ViewDidLoad();

            emptyView = EmptyListView.Create();
            customSpinnerView = CustomSpinnerView.Create();

            scheduleHourViewModels = new List<ScheduleHourViewModel>();
            subtotalViewModel = new SubtotalViewModel("0", "0");
            viewModel = new ScheduleViewModel(ParametersManager.UserContext, DeviceManager.Instance);

            tableView.RegisterNibForCellReuse(LeftCheckedTableViewCell.Nib, LeftCheckedTableViewCell.Key);

            source = new ScheduleSource(scheduleHourViewModels);
            tableView.Source = source;
            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.EstimatedRowHeight = 90;

            LoadSubtotalView();
            subtotalViewModel.BagTaxInfoHandler += BagTaxButton_TouchUpInside;
            viewModel.Delegate = this;

            subtotalViewModel.UpdateContentCommand.Execute(null);

            dayOneButton.TouchUpInside += DayButton_TouchUpInside;
            dayTwoButton.TouchUpInside += DayButton_TouchUpInside;
            dayThreeButton.TouchUpInside += DayButton_TouchUpInside;
            continueButton.TouchUpInside += ContinueButton_TouchUpInside;

            continueButton.Layer.CornerRadius = ConstantStyle.CornerRadius;

            scheduleSubtitleLabel.Text = ParametersManager.UserContext.Address == null ? AppMessages.ScheduledInStoreMessage : AppMessages.ScheduledDeliveryMessage;
        }

        private void SetScheduleTypeOptions()
        {
            scheduleCheckboxImageView.Layer.CornerRadius = scheduleCheckboxImageView.Frame.Size.Width / 2;
            expressCheckboxImageView.Layer.CornerRadius = expressCheckboxImageView.Frame.Size.Width / 2;

            scheduleCheckboxImageView.ClipsToBounds = true;
            expressCheckboxImageView.ClipsToBounds = true;

            scheduleCheckboxImageView.Layer.BorderWidth = ConstantStyle.CircumferenceBorderWidth;
            expressCheckboxImageView.Layer.BorderWidth = ConstantStyle.CircumferenceBorderWidth;

            scheduleCheckboxImageView.Layer.BorderColor = ConstantColor.UiFilterOrderTextSelected.CGColor;
            expressCheckboxImageView.Layer.BorderColor = ConstantColor.UiFilterOrderTextSelected.CGColor;

            scheduleBackgroundView.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;
            expressBackgroundView.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;
        }

        private void LoadSubtotalView()
        {
            SubtotalView subtotalView = SubtotalView.Create();
            subtotalView.Frame = subtotalParentView.Bounds;
            subtotalParentView.AddSubview(subtotalView);
            subtotalView.Setup(subtotalViewModel);
            subtotalView.LayoutIfNeeded();
        }

        private bool IsPrimeViewAllowed()
        {
            return (ParametersManager.UserContext.Address != null) && ParametersManager.UserContext.Prime;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            ConfigureNavigationBar();

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.GetScheduleCommand.Execute(ParametersManager.Order.TotalProducts);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        private void ConfigureNavigationBar()
        {
            this.NavigationController.NavigationBar.Hidden = false;
            NavigationHeaderView navigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            navigationView.HiddenCarData();
            navigationView.IsSummaryDisabled = true;
            navigationView.HiddenAccountProfile();
            navigationView.EnableBackButton(true);
            navigationView.IsAccountEnabled = false;
            this.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
        }

        private void ShowSpinner()
        {
            if (!isSpinnerAdded)
            {
                isSpinnerAdded = true;
                View.AddSubview(customSpinnerView);
                customSpinnerView.Frame = View.Bounds;
                customSpinnerView.Start();
            }
        }

        private void HideSpinner()
        {
            if (isSpinnerAdded)
            {
                isSpinnerAdded = false;
                customSpinnerView.Stop();
                customSpinnerView.RemoveFromSuperview();
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                var propertyName = e.PropertyName;
                switch (propertyName)
                {
                    case nameof(viewModel.ScheduleResponse):
                        {
                            InvokeOnMainThread(() =>
                            {
                                isExpressOptionSelected = viewModel.ScheduleResponse.IsExpress;
                                ConfigureView(viewModel.ScheduleResponse);
                            });
                        }
                        break;
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
                    case nameof(viewModel.ContingencyResponse):
                        {
                            SaveContingencyOrder(viewModel.ContingencyResponse);
                            InvokeOnMainThread(() =>
                            {
                                ShowDeliveryPromise(ParametersManager.Order);
                            });
                        }
                        break;
                    case nameof(viewModel.SuccessReservation):
                        {
                            if (viewModel.SuccessReservation)
                            {
                                InvokeOnMainThread(() =>
                                {
                                    PaymentContainerController paymentContainerController = (PaymentContainerController)this.Storyboard.InstantiateViewController(nameof(PaymentContainerController));
                                    this.NavigationController.PushViewController(paymentContainerController, true);
                                });
                            }
                            else
                            {
                                GenericErrorView errorView = GenericErrorView.Create();
                                errorView.Configure(EnumErrorCode.ErrorServiceUnavailable.ToString(), AppMessages.ScheduleReservationErroMessage, (senderd, ed) => errorView.RemoveFromSuperview());
                                errorView.Frame = View.Bounds;
                                View.AddSubview(errorView);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(ScheduleContainerController), "");
            }
        }

        private void ConfigureView(OrderScheduleResponse response)
        {
            foreach (var item in shipmentTypeStackView.ArrangedSubviews)
            {
                shipmentTypeStackView.RemoveArrangedSubview(item);
            }

            if (response.IsExpress)
            {
                shipmentTypeStackView.Hidden = false;

                if (shipmentType1View == null)
                {
                    shipmentType1View = ShipmentTypeView.Create();
                    shipmentType1View.TouchUpAction += ShipmentType1View_TouchUpAction; ;
                }

                shipmentType1View.Configure(ShipmentType.ExpressDelivery, isExpressOptionSelected, response.PricePromise, response.MinutesPromiseDelivery);
                shipmentTypeStackView.AddArrangedSubview(shipmentType1View);

                if (response.Schedules.Any())
                {
                    if (shipmentType2View == null)
                    {
                        shipmentType2View = NSBundle.MainBundle.LoadNib(nameof(ShipmentTypeView), Self, null).GetItem<ShipmentTypeView>(0);
                        shipmentType2View.TouchUpAction += ShipmentType2View_TouchUpAction; ;
                    }

                    shipmentType2View.Configure(ShipmentType.ScheduleDelivery, !isExpressOptionSelected, null, 0);
                    shipmentTypeStackView.AddArrangedSubview(shipmentType2View);
                }
                else
                {
                    shipmentTypeStackView.AddArrangedSubview(new UIView());
                }
            }
            else
            {
                shipmentTypeStackView.Hidden = true;

                if (response.Schedules.Any())
                {
                    LoadDays(viewModel.ScheduleResponse.Schedules);
                    ConfigureDaysView(response);
                }
            }

            ConfigurePrimeView(response);
            LoadDays(viewModel.ScheduleResponse.Schedules);
            ConfigureDaysView(response);
            ReloadTableView();
        }

        private void ConfigureDaysView(OrderScheduleResponse response)
        {
            if (!isExpressOptionSelected)
            {
                if (response.Schedules.Any())
                {
                    dayLabel.Hidden = true;
                    hourLabel.Hidden = true;
                    daysStackView.Hidden = true;
                }
            }
            else
            {
                dayLabel.Hidden = true;
                hourLabel.Hidden = true;

                daysStackView.Hidden = true;
            }

            daysStackView.Hidden = isExpressOptionSelected;

            dayLabel.Hidden = isExpressOptionSelected;
            hourLabel.Hidden = isExpressOptionSelected;
        }

        private void ConfigurePrimeView(OrderScheduleResponse response)
        {
            if (ParametersManager.UserContext.Address == null)
            {
                primeView.Hidden = true;
            }
            else
            {
                if (IsPrimeViewAllowed() && !isExpressOptionSelected)
                {
                    primeView.Hidden = false;

                    primeView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    primeView.Layer.BorderColor = UIColor.Orange.CGColor;
                    primeView.Layer.BorderWidth = 2;
                    primeLabel.Text = "PRIME: Llevaremos tu mercado sin costo una vez el total de la compra después de seleccionar el medio de pago sea mayor o igual a $50.000";
                    primeLabel.TextColor = UIColor.Orange;
                    primeIconImageView.Image = UIImage.FromFile(ConstantImages.PrimeOrange);
                }
                else
                {
                    primeView.Hidden = true;
                }
            }
        }

        private void ReloadTableView()
        {
            tableView.Hidden = isExpressOptionSelected;
            if (!isExpressOptionSelected)
            {
                CreateHourViewModels(viewModel.ScheduleResponse.Schedules[(int)daySelected]);
                hoursTableViewHeightConstraint.Constant = scheduleHourViewModels.Count * ConstantViewSize.ScheduleDeliveryTimeCellHeight;
                tableView.ReloadData();
                HourSelected = viewModel.ScheduleResponse.Schedules[(int)daySelected].Hours[0];
                tableView.SelectRow(NSIndexPath.FromRowSection(0, 0), false, UITableViewScrollPosition.None);
            }
            else hoursTableViewHeightConstraint.Constant = 0;
        }

        private void ScrollToView(UIView view)
        {
            if (view != null)
            {
                InvokeOnMainThread(() =>
                {
                    mainScrollView.ScrollRectToVisible(new CGRect(view.Frame.X, view.Frame.Y, mainScrollView.Frame.Width, mainScrollView.Frame.Height), true);
                });
            }
        }

        private void SaveContingencyOrder(ScheduleContingencyResponse contingencyResponse)
        {
            var order = ParametersManager.Order;
            if (order != null && contingencyResponse != null)
            {
                order.TypeOfDispatch = contingencyResponse.TypeShippingGroup;
                order.DateSelected = contingencyResponse.DateSelected;
                order.PluDispatch = contingencyResponse.PluDispatch;
                order.shippingCost = contingencyResponse.ShippingCost;
                order.TypeDispatch = contingencyResponse.TypeDispatch;
                order.Schedule = contingencyResponse.UserSchedule;
                order.Contingency = true;

                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
            }
        }

        private bool SaveOrder()
        {
            Order order = ParametersManager.Order;
            if (order != null)
            {
                order.TypeModality = isExpressOptionSelected ? ConstTypeModality.Express : ConstTypeModality.ScheduledPickup;

                if (!isExpressOptionSelected)
                {
                    order.shippingCost = string.IsNullOrEmpty(HourSelected.ShippingCostValue) ? "0" : HourSelected.ShippingCostValue;
                    order.Schedule = HourSelected.Shedule;
                    order.DateSelected = viewModel.ScheduleResponse.Schedules[(int)daySelected].Description;
                    order.PluDispatch = HourSelected.ShippingCostPlu;
                }
                else
                {
                    order.PluDispatch = viewModel.ScheduleResponse.ShippingCostPlu;
                    order.shippingCost = viewModel.ScheduleResponse.PricePromise;
                    order.MinutesPromiseDelivery = viewModel.ScheduleResponse.MinutesPromiseDelivery;

                    order.DateSelected = string.Empty;
                    order.Schedule = string.Empty;
                }

                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ShowDeliveryPromise(Order order)
        {
            DeliveryPromiseViewController deliveryPromiseViewController = (DeliveryPromiseViewController)this.Storyboard.InstantiateViewController(nameof(DeliveryPromiseViewController));
            deliveryPromiseViewController.PricePromise = order.shippingCost;
            deliveryPromiseViewController.Day = order.DateSelected;
            deliveryPromiseViewController.HoursRange = order.Schedule;
            deliveryPromiseViewController.BagTax = subtotalViewModel.BagTax;
            deliveryPromiseViewController.Subtotal = subtotalViewModel.Subtotal;

            var viewControllers = this.NavigationController.ViewControllers;
            viewControllers[viewControllers.Count() - 1] = deliveryPromiseViewController;
            this.NavigationController.ViewControllers = viewControllers;
        }

        private void LoadDays(IList<ScheduleDays> schedules)
        {
            dayOneLabel.Text = schedules[0].Description;
            dayOneView.Layer.CornerRadius = 5;

            if (daySelected == 0)
            {
                dayOneLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersBodySize);
                dayOneLabel.TextColor = ConstantColor.DefaultSelectedText;
            }
            else
            {
                dayOneLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                dayOneLabel.TextColor = ConstantColor.DefaultDeselectedText;
            }

            string day = DateTime.Now.ToString("dd");
            day = day.TrimStart('0');
            string[] splitday = schedules[0].Description.Split(' ');

            if ((splitday != null) && splitday.Length > 0 && (day.Equals(splitday[1])))
            {
                dayOneLabel.Text = "HOY\n" + schedules[0].Description;
            }

            if (schedules.Count > 1)
            {
                dayTwoLabel.Text = schedules[1].Description;
                dayTwoView.Layer.CornerRadius = 5;

                if (daySelected == 1)
                {
                    dayTwoLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersBodySize);
                    dayTwoLabel.TextColor = ConstantColor.DefaultSelectedText;
                }
                else
                {
                    dayTwoLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                    dayTwoLabel.TextColor = ConstantColor.DefaultDeselectedText;
                }
            }
            else
            {
                dayOneView.RemoveFromSuperview();
                dayTwoView.RemoveFromSuperview();
            }
            if (schedules.Count > 2)
            {
                dayThreeLabel.Text = schedules[2].Description;
                dayThreeView.Layer.CornerRadius = 5;

                if (daySelected == 2)
                {
                    dayThreeLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersBodySize);
                    dayThreeLabel.TextColor = ConstantColor.DefaultSelectedText;
                }
                else
                {
                    dayThreeLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                    dayThreeLabel.TextColor = ConstantColor.DefaultDeselectedText;
                }
            }
            else
            {
                dayThreeView.RemoveFromSuperview();
            }
        }

        private void CreateHourViewModels(ScheduleDays schedules)
        {
            scheduleHourViewModels.Clear();

            foreach (var item in schedules.Hours)
            {
                ScheduleHourViewModel hourViewModel = new ScheduleHourViewModel(item);
                hourViewModel.CellSelected += HourViewModel_CellSelected;

                scheduleHourViewModels.Add(hourViewModel);
            }
        }

        void HourViewModel_CellSelected(object sender, EventArgs e)
        {
            if(sender is ScheduleHourViewModel hourViewModel)
            {
                HourSelected = hourViewModel.Hour;
            }
        }

        private void UpdateDayButtons(nint tag)
        {
            InvokeOnMainThread(() =>
            {
                switch (tag)
                {
                    case 0:
                        daySelected = 0;
                        dayOneLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersBodySize);
                        dayTwoLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                        dayThreeLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                        dayOneView.BackgroundColor = ConstantColor.UiPrimary;
                        dayTwoView.BackgroundColor = ConstantColor.UiBackgroundSummarySelector;
                        dayThreeView.BackgroundColor = ConstantColor.UiBackgroundSummarySelector;
                        dayOneLabel.TextColor = ConstantColor.DefaultSelectedText;
                        dayTwoLabel.TextColor = ConstantColor.DefaultDeselectedText;
                        dayThreeLabel.TextColor = ConstantColor.DefaultDeselectedText;
                        break;
                    case 1:
                        daySelected = 1;
                        dayOneLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                        dayTwoLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersBodySize);
                        dayThreeLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                        dayOneView.BackgroundColor = ConstantColor.UiBackgroundSummarySelector;
                        dayTwoView.BackgroundColor = ConstantColor.UiPrimary;
                        dayThreeView.BackgroundColor = ConstantColor.UiBackgroundSummarySelector;
                        dayTwoLabel.TextColor = ConstantColor.DefaultSelectedText;
                        dayOneLabel.TextColor = ConstantColor.DefaultDeselectedText;
                        dayThreeLabel.TextColor = ConstantColor.DefaultDeselectedText;
                        break;
                    case 2:
                        daySelected = 2;
                        dayOneLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                        dayTwoLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                        dayThreeLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersBodySize);
                        dayOneView.BackgroundColor = ConstantColor.UiBackgroundSummarySelector;
                        dayTwoView.BackgroundColor = ConstantColor.UiBackgroundSummarySelector;
                        dayThreeView.BackgroundColor = ConstantColor.UiPrimary;
                        dayThreeLabel.TextColor = ConstantColor.DefaultSelectedText;
                        dayTwoLabel.TextColor = ConstantColor.DefaultDeselectedText;
                        dayOneLabel.TextColor = ConstantColor.DefaultDeselectedText;
                        break;
                }
            });
        }

        void BagTaxButton_TouchUpInside(object sender, EventArgs e)
        {
            PopUpInformationView bagTaxView = PopUpInformationView.Create(AppMessages.BagTax, AppMessages.BagTaxDisclaimer);
            this.NavigationController.SetNavigationBarHidden(true, true);
            bagTaxView.Frame = View.Bounds;
            bagTaxView.LayoutIfNeeded();
            View.AddSubview(bagTaxView);

            bagTaxView.AcceptButtonHandler += () =>
            {
                this.NavigationController.SetNavigationBarHidden(false, true);
                bagTaxView.RemoveFromSuperview();
            };
            bagTaxView.CloseButtonHandler += () =>
            {
                this.NavigationController.SetNavigationBarHidden(false, true);
                bagTaxView.RemoveFromSuperview();
            };
        }

        void ShipmentType1View_TouchUpAction()
        {
            bool responseNetWorking = DeviceManager.Instance.IsNetworkAvailable().Result;
            if (!responseNetWorking)
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage, (sender, e) =>
                {
                    viewModel.GetScheduleCommand.Execute(ParametersManager.Order.TotalProducts);
                    errorView.RemoveFromSuperview();
                });
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            }
            else
            {
                if (!isExpressOptionSelected)
                {
                    isExpressOptionSelected = true;
                    ConfigureView(viewModel.ScheduleResponse);
                }
            }
        }

        void ShipmentType2View_TouchUpAction()
        {
            bool responseNetWorking = DeviceManager.Instance.IsNetworkAvailable().Result;
            if (!responseNetWorking)
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage, (sender, e) =>
                {
                    viewModel.GetScheduleCommand.Execute(ParametersManager.Order.TotalProducts);
                    errorView.RemoveFromSuperview();
                });
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            }
            else
            {
                if (isExpressOptionSelected)
                {
                    isExpressOptionSelected = false;
                    ConfigureView(viewModel.ScheduleResponse);
                }
            }
        }

        void ScheduleType_TouchUpInside(object sender, EventArgs e)
        {
            UIButton paymentTypeButton = (UIButton)sender;
            if (paymentTypeButton.Tag != scheduleTypeOption)
            {
                scheduleTypeOption = paymentTypeButton.Tag;
                SetOptionSelected();

                switch (scheduleTypeOption)
                {
                    case 0:
                        ConfigureView(viewModel.ScheduleResponse);
                        break;
                    case 1:
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetOptionSelected()
        {
            switch (scheduleTypeOption)
            {
                case 0:
                    SelectCash(true);
                    SelectDataphone(false);
                    break;
                case 1:
                    SelectDataphone(true);
                    SelectCash(false);
                    break;
                default:
                    SelectCash(false);
                    SelectDataphone(false);
                    break;
            }
        }

        private void SelectDataphone(bool select)
        {
            if (select)
            {
                expressBackgroundView.BackgroundColor = ConstantColor.UiPrimary;
                expressCheckboxImageView.Image = UIImage.FromFile(ConstantImages.SeleccionarCirculo);
                expressCheckboxImageView.BackgroundColor = UIColor.Clear;
                expressLabel.TextColor = ConstantColor.DefaultSelectedText;
                expressDescriptionLabel.TextColor = ConstantColor.DefaultSelectedText;
            }
            else
            {
                expressBackgroundView.BackgroundColor = ConstantColor.UiGrayBackground;
                expressCheckboxImageView.Image = null;
                expressCheckboxImageView.BackgroundColor = ConstantColor.UiGrayBackground;
                expressLabel.TextColor = ConstantColor.DefaultDeselectedText;
                expressDescriptionLabel.TextColor = ConstantColor.DefaultDeselectedText;
            }
        }

        private void SelectCash(bool select)
        {
            if (select)
            {
                scheduleBackgroundView.BackgroundColor = ConstantColor.UiPrimary;
                scheduleCheckboxImageView.Image = UIImage.FromFile(ConstantImages.SeleccionarCirculo);
                scheduleCheckboxImageView.BackgroundColor = UIColor.Clear;
                scheduleLabel.TextColor = ConstantColor.DefaultSelectedText;
                scheduleDescriptionLabel.TextColor = ConstantColor.DefaultSelectedText;
            }
            else
            {
                scheduleBackgroundView.BackgroundColor = ConstantColor.UiGrayBackground;
                scheduleCheckboxImageView.Image = null;
                scheduleCheckboxImageView.BackgroundColor = ConstantColor.UiGrayBackground;
                scheduleLabel.TextColor = ConstantColor.DefaultDeselectedText;
                scheduleDescriptionLabel.TextColor = ConstantColor.DefaultDeselectedText;
            }
        }

        void DayButton_TouchUpInside(object sender, EventArgs e)
        {
            daySelected = ((UIButton)sender).Tag;
            UpdateDayButtons(daySelected);
            ReloadTableView();
        }

        void ContinueButton_TouchUpInside(object sender, EventArgs e)
        {
            bool result = SaveOrder();
            if (result)
            {
                if (ParametersManager.Order.TypeModality == ConstTypeModality.Express)
                {
                    PaymentContainerController paymentContainerController = (PaymentContainerController)this.Storyboard.InstantiateViewController(nameof(PaymentContainerController));
                    this.NavigationController.PushViewController(paymentContainerController, true);
                }
                else
                {
                    viewModel.ScheduleReservationCommand.Execute(ParametersManager.Order);
                }
            }

            RegisterEvent();
        }

        private void RegisterEvent()
        {
            FirebaseEventRegistrationService.Instance.Schedule();
        }
    }

    public class ScheduleSource : UITableViewSource
    {
        private List<ScheduleHourViewModel> scheduleHourViewModels;

        public ScheduleSource(List<ScheduleHourViewModel> scheduleHourViewModels)
        {
            this.scheduleHourViewModels = scheduleHourViewModels;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(LeftCheckedTableViewCell.Key, indexPath);

            ((GenericCell)cell).Setup(scheduleHourViewModels[indexPath.Row]);

            cell.LayoutMargins = UIEdgeInsets.Zero;
            cell.LayoutIfNeeded();

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return scheduleHourViewModels.Count;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.ScheduleDeliveryTimeCellHeight;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ScheduleHourViewModel viewModel = scheduleHourViewModels[indexPath.Row];
            viewModel.CellSelected.Invoke(viewModel, null);
        }
    }

    public partial class ScheduleContainerController : IScheduleModel
    {
        public void HandleError(Exception ex)
        {
            try
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure(ex.Data[nameof(EnumExceptionDataKeys.Code)].ToString(), ex.Data[nameof(EnumExceptionDataKeys.Message)].ToString(), (sender, e) =>
                {
                    viewModel.GetScheduleCommand.Execute(ParametersManager.Order.TotalProducts);
                    errorView.RemoveFromSuperview();
                });
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, nameof(ScheduleContainerController), "");
            }
        }
    }
}

