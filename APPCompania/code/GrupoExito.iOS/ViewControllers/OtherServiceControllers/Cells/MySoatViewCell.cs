using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers.Cells
{
    public partial class MySoatViewCell : UITableViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("MySoatViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Constructors 
        static MySoatViewCell()
        {
            Nib = UINib.FromName("MySoatViewCell", NSBundle.MainBundle);
        }

        protected MySoatViewCell(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Overrides Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            LoadCorners();
            Util.CreateShadowLayer(ContentView.Subviews[0], 4.0f, 0.5f);

        }

        #endregion

        #region Private Methods
        private void LoadCorners()
        {
            this.ContentView.Subviews[0].Layer.CornerRadius = ConstantStyle.CornerRadius;
        }
        #endregion

        #region Public Methods
        public void setTitle(string value)
        {
            titleLabel.Text = value;
        }
        #endregion


    }
}
