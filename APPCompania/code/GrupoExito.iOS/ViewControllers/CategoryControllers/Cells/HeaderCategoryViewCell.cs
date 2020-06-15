using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.CategoryControllers.Cells
{
    public partial class HeaderCategoryViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("HeaderCategoryViewCell");
        public static readonly UINib Nib;

        static HeaderCategoryViewCell()
        {
            Nib = UINib.FromName("HeaderCategoryViewCell", NSBundle.MainBundle);
        }

        protected HeaderCategoryViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
