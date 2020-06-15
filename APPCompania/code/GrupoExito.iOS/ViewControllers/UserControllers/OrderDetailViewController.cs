using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.Entities;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class OrderDetailViewController : BaseOrderController
    {
        #region Attributes
        private string orderId;
        private OrderDetailDataSource dataSource;
        private OrderDetailResponse OrderDetailResponse;
        private bool _isSelectedAll = false;
        #endregion

        #region Properties
        public string OrderId { get => orderId; set => orderId = value; }
        #endregion

        #region Constructors
        public OrderDetailViewController(IntPtr handle) : base(handle)
        {
            _orderModel = new OrderModel(new OrderService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                SetFonts();
                LoadExternalViews();
                GetOrderDetail();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OrderDetailViewController, ConstantMethodName.ViewDidLoad);
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
                NavigationView.HiddenAccountProfile();
                NavigationView.HiddenCarData();
                SetHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OrderDetailViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            try
            {
                RemoveHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OrderDetailViewController, ConstantMethodName.ViewWillDisappear);
                ShowMessageException(exception);
            }
            base.ViewWillDisappear(animated);
        }
        #endregion

        #region Methods
        private void SetFonts()
        {
            addToCarLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle3Size);
            addToListLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle3Size);
        }

        private void LoadExternalViews()
        {
            orderTableView.RegisterNibForCellReuse(HeaderOrderDetailViewCell.Nib, HeaderOrderDetailViewCell.Key);
            orderTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.OrderDetailTableViewCell, NSBundle.MainBundle), ConstantIdentifier.OrderDetailCellIdentifier);

            LoadNavigationView(this.NavigationController.NavigationBar);
            LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
            CustomSpinnerViewFromBase = customSpinnerView;
            SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;

            listImageView.Image = UIImage.FromFile(ConstantImages.Lista);
            carImageView.Image = UIImage.FromFile(ConstantImages.Carrito);
            addToCarView.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void GetOrderDetail()
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                InvokeOnMainThread(async () =>
                {
                    await this.GetOrderDetailAsync();
                });
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private async Task GetOrderDetailAsync()
        {
            if (!string.IsNullOrEmpty(OrderId))
            {
                OrderDetailResponse = await GetHistoricalOrderDetail(OrderId);

                if (OrderDetailResponse != null)
                {
                    if (OrderDetailResponse.Products != null)
                    {
                        dataSource = new OrderDetailDataSource(OrderDetailResponse);
                        orderTableView.Source = dataSource;
                        dataSource.SelectAllAction = SelectAllButton_TouchUpInside;
                        orderTableView.EstimatedRowHeight = 190;
                        orderTableView.RowHeight = UITableView.AutomaticDimension;
                        orderTableView.ReloadData();
                    }
                }
            }
        }

        private void SetHandlers()
        {
            addToListButton.TouchUpInside += AddToListButtonTouchUpInside;
            addToCarButton.TouchUpInside += AddToCarButtonTouchUpInside;
        }

        private void RemoveHandlers()
        {
            addToListButton.TouchUpInside -= AddToListButtonTouchUpInside;
            addToCarButton.TouchUpInside -= AddToCarButtonTouchUpInside;
        }

        private void RegisterAddProductEvent(Product product)
        {
            Utilities.Analytic.FirebaseEventRegistrationService.Instance.AddProductToCart(product, product.CategoryName);
            Utilities.Analytic.FacebookEventRegistrationService.Instance.AddProductToCart(product);
        }
        #endregion

        #region Events
        private void SelectAllButton_TouchUpInside(object sender, EventArgs e)
        {
            _isSelectedAll = !_isSelectedAll;

            dataSource.ChangeTableViewSelection(_isSelectedAll, orderTableView);
        }

        private void AddToCarButtonTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                IList<Product> productList = OrderDetailResponse.Products.Where(x => x.Selected).ToList();
                if (productList != null && productList.Any())
                {
                    foreach (Product product in productList)
                    {
                        DataBase.UpSertProduct(product);
                        RegisterAddProductEvent(product);
                    }
                }
                UpdateCar(true);
                Util.LoadCenterToast(AppMessages.ProductAddedToCar).Show();
                ParametersManager.Products = null;
                SummaryContainerController summaryContainer = (SummaryContainerController)this.Storyboard.InstantiateViewController(nameof(SummaryContainerController));
                summaryContainer.HidesBottomBarWhenPushed = true;
                this.NavigationController.PushViewController(summaryContainer, true);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OrderDetailViewController, ConstantMethodName.AddToCar);
                ShowMessageException(exception);
            }
        }

        private void AddToListButtonTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                //Event to Add product to custom list
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OrderDetailViewController, ConstantMethodName.AddToList);
                ShowMessageException(exception);
            }
        }
        #endregion
    }
}