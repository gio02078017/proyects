using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class MenuServicesTableViewCell : UITableViewCell
    {
        #region Properties
        public UIImageView Image
        {
            get { return imageViewMenuItem; }
        }

        public UILabel Title
        {
            get { return labelTitle; }
        }

        public UILabel Descriptions
        {
            get { return labelDescription; }
        }

        public UILabel StatusNotifications
        {
            get { return labelStatusNotifications; }
        }
        #endregion

        #region Constructors
        static MenuServicesTableViewCell()
        {
            //Static default Constructor without parameters
        }

        protected MenuServicesTableViewCell(IntPtr handle) : base(handle)
        {
            //Static default Constructor with parameter
        }
        #endregion
    }
}
