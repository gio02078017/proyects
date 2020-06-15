using System;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using SDWebImage;
using Shimmer;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Cells
{
    public partial class ProductViewCell : UICollectionViewCell
    {
        #region Attributes 
        private Product product;
        private UnitWeightView _unitWeightView;
        private bool UnitSelected = true;
        private bool _isSearcherProduct = false;
        private FBShimmeringView shimmeringView;
        private ProductCarModel dataBase;
        #endregion

        #region Attributes Events
        private EventHandler actionSelected;
        private EventHandler actionAddingAndRemoveProducts;
        private EventHandler actionAddProductToList;
        private EventHandler actionSummary;
        #endregion

        #region Properties 
        public bool IsSearcherProduct { get => _isSearcherProduct; set => _isSearcherProduct = value; }
        public Product Product { get => product; set => product = value; }
        #endregion

        #region Properties Events
        public EventHandler ActionSelected { get => actionSelected; set => actionSelected = value; }
        public EventHandler ActionAddingAndRemoveProducts { get => actionAddingAndRemoveProducts; set => actionAddingAndRemoveProducts = value; }
        public EventHandler ActionAddProductToList { get => actionAddProductToList; set => actionAddProductToList = value; }
        public EventHandler ActionSummary { get => actionSummary; set => actionSummary = value; }
        #endregion

        #region Constructors 
        static ProductViewCell()
        {
            //Static default Constructor
        }
        protected ProductViewCell(IntPtr handle) : base(handle)
        {
            //Default constructor with parameter
        }
        #endregion

        #region Overrides methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.InitializeComponents();
            this.LoadHandlers();
            this.LoadColors();
            this.ContentView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
        }

        public override void LayoutSubviews()
        {
            this.LayoutIfNeeded();
        }
        #endregion

        #region Methods 
        private void InitializeComponents()
        {
            dataBase = new ProductCarModel(ProductCarDataBase.Instance);
        }

        public void LoadProductViewCell(Product product)
        {
            this.Product = product;
            if (shimmeringView == null)
            {
                shimmeringView = new FBShimmeringView()
                {
                    Frame = new CGRect(0, 0, ContentView.Frame.Width, ContentView.Frame.Height)
                };
                ContentView.Add(shimmeringView);
            }
            shimmeringView.ContentView = containerView;
            shimmeringView.ShimmeringOpacity = ConstantStyle.ShimmeringOpacity;
            shimmeringView.ShimmeringAnimationOpacity = ConstantStyle.AnimationShimmeringOpacity;
            shimmeringView.ShimmeringSpeed = ConstantDuration.SpeedShimmering;
            shimmeringView.ShimmeringHighlightLength = ConstantStyle.ShimmeringHighlightLength;
            shimmeringView.Shimmering = true;
            if (product != null && product.SiteId != null && product.SiteId.Equals(AppMessages.Template))
            {
                DrawTemplate();
            }
            else
            {
                ClearTemplate();
                if (product != null)
                {
                    if (product.Price != null && product.Price.DiscountPercent > 0)
                    {
                        percentDiscountLabel.Hidden = false;
                        percentDiscountLabel.Text = StringFormat.ToPercerntaje(product.Price.DiscountPercent);
                        discountImageView.Hidden = false;

                        if (!string.IsNullOrEmpty(product.Price.PreviousPrice.ToString()) && product.Price.PreviousPrice > 0)
                        {
                            productPreviousPriceLabel.Hidden = false;
                            NSAttributedString attrString = new NSAttributedString(AppMessages.BeforeText + StringFormat.ToPrice(product.Price.PreviousPrice), new UIStringAttributes { StrikethroughStyle = NSUnderlineStyle.Single });
                            productPreviousPriceLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.ProductBody);
                            productPreviousPriceLabel.AttributedText = attrString;
                        }
                        else
                        {
                            productPreviousPriceLabel.Hidden = true;
                        }

                        productCurrentPriceLabel.Text = AppMessages.NowText;
                        productCurrentPriceLabel.TextColor = UIColor.Red;

                        if (product.Price.DiscountImage != null)
                        {
                            discountTargetImageView.Hidden = false;
                            discountTargetImageView.SetImage(new NSUrl(product.Price.DiscountImage));
                        }
                        else
                        {
                            discountTargetImageView.Hidden = true;
                        }

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
                        percentDiscountLabel.Hidden = true;
                        priceOtherMeansLabel.Hidden = true;
                        discountImageView.Hidden = true;
                        discountTargetImageView.Hidden = true;
                        productPreviousPriceLabel.Hidden = true;
                        productCurrentPriceLabel.TextColor = UIColor.Black;
                        productCurrentPriceLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductBody);
                        productCurrentPriceLabel.Text = String.Empty;
                    }
                    productImageView.SetImage(new NSUrl(product.UrlMediumImage), UIImage.FromFile(ConstantImages.SinImagen), HandleSDExternalCompletionHandler);
                    productNameLabel.Text = product.Name;
                    pumLabel.Text = product.Price.Pum;
                    productCurrentPriceLabel.Text += StringFormat.ToPrice(product.Price.ActualPrice);
                    addToCarView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    LoadControllers();
                }
            }
        }

        public void DrawBorderForType(string TypeCollection, NSIndexPath indexPath, int products)
        {
            if (TypeCollection.Equals(ConstantIdentifier.Discount_products))
            {
                if (indexPath.Row == products)
                {
                    borderRight.Hidden = true;
                    borderBottomView.Hidden = true;
                }
            }
            else if (TypeCollection.Equals(ConstantIdentifier.Favorite_products) || TypeCollection.Equals(ConstantIdentifier.Products_by_category))
            {
                if (indexPath.Row % 2 == 0)
                {
                    borderRight.Hidden = false;
                }
                else
                {
                    borderRight.Hidden = true;
                }
            }
        }

        private void DrawTemplate()
        {
            productImageView.BackgroundColor = ConstantColor.UIBackgroundShimmer;
            productNameLabel.BackgroundColor = productImageView.BackgroundColor;
            productNameLabel.TextColor = productImageView.BackgroundColor;
            productCurrentPriceLabel.BackgroundColor = productImageView.BackgroundColor;
            productCurrentPriceLabel.TextColor = productImageView.BackgroundColor;
            productPreviousPriceLabel.BackgroundColor = productImageView.BackgroundColor;
            productPreviousPriceLabel.TextColor = productImageView.BackgroundColor;
            pumLabel.BackgroundColor = productImageView.BackgroundColor;
            pumLabel.TextColor = productImageView.BackgroundColor;
            priceOtherMeansLabel.BackgroundColor = productImageView.BackgroundColor;
            priceOtherMeansLabel.TextColor = productImageView.BackgroundColor;
            addToCarView.BackgroundColor = productImageView.BackgroundColor;
            addToCarView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            productImageView.Image = null;
            addToCarLabel.Hidden = true;
            addProductButton.Enabled = false;
            addToCarImageView.Hidden = true;
            productDetailButton.Enabled = false;
            addProductButton.Hidden = false;
            addListButton.BackgroundColor = UIColor.White;
            discountImageView.Hidden = true;
            discountTargetImageView.Hidden = true;
            percentDiscountLabel.Hidden = true;
            if(_unitWeightView != null)
            {
                _unitWeightView.Adds.Enabled = false;
                _unitWeightView.Substraction.Enabled = false;
                unitWeightHeightConstraint.Constant = 0;
            }
        }

        private void ClearTemplate()
        {
            productImageView.BackgroundColor = UIColor.Clear;
            productNameLabel.BackgroundColor = UIColor.Clear;
            productNameLabel.TextColor = UIColor.Black;
            productCurrentPriceLabel.BackgroundColor = UIColor.Clear;
            productCurrentPriceLabel.TextColor = productNameLabel.TextColor;
            productPreviousPriceLabel.BackgroundColor = UIColor.Clear;
            productPreviousPriceLabel.TextColor = productNameLabel.TextColor;
            priceOtherMeansLabel.BackgroundColor = UIColor.Clear;
            priceOtherMeansLabel.TextColor = UIColor.DarkGray; ;
            addToCarView.BackgroundColor = ConstantColor.UiPrimary;
            pumLabel.BackgroundColor = UIColor.Clear;
            pumLabel.TextColor = UIColor.DarkGray;
            addToCarLabel.Hidden = false;
            addToCarImageView.Hidden = false;
            productDetailButton.Enabled = true;
            addProductButton.Enabled = true;
            addListButton.BackgroundColor = UIColor.Clear;
        }

        private void LoadControllers()
        {
            try
            {
                //Se añade regla para determinar si el producto se encuentra en la base de datos local
                Product exists = dataBase.GetProduct(this.Product.Id);
                if(exists != null)
                {
                    this.Product.Quantity = exists.Quantity;
                     dataBase.UpSertProduct(this.Product);
                }
                //Fin de regla 20190117
                if (Product != null && Product.Quantity > 0)
                {
                    LoadUnitWeightView();
                }
                else if (_unitWeightView != null)
                {
                    HiddenUnitWeightView();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.ProductViewCell, ConstantMethodName.LoadControllers);
            }
        }

        private void LoadFonts()
        {
            percentDiscountLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.BodyGeneric);
            productNameLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductSubtitle);
            productCurrentPriceLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductTitle);
            productPreviousPriceLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PreviousPrice);
            addToCarLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductBody);
        }

        private void LoadColors()
        {
            this.addToCarView.BackgroundColor = ConstantColor.UiPrimary;
            this.addToCarLabel.TextColor = ConstantColor.UiTextColorGeneric;
        }

        private void LoadHandlers()
        {
            addProductButton.TouchUpInside += AddProductTouchUpInside;
            addListButton.TouchUpInside += AddListButtonTouchUpInside;
            productDetailButton.TouchUpInside += ProductDetailButtonTouchUpInside;
        }

        private void LoadUnitWeightView()
        {
            _unitWeightView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.UnitWeightView, Self, null).GetItem<UnitWeightView>(0);
            double widthView = (this.Superview != null ? (this.Superview.Frame.Width / 2) - 30 : this.Frame.Width - 30);
            CGRect unitWeightFrame = new CGRect(0, 0, widthView, ConstantViewSize.ProductCellHeightViewUnitWeight);
            _unitWeightView.Frame = unitWeightFrame;
            unitWeightView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            unitWeightView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            unitWeightView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            unitWeightView.AddSubview(_unitWeightView);
            _unitWeightView.Adds.TouchUpInside += AddTouchUpInside;
            _unitWeightView.Substraction.TouchUpInside += SubstractionTouchUpInside;
            _unitWeightView.Unit.TouchUpInside += UnitTouchUpInside;
            _unitWeightView.Weight.TouchUpInside += WeightTouchUpInside;
            _unitWeightView.Count.Text = Product.Quantity.ToString();
            Util.SetConstraint(unitWeightView, unitWeightView.Frame.Height, ConstantViewSize.ProductCellHeightViewUnitWeight);
            unitWeightView.Hidden = false;
            UnitSelected = true;
            if (Product.IsEstimatedWeight)
            {
                _unitWeightView.ShowUnitWeight();
            }
            else
            {
                _unitWeightView.HiddenWeight();
            }
            _unitWeightView.selectedUnit();
            addProductButton.Hidden = true;
        }

        private void HiddenUnitWeightView()
        {
            _unitWeightView.HiddenControls();
            _unitWeightView.Hidden = true;
            _unitWeightView.RemoveFromSuperview();
            unitWeightView.Hidden = true;
            addProductButton.Hidden = false;
            addProductButton.Enabled = true;
        }
        #endregion

        #region Events 
        private void ProductDetailButtonTouchUpInside(object sender, EventArgs e)
        {
            actionSelected?.Invoke(product, e);
        }

        private void AddListButtonTouchUpInside(object sender, EventArgs e)
        {
            actionAddProductToList?.Invoke(this.Product, e);
        }

        private void AddProductTouchUpInside(object sender, EventArgs e)
        {
            this.containerView.Frame = new CGRect(this.containerView.Frame.X, this.containerView.Frame.Y, this.containerView.Frame.Width, ConstantViewSize.ProductHeightWithProductsAdded);
            LoadUnitWeightView();
            string countProduct = string.Empty;
            decimal weight = Product.Weight;
            decimal presentation = Product.Weight == 0 ? 1 : Product.Weight;
            countProduct = (Product.IsEstimatedWeight && presentation > 0) ? Util.ConvertToUnit(weight, presentation).ToString() : "1";
            _unitWeightView.Count.Text = countProduct;
            _unitWeightView.selectedUnit();
            if (this.Frame.Height.Equals(ConstantViewSize.ProductHeightCell) || !DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                ActionAddingAndRemoveProducts?.Invoke(sender, e);
            }
            if (Product.IsEstimatedWeight)
            {
                _unitWeightView.ShowWeight();
            }
            else
            {
                _unitWeightView.HiddenWeight();
            }
            addProductButton.Hidden = true;
            dataBase.UpSertProduct(Product, true);
            actionSummary?.Invoke(this.Product, e);
            RegisterAddProductEvent();
        }

        private void AddTouchUpInside(object sender, EventArgs e)
        {
            string[] values = _unitWeightView.Count.Text.Split(' ');
            decimal currentValue = 0;
            decimal presentationValue = 0;
            currentValue = decimal.Parse(values[0]);
            presentationValue = Product.Weight == 0 ? 1 : Product.Weight;
            if (Product.IsEstimatedWeight && !UnitSelected)
            {
                currentValue += presentationValue;
                Product.Quantity = Util.ConvertToUnit(currentValue, Decimal.Parse(presentationValue.ToString()));
            }
            else
            {
                currentValue += 1;
                Product.Quantity = (int)currentValue;
            }
            _unitWeightView.Count.Text = currentValue.ToString();
            if (!UnitSelected && values != null && values[1] != null)
            {
                _unitWeightView.Count.Text += " " + values[1];
            }
            dataBase.UpSertProduct(this.Product);
            actionSummary?.Invoke(this.Product, e);
            RegisterAddProductEvent();
        }

        private void SubstractionTouchUpInside(object sender, EventArgs e)
        {
            string[] values = _unitWeightView.Count.Text.Split(' ');
            decimal currentValue = 0;
            decimal presentationValue = 0;
            currentValue = values != null && values.Length > 0 ? decimal.Parse(values[0]) : currentValue;
            presentationValue = Product.Weight == 0 ? 1 : Product.Weight;
            if (Product.IsEstimatedWeight && !UnitSelected)
            {
                currentValue -= presentationValue;
                Product.Quantity = Util.ConvertToUnit(currentValue, presentationValue);
            }
            else
            {  
                currentValue -= 1;
                Product.Quantity = (int)currentValue;
            }
            _unitWeightView.Count.Text = currentValue.ToString();
            if (!UnitSelected && values != null && values[1] != null)
            {
                _unitWeightView.Count.Text += " " + values[1];
            }
            if (currentValue <= 0)
            {
                Product.Quantity = 0;
                ActionAddingAndRemoveProducts?.Invoke(sender, e);
                currentValue = 0;
                addProductButton.Hidden = false;
                unitWeightHeightConstraint.Constant = 0;
                _unitWeightView.RemoveFromSuperview();
            }
            dataBase.UpSertProduct(this.Product);
            actionSummary?.Invoke(this.Product, e);
            RegisterDeleteProductFromCartEvent();
        }

        private void UnitTouchUpInside(object sender, EventArgs e)
        {
            if (!UnitSelected)
            {
                UnitSelected = true;
                _unitWeightView.selectedUnit();
                int count = Util.ConvertToUnit(decimal.Parse(_unitWeightView.Count.Text.Split(' ')[0]), Product.Weight);
                _unitWeightView.Count.Text = count.ToString();
            }
        }

        private void WeightTouchUpInside(object sender, EventArgs e)
        {
            if (UnitSelected)
            {
                UnitSelected = false;
                _unitWeightView.selectedWeight();
                decimal count = Util.ConvertToWeight(int.Parse(_unitWeightView.Count.Text.Split(' ')[0]), Product.Weight);
                _unitWeightView.Count.Text = count + " " + Product.WeightUnits;
            }
        }

        private void HandleSDExternalCompletionHandler(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl imageUrl)
        {
            try
            {
                if (shimmeringView != null)
                {
                    shimmeringView.Shimmering = false;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.ProductViewCell, ConstantEventName.HandleSDExternalCompletionHandler);
            }
        }

        public void RegisterAddProductEvent()
        {
            FirebaseEventRegistrationService.Instance.AddProductToCart(Product, Product.CategoryName);
            FacebookEventRegistrationService.Instance.AddProductToCart(Product);
        }

        public void RegisterDeleteProductFromCartEvent()
        {
            FirebaseEventRegistrationService.Instance.DeleteProductFromCart(product, product.CategoryName);
        }
        #endregion 
    }
}
