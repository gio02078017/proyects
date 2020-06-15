using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Cells
{
    public partial class HeaderListViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("HeaderListViewCell");
        public static readonly UINib Nib;

        static HeaderListViewCell()
        {
            Nib = UINib.FromName("HeaderListViewCell", NSBundle.MainBundle);
        }

        protected HeaderListViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
