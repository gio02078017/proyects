using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Entiites;
using UIKit;

namespace GrupoExito.iOS
{
    public partial class AccessInitialViewCell : UICollectionViewCell
    {

        public static readonly NSString Key = new NSString("AccessInitialViewCell");
        public static readonly UINib Nib;    

        static AccessInitialViewCell()
        {
            Nib = UINib.FromName("AccessInitialViewCell", NSBundle.MainBundle);
        }

        public AccessInitialViewCell (IntPtr handle) : base (handle)
        {
        }

        public  void LoadData(IList<MenuItem> menuItems, NSIndexPath indexPath)
        {
            menuIconImageView.Image = UIImage.FromFile(menuItems[indexPath.Row].Icon);
            menuNameLabel.Text = menuItems[indexPath.Row].Title;
        }
    }
}