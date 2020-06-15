using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers.Cells
{
    public partial class HeaderOtherServiceViewCell : UITableViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("HeaderOtherServiceViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Constructors
        static HeaderOtherServiceViewCell()
        {
            Nib = UINib.FromName("HeaderOtherServiceViewCell", NSBundle.MainBundle);
        }
        #endregion

        #region protected Methods 
        protected HeaderOtherServiceViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Public Methods
        public void setTitle(string value)
        {
            titleLabel.Text = value;
        }
        #endregion

    }
}
