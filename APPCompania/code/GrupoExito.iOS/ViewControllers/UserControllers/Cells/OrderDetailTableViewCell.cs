using System;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities.Constant;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    public partial class OrderDetailTableViewCell : UITableViewCell
    {
        #region Attributes
        private Product _product;
        #endregion

        #region Constructors
        static OrderDetailTableViewCell()
        {
            //Static default Constructor this class 
        }

        protected OrderDetailTableViewCell(IntPtr handle) : base(handle)
        {
            //Default Constructor this class 
        }
        #endregion

        #region Methods
        public void Configure(Product product)
        {
            _product = product;
            itemImageView.SetImage(new NSUrl(product.UrlMediumImage));
            itemTitleLabel.Text = product.Name;
            itemCantLabel.Text = Convert.ToString(product.Quantity) + " Unds.";
            itemPriceLabel.Text = product.SalePrice.ToString();
            howDoYouLikeItView.BackgroundColor = ConstantColor.UiBackgroundHowDoYouLikeit;
            howDoYouLikeItView.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;
            ChangeSelection(product.Selected);
            if (product.Note != null)
            {
                howDoYouLikeItView.Hidden = false;
                howDoYouLikeItTitleLabel.Text = "¿Cómo te gusta?";
                howDoYouLikeItValueLabel.Text = product.Note;
            }
            else
            {
                //howDoYouLikeItTitleLabel.Text = null;
                //howDoYouLikeItValueLabel.Text = null;
                //howDoYouLikeItView.Hidden = true;
                howDoYouLikeItView.RemoveFromSuperview();
            }
        }

        public void ChangeSelection(bool selected)
        {
            if (selected)
            {
                checkImageView.Image = UIImage.FromFile(ConstantImages.CheckBoxSelected);
            }
            else
            {
                checkImageView.Image = UIImage.FromFile(ConstantImages.CheckboxUnselected);
            }
        }
        #endregion
    }
}
