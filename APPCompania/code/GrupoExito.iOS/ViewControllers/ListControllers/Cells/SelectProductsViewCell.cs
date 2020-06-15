using System;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities.Constant;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Cells
{
    public partial class SelectProductsViewCell : UITableViewCell
    {
        public SelectProductsViewCell(IntPtr handle) : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            checkImageView.Layer.CornerRadius = checkImageView.Layer.Frame.Height / 2;
            leftAccesoryView.Layer.BorderWidth = 1;
            leftAccesoryView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            leftAccesoryView.Layer.CornerRadius = 2;
        }

        public void LoadProductListViewCell(Product product) 
        {
            nameProductLabel.Text = product.Name;
            PriceProductLabel.Text = product.Price.ToString();
            sizeLabel.Text = product.Presentation;
            productImageView.SetImage(new NSUrl(product.UrlMediumImage));
        }

        public override void SetSelected(bool selected, bool animated)
        {
            if(selected)
            {
                contentView.BackgroundColor = ConstantColor.UiPrimary;
                leftAccesoryView.BackgroundColor = ConstantColor.UiPrimary;
            }
            else
            {
                contentView.BackgroundColor = UIColor.White;
                leftAccesoryView.BackgroundColor = UIColor.White;
            }
        }
    }
}

