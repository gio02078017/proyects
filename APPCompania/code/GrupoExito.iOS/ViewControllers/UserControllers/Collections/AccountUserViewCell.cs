using System;
using Foundation;
using GrupoExito.Entities.Entiites;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Collections
{
    public partial class AccountUserViewCell : UICollectionViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("AccountUserViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Properties
        public UIStackView Container
        {
            get { return containerStackView; }
        }

        public UIView viewVertical
        {
            get { return iconView; }
        }

        public UIView viewHorizontal
        {
            get { return horizontalView; }
        }
        #endregion

        #region Constructors
        static AccountUserViewCell()
        {
            Nib = UINib.FromName("AccountUserViewCell", NSBundle.MainBundle);
        }

        protected AccountUserViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Methods
        public void LoadProfileIconViewCell(MenuItem menuItem)
        {
            iconProfileImageView.Image = UIImage.FromFile(menuItem.IconBlue);
            nameIconLabel.Text = menuItem.Title;
        }
        #endregion
    }
}
