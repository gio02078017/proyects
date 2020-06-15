using System;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views
{
    public partial class PopPupShiftlnBoxView : UIView
    {
        #region Attributes
        private ShiftInBoxView _shiftInBoxView;
        #endregion

        #region Properties 
        public UIButton refuse
        {
            get { return refusedButton; }
        }

        public UIButton accept
        {
            get { return acceptButton; }
        }

        #endregion

        #region Constructors
        static PopPupShiftlnBoxView()
        {
        }

        protected PopPupShiftlnBoxView(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods

        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            LoadCorners();
            LoadHandlers();
        }

        #endregion

        #region Methods 
        private void LoadCorners()
        {
            acceptButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            refusedButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            refusedButton.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            refusedButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
        }

        private void LoadHandlers()
        {
        }

        private void LoadExternalViews()
        {
            _shiftInBoxView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.ShiftInBoxView, Self, null).GetItem<ShiftInBoxView>(0);
            CGRect shiftInBoxFrame = new CGRect(0, 0, shiftInBoxView.Frame.Size.Width, shiftInBoxView.Frame.Size.Height);
            _shiftInBoxView.Frame = shiftInBoxFrame;
            shiftInBoxView.AddSubview(_shiftInBoxView);
        }

        public void LoadData(StatusCashDrawerTurnResponse data)
        {
            this.LoadExternalViews();
            _shiftInBoxView.LoadData(data);
        }
        #endregion
    }
}

