using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Cells
{
    public partial class OrderByViewCell : UITableViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("OrderByViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        static OrderByViewCell()
        {
            Nib = UINib.FromName("OrderByViewCell", NSBundle.MainBundle);
        }

        protected OrderByViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Public Methods
        public void SetData(string value, bool status)
        {
            orderByLabel.Text = value;
            statusSwitch.On = status;
        }

        public void SetStatus(bool status)
        {
            statusSwitch.On = status;
        }

        public bool GetStatus()
        {
           return statusSwitch.On;
        }
        #endregion
    }
}
