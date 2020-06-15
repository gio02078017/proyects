using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Generic;
using GrupoExito.Entities.Entiites.Generic.Contents;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters.Generic;
using GrupoExito.Entities.Responses.Generic;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers.Source;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ProductControllers;
using GrupoExito.iOS.ViewControllers.ProductControllers.Sources;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers;
using GrupoExito.iOS.ViewControllers.UserControllers;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers
{
    public partial class HomeViewController : BaseProductController
    {
        #region Properties
        private HomeModel _homeModel;
        private ContentsModel _contentsModel;

        private ProductCarModel dataBase;
        private IList<Product> DiscountProducts;
        private IList<Product> CustomerProducts;
        private IList<BannerPromotion> Banners;

        private bool IsSearcherProduct = false;

        private ProductsCollectionViewSource DiscountProductsSource = null;
        private ProductsCollectionViewSource CustomerProductsSource = null;
        private BannerSource BannersSource = null;

        private TutorialView tutorialView;
        private SplashPromotionsView splashPromotionsView;
        #endregion

        #region Constructors 
        public HomeViewController(IntPtr handle) : base(handle)
        {
            _homeModel = new HomeModel(new HomeService(DeviceManager.Instance));
            _contentsModel = new ContentsModel(new ContentsService(DeviceManager.Instance));
            dataBase = new ProductCarModel(ProductCarDataBase.Instance);
            _myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
        }
        #endregion

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {                
                this.LoadExternalViews();
                if (!ParametersManager.ChangeAddress)
                {
                    this.DownloadDataBanner();
                }
                this.LoadCorners();
                this.LoadHandlers();
                base.StopActivityIndicatorCustom();
                this.DownloadDataHome();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        private void GetTutorials()
        {
            IList<Tutorial> tutorialesList = new ContentsModel(new ContentsService(DeviceManager.Instance)).GetTutorials();
            foreach (Tutorial current in tutorialesList)
            {
                if (ConstNameViewTutorial.Home.Equals(current.Name))
                {
                    this.CreateTutorialView(this.View, tutorialView, current.ImagesTutorial, 0, false);
                    break;
                }
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                this.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
                changeAddressStoreButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                changeAddressStoreButton.Layer.BorderColor = UIColor.LightGray.CGColor;
                changeAddressStoreButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
                if (ParametersManager.ChangeAddress)
                {
                    base.GetProductsPrice();
                    this.DownloadDataBanner();
                    base.RegisterNotificationTags();
                    ParametersManager.ChangeAddress = false;
                }
                if (ParametersManager.ContainChanges)
                {
                    this.DownloadDataHome();
                    ParametersManager.ContainChanges = false;
                }
                if (CustomerProducts != null)
                {
                    this.UpdateCustomerProducts();
                    favoriteProductsCollectionView.ReloadData();
                }
                if (DiscountProducts != null)
                {
                    this.UpdateDiscountProducts();
                    discountProductsCollectionView.ReloadData();
                }
                this.SetUserName();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        private void UpdateDiscountProducts()
        {
            foreach (var item in DiscountProducts)
            {
                if (item.Id != null)
                {
                    Product exists = dataBase.GetProduct(item.Id);
                    if (exists == null)
                    {
                        item.Quantity = 0;
                    }
                }
            }
        }

        private void UpdateCustomerProducts()
        {
            foreach (var item in CustomerProducts)
            {
                if (item.Id != null)
                {
                    Product exists = dataBase.GetProduct(item.Id);
                    if (exists == null)
                    {
                        item.Quantity = 0;
                    }
                }
            }
        }


        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Home, nameof(HomeViewController));
                this.LoadAnimationLaterIncomeView();
                base.navigationView.ShowCarData();
                base.NavigationView.ShowAccountProfile();
                base.NavigationView.IsSummaryDisabled = false;
                base.NavigationView.IsAccountEnabled = true;
                base.NavigationView.LoadControllers(true, false, true, this);
                base.navigationView.EnableLobbyButton(true);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods

        private async Task GetSplashPromotions()
        {
            try
            {
                PromotionParameters promotionParameters = new PromotionParameters()
                {
                };
                PromotionResponse response = await _contentsModel.GetPromotions(promotionParameters);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {

                }
                else
                {
                    if (response.Promotions != null && response.Promotions.Any())
                    {
                        if (response.HaveNewPromotion)
                        {
                            if (!DeviceManager.Instance.ValidateAccessPreference(ConstPreferenceKeys.DoNotShowAgain))
                            {
                                nfloat heightTabBar = this.TabBarController.TabBar.Frame.Height;
                                nfloat heightNavigationBar = this.NavigationController.NavigationBar.Frame.Height;
                                this.TabBarController.TabBar.Hidden = true;
                                this.NavigationController.NavigationBarHidden = true;
                                this.View.LayoutIfNeeded();
                                splashPromotionsView = SplashPromotionsView.Create();
                                splashPromotionsView.LoadData(response.Promotions);
                                splashPromotionsView.Frame = new CGRect(0, 0, this.View.Frame.Width, UIScreen.MainScreen.Bounds.Height);
                                splashPromotionsView.RemoveEvent += (object sender, EventArgs e) =>
                                {
                                    this.NavigationController.NavigationBarHidden = false;
                                    this.TabBarController.TabBar.Hidden = false;
                                    splashPromotionsView.RemoveFromSuperview();
                                    this.View.LayoutIfNeeded();
                                    this.GetTutorials();
                                };
                                this.View.AddSubview(splashPromotionsView);
                            }
                        }
                        else
                        {
                            this.GetTutorials();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(HomeViewController), nameof(GetSplashPromotions));
            }
        }

        private void DownloadDataHome()
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                InvokeOnMainThread(async () =>
                {
                    await this.GetHomeProductsAsync();
                });
            }
            else
            {
                ParametersManager.ContainChanges = true;
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private void DownloadDataBanner()
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                InvokeOnMainThread(async () =>
                {
                    await this.GetSplashPromotions();
                    await this.GetBannersAsync();
                });
            }
            else
            {
                ParametersManager.ContainChanges = true;
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private void SetUserName()
        {
            try
            {
                UserContext userContext = ParametersManager.UserContext;
                if (userContext != null && userContext.Store != null)
                {
                    addressStoreTitleLabel.Text = AppMessages.PickUpOn;
                    addressStoreSaveLabel.Text = userContext.Store.City + ", " + userContext.Store.Name;
                }
                else if (userContext != null && userContext.Address != null)
                {
                    addressStoreSaveLabel.Text = userContext.Address.City + ", " + userContext.Address.AddressComplete;
                }
                if (userContext != null && !string.IsNullOrEmpty(userContext.FirstName))
                {
                    userNameLabel.Text = " " + Util.Capitalize(userContext.FirstName).TrimEnd();
                }
                userNameLabel.Text += "!";
                if (!AppServiceConfiguration.SiteId.Equals("exito"))
                {
                    if (userContext != null && userContext.UserType != null && !string.IsNullOrEmpty(userContext.UserType.Name))
                    {
                        infoTextUserNameLabel.Hidden = false;
                        infoTextUserNameLabel.Text = userContext.UserType.Name;
                    }
                    else
                    {
                        infoTextUserNameLabel.Hidden = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.SetUserName);
                ShowMessageException(exception);
            }
        }

        private void LoadTemplateDiscountProductsShimmer()
        {
            this.DiscountProducts = new List<Product>();
            for (int i = 0; i < 2; i++)
            {
                this.DiscountProducts.Add(new Product() { SiteId = AppMessages.Template });
            }
            DiscountProductsSource = new ProductsCollectionViewSource(this.DiscountProducts, ConstantIdentifier.Discount_products, this, IsSearcherProduct);
            discountProductsCollectionView.Source = DiscountProductsSource;
            discountProductsCollectionView.ReloadData();
        }

        private void LoadTemplateCustomerProductsShimmer()
        {
            Util.SetConstraint(customerProductsLabel, customerProductsLabel.Frame.Height, 80);
            customerProductsLabel.Hidden = false;
            this.CustomerProducts = new List<Product>();
            for (int i = 0; i < 4; i++)
            {
                this.CustomerProducts.Add(new Product() { SiteId = AppMessages.Template });
            }
            var newHeight = (CustomerProducts.Count * (ConstantViewSize.ProductHeightCell / 2)) + (CustomerProducts.Count % 2 != 0 ? (ConstantViewSize.ProductHeightCell / 2) : 0) + ConstantViewSize.ProductHeightSpacingFinalCell;
            favoriteProductsHeightConstraint.Constant = newHeight;
            CustomerProductsSource = new ProductsCollectionViewSource(this.CustomerProducts, ConstantIdentifier.Favorite_products, this, IsSearcherProduct);
            favoriteProductsCollectionView.Source = CustomerProductsSource;
            favoriteProductsCollectionView.ReloadData();
        }

        private void LoadTemplateBannersShimmer()
        {
            this.Banners = new List<BannerPromotion>();
            for (int i = 0; i < 4; i++)
            {
                this.Banners.Add(new BannerPromotion()
                {
                    Image = string.Empty
                });
            }
            BannersSource = new BannerSource(this.Banners);
            bannerCollectionView.Source = BannersSource;
            bannerCollectionView.ReloadData();
        }

        private async Task GetHomeProductsAsync()
        {
            try
            {
                LoadTemplateDiscountProductsShimmer();
                LoadTemplateCustomerProductsShimmer();
                UpdateCar(true);
                SearchProductsParameters parameters = new SearchProductsParameters()
                {
                    DependencyId = ValidateDependecy(),
                    Size = ParametersManager.Size,
                    From = ParametersManager.From,
                    OrderType = ParametersManager.OrderType,
                    OrderBy = ParametersManager.OrderBy
                };
                GetCustomerProductsAsync(parameters);
                DiscountProductsResponse discountProductsResponse = await _homeModel.ProductsAppDiscounts(parameters);
                if (discountProductsResponse.Result != null && discountProductsResponse.Result.HasErrors &&
                    discountProductsResponse.Result.Messages != null && discountProductsResponse.Result.Messages.Any())
                {
                    string message = MessagesHelper.GetMessage(discountProductsResponse.Result);
                    StartActivityErrorMessage(discountProductsResponse.Result.Messages[0].Code, message);
                }
                else
                {
                    if (discountProductsResponse.DiscountProducts != null && discountProductsResponse.DiscountProducts.Any())
                    {
                        this.DiscountProducts = new List<Product>();
                        this.DiscountProducts = discountProductsResponse.DiscountProducts;
                    }
                    else
                    {
                        productDiscountHeightConstraint.Constant = 0;
                        moveNextProductmageView.Hidden = true;
                        moveBeforeProductImageView.Hidden = true;
                    }
                    ParametersManager.ContainChanges = false;
                }

                this.DrawCustomerListProducts();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }

        private async Task GetBannersAsync()
        {
            LoadTemplateBannersShimmer();
            ContentHomeResponse contentHomeResponse = await _contentsModel.GetContentHome();
            if (contentHomeResponse.Result != null && contentHomeResponse.Result.HasErrors &&
                contentHomeResponse.Result.Messages != null && contentHomeResponse.Result.Messages.Any())
            {
                string message = MessagesHelper.GetMessage(contentHomeResponse.Result);
                StartActivityErrorMessage(contentHomeResponse.Result.Messages[0].Code, message);
            }
            else
            {
                this.Banners = contentHomeResponse.Images;
                this.DrawBannerViews();
            }
        }

        private async Task GetCustomerProductsAsync(SearchProductsParameters parameters)
        {
            CustomerProductsResponse customerProductsResponse = await _homeModel.CustomerProducts(parameters);
            if (customerProductsResponse.Result != null && customerProductsResponse.Result.HasErrors && customerProductsResponse.Result.Messages != null && customerProductsResponse.Result.Messages.Any())
            {
                string message = MessagesHelper.GetMessage(customerProductsResponse.Result);
                StartActivityErrorMessage(customerProductsResponse.Result.Messages[0].Code, message);
            }
            else
            {
                if (customerProductsResponse.CustomerProducts != null && customerProductsResponse.CustomerProducts.Any())
                {
                    this.CustomerProducts = customerProductsResponse.CustomerProducts;
                }
                else
                {
                    this.CustomerProducts = new List<Product>();
                }
            }
            this.DrawDiscountListProducts();
        }

        private void DrawBannerViews()
        {
            if (Banners != null && Banners.Any())
            {
                if (BannersSource == null)
                {
                    BannersSource = new BannerSource(this.Banners);
                    bannerCollectionView.Source = BannersSource;
                }
                else
                {
                    BannersSource.BannerAction += BannersSource_BannerAction;
                    BannersSource.Banners = this.Banners;
                }
                bannerCollectionView.ReloadData();
            }
            else
            {
                Util.SetConstraint(bannerView, bannerView.Frame.Height, 0);
            }
        }

        private void BannersSource_BannerAction(BannerPromotion bannerAction)
        {
            try
            {
                var vcInstance = this.Storyboard.InstantiateViewController(bannerAction.ActionIos);
                if (vcInstance != null)
                {
                    BannerParameter parameters = null;
                    if (!string.IsNullOrEmpty(bannerAction.ParameterAction))
                    {
                        parameters = JsonService.Deserialize<BannerParameter>(bannerAction.ParameterAction);
                    }
                    switch (bannerAction.ActionIos)
                    {
                        case (nameof(StickersViewController)):
                            RegisterBannerEvent();
                            StickersViewController stickersViewController = (StickersViewController)vcInstance;
                            stickersViewController.HidesBottomBarWhenPushed = true;
                            stickersViewController.OpenFromInitialOption = true;
                            this.NavigationController.PushViewController(vcInstance, true);
                            break;
                        case (nameof(ProductByCategoryViewController)):
                            if (parameters != null)
                            {
                                ProductByCategoryViewController productByCategoryViewController = (ProductByCategoryViewController)vcInstance;
                                productByCategoryViewController.HidesBottomBarWhenPushed = true;
                                Category category = new Category()
                                {
                                    Id = parameters.Id,
                                    ImageCategory = parameters.URLImage,
                                    Name = parameters.Name,
                                    IconCategory = parameters.URLImage,
                                    IconCategoryGray = parameters.URLImage
                                };
                                productByCategoryViewController._category = category;
                                this.NavigationController.PushViewController(vcInstance, true);
                            }
                            break;
                        case (nameof(ProductDetailViewController)):
                            if (parameters != null)
                            {
                                ProductDetailViewController productDetailViewController = (ProductDetailViewController)vcInstance;
                                productDetailViewController.HidesBottomBarWhenPushed = true;
                                Product product = new Product()
                                {
                                    Id = parameters.Id,
                                    Name = parameters.Name,
                                    ProductType = parameters.ProductType,
                                    SkuId = parameters.SkuId
                                };
                                productDetailViewController.ProductCurrent = product;
                                this.NavigationController.PushViewController(vcInstance, true);
                            }
                            break;
                        default:
                            UIViewControllerBase genericViewController = (UIViewControllerBase)vcInstance;
                            genericViewController.HidesBottomBarWhenPushed = true;
                            this.NavigationController.PushViewController(genericViewController, true);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, ConstantControllersName.HomeViewController, ConstantMethodName.BannersAction);
            }
        }

        private void ShowMessageProductsNotFound()
        {
            var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.NotFoundProductAvailable, UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
            alertController.AddAction(UIAlertAction.Create(AppMessages.RetryText, UIAlertActionStyle.Default, async action => { await this.GetHomeProductsAsync(); }));
            PresentViewController(alertController, true, null);
        }

        private void DrawDiscountListProducts()
        {
            if (CustomerProducts != null && CustomerProducts.Any())
            {
                Util.SetConstraint(customerProductsLabel, customerProductsLabel.Frame.Height, 80);
                customerProductsLabel.Hidden = false;
                customerProductsImageView.Hidden = false;
                nfloat newHeight = Util.GetTableHeightProducts(CustomerProducts);
                favoriteProductsHeightConstraint.Constant = newHeight + ConstantViewSize.ProductHeightSpacingFinalCell;

                if (CustomerProductsSource == null)
                {
                    CustomerProductsSource = new ProductsCollectionViewSource(CustomerProducts, ConstantIdentifier.Favorite_products, this, IsSearcherProduct);
                    favoriteProductsCollectionView.Source = CustomerProductsSource;
                }
                else
                {
                    CustomerProductsSource.Products = CustomerProducts;
                    RegisterCustomerProductImpressionEvent();
                }

                CustomerProductsSource.ActionAddingRemoveProducts += CustomerProductsSourceActionAddingRemoveProducts;
                CustomerProductsSource.ActionSummaryProducts += SummaryActionProducts;
                favoriteProductsCollectionView.ReloadData();
            }
            else
            {
                Util.SetConstraint(customerProductsLabel, customerProductsLabel.Frame.Height, 0);
                customerProductsLabel.Hidden = true;
                customerProductsImageView.Hidden = true;
                favoriteProductsHeightConstraint.Constant = 0;
            }
        }

        private void DrawCustomerListProducts()
        {
            try
            {
                if (DiscountProducts != null && DiscountProducts.Any())
                {
                    RegisterDiscountProductImpressionEvent();
                    productNotFoundImageView.Hidden = true;
                    DiscountProductsSource = new ProductsCollectionViewSource(DiscountProducts, ConstantIdentifier.Discount_products, this, IsSearcherProduct); ;
                    DiscountProductsSource.ActionAddingRemoveProducts += DiscountProductsSourceActionAddingRemoveProducts;
                    DiscountProductsSource.ActionSummaryProducts += SummaryActionProducts;
                    discountProductsCollectionView.Source = DiscountProductsSource;
                    discountProductsCollectionView.ReloadData();

                    if (DiscountProducts.Count <= 2)
                    {
                        moveNextProductmageView.Hidden = true;
                        moveBeforeProductImageView.Hidden = true;
                    }
                }
                else
                {
                    productNotFoundImageView.Hidden = false;
                }

                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.DrawCustomerList);
                ShowMessageException(exception);
            }
        }

        private void LoadFonts()
        {
            userNameLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.UserNameHome);
            infoTextUserNameLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.HomeDescriptionUser);
        }

        private void LoadHandlers()
        {
            MyCouponsButton.TouchUpInside += MyCouponsButtonTouchUpInside;
            moveNextProductButton.TouchUpInside += MoveNextProductsDiscount;
            moveBeforeProductButton.TouchUpInside += MoveBeforeProductsDiscount;
            changeAddressStoreButton.TouchUpInside += ChangeAddressStoreButtonTouchUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }


        private void LoadCorners()
        {
            try
            {
                MyCouponsButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
                MyCouponsButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                MyCouponsButton.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
                moveBeforeProductImageView.Transform = CGAffineTransform.MakeRotation((float)Math.PI);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                moveBeforeProductImageView.Hidden = true;
                moveBeforeProductButton.Hidden = true;
                discountProductsCollectionView.RegisterNibForCell(UINib.FromName(ConstantReusableViewName.ProductViewCell, NSBundle.MainBundle), ConstantIdentifier.ProductsIdentifier);
                favoriteProductsCollectionView.RegisterNibForCell(UINib.FromName(ConstantReusableViewName.ProductViewCell, NSBundle.MainBundle), ConstantIdentifier.ProductsIdentifier);
                bannerCollectionView.RegisterNibForCell(UINib.FromName(ConstantReusableViewName.BannerViewCell, NSBundle.MainBundle), ConstantIdentifier.BannersIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadSearchProductsView(searchProductView);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadAnimationLaterIncomeView()
        {
            changeAddressStoreButton.LayoutIfNeeded();
            changeAddressStoreStackView.LayoutIfNeeded();
            UIView.Animate(0.1, 0.0,
            UIViewAnimationOptions.TransitionFlipFromRight,
            () => { LeftViewAnimating(); },
            () =>
            {
                UIView.Animate(2, 0.0,
                UIViewAnimationOptions.TransitionFlipFromLeft,
                () => { RightViewAnimating(); },
                () =>
                {
                    UIView.Animate(0.1, 0.0,
                        UIViewAnimationOptions.CurveLinear,
                        () => { UpViewAnimating(); },
                        () =>
                        {
                            UIView.Animate(0.1, 0.0,
                            UIViewAnimationOptions.CurveLinear,
                            () => { DownViewAnimating(); },
                            () =>
                            {
                                UIView.Animate(0.1, 0.0,
                                    UIViewAnimationOptions.CurveLinear,
                                    () => { UpViewAnimating(); },
                                    () =>
                                    {
                                        UIView.Animate(0.1, 0.0,
                                        UIViewAnimationOptions.CurveLinear,
                                        () => { DownViewAnimating(); },
                                        () =>
                                        {
                                            UIView.Animate(0.1, 0.0,
                                                UIViewAnimationOptions.CurveLinear,
                                                () => { UpViewAnimating(); },
                                                () =>
                                                {
                                                    UIView.Animate(0.1, 0.0,
                                                        UIViewAnimationOptions.CurveLinear,
                                                        () => { DownViewAnimating(); },
                                                        () => { });
                                                });
                                        });
                                    });
                            });
                        });
                });
            });
        }
        #endregion

        private void LeftViewAnimating()
        {
            CGRect changeAddressStoreStackViewFrame = changeAddressStoreStackView.Frame;
            CGRect changeAddressStoreButtonFrame = changeAddressStoreButton.Frame;
            nfloat displaceView = -(changeAddressStoreStackViewFrame.Width + changeAddressStoreStackViewFrame.X);
            nfloat displaceButton = -(changeAddressStoreButtonFrame.Width + changeAddressStoreButtonFrame.X);
        }

        private void RightViewAnimating()
        {
            CGRect changeAddressStoreStackViewFrame = changeAddressStoreStackView.Frame;
            CGRect changeAddressStoreButtonFrame = changeAddressStoreButton.Frame;
            nfloat displaceView = 25;
            nfloat displaceButton = 15;
        }

        private void UpViewAnimating()
        {
            var height = 3;
            CGRect changeAddressStoreStackViewFrame = changeAddressStoreStackView.Frame;
            CGRect changeAddressStoreButtonFrame = changeAddressStoreButton.Frame;
            changeAddressStoreStackView.Frame = new CGRect(changeAddressStoreStackViewFrame.X - height, changeAddressStoreStackViewFrame.Y, changeAddressStoreStackViewFrame.Width, changeAddressStoreStackViewFrame.Height);
            changeAddressStoreButton.Frame = new CGRect(changeAddressStoreButtonFrame.X - height, changeAddressStoreButtonFrame.Y, changeAddressStoreButtonFrame.Width, changeAddressStoreButtonFrame.Height);
        }

        private void DownViewAnimating()
        {
            CGRect changeAddressStoreStackViewFrame = changeAddressStoreStackView.Frame;
            CGRect changeAddressStoreButtonFrame = changeAddressStoreButton.Frame;
            var height = 3;
            changeAddressStoreStackView.Frame = new CGRect(changeAddressStoreStackViewFrame.X + height, changeAddressStoreStackViewFrame.Y, changeAddressStoreStackViewFrame.Width, changeAddressStoreStackViewFrame.Height);
            changeAddressStoreButton.Frame = new CGRect(changeAddressStoreButtonFrame.X + height, changeAddressStoreButtonFrame.Y, changeAddressStoreButtonFrame.Width, changeAddressStoreButtonFrame.Height);
        }

        #region events
        private void MyCouponsButtonTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                MyDiscountViewController myDiscountViewController = (MyDiscountViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.MyDiscountViewController);
                myDiscountViewController.HidesBottomBarWhenPushed = true;
                this.NavigationController.PushViewController(myDiscountViewController, true);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantEventName.SeeMyCupons_btnUpInside);
                ShowMessageException(exception);
            }
        }

        private void RemoveChild(UIViewController vc)
        {
            vc.WillMoveToParentViewController(null);
            vc.View.RemoveFromSuperview();
            vc.RemoveFromParentViewController();
        }

        private void ShowError(string message)
        {
            InvokeOnMainThread(() =>
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure("", message,
                    (errorSender, ea) => errorView.RemoveFromSuperview());
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            });
        }

        private void MoveNextProductsDiscount(object sender, EventArgs e)
        {
            try
            {
                if (DiscountProducts != null && DiscountProducts.Any())
                {
                    NSIndexPath nextCell = new NSIndexPath();
                    var indexPath = this.discountProductsCollectionView.IndexPathsForVisibleItems;
                    var orderArray = indexPath.OrderBy((NSIndexPath arg) => arg.Item).ToArray();
                    var lastVisibleCell = orderArray[orderArray.Length - 1];

                    if (lastVisibleCell.Row != DiscountProducts.Count - 1)
                    {
                        moveNextProductmageView.Hidden = false;
                        nextCell = NSIndexPath.FromRowSection(lastVisibleCell.Row + 1, 0);
                    }
                    else
                    {
                        nextCell = NSIndexPath.FromRowSection(lastVisibleCell.Row, 0);
                    }

                    discountProductsCollectionView.ScrollToItem(nextCell, UICollectionViewScrollPosition.Right, true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.MoveProductsDiscount);
            }
        }

        private void MoveBeforeProductsDiscount(object sender, EventArgs e)
        {
            try
            {
                if (DiscountProducts != null && DiscountProducts.Any())
                {
                    NSIndexPath nextCell = new NSIndexPath();
                    var indexPath = this.discountProductsCollectionView.IndexPathsForVisibleItems;
                    var orderArray = indexPath.OrderBy((NSIndexPath arg) => arg.Item).ToArray();
                    var lastVisibleCell = orderArray[orderArray.Length - 1];

                    if (lastVisibleCell.Row != 0)
                    {
                        moveNextProductmageView.Hidden = false;
                        moveBeforeProductImageView.Hidden = false;
                        nextCell = NSIndexPath.FromRowSection(lastVisibleCell.Row - 1, 0);
                    }
                    else
                    {
                        moveNextProductmageView.Hidden = false;
                        moveBeforeProductImageView.Hidden = true;
                        nextCell = NSIndexPath.FromRowSection(lastVisibleCell.Row, 0);
                    }

                    discountProductsCollectionView.ScrollToItem(nextCell, UICollectionViewScrollPosition.Right, true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.MoveProductsDiscount);
            }
        }

        private void ChangeAddressStoreButtonTouchUpInside(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                PopUpInformationView addressChangeAdviceView = PopUpInformationView.Create("", AppMessages.MessageChangeAdress);
                this.NavigationController.SetNavigationBarHidden(true, false);
                addressChangeAdviceView.Frame = View.Bounds;
                addressChangeAdviceView.LayoutIfNeeded();
                addressChangeAdviceView.SetTitleAcceptButton(AppMessages.MessagebtnChangeAdress, UIControlState.Normal);
                View.AddSubview(addressChangeAdviceView);

                addressChangeAdviceView.AcceptButtonHandler += () =>
                {
                    this.NavigationController.SetNavigationBarHidden(false, true);
                    addressChangeAdviceView.RemoveFromSuperview();
                    if (this.Storyboard.InstantiateViewController(ConstantControllersName.LaterIncomeViewController) is LaterIncomeViewController laterIncomeViewController)
                    {
                        RegisterEventAddress();
                        laterIncomeViewController.HidesBottomBarWhenPushed = true;
                        this.NavigationController.PushViewController(laterIncomeViewController, true);
                    }
                };
                addressChangeAdviceView.CloseButtonHandler += () =>
                {
                    this.NavigationController.SetNavigationBarHidden(false, true);
                    addressChangeAdviceView.RemoveFromSuperview();
                };
            });
        }

        private void RegisterEventAddress()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEvent(AnalyticsEvent.HomeAddress);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEvent(AnalyticsEvent.HomeAddress);
        }

        private void RegisterBannerEvent()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventWithUserIdAndLaunchedFrom(AnalyticsEvent.Stickers, AnalyticsReferenceAction.Banner);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEventWithUserIdAndLaunchedFrom(AnalyticsEvent.Stickers, AnalyticsReferenceAction.Banner);
        }

        private void DiscountProductsSourceActionAddingRemoveProducts(object sender, EventArgs e)
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                discountProductsCollectionView.ReloadData();
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private void CustomerProductsSourceActionAddingRemoveProducts(object sender, EventArgs e)
        {
            if (!DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private void SummaryActionProducts(object sender, EventArgs e)
        {
            UpdateCar(true);
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                StopActivityIndicatorCustom();
                DownloadDataHome();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantEventName.RetryTouchUpInside);
                ShowMessageException(exception);
            }
        }

        private void RegisterCustomerProductImpressionEvent()
        {
            FirebaseEventRegistrationService.Instance.ProductImpression(CustomerProducts, AnalyticsParameter.CustomerProducts);
        }

        private void RegisterDiscountProductImpressionEvent()
        {
            FirebaseEventRegistrationService.Instance.ProductImpression(DiscountProducts, AnalyticsParameter.DiscountProducts);
        }

        #endregion
    }
}