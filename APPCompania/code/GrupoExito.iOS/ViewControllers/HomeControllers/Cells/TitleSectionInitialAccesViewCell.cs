using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Cells
{
    public partial class TitleSectionInitialAccesViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("TitleSectionInitialAccesViewCell");
        public static readonly UINib Nib;

        static TitleSectionInitialAccesViewCell()
        {
            Nib = UINib.FromName("TitleSectionInitialAccesViewCell", NSBundle.MainBundle);
        }

        protected TitleSectionInitialAccesViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
