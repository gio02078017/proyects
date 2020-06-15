using System;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class MessageStatusView : UIView
    {
        #region Properties 
        public UIImageView ImageStatus
        {
            get { return imageStatusImageView; }
        }

        public UILabel Title
        {
            get { return titleLabel; }
        }

        public UILabel Message
        {
            get { return messageLabel; }
        }

        public UIButton Action
        {
            get { return actionButton; }
        }

        public UIButton Close
        {
            get { return closeButton; }
        }
        #endregion

        #region Constructors
        static MessageStatusView() 
        {
            //Static default constructor this class 
        }
        protected MessageStatusView(IntPtr handle) : base(handle) 
        {
            //Default constructor this class
        }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadCorners();
            this.LoadColors();
        }
        #endregion

        #region Methods
        private void LoadCorners()
        {
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            actionButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadColors(){
            actionButton.BackgroundColor = ConstantColor.UiPrimary;
        }
        #endregion
    }
}
