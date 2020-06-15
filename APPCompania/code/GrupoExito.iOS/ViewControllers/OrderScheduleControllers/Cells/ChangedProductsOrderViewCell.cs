using System;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities.Constant;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Cells
{
    public partial class ChangedProductsOrderViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ChangedProductsOrderViewCell");
        public static readonly UINib Nib;

        static ChangedProductsOrderViewCell()
        {
            Nib = UINib.FromName("ChangedProductsOrderViewCell", NSBundle.MainBundle);
        }

        protected ChangedProductsOrderViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void LoadProductListViewCell(Product product)
        {
            nameProductLabel.Text = product.Name;
            Price price = product.Price;
            if (price.PriceOtherMeans > 0)
            {
                PriceProductLabel.Text = price.PriceOtherMeans.ToString();
            }
            else
            {
                PriceProductLabel.Text = price.ActualPrice.ToString();
            }
            if (product.Price != null && product.Price.Pum != null)
            {
                pumLabel.Hidden = false;
                pumLabel.Text = product.Price.Pum;
            }
            else
            {
                pumLabel.Hidden = true;
            }
            productImageView.SetImage(new NSUrl(product.UrlMediumImage), UIImage.FromFile(ConstantImages.SinImagen));
        }
    }
}
