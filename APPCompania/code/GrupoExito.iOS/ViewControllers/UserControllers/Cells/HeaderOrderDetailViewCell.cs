using System;
using Foundation;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    public partial class HeaderOrderDetailViewCell : UITableViewCell
    {
        #region Attributes 
        public static readonly NSString Key = new NSString("HeaderOrderDetailViewCell");
        public static readonly UINib Nib;
        private EventHandler selectAllAction;
        #endregion

        #region Properties
        public EventHandler SelectAllAction { get => selectAllAction; set => selectAllAction = value; }
        #endregion

        #region Constructors
        static HeaderOrderDetailViewCell()
        {
            Nib = UINib.FromName("HeaderOrderDetailViewCell", NSBundle.MainBundle);
        }

        protected HeaderOrderDetailViewCell(IntPtr handle) : base(handle)
        {
        }
        #endregion

        public void LoadOrder(OrderDetailResponse orderDetail)
        {
            DrawCheck();
            LoadHandlers();
            orderIdLabel.Text = orderDetail.OrderId;
            dateOrderLabel.Text = orderDetail.OrderDate;
            numberProductsLabel.Text = orderDetail.Products != null ? Convert.ToString(orderDetail.Products.Count) : string.Empty;
        }

        private void SetFonts()
        {
            orderIdTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersTitleSize);
            dateOrderTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle3Size);
            dateOrderLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle3Size);
            numberProductsTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle3Size);
            numberProductsLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle3Size);
            orderIdLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersTitleSize);
            selectAllTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersSubtitle2Size);
        }

        private void LoadHandlers()
        {
            selectAllButton.TouchUpInside -= SelectAllTouchUpInside;
            selectAllButton.TouchUpInside += SelectAllTouchUpInside;
        }

        private void DrawCheck()
        {
            UIImage image = UIImage.FromFile(ConstantImages.CheckboxUnselected);
            if (checkImageView.Image == image)
            {
                checkImageView.Image = UIImage.FromFile(ConstantImages.CheckBoxSelected);
            }
            else
            {
                checkImageView.Image = image;
            }
        }

        #region Events
        private void SelectAllTouchUpInside(object sender, EventArgs e)
        {
            DrawCheck();
            selectAllAction?.Invoke(sender, e);
        }
        #endregion
    }
}
