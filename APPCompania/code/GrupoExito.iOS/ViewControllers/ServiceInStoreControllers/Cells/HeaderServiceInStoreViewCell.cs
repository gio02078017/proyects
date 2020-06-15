using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Cells
{
    public partial class HeaderServiceInStoreViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("HeaderServiceInStoreViewCell");
        public static readonly UINib Nib;

        static HeaderServiceInStoreViewCell()
        {
            Nib = UINib.FromName("HeaderServiceInStoreViewCell", NSBundle.MainBundle);
        }

        protected HeaderServiceInStoreViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
