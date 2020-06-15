using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class CreditCardHeaderCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("CreditCardHeaderCell");
        public static readonly UINib Nib;

        static CreditCardHeaderCell()
        {
            Nib = UINib.FromName("CreditCardHeaderCell", NSBundle.MainBundle);
        }

        protected CreditCardHeaderCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
