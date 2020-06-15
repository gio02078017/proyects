using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class MyOrdersViewController : BaseOrderController
    {
        #region Attributes
        private OrdersResponse response;
        private CurrentOrdersDataSource currentOrdersDataSource;
        private HistoricalOrdersDataSource historicalOrdersDataSource;
        #endregion

        #region Constructors
        public MyOrdersViewController(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        #endregion

        #region Life Cycle

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.MyOrders, nameof(MyOrdersViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {                
                this.LoadFonts();
                this.SetColors();
                this.LoadExternalViews();
                this.DisplayOrders();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyOrdersViewController, ConstantMethodName.ViewDidLoad);
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
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OrderDetailViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods
        private void LoadExternalViews()
        {
            try
            {
                currentOrdersTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.CurrentOrderTableViewCell, NSBundle.MainBundle), ConstantIdentifier.CurrentOrderCellIdentifier);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                currentOrdersQuantityLabel.Layer.CornerRadius = currentOrdersQuantityLabel.Frame.Size.Width / 2;
                currentOrdersQuantityLabel.ClipsToBounds = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyOrdersViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadFonts()
        {
            try
            {
                myOrdersTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersTitleSize);
                myOrdersSubtitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle2Size);
                currentOrdersTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle1Size);
                currentOrdersQuantityLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle2Size);
                historicalOrdersTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle1Size);
                historicalOrdersValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle1Size);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyOrdersViewController, ConstantMethodName.LoadFonts);
                ShowMessageException(exception);
            }
        }

        private void SetColors()
        {
            try
            {
                currentOrdersQuantityLabel.BackgroundColor = ConstantColor.UiBackgroundOrderQuantity;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyOrdersViewController, ConstantMethodName.LoadCorners);
            }
        }

        public void RecalculateCurrentTableViewHeight()
        {
            currentOrdersTableViewHeightConstraint.Constant = (response.Orders[0].HomeDelivery.Count * ConstantViewSize.HomeDeliveryCurrentOrderCellHeight) +
                (response.Orders[0].PickUp.Count * ConstantViewSize.PickUpCurrentOrderCellHeight) + ConstantViewSize.CurrentOrderTableViewHeaderHeight * 2;
        }

        public void RecalculateHistoricalTableViewHeight()
        {
            historicalOrdersTableViewHeightConstraint.Constant = (response.HistoricalOrders.Count * ConstantViewSize.HistoricalOrderCellHeight) + ConstantViewSize.CurrentOrderTableViewHeaderHeight;
        }

        private async Task DisplayOrders(bool showCustomIndicator = true)
        {
            try
            {
                OrderParameters orderParameters = new OrderParameters()
                {
                    From = ParametersManager.From,
                    Size = ParametersManager.Size
                };

                response = await GetOrders(orderParameters);
                if (response != null && response.Orders != null && response.Orders.Any())
                {
                    InvokeOnMainThread(() =>
                    {
                        currentOrdersQuantityLabel.Text = (response.Orders[0].HomeDelivery.Count() + response.Orders[0].PickUp.Count()).ToString();
                        currentOrdersDataSource = new CurrentOrdersDataSource(response.Orders);
                        currentOrdersDataSource.ShowOrderHandler += ShowOrderHandler;
                        currentOrdersTableView.Source = currentOrdersDataSource;
                        currentOrdersTableView.RowHeight = UITableView.AutomaticDimension;
                        currentOrdersTableView.EstimatedRowHeight = (ConstantViewSize.HomeDeliveryCurrentOrderCellHeight + ConstantViewSize.PickUpCurrentOrderCellHeight) / 2;
                        currentOrdersTableView.ReloadData();
                        RecalculateCurrentTableViewHeight();
                    });
                }

                if (response != null && response.HistoricalOrders != null && response.HistoricalOrders.Any())
                {
                    InvokeOnMainThread(() =>
                    {
                        historicalOrdersValueLabel.Text = response.HistoricalOrders.Count().ToString();
                        historicalOrdersDataSource = new HistoricalOrdersDataSource(response.HistoricalOrders);
                        historicalOrdersDataSource.ShowOrderHandler += ShowHistoricalOrderHandler;
                        historicalOrdersTableView.Source = historicalOrdersDataSource;
                        historicalOrdersTableView.RowHeight = UITableView.AutomaticDimension;
                        historicalOrdersTableView.EstimatedRowHeight = ConstantViewSize.HistoricalOrderCellHeight;
                        historicalOrdersTableView.ReloadData();
                        RecalculateHistoricalTableViewHeight();
                    });
                }
                else
                {
                    historicalTitleView.Hidden = true;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyOrdersViewController, ConstantMethodName.DisplayOrders);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Events
        private void ShowOrderHandler(Object sender, Order order)
        {
            OrderStatusViewController orderStatusViewController = (OrderStatusViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.OrderStatusViewController);
            orderStatusViewController.Order = order;
            this.NavigationController.PushViewController(orderStatusViewController, true);
        }

        private void ShowHistoricalOrderHandler(object sender, Order order)
        {
            OrderDetailViewController orderDetailViewController = (OrderDetailViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.OrderDetailViewController);
            orderDetailViewController.OrderId = order.Id;
            this.NavigationController.PushViewController(orderDetailViewController, true);
        }
        #endregion
    }
}

