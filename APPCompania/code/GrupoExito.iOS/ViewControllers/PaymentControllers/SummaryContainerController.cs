using System;
using System.Collections.Generic;
using System.Linq;
using CoreLocation;
using Foundation;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Products;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers;
using GrupoExito.iOS.ViewControllers.OrderScheduleControllers;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ProductControllers;
using GrupoExito.iOS.ViewControllers.RecipesControllers;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers;
using GrupoExito.iOS.Views.PaymentViews;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Models.Contracts;
using GrupoExito.Models.Enumerations;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    public partial class SummaryContainerController : UIViewController
    {
        private SummaryViewModel summaryViewModel;
        private AddressViewModel addressViewModel;
        private SubtotalViewModel subtotalViewModel;
        private List<ProductViewModel> productViewModels;

        private CLLocationManager LocationManager;
        private SummarySource source;
        private bool isSpinnerAdded = false;
        private EmptyListView emptyView;
        private CustomSpinnerView customSpinnerView;

        public SummaryContainerController(IntPtr handle) : base(handle) { }


        public override void ViewDidLoad()
        {            
            base.ViewDidLoad();
            this.productViewModels = new List<ProductViewModel>();

            LocationManager = new CLLocationManager();

            addressViewModel = new AddressViewModel(ParametersManager.UserContext);
            addressViewModel.CellSelected += AddressViewModel_CellSelected;
            subtotalViewModel = new SubtotalViewModel("0", "0");
            summaryViewModel = new SummaryViewModel(ParametersManager.UserContext, DeviceManager.Instance);

            subtotalViewModel.BagTaxInfoHandler += SubtotalViewModel_BagTaxInfoHandler;
            summaryViewModel.Delegate = this;

            source = new SummarySource(productViewModels, addressViewModel);

            tableView.RegisterNibForCellReuse(DeliveryAddressCell.Nib, DeliveryAddressCell.Key);
            tableView.RegisterNibForCellReuse(SummaryPriceInfoCell.Nib, SummaryPriceInfoCell.Key);
            tableView.RegisterNibForCellReuse(SummaryProductViewCell.Nib, SummaryProductViewCell.Key);
            tableView.RegisterNibForCellReuse(SubtotalViewCell.Nib, SubtotalViewCell.Key);

            tableView.Source = source;
            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.EstimatedRowHeight = 140;

            continueButton.TouchUpInside += ContinueButton_TouchUpInside;
            emptyCarButton.TouchUpInside += EmptyCarButton_TouchUpInside;
            searchButton.TouchUpInside += SearchButton_TouchUpInside;
            checkerButton.TouchUpInside += CheckerButton_TouchUpInside;

            SubtotalView subtotalView = SubtotalView.Create();
            subtotalView.Frame = subtotalParent.Bounds;
            subtotalView.Setup(subtotalViewModel);
            subtotalParent.AddSubview(subtotalView);
            subtotalView.LayoutIfNeeded();

            emptyView = EmptyListView.Create();
            customSpinnerView = CustomSpinnerView.Create();
            emptyView.AddProductsHandler += () =>
            {
                this.NavigationController.PopViewController(true);
            };
            emptyCarButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            emptyCarButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
            emptyCarButton.Layer.BorderColor = ConstantColor.UiLaterIncomeTitleButtonsNotSelected.CGColor;
            continueButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            summaryViewModel.Dependency = ParametersManager.UserContext.DependencyId;
            summaryViewModel.ValidateDependencyChangeCommand.Execute(null);
            summaryViewModel.PropertyChanged += SummaryViewModel_PropertyChanged;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (ParametersManager.ChangeAddress)
            {
                summaryViewModel.Dependency = ParametersManager.UserContext.DependencyId;
                summaryViewModel.ValidateDependencyChangeCommand.Execute(null);
                ProductCarModel DataBase = new ProductCarModel(ProductCarDataBase.Instance);
                Dictionary<string, object> summary = DataBase.RecalculateSummary();
                int quantity = 0;
                decimal price = 0;
                if (summary != null)
                {
                    quantity = int.Parse(summary["productQuantity"].ToString());
                    price = decimal.Parse(summary["totalPrice"].ToString());
                }
                UpdateTopBar(summary);
                addressViewModel.UpdateData(ParametersManager.UserContext);
                Plugin.FirebasePushNotification.CrossFirebasePushNotification.Current.RegisterForPushNotifications();
                ParametersManager.ChangeAddress = false;
            }
            ConfigureNavigationBar();
            summaryViewModel.LoadSummaryCommand.Execute(null);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Summary, nameof(SummaryContainerController));
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            //summaryViewModel.PropertyChanged -= SummaryViewModel_PropertyChanged;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void ConfigureNavigationBar()
        {
            this.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
            this.NavigationController.NavigationBar.Hidden = false;
            NavigationHeaderView navigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            navigationView.LoadControllerBase(this);
            navigationView.HiddenCarData();
            navigationView.HiddenAccountProfile();
            navigationView.EnableBackButton(true);
        }

        private void CreateProductViewModels()
        {
            productViewModels.Clear();

            string currentCategoryId = string.Empty;

            foreach (var product in summaryViewModel.Products)
            {
                if (!string.IsNullOrEmpty(product.CategoryId))
                {
                    if (product.CategoryId.Equals(currentCategoryId))
                    {
                        product.CategoryId = string.Empty;
                    }
                    else
                    {
                        currentCategoryId = product.CategoryId;
                    }
                }

                ProductViewModel productViewModel = new ProductViewModel(product);
                productViewModel.PropertyChanged += ProductViewModel_PropertyChanged;
                productViewModels.Add(productViewModel);
            }
        }

        public void RegisterAddProductEvent(Product product)
        {
            FirebaseEventRegistrationService.Instance.AddProductToCart(product, product.CategoryName);
            FacebookEventRegistrationService.Instance.AddProductToCart(product);
        }

        void ProductViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                ProductViewModel vm = (ProductViewModel)sender;
                var propertyName = e.PropertyName;
                switch (propertyName)
                {
                    case nameof(vm.ProductDeletedId):
                        {
                            InvokeOnMainThread(() =>
                            {
                                ProductViewModel productViewModel = productViewModels.First((viewModel) => vm.ProductDeletedId.Equals(viewModel.ProductId));
                                if (productViewModel != null)
                                {
                                    productViewModels.Remove(productViewModel);

                                    if (productViewModels.Any())
                                    {
                                        tableView.ReloadData();
                                    }
                                    else ShowEmptySummary();

                                    subtotalViewModel.UpdateContentCommand.Execute(null);
                                    ParametersManager.ProductUpdated = true;
                                }
                            });
                        }
                        break;
                    case nameof(vm.ProductUpdated):
                        {
                            InvokeOnMainThread(() =>
                            {
                                tableView.ReloadData();
                                subtotalViewModel.UpdateContentCommand.Execute(null);
                                ParametersManager.ProductUpdated = true;
                                if (vm.IsAdd)
                                {
                                    if (vm.ProductUpdated != null)
                                    {
                                        RegisterAddProductEvent(vm.ProductUpdated);
                                    }
                                }
                                else
                                {
                                    if (vm.ProductUpdated != null)
                                    {
                                        FirebaseEventRegistrationService.Instance.DeleteProductFromCart(vm.ProductUpdated, vm.ProductUpdated.CategoryName);
                                    }
                                }
                            });
                        }
                        break;
                    case nameof(vm.ProductToBeDeleted):
                        {
                            InvokeOnMainThread(() =>
                            {
                                ShowDeleteRequest(vm);
                            });
                        }
                        break;
                    case nameof(vm.HowDoYouLikeItProduct):
                        {
                            InvokeOnMainThread(() =>
                            {
                                HowDoYouLikeIt(vm);
                            });
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(SummaryContainerController), "");
            }
        }

        void SummaryViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                var propertyName = e.PropertyName;
                switch (propertyName)
                {
                    case nameof(summaryViewModel.IsBusy):
                        {
                            InvokeOnMainThread(() =>
                            {
                                if (summaryViewModel.IsBusy && !isSpinnerAdded)
                                    ShowSpinner();
                                else if (!summaryViewModel.IsBusy)
                                    HideSpinner();
                            });
                        }
                        break;
                    case nameof(summaryViewModel.Products):
                        {
                            InvokeOnMainThread(() =>
                            {
                                if (summaryViewModel.Products.Any())
                                {
                                    HideEmptySummary();
                                    CreateProductViewModels();
                                    tableView.ReloadData();
                                }
                                else
                                {
                                    ParametersManager.ProductUpdated = true;
                                    ShowEmptySummary();
                                }
                            });

                            subtotalViewModel.UpdateContentCommand.Execute(null);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(SummaryContainerController), "");
            }
        }

        void AddressViewModel_CellSelected(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                PopUpInformationView addressChangeAdviceView = PopUpInformationView.Create("", AppMessages.MessageChangeAdress);
                this.NavigationController.SetNavigationBarHidden(true, false);
                addressChangeAdviceView.SetTitleAcceptButton(AppMessages.MessagebtnChangeAdress, UIControlState.Normal);
                addressChangeAdviceView.Frame = View.Bounds;
                addressChangeAdviceView.LayoutIfNeeded();
                View.AddSubview(addressChangeAdviceView);

                addressChangeAdviceView.AcceptButtonHandler += () =>
                {
                    this.NavigationController.SetNavigationBarHidden(false, true);
                    addressChangeAdviceView.RemoveFromSuperview();
                    LaterIncomeViewController laterIncomeViewController = (LaterIncomeViewController)this.Storyboard.InstantiateViewController(nameof(LaterIncomeViewController));
                    //RegisterEventAddress();
                    laterIncomeViewController.HidesBottomBarWhenPushed = true;
                    this.NavigationController.PushViewController(laterIncomeViewController, true);
                };
                addressChangeAdviceView.CloseButtonHandler += () =>
                {
                    this.NavigationController.SetNavigationBarHidden(false, true);
                    addressChangeAdviceView.RemoveFromSuperview();
                };

                //addressChangeAdvice.AcceptButtonHandler += (string text) =>
                //{
                //    addressChangeAdvice.RemoveFromSuperview();
                //    LaterIncomeViewController laterIncomeViewController = (LaterIncomeViewController)this.Storyboard.InstantiateViewController(nameof(LaterIncomeViewController));
                //    //RegisterEventAddress();
                //    laterIncomeViewController.HidesBottomBarWhenPushed = true;
                //    this.NavigationController.PushViewController(laterIncomeViewController, true);
                //};
            });
        }

        private void ShowConfirmation(string message, Action acceptAction)
        {
            var defaultAlert = UIAlertController.Create(AppMessages.TitleGenericDialog, message, UIAlertControllerStyle.Alert);
            defaultAlert.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, alert =>
            {
                acceptAction?.Invoke();
            }));
            defaultAlert.AddAction(UIAlertAction.Create(AppMessages.CancelButtonText, UIAlertActionStyle.Default, alert =>
            {
            }));
            PresentViewController(defaultAlert, true, null);
        }

        private void ShowDeleteRequest(ProductViewModel productViewModel)
        {
            ShowConfirmation(AppMessages.DeleteProductSummaryMessage, () =>
            {
                productViewModel.DeleteCommand.Execute(null);
                if (productViewModel.ProductToBeDeleted != null)
                {
                    FirebaseEventRegistrationService.Instance.DeleteProductFromCart(productViewModel.ProductToBeDeleted, productViewModel.ProductToBeDeleted.CategoryName);
                }
            });
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

        private void UpdateTopBar(Dictionary<string, object> totalUpdated)
        {
            int quantity = int.Parse(totalUpdated[ConstDataBase.ProductQuantity].ToString());
            decimal price = decimal.Parse(totalUpdated[ConstDataBase.TotalPrice].ToString());

            NavigationHeaderView NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            NavigationView.UpdateCar(StringFormat.ToPrice(price), StringFormat.ToQuantity(quantity));
        }

        void ContinueButton_TouchUpInside(object sender, EventArgs e)
        {
            summaryViewModel.BuyCommand.Execute(null);
        }

        void EmptyCarButton_TouchUpInside(object sender, EventArgs e)
        {
            ShowConfirmation(AppMessages.FlushCarMessage, () =>
            {
                FirebaseEventRegistrationService.Instance.DeleteProductsFromCart(summaryViewModel.Products);
                summaryViewModel.EmptyCarCommand.Execute(null);
            });
        }

        void SubtotalViewModel_BagTaxInfoHandler(object sender, EventArgs e)
        {
            PopUpInformationView bagTaxView = PopUpInformationView.Create(AppMessages.BagTax, AppMessages.BagTaxDisclaimer);
            this.NavigationController.SetNavigationBarHidden(true, false);
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

        void SearchButton_TouchUpInside(object sender, EventArgs e)
        {
            SearchProductViewController searchProductViewController = (SearchProductViewController)this.Storyboard.InstantiateViewController(nameof(SearchProductViewController));
            searchProductViewController.HidesBottomBarWhenPushed = true;
            this.NavigationController.PushViewController(searchProductViewController, true);
        }

        void CheckerButton_TouchUpInside(object sender, EventArgs e)
        {
            InitialRecipeViewController initialRecipeViewController = (InitialRecipeViewController)this.Storyboard.InstantiateViewController(nameof(InitialRecipeViewController));
            initialRecipeViewController.HidesBottomBarWhenPushed = true;
            this.NavigationController.PushViewController(initialRecipeViewController, true);
        }

        private void RegisterEventAddress()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEvent(AnalyticsEvent.HomeAddress);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEvent(AnalyticsEvent.HomeAddress);
        }

        private void ShowEmptySummary()
        {
            emptyView.Frame = View.Bounds;
            View.AddSubview(emptyView);
            emptyView.LayoutIfNeeded();
        }

        private void HideEmptySummary()
        {
            emptyView.RemoveFromSuperview();
        }

        public void HowDoYouLikeIt(ProductViewModel productViewModel)
        {
            PopUpEnterTextView howDoYouLikeItView = PopUpEnterTextView.Create();
            howDoYouLikeItView.SetText("¿Cómo te gusta?", productViewModel.Note);
            this.NavigationController.SetNavigationBarHidden(true, false);
            howDoYouLikeItView.SetAsEditable(true);
            howDoYouLikeItView.Frame = View.Bounds;
            howDoYouLikeItView.LayoutIfNeeded();
            View.AddSubview(howDoYouLikeItView);

            howDoYouLikeItView.AcceptButtonHandler += (string text) =>
            {
                this.NavigationController.SetNavigationBarHidden(false, true);
                howDoYouLikeItView.RemoveFromSuperview();
                if (!string.IsNullOrEmpty(text.Trim()))
                {
                    productViewModel.Note = text.Trim();
                    productViewModel.UpdateCommand.Execute(null);
                }
            };

            howDoYouLikeItView.CloseButtonHandler += () =>
            {
                this.NavigationController.SetNavigationBarHidden(false, true);
                howDoYouLikeItView.RemoveFromSuperview();
            };
        }

        public void RegisterContinueEvent()
        {
            FirebaseEventRegistrationService.Instance.Summary();
            FacebookEventRegistrationService.Instance.InitiatedCheckout(true, ParametersManager.Order.Products);
        }
    }

    public partial class SummaryContainerController : ISummaryViewModel
    {
        public void HandleError(Exception ex)
        {
            try
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure(ex.Data[nameof(EnumExceptionDataKeys.Code)].ToString(), ex.Data[nameof(EnumExceptionDataKeys.Message)].ToString(), (sender, e) =>
                {
                    errorView.RemoveFromSuperview();
                    summaryViewModel.LoadSummaryCommand.Execute(null);
                });
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, nameof(SummaryContainerController), "");
            }
        }

        public void HasNotCorrespondenceAddress()
        {
            try
            {
                AddCorrespondenceAddressController addCorrespondenceController = new AddCorrespondenceAddressController
                {
                    HidesBottomBarWhenPushed = true
                };
                this.NavigationController.PushViewController(addCorrespondenceController, false);
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(SummaryContainerController), "");
            }
        }

        public void OrderUploaded()
        {
            try
            {
                HideSpinner();
                RegisterContinueEvent();
                ScheduleContainerController scheduleDeliveryViewController = (ScheduleContainerController)this.Storyboard.InstantiateViewController(nameof(ScheduleContainerController));
                this.NavigationController.PushViewController(scheduleDeliveryViewController, true);
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(SummaryContainerController), "");
            }
        }

        public void ConnectionUnavailable()
        {
            try
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure(nameof(EnumErrorCode.InternetErrorMessage), AppMessages.InternetErrorMessage, (sender, e) => { errorView.RemoveFromSuperview(); });
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(SummaryContainerController), "");
            }
        }

        public void ProductsDeletedDueToDependencyChange(List<Product> deletedProducts)
        {
            try
            {
                ChangedOrderViewController changedOrderViewController = (ChangedOrderViewController)this.Storyboard.InstantiateViewController(nameof(ChangedOrderViewController));
                changedOrderViewController.Products = deletedProducts;
                this.PresentViewController(changedOrderViewController, true, null);
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(SummaryContainerController), "");
            }
        }
    }

    public class SummarySource : UITableViewSource
    {
        private AddressViewModel _addressViewModel { get; set; }
        private List<ProductViewModel> _productViewModels { get; set; }

        public SummarySource(List<ProductViewModel> productViewModels, AddressViewModel addressViewModel)
        {
            this._addressViewModel = addressViewModel;
            this._productViewModels = productViewModels;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            string identifier = string.Empty;

            switch (indexPath.Section)
            {
                case 0:
                    identifier = DeliveryAddressCell.Key;
                    break;
                default:
                    identifier = SummaryProductViewCell.Key;
                    break;
            }

            var cell = tableView.DequeueReusableCell(identifier, indexPath);

            ((GenericCell)cell).Setup(GetModelForCellAt(indexPath));

            cell.LayoutMargins = UIEdgeInsets.Zero;
            cell.LayoutIfNeeded();

            return cell;
        }

        private object GetModelForCellAt(NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    return _addressViewModel;
                default:
                    return _productViewModels[indexPath.Row];
            }
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 2;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch(section)
            {
                case 0:
                    return 1;
                default:
                    return _productViewModels.Count;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.CellAt(indexPath) is DeliveryAddressCell cell)
            {
                tableView.DeselectRow(indexPath, false);
            }
        }


    }
}

