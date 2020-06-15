using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites.Products;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ListControllers;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers
{
    public partial class ProductDetailViewController : BaseProductController
    {
        #region Atributes
        private ProductModel _productModel;

        private bool isSearcherProduct = false;
        private Product product;
        private Product productDetail;
        private int NumberSlider = 0;
        private int Quantity { get; set; }
        private bool UnitSelected = true;
        #endregion

        #region properties
        public bool IsSearcherProduct { get => isSearcherProduct; set => isSearcherProduct = value; }
        public Product ProductCurrent { get => product; set => product = value; }
        public Product ProductDetailCurrent { get => productDetail; set => productDetail = value; }
        #endregion

        #region constructors 
        public ProductDetailViewController(IntPtr handle) : base(handle)
        {
            _productModel = new ProductModel(new ProductsService(DeviceManager.Instance));
        }
        #endregion

        #region life cycle 

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {               
                this.LoadExternalViews();
                this.LoadCorners();
                this.LoadHandlers();
                this.GetProductDetail();
                this.ValidateTextView();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Product, nameof(ProductDetailViewController));
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.ShowAccountProfile();
                NavigationView.IsSummaryDisabled = false;
                NavigationView.IsAccountEnabled = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region methods
        private void GetProductDetail()
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                InvokeOnMainThread(async () =>
                {
                    await this.GetProductDetailAsync();
                });
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                //LoadNavigationView(this.NavigationController.NavigationBar);
                LoadSearchProductsView(searchProductView);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                if (IsSearcherProduct)
                {
                    HiddenSearchProduct(searchProductView);
                }
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }
        private void ValidateTextView()
        {

            howLikeTextView.Changed += (sender, e) =>
            {
                if (howLikeTextView.Text.Length > 120)
                {
                    howLikeTextView.Text = howLikeTextView.Text.Substring(0, howLikeTextView.Text.Length - 1);
                }
            };
        }


        private void LoadFonts()
        {
            try
            {
                productPreviousPriceLabel.TextColor = ConstantColor.UiPricePrevious;
                productPreviousPriceLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductDetailPreviousPrice);
                productPriceLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductDetailPrice);
                productCountLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.ProductDetailCount);
                productDiscountLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductDetailDescount);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.LoadFonts);
                ShowMessageException(exception);
            }
        }

        private void LoadCorners()
        {
            try
            {
                productAddCarView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
                productAddCarView.Layer.BorderWidth = ConstantStyle.BorderWidth;
                productAddCarView.Layer.CornerRadius = ConstantStyle.CornerRadius;

                productCountView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
                productCountView.Layer.BorderWidth = ConstantStyle.BorderWidth;
                productCountView.Layer.CornerRadius = ConstantStyle.CornerRadius;

                howLikeTextView.Layer.BorderColor = ConstantColor.UiOrderFilterButton.ColorWithAlpha(0.3f).CGColor;
                howLikeTextView.Layer.BorderWidth = ConstantStyle.BorderWidth;
                howLikeTextView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private async Task GetProductDetailAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                ProductParameters parameters = new ProductParameters()
                {
                    SkuId = product.SkuId,
                    ProductId = product.Id,
                    DependencyId = ValidateDependecy(),
                    ProductType = product.ProductType
                };
                ProductResponse productResponse = await _productModel.GetProduct(parameters);
                productDetail = productResponse.Product;
                if (productDetail != null)
                {
                    this.LoadData();
                }
                else
                {
                    StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.productDetailNotFound);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.GetProductDetail);
                ShowMessageException(exception);
            }
        }

        private void LoadDataDiscountPercent()
        {
            if (productDetail.Price.DiscountPercent > 0)
            {
                productDiscountLabel.Hidden = false;
                productDiscountLabel.Text = StringFormat.ToPercerntaje(productDetail.Price.DiscountPercent);
                productDisountImageView.Hidden = false;

                if (!string.IsNullOrEmpty(product.Price.PreviousPrice.ToString()) && product.Price.PreviousPrice > 0)
                {
                    productPreviousPriceLabel.Hidden = false;
                    var attrString = new NSAttributedString(AppMessages.BeforeText + " " + StringFormat.ToPrice(productDetail.Price.PreviousPrice), new UIStringAttributes { StrikethroughStyle = NSUnderlineStyle.Single });
                    productPreviousPriceLabel.AttributedText = attrString;
                    productPreviousPriceLabel.TextColor = UIColor.Black;
                }
                else
                {
                    productPreviousPriceLabel.Hidden = true;
                }
                productPriceLabel.Text = AppMessages.NowText + " ";
                productPriceLabel.TextColor = ConstantColor.UiPriceWhitDiscount;
                if (product.Price.PriceOtherMeans > 0 && product.Price.PreviousPrice != product.Price.PriceOtherMeans)
                {
                    priceOtherMeansLabel.Text = AppMessages.OtherPaymentMethods + StringFormat.ToPrice(product.Price.PriceOtherMeans);
                    priceOtherMeansLabel.Hidden = false;
                }
                else
                {
                    priceOtherMeansLabel.Hidden = true;
                }

            }
            else
            {
                productDiscountLabel.Hidden = true;
                productDiscountLabel.Text = String.Empty;
                productDisountImageView.Hidden = true;
                productPreviousPriceLabel.Hidden = true;
                productPriceLabel.TextColor = ConstantColor.UiPriceWhitOutDiscount;
                productPriceLabel.Text = String.Empty;
                productPreviousPriceLabel.Hidden = true;
                productPriceLabel.Text = String.Empty;
                productPreviousPriceLabel.Text = String.Empty;
            }
            if (product.Price.DiscountImage != null)
            {
                discountImageView.Hidden = false;
                discountImageView.SetImage(new NSUrl(product.Price.DiscountImage));
            }
            productPriceLabel.Text += StringFormat.ToPrice(productDetail.Price.ActualPrice);
        }

        private void LoadData()
        {
            try
            {
                productDescriptionLabel.Text = productDetail.Name;
                LoadDataDiscountPercent();
                if (productDetail.UrlImagesXL.Any())
                {
                    if (productDetail.UrlImagesXL.Count <= 1)
                    {
                        ProductImagePageControl.Hidden = true;
                    }
                    ProductImagePageControl.Pages = productDetail.UrlImagesXL.Count;
                    ProductImagePageControl.CurrentPage = NumberSlider;
                    PageControlEditingChanged(ProductImagePageControl, null);
                    productImageImageView.SetImage(new NSUrl(productDetail.UrlImagesXL[NumberSlider]), UIImage.FromFile(ConstantImages.SinImagen));
                }
                else
                {
                    productImageImageView.Hidden = true;
                    ProductImagePageControl.Hidden = true;
                }

                if (productDetail.Description != null)
                {
                    productInfoDetailTextView.Text = productDetail.Description;
                }
                else
                {
                    borderBottomTableContainerView.Hidden = true;
                    productInfoDetailTextView.Hidden = true;
                }
                if (productDetail.UrlImageNutritionFact != null)
                {
                    borderBottomTableContainerView.Hidden = false;
                    productContainerTableImageView.Hidden = false;
                    productContainerTableImageView.SetImage(new NSUrl(productDetail.UrlImageNutritionFact), UIImage.FromFile(ConstantImages.SinImagen));
                }
                else
                {
                    productContainerTableImageView.Hidden = true;
                    if (productDetail.Description != null && productDetail.Description.Trim() == string.Empty)
                    {
                        borderBottomPriceView.Hidden = true;
                    }
                }
                UnitSelected = true;
                string countProduct = string.Empty;
                decimal weight = product.Weight;
                decimal presentation = product.Weight;
                countProduct = presentation > 0 ? Util.ConvertToUnit(weight, presentation).ToString() : "1";
                productCountLabel.Text = product.Quantity == 0 ? countProduct : product.Quantity.ToString();
                if (productDetail.IsEstimatedWeight)
                {
                    howLikeLabelButton.Hidden = false;
                    howLikeTextView.Hidden = false;
                    weigthOptionView.Hidden = true;
                    weightOptionButton.Hidden = false;
                    noteVariablePriceWeightStackView.Hidden = false;
                    borderBottomAddToListView.Hidden = false;
                }
                else
                {
                    weigthOptionView.Hidden = true;
                    weightOptionButton.Hidden = true;
                    unitOptionView.Hidden = true;
                    unitOptionButton.Hidden = true;
                    howLikeLabelButton.Hidden = true;
                    howLikeTextView.Hidden = true;
                    noteVariablePriceWeightStackView.Hidden = true;
                    borderBottomAddToListView.Hidden = true;
                }
                Quantity = product.Quantity;
                productPumLabel.Text = product.Price.Pum;
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.LoadData);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            addListButton.TouchUpInside += AddListButtonTouchUpInside;
            ProductImagePageControl.EditingChanged += PageControlEditingChanged;
            ProductImagePageControl.ValueChanged += PageControlEditingChanged;
            addProductToCarButton.TouchUpInside += AddProductToCarUpInside;
            unitOptionButton.TouchUpInside += UnitOptionUpInside;
            weightOptionButton.TouchUpInside += WeightOptionUpInside;
            AddCountProductButton.TouchUpInside += AddOProductCountUpInside;
            SubstractCountProductButton.TouchUpInside += SubtractProductCountUpInside;
            showFullImageButton.TouchUpInside += ShowViewFullImageUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            WireUpSwipeRight();
            WireUpSwipeLeft();
        }

        private void CustomPageControl()
        {
            try
            {
                var x = 0;
                var width = 15;
                var height = 15;
                for (int i = 0; i < ProductImagePageControl.Subviews.Length; i++)
                {
                    UIView dot = ProductImagePageControl.Subviews[i];
                    dot.Frame = new CGRect(x, dot.Frame.Y, width, height);
                    dot.Layer.BorderWidth = 1;
                    dot.Layer.CornerRadius = dot.Frame.Width / 2;
                    dot.Layer.BorderColor = new CGColor(0, 0, 0, 1);
                    if (i == ProductImagePageControl.CurrentPage)
                    {
                        dot.BackgroundColor = ConstantColor.UiPageControlDot;
                    }
                    else
                    {
                        dot.BackgroundColor = UIColor.Clear;
                    }
                }
                ProductImagePageControl.ReloadInputViews();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.ConfigurePageControl);
                ShowMessageException(exception);
            }
        }

        private void AddProduct()
        {
            try
            {
                var productSaved = DataBase.GetProduct(product.Id);
                if (productSaved == null)
                {
                    RegisterAddProductEvent();
                }

                string[] values = productCountLabel.Text.Split(' ');
                decimal value = decimal.Parse(values[0]);
                if (!UnitSelected)
                {
                    Quantity = Util.ConvertToUnit(value, productDetail.Weight);
                }
                else
                {
                    Quantity = (int)value;
                }
                product.Quantity = Quantity;
                DataBase.UpSertProduct(product);
                ParametersManager.ContainChanges = true;
                this.NavigationController.PopViewController(true);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.AddProduct);
            }
        }

        private void RegisterAddProductEvent()
        {
            FirebaseEventRegistrationService.Instance.AddProductToCart(product, product.CategoryName);
            FacebookEventRegistrationService.Instance.AddProductToCart(product);
        }
        #endregion

        #region Events Actions
        partial void ChangeImageProductDetail(UIPageControl sender)
        {
            try
            {
                var currentPage = sender.CurrentPage;
                var myImages = productDetail.UrlImagesXL[(int)currentPage];
                productImageImageView.SetImage(new NSUrl(myImages), UIImage.FromFile(ConstantImages.SinImagen));
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantMethodName.ChangeImageProductDetail);
                ShowMessageException(exception);
            }
        }

        private void AddProductToCarUpInside(object sender, EventArgs e)
        {
            try
            {
                //ParametersManager.ProductUpdated = true;
                AddProduct();
                UpdateCar(true);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantEventName.AddProductToCarUpInside);
                ShowMessageException(exception);
            }
        }

        private void AddOProductCountUpInside(object sender, EventArgs e)
        {
            try
            {
                String[] values = productCountLabel.Text.Split(' ');
                decimal currentValue = 0;
                decimal presentationValue = 0;
                currentValue = decimal.Parse(values[0]);
                presentationValue = productDetail.Weight;
                if (productDetail.IsEstimatedWeight && !UnitSelected)
                {
                    currentValue += presentationValue;
                }
                else
                {
                    currentValue += 1;
                }
                productCountLabel.Text = currentValue.ToString() + (!UnitSelected ? (values[1] == null ? " " : " " + values[1]) : string.Empty);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantEventName.AddProductCountUpInside);
                ShowMessageException(exception);
            }
        }

        private void SubtractProductCountUpInside(object sender, EventArgs e)
        {
            try
            {
                String[] values = productCountLabel.Text.Split(' ');
                decimal currentValue = 0;
                decimal presentationValue = 0;
                currentValue = decimal.Parse(values[0]);
                presentationValue = productDetail.Weight;
                if (productDetail.IsEstimatedWeight && !UnitSelected)
                {
                    currentValue -= presentationValue;
                }
                else
                {
                    currentValue -= 1;
                }

                if (currentValue < 0)
                {
                    currentValue = 0;
                }
                productCountLabel.Text = currentValue.ToString() + (!UnitSelected ? (values[1] == null ? " " : " " + values[1]) : string.Empty);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantEventName.SubtractProductCountUpInside);
                ShowMessageException(exception);
            }
        }


        private void ShowViewFullImageUpInside(object sender, EventArgs e)
        {
            try
            {
                if (productDetail.UrlImagesXL != null)
                {
                    if (this.Storyboard.InstantiateViewController(ConstantControllersName.ProductDetailFullImageViewController) is ProductDetailFullImageViewController productDetailFullImageViewController_)
                    {
                        productDetailFullImageViewController_.loadImages(productDetail.UrlImagesXL);
                        this.NavigationController.PushViewController(productDetailFullImageViewController_, true);
                    }
                }
                else
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.ProductSelectedImagesNotFound, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                    PresentViewController(alertController, animated: true, completionHandler: null);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailViewController, ConstantEventName.AddProductCountUpInside);
                ShowMessageException(exception);
            }
        }

        private void PageControlEditingChanged(object sender, EventArgs e)
        {
            CustomPageControl();
        }

        private void UnitOptionUpInside(object sender, EventArgs e)
        {
            if (!UnitSelected)
            {
                UnitSelected = true;
                unitOptionView.Hidden = false;
                unitOptionButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductDetailUnitWeight);

                weigthOptionView.Hidden = true;
                weightOptionButton.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.ProductDetailUnitWeight);

                int count = Util.ConvertToUnit(decimal.Parse(productCountLabel.Text.Split(' ')[0]), productDetail.Weight);
                productCountLabel.Text = count.ToString();
            }
        }

        private void WeightOptionUpInside(object sender, EventArgs e)
        {
            if (UnitSelected)
            {
                UnitSelected = false;
                unitOptionView.Hidden = true;
                unitOptionButton.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.ProductDetailUnitWeight);
                weigthOptionView.Hidden = false;
                weightOptionButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductDetailUnitWeight);
                decimal count = Util.ConvertToWeight(int.Parse(productCountLabel.Text.Split(' ')[0]), productDetail.Weight);
                productCountLabel.Text = count + " " + productDetail.WeightUnits;
            }
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.GetProductDetail();
        }
        #endregion

        #region Gestures Events
        private void AddListButtonTouchUpInside(object sender, EventArgs e)
        {
            MyCustomListViewController myCustomListViewController = (MyCustomListViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.MyCustomListViewController);
            string value = JsonService.Serialize<Product>(product);
            ProductList productList = new ProductList();
            productList = JsonService.Deserialize<ProductList>(value);
            myCustomListViewController.Product = productList;
            ParametersManager.ShoppingListSelectedId = "";
            this.NavigationController.PushViewController(myCustomListViewController, true);
        }

        private void WireUpSwipeRight()
        {
            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            productImageImageView.AddGestureRecognizer(gesture);
        }

        private void WireUpSwipeLeft()
        {
            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            productImageImageView.AddGestureRecognizer(gesture);
        }

        private void HandleDrag(UISwipeGestureRecognizer recognizer)
        {
            if (recognizer.Direction == UISwipeGestureRecognizerDirection.Right)
            {
                if (productDetail.UrlImagesXL != null && productDetail.UrlImagesXL.Any())
                {
                    NumberSlider -= 1;

                    if (NumberSlider < 0)
                    {
                        NumberSlider = productDetail.UrlImagesXL.Count - 1;
                    }

                    ProductImagePageControl.CurrentPage = NumberSlider;
                    PageControlEditingChanged(ProductImagePageControl, null);
                    UIView.Animate(0.5, 0, UIViewAnimationOptions.TransitionFlipFromLeft, () =>
                    {
                        productImageImageView.SetImage(new NSUrl(productDetail.UrlImagesXL[NumberSlider]), UIImage.FromFile(ConstantImages.SinImagen));
                    }, null);
                }
            }
            else if (recognizer.Direction == UISwipeGestureRecognizerDirection.Left)
            {
                if (productDetail.UrlImagesXL != null && productDetail.UrlImagesXL.Any())
                {
                    NumberSlider += 1;
                    if (NumberSlider > (productDetail.UrlImagesXL.Count - 1))
                    {
                        NumberSlider = 0;
                    }
                    ProductImagePageControl.CurrentPage = NumberSlider;
                    PageControlEditingChanged(ProductImagePageControl, null);
                    UIView.Animate(0.5, 0, UIViewAnimationOptions.TransitionFlipFromLeft, () =>
                    {
                        productImageImageView.SetImage(new NSUrl(productDetail.UrlImagesXL[NumberSlider]), UIImage.FromFile(ConstantImages.SinImagen));
                    }, null);
                }
            }
        }
        #endregion
    }
}

