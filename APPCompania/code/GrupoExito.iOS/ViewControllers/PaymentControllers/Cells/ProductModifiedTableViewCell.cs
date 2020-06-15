using System;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Helpers;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Cells
{
    public partial class ProductModifiedTableViewCell : UITableViewCell
    {
        #region Enum
        public enum SoldOutType
        {
            ProductDeleted,
            ProductChanged
        };
        #endregion

        #region Attributes 
        private static readonly NSString Key = new NSString(ConstantReusableViewName.ProductModifiedTableViewCell);
        private static readonly UINib Nib;
        private SoldOutType CellType { get; set; }
        #endregion

        #region Constructors
        static ProductModifiedTableViewCell()
        {
            Nib = UINib.FromName(ConstantReusableViewName.ProductModifiedTableViewCell, NSBundle.MainBundle);
        }

        protected ProductModifiedTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Methods
        public void Configure(SoldOutType type)
        {
            contentView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            switch (type)
            {
                case SoldOutType.ProductChanged:
                    iconImageView.RemoveFromSuperview();
                    break;
                case SoldOutType.ProductDeleted:
                    cantLabel.RemoveFromSuperview();
                    break;
            }
        }

        public void LoadData(SoldOut product)
        {
            if (CellType.Equals(SoldOutType.ProductDeleted))
            {
                iconImageView.Image = UIImage.FromFile(ConstantImages.Cancelar);
            }
            else
            {
                cantLabel.Text = StringFormat.ToQuantity(product.Quantity);
            }
            imageView.SetImage(new NSUrl(product.ImagePath), UIImage.FromFile(ConstantImages.SinImagen));
            cellTitleLabel.Text = product.Name;
            descriptionLabel.Text = StringFormat.ToQuantity(product.Quantity) + " " + product.Presentation;
        }
        #endregion
    }
}
