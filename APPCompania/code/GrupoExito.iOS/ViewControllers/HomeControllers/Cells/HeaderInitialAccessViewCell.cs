using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Cells
{
    public partial class HeaderInitialAccessViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("HeaderInitialAccessViewCell");
        public static readonly UINib Nib;

        static HeaderInitialAccessViewCell()
        {
            Nib = UINib.FromName("HeaderInitialAccessViewCell", NSBundle.MainBundle);
        }

        protected HeaderInitialAccessViewCell(IntPtr handle) : base(handle)
        {
        }
    }
}
