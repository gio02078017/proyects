using System;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class PopUpInformationView : UIView
    {
        #region Properties
        public Action AcceptButtonHandler { get; set; }
        public Action CloseButtonHandler { get; set; }
        #endregion

        #region Constructors
        public PopUpInformationView (IntPtr handle) : base (handle)
        {
        }

        public static PopUpInformationView Create(string title, string description)
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(PopUpInformationView), null, null);
            var v = Runtime.GetNSObject<PopUpInformationView>(arr.ValueAt(0));

            v.titleLabel.Text = title;
            v.descriptionLabel.Text = description;

            return v;
        }
        #endregion

        #region Overrides Methods
        public override void AwakeFromNib()
        {
            contentView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            acceptButton.BackgroundColor = ConstantColor.UiPrimary;
            acceptButton.SetTitleColor(ConstantColor.UiTextColorGeneric, UIControlState.Normal);
            closeButton.SetTitleColor(ConstantColor.UiTextColorGeneric, UIControlState.Normal);

            acceptButton.Layer.CornerRadius = 10;

            acceptButton.TouchUpInside += (sender, e) => AcceptButtonHandler?.Invoke();
            closeButton.TouchUpInside += (sender, e) => CloseButtonHandler?.Invoke();
            backgroundButton.TouchUpInside += (sender, e) => CloseButtonHandler?.Invoke();
        }
        #endregion

        #region Public Methods
        public void SetTitleAcceptButton(string title, UIControlState statate)
        {
            acceptButton.SetTitle(title, statate);
        }

        public void SetTitleCloseButton(string title, UIControlState statate)
        {
            closeButton.SetTitle(title, statate);
        }
        #endregion
    }
}