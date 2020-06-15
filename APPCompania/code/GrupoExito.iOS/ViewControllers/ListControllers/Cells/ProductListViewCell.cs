using System;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Helpers;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Cells
{
    public partial class ProductListViewCell : UITableViewCell
    {
        #region Attributes
        private Product Product;
        private bool UnitSelected = true;
        private UnitWeightView _unitWeightView;
        private NSIndexPath indexPath;
        private EventHandler addAction;
        private EventHandler substractionAction;
        #endregion

        #region Properties 
        public UIButton SelectButtonList { get => SelectButton; }
        public UIImageView SelectImage { get => selectImageView; }
        public UnitWeightView UnitWeightView { get => _unitWeightView; set => _unitWeightView = value; }
        public EventHandler AddAction { get => addAction; set => addAction = value; }
        public EventHandler SubstractionAction { get => substractionAction; set => substractionAction = value; }
        public NSIndexPath IndexPath { get => indexPath; set => indexPath = value; }
        #endregion

        #region Constructors
        static ProductListViewCell()
        {
            //Constructor static this class
        }

        protected ProductListViewCell(IntPtr handle) : base(handle)
        {
            //Default Constructor this class
        }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            LoadCorners();
            LoadHandlers();
        }
        #endregion

        #region Methods 
        public void LoadProductListViewCell(Product product)
        {
            this.Product = product;
            if (product.UrlMediumImage != null)
            {
                productImageView.SetImage(new NSUrl(product.UrlMediumImage));
            }
            else
            {
                productImageView.Image = UIImage.FromFile(ConstantImages.SinImagen);
            }
            productNameLabel.Text = product.Name;
            productInfoLabel.Text = product.Description;
            priceLabel.Text = StringFormat.ToPrice(product.Price.ActualPrice);
            if (product.Selected)
            {
                iconCheckedImageView.Image = UIImage.FromFile(ConstantImages.CheckBoxSelected);
                containerCheckedView.BackgroundColor = ConstantColor.UiPrimary;
                this.BackgroundColor = ConstantColor.UiPrimary.ColorWithAlpha(0.5f);
                containerCheckedView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            else
            {
                iconCheckedImageView.Image = UIImage.FromFile(ConstantImages.CheckboxUnselected);
                containerCheckedView.BackgroundColor = UIColor.White;
                this.BackgroundColor = UIColor.White;
            }
            if (!string.IsNullOrEmpty(product.Price.Pum))
            {
                pumLabel.Text = product.Price.Pum;
            }
            else
            {
                pumLabel.Hidden = true;
            }
            LoadControllers();
        }

        private void LoadCorners()
        {
            productImageView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            productImageView.ClipsToBounds = true;
            unitWeightView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            unitWeightView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            unitWeightView.Layer.BorderWidth = ConstantStyle.BorderWidth;
        }

        private void LoadHandlers()
        {
            //Load events controls
        }

        private void LoadControllers()
        {
            unitWeightView.LayoutIfNeeded();
            if (Product != null && Product.Quantity >= 0)
            {
                int heightView = Product.IsEstimatedWeight ? ConstantViewSize.ProductCellHeightViewUnitWeight : ConstantViewSize.ProductCellHeightViewUnit;
                if (UnitWeightView == null)
                {
                    UnitWeightView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.UnitWeightView, Self, null).GetItem<UnitWeightView>(0);
                    CGRect unitWeightFrame = new CGRect(0, 0, unitWeightView.Frame.Size.Width, heightView);
                    Util.SetConstraint(unitWeightView, unitWeightView.Frame.Height, heightView);
                    UnitWeightView.Frame = unitWeightFrame;
                    unitWeightView.AddSubview(UnitWeightView);
                    UnitWeightView.Adds.TouchUpInside += AddTouchUpInside;
                    UnitWeightView.Substraction.TouchUpInside += SubstractionTouchUpInside;
                    UnitWeightView.Unit.TouchUpInside += UnitTouchUpInside;
                    UnitWeightView.Weight.TouchUpInside += WeightTouchUpInside;
                }
                if (Product.IsEstimatedWeight)
                {
                    UnitWeightView.ShowUnitWeight();
                }
                else
                {
                    UnitWeightView.HiddenUnitWeight();
                    heightView = ConstantViewSize.ProductCellHeightViewUnit;
                }
                UnitSelected = true;
                UnitWeightView.Count.Text = Product.Quantity.ToString();
            }
        }

        public void HiddenEditableImage()
        {
            selectImageView.Hidden = true;
            SelectButton.Hidden = true;
        }
        #endregion

        #region Events
        private void AddTouchUpInside(object sender, EventArgs e)
        {
            String[] values = UnitWeightView.Count.Text.Split(' ');
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
            UnitWeightView.Count.Text = currentValue.ToString() + (!UnitSelected ? (values[1] == null ? " " : " " + values[1]) : string.Empty);
            addAction?.Invoke(IndexPath, e);
        }

        private void SubstractionTouchUpInside(object sender, EventArgs e)
        {
            String[] values = UnitWeightView.Count.Text.Split(' ');
            decimal currentValue = 0;
            decimal presentationValue = 0;
            currentValue = decimal.Parse(values[0]);
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

            if (currentValue <= 0)
            {
                currentValue = 0;
            }
            UnitWeightView.Count.Text = currentValue.ToString() + (!UnitSelected ? (values[1] == null ? " " : " " + values[1]) : string.Empty);
            substractionAction(IndexPath, e);
        }

        private void UnitTouchUpInside(object sender, EventArgs e)
        {
            if (!UnitSelected)
            {
                UnitWeightView.selectedUnit(); 
                int count = Util.ConvertToUnit(decimal.Parse(UnitWeightView.Count.Text.Split(' ')[0]), Product.Weight);
                UnitWeightView.Count.Text = count.ToString();
            }
        }

        private void WeightTouchUpInside(object sender, EventArgs e)
        {
            if (UnitSelected)
            {
                UnitSelected = false;
                UnitWeightView.selectedWeight();
                decimal count = Util.ConvertToWeight(int.Parse(UnitWeightView.Count.Text.Split(' ')[0]), Product.Weight);
                UnitWeightView.Count.Text = count + " " + Product.WeightUnits;
            }
        }
        #endregion
    }
}
