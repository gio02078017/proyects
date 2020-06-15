using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Cells
{
    public partial class FilterByViewCell : UITableViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("FilterByViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        static FilterByViewCell()
        {
            Nib = UINib.FromName("FilterByViewCell", NSBundle.MainBundle);
        }

        protected FilterByViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Public Methods
        public void SetData(string title, string value)
        {
            nameLabel.Text = title;
            quantityLabel.Text = value;
        }

        public UILabel GetTitle()
        {
            return nameLabel;
        }

        public UILabel GetQuantity()
        {
            return quantityLabel;
        }
        #endregion
    }
}
