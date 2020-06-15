using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    public partial class menuAccountItemViewCell : UITableViewCell
    {
        #region Properties 
        public UIImageView image{
            get { return imageViewMenuItem; }
        }

        public UILabel title{
            get { return labelTitle; }
        }

        public UILabel description{
            get { return labelDescription; }
        }

        public UILabel statusNotifications{
            get { return labelStatusNotifications; }
        }
        #endregion

        #region Constructors 
        static menuAccountItemViewCell()
        {
            //Static default constructor this class
        }

        protected menuAccountItemViewCell(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            LoadCorners();
        }
        #endregion

        #region Methods 
        private void LoadCorners(){
            statusNotifications.Layer.CornerRadius = statusNotifications.Frame.Width / 2;
        }
        #endregion
    }
}
