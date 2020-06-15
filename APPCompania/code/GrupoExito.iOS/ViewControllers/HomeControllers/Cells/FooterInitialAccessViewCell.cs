using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Cells
{
    public partial class FooterInitialAccessViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("FooterInitialAccessViewCell");
        public static readonly UINib Nib;

        static FooterInitialAccessViewCell()
        {
            Nib = UINib.FromName("FooterInitialAccessViewCell", NSBundle.MainBundle);
        }

        protected FooterInitialAccessViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
