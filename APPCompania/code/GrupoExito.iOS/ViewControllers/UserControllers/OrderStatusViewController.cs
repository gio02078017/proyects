using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class OrderStatusViewController : BaseOrderController
    {
        #region Attributes
        private Order order;
        private TrackingOrderResponse TrackingOrderResponse;
        private OrderDetailCollectionViewCell _orderDetailCollectionViewCell;
        private const int CancelViewHeightConstraintEnabled = 180;
        private const int DeliveryViewHeightConstraintEnabled = 130;
        #endregion

        #region Properties 
        public Order Order { get => order; set => order = value; }
        #endregion

        #region Constructors
        public OrderStatusViewController(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        #endregion

        #region Life Cycle

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.OrderStatus, nameof(OrderStatusViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {                
                this.LoadExternalViews();
                base.StartActivityIndicatorCustom();
                this.LoadCorners();
                this.LoadData();
                this.LoadHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OrderStatusViewController, ConstantMethodName.ViewDidLoad);
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
                Util.LogException(exception, ConstantControllersName.OrderStatusViewController, ConstantMethodName.ViewWillAppear);
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
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                LoadProductStatus();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OrderStatusViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            try
            {
                this.DrawOrderInfo();
                this.DrawDeliveryDate();

                if (Order.Status.Equals(AppMessages.CancelledText))
                {
                    this.DrawCanceledInfo();
                }
                else
                {
                    this.DrawTrackingData();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OrderStatusViewController, ConstantMethodName.LoadData);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            this.supportButton.TouchUpInside += SupportButtonTouchUpInside;
        }

        private void DrawOrderInfo()
        {
            _orderDetailCollectionViewCell.UpdateCell(this.Order);
        }

        private void DrawDeliveryDate()
        {
            deliveryDateLabel.Text = Order.Date;
            addressLabel.Text = Order.Address;
        }

        private void DrawCanceledInfo()
        {
            cancelView.Hidden = false;
            deliveryContainerView.Hidden = true;
            deliveryViewHeightConstraint.Constant = 0;
            cancelViewHeightConstraint.Constant = CancelViewHeightConstraintEnabled;
            cancelView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            cancelView.Layer.BorderColor = ConstantColor.UiMessageError.CGColor;
            cancelView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            stepOneView.OrderCancel();
            stepTwoView.OrderCancel();
            stepThreeView.OrderCancel();
            stepFourView.OrderCancel();
            productsViewButton.TouchUpInside += ProductsViewButtonTouchUpInside;
            seeReturnProcessButton.TouchUpInside += SeeReturnProcessButtonTouchUpInside;
            StopActivityIndicatorCustom();
        }

        private async Task DrawTrackingData()
        {
            cancelView.Hidden = true;
            deliveryContainerView.Hidden = false;
            deliveryViewHeightConstraint.Constant = DeliveryViewHeightConstraintEnabled;
            cancelViewHeightConstraint.Constant = 0;

            if (Order != null && !string.IsNullOrEmpty(Order.Id))
            {
                TrackingOrderResponse = await _orderModel.GetTrackingOrder(new Order { Id = Order.Id });

                if (TrackingOrderResponse != null)
                {
                    if (TrackingOrderResponse.Result != null && TrackingOrderResponse.Result.HasErrors && TrackingOrderResponse.Result.Messages != null)
                    {
                        if (TrackingOrderResponse.Result.Messages.Any())
                        {
                            string message = MessagesHelper.GetMessage(TrackingOrderResponse.Result);
                            StartActivityErrorMessage(TrackingOrderResponse.Result.Messages[0].Code, message);
                        }
                    }
                    else if (TrackingOrderResponse.TrackingOrders != null)
                    {
                        if (TrackingOrderResponse.TrackingOrders != null &&
                            TrackingOrderResponse.TrackingOrders.Where(x => x.StatusName.Equals(ConstOrderStatus.Cancel)).Any())
                        {
                            this.DrawCanceledInfo();
                        }
                        else
                        {
                            DrawStepOneData();
                            DrawStepTwoData();
                            DrawStepThreeData();
                            StopActivityIndicatorCustom();
                        }
                    }
                    else
                    {
                        StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.OrderWithoutStatusText);
                    }
                }
                else
                {
                    StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.OrderWithoutStatusText);
                }
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.OrderNotFoundText);
            }
        }

        private void DrawStepOneData()
        {
            if (TrackingOrderResponse.TrackingOrders.Any())
            {
                stepOneView.SetStatus(TrackingOrderResponse.TrackingOrders[0].Status);
                stepOneView.SetDescription(TrackingOrderResponse.TrackingOrders[0].StatusName);
            }
            else
            {
                stepOneView.SetStatus(false);
                stepOneView.SetDescription(string.Empty);
            }
        }

        private void DrawStepTwoData()
        {
            if (TrackingOrderResponse.TrackingOrders.Count > 1)
            {
                stepTwoView.SetStatus(TrackingOrderResponse.TrackingOrders[1].Status);
                stepTwoView.SetDescription(TrackingOrderResponse.TrackingOrders[1].StatusName);
            }
            else
            {
                stepTwoView.SetStatus(false);
                stepTwoView.SetDescription(string.Empty);
            }
        }

        private void DrawStepThreeData()
        {
            if (TrackingOrderResponse.TrackingOrders.Count > 2)
            {
                stepFourView.SetStatus(TrackingOrderResponse.TrackingOrders[2].Status);
                stepFourView.SetTitle(AppMessages.StepThreeText);
                stepFourView.SetDescription(TrackingOrderResponse.TrackingOrders[2].StatusName + " " + TrackingOrderResponse.TrackingOrders[2].Date);
            }
            else
            {
                stepFourView.SetStatus(false);
                stepFourView.SetDescription(string.Empty);
            }
        }

        private void LoadProductStatus()
        {
            orderDetailView.LayoutIfNeeded();
            _orderDetailCollectionViewCell = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.OrderDetailCollectionViewCell, Self, null).GetItem<OrderDetailCollectionViewCell>(0);
            _orderDetailCollectionViewCell.DisplayStateHandler += DisplayTotalPriceDetailHandler;
            _orderDetailCollectionViewCell.DisplayProductsHandler += DisplayProductsHandler;
            CGRect orderDetailFrame = orderDetailView.Frame;
            orderDetailFrame = new CGRect(0, 0, orderDetailView.Frame.Size.Width, _orderDetailCollectionViewCell.Frame.Size.Height);
            _orderDetailCollectionViewCell.Frame = orderDetailFrame;
            _orderDetailCollectionViewCell.Layer.CornerRadius = ConstantStyle.CornerRadius;
            orderDetailView.Frame = new CGRect(orderDetailView.Frame.X, orderDetailView.Frame.Y, orderDetailView.Frame.Width, _orderDetailCollectionViewCell.Frame.Height);
            orderDetailView.AddSubview(_orderDetailCollectionViewCell);
            _orderDetailCollectionViewCell.Configure();
        }

        private void LoadCorners()
        {
            this.orderDetailView.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }
        #endregion

        #region Events
        private void DisplayTotalPriceDetailHandler(Object sender, bool display)
        {
            if (display)
            {
                CGRect orderDetailFrame = orderDetailView.Frame;
                orderDetailFrame = new CGRect(0, 0, orderDetailView.Frame.Size.Width, ConstantViewSize.OrderDetailStatusCellDisplayed_height);
                orderDetailView.Frame = orderDetailFrame;
                _orderDetailCollectionViewCell.Frame = orderDetailFrame;
                totalPriceViewHeightConstraint.Constant = ConstantViewSize.OrderDetailStatusCellDisplayed_height;
            }
            else
            {
                CGRect orderDetailFrame = orderDetailView.Frame;
                orderDetailFrame = new CGRect(0, 0, orderDetailView.Frame.Size.Width, ConstantViewSize.OrderDetailStatusCellContracted_height);
                orderDetailView.Frame = orderDetailFrame;
                _orderDetailCollectionViewCell.Frame = orderDetailFrame;
                totalPriceViewHeightConstraint.Constant = ConstantViewSize.OrderDetailStatusCellContracted_height;
            }
        }

        private void DisplayProductsHandler(Object sender, Object args)
        {
            if (Order.TotalProducts > 0)
            {
                OrderDetailViewController orderDetailViewController = (OrderDetailViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.OrderDetailViewController);
                orderDetailViewController.OrderId = Order.Id;
                this.NavigationController.PushViewController(orderDetailViewController, true);
            }
        }

        private void SeeReturnProcessButtonTouchUpInside(object sender, EventArgs e)
        {
        }

        private void ProductsViewButtonTouchUpInside(object sender, EventArgs e)
        {
        }

        private void SupportButtonTouchUpInside(object sender, EventArgs e)
        {
            ContactUsViewController contactUsViewController = (ContactUsViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.ContactUsViewController);
            this.NavigationController.PushViewController(contactUsViewController, true);
        }
        #endregion
    }
}

