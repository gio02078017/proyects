using System;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class SummaryProductViewCell : GenericCell
    {
        public static readonly NSString Key = new NSString("SummaryProductViewCell");
        public static readonly UINib Nib;
        private ProductViewModel viewModel;

        static SummaryProductViewCell()
        {
            Nib = UINib.FromName("SummaryProductViewCell", NSBundle.MainBundle);
        }

        protected SummaryProductViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            quantityView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            quantityView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            quantityView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            Util.CreateShadowLayer(ContentView.Subviews[0], 1.0f, 0.5f);
        }

        public override void Setup(object v)
        {
            if (v is ProductViewModel model)
            {
                this.viewModel = model;
                productTitleLabel.Text = model.Name;

                howDoYouLikeItButton.Hidden = !viewModel.IsEstimatedWeight;
                decimal price = ModelHelper.GetPrice(model.Price);
                priceLabel.Text = StringFormat.ToPrice(price * model.Quantity);
                pumLabel.Text = viewModel.Price.Pum;
                productImageView.SetImage(NSUrl.FromString(model.ImageUrl), UIImage.FromFile(ConstantImages.SinImagen));

                if (!model.IsEstimatedWeight)
                {
                    unitWeightStackView.Hidden = true;
                }
                SetQuantityLabel(model.WeightSelected);
                SetQuantityPresentation(model.WeightSelected);

                unitButton.TouchUpInside += (sender, e) =>
                {
                    viewModel.WeightSelected = false;
                    SetQuantityLabel(viewModel.WeightSelected);
                    SetQuantityPresentation(viewModel.WeightSelected);
                };
                weightButton.TouchUpInside += (sender, e) =>
                {
                    viewModel.WeightSelected = true;
                    SetQuantityLabel(viewModel.WeightSelected);
                    SetQuantityPresentation(viewModel.WeightSelected);
                };

                if(viewModel.Quantity <= 1)
                {
                    substractImageView.Image = UIImage.FromFile(ConstantImages.Eliminar);
                }
                else
                {
                    substractImageView.Image = UIImage.FromFile(ConstantImages.Menos);
                }
                addButton.TouchUpInside += viewModel.AddQuantity;
                substractButton.TouchUpInside += viewModel.SubstractQuantity;
                deleteButton.TouchUpInside += viewModel.DeleteProduct;
                howDoYouLikeItButton.TouchUpInside += viewModel.HowDoYouLikeIt;

                this.LayoutIfNeeded();
            }
        }

        private void SetQuantityPresentation(bool isEstimatedWeight)
        {
            unitButton.Enabled = isEstimatedWeight;
            weightButton.Enabled = !isEstimatedWeight;

            weightAccesoryView.Hidden = !isEstimatedWeight;
            unitAccesoryView.Hidden = isEstimatedWeight;

            UIFont subtitleFont = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.ProductByCategoryFilter);
            UIFont mediumFont = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.ProductByCategoryFilter);

            unitButton.Font = isEstimatedWeight ? mediumFont : subtitleFont;
            weightButton.Font = isEstimatedWeight ? subtitleFont : mediumFont;
        }

        private void SetQuantityLabel(bool isEstimatedWeight)
        {
            if (isEstimatedWeight)
            {
                decimal minimumWeight = viewModel.Weight;
                decimal totalWeight = minimumWeight * viewModel.Quantity;
                quantityLabel.Text = totalWeight < 0 ? "0.0" + " " + viewModel.WeightUnits : totalWeight.ToString() + " " + viewModel.WeightUnits;
            }
            else
            {
                quantityLabel.Text = GetQuantityLabel(viewModel.Quantity);
            }
        }

        private string GetQuantityLabel(int quantity)
        {
            if (quantity <= 0) return "0";
            else if (quantity > 0 && !viewModel.IsEstimatedWeight) return quantity.ToString();
            else return quantity.ToString();
        }

        public override void PrepareForReuse()
        {
            unitWeightStackView.Hidden = false;
            addButton.TouchUpInside -= viewModel.AddQuantity;
            substractButton.TouchUpInside -= viewModel.SubstractQuantity;
            deleteButton.TouchUpInside -= viewModel.DeleteProduct;
            howDoYouLikeItButton.TouchUpInside -= viewModel.HowDoYouLikeIt;
            viewModel = null;
        }
    }
}
