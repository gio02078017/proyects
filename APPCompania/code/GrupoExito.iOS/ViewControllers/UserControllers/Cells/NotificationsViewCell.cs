using System;
using Foundation;
using GrupoExito.Entities;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    public partial class NotificationsViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("NotificationsViewCell");
        public static readonly UINib Nib;

        #region Constructors 
        static NotificationsViewCell()
        {
            Nib = UINib.FromName("NotificationsViewCell", NSBundle.MainBundle);
        }

        protected NotificationsViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Methods 
        public void LoadData(AppNotification appNotification)
        {
            appNotification.Text = notificationsLabel.Text;
        }

        public void Ingredients(string data)
        {
            notificationsLabel.Text = "*  " + data;
        }

        public void Preparations(string data, NSIndexPath indexPath)
        {
            int productsCount = indexPath.Row;
            productsCount++;
            notificationsLabel.Text = productsCount + ".  " + data;
        }
        #endregion
    }
}
