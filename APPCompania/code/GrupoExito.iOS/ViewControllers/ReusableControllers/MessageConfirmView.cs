using System;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class MessageConfirmView : UIView
    {
        #region Properties 
        public UIButton Negation{
            get { return noButton; }
        }

        public UIButton Afirmation{
            get { return yesButton; }
        }

        public UILabel Title{
            get { return titleWaitLabel; }
        }

        public UILabel Message{
            get { return messageLabel; }
        }
        public UIButton Close
        {
            get { return closeViewButton; }
        }


        #endregion

        #region Constructors 
        static MessageConfirmView()
        {
            //Static default constructor this class
        }

        protected MessageConfirmView(IntPtr handle) : base(handle)
        {
            //default constructor this class
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
            noButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            yesButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            yesButton.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            yesButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
        }

        private void LoadColors(){
            noButton.BackgroundColor = ConstantColor.UiPrimary;
        }
        #endregion
    }
}

