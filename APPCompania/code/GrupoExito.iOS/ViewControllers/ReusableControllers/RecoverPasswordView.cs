using System;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class RecoverPasswordView : UIView
    {
        #region Properties
        public UIButton Send
        {
            get { return sendButton; }
        }

        public UIButton Close
        {
            get { return closeButton; }
        }

        public UITextField Email
        {
            get { return emailTextField; }
        }
        #endregion

        #region Constructors
        static RecoverPasswordView() 
        {
            //Static default Constructor this class 
        }
        protected RecoverPasswordView(IntPtr handle) : base(handle) 
        {
            //Default Constructor this class 
        }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadCorners();
            this.LoadHandlers();
            this.LoadColors();
        }
        #endregion

        #region Methods 
        private void LoadCorners()
        {
            this.containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            sendButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadHandlers()
        {
            emailTextField.ShouldReturn = (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
        }

        private void LoadColors(){
            this.sendButton.BackgroundColor = ConstantColor.UiPrimary;
        }
        #endregion 
    }
}
