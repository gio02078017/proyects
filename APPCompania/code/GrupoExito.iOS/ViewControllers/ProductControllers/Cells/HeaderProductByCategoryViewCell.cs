using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Cells
{
    public partial class HeaderProductByCategoryViewCell : UICollectionViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("HeaderProductByCategoryViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Properties

        #endregion

        #region Constructors
        static HeaderProductByCategoryViewCell()
        {
            Nib = UINib.FromName("HeaderProductByCategoryViewCell", NSBundle.MainBundle);
        }

        protected HeaderProductByCategoryViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion
    }
}
