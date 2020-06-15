using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    public partial class DetailPriceCheckerViewController : UIViewControllerBase
    {
        #region Atributtes
        private CheckerPriceParameters checkerPriceParameters;
        private CheckerPriceModel checkerPriceModel;
        private string data;
        private string dependecyId;
        private string dependecyName;
        #endregion

        #region Properties
        public string Data { get => data; set => data = value; }
        public string DependecyId { get => dependecyId; set => dependecyId = value; }
        public string DependecyName { get => dependecyName; set => dependecyName = value; }
        #endregion

        #region Constructor
        public DetailPriceCheckerViewController(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        #endregion

        #region lyfe cycle

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ProductDetailPriceCheck, nameof(DetailPriceCheckerViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {               
                this.LoadExternalViews();
                base.StartActivityIndicatorCustom();
                this.LoadHandlers();
                checkerPriceModel = new CheckerPriceModel(new CheckerPriceService(DeviceManager.Instance));
                checkerPriceParameters = new CheckerPriceParameters()
                {
                    Barcode = Data,
                    DependencyId = DependecyId,
                    Size = "XS"
                };

                this.GetCheckedProductAsync();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DetailPriceCheckerViewController, ConstantMethodName.ViewDidLoad);
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
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DetailPriceCheckerViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        #endregion

        #region Methods
        public async Task GetCheckedProductAsync()
        {
            try
            {
                CheckerPriceResponse response = await checkerPriceModel.CheckerPrice(checkerPriceParameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    producImageView.SetImage(new NSUrl(response.Image), UIImage.FromFile(ConstantImages.SinImagen));
                    priceProductLabel.Text = StringFormat.ToPrice(decimal.Parse(response.Price));
                    productNameLabel.Text = response.Name;
                    quantityProductLabel.Text = response.Presentation;
                    gramProductLabel.Text = response.Pum;
                    referenceLabel.Text = AppMessages.PriceStoreText + (dependecyName);

                    if (gramProductLabel == null)
                    {
                        gramProductLabel.Hidden = true;
                    }

                    StopActivityIndicatorCustom();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DetailPriceCheckerViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
                StopActivityIndicatorCustom();
            }
        }

        private void LoadHandlers()
        {
            this._spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceInStoreViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Events
        private void HandleSDExternalCompletionHandler(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl imageUrl)
        {
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.NavigationController.PopViewController(true);
        }
        #endregion
    }
}

