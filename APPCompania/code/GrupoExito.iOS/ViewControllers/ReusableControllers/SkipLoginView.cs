using System;
using Foundation;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class SkipLoginView : UIView
    {
        #region Attributes
        private SkipLoginView CurrentView;
        private UIViewControllerBase ControllerBase;
        #endregion

        #region Properties
        public UIView Content{
            get { return contentView; }
        }

        public UIButton ReturnLogin{
            get { return returnLoginButton; }
        }

        public UIButton SkipeLogin{
            get { return continueButton; }
        }
        #endregion

        #region Constructor
        public SkipLoginView(IntPtr handle) : base(handle)
        {
            //default Constructor this class
        }
        #endregion

        #region Overrides Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadCorners();
        }
        #endregion

        #region Methods 
        public void LoadView(SkipLoginView view, UIViewControllerBase controllerBase){
            this.CurrentView = view;
            this.ControllerBase = controllerBase;
        }

        private void LoadCorners(){
            contentView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            returnLoginButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }
        #endregion
    }
}