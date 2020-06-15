using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Cells
{
    public partial class HeaderChangedOrderViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("HeaderChangedOrderViewCell");
        public static readonly UINib Nib;

        static HeaderChangedOrderViewCell()
        {
            Nib = UINib.FromName("HeaderChangedOrderViewCell", NSBundle.MainBundle);
        }

        protected HeaderChangedOrderViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
