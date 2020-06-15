using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using ObjCRuntime;
using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class PopUpEnterTextView : UIView
    {
        public UIButton Button
        {
            get { return acceptButton; }
        }

        public Action<string> AcceptButtonHandler { get; set; }
        public Action CloseButtonHandler { get; set; }

        public PopUpEnterTextView (IntPtr handle) : base (handle)
        {
        }

        public static PopUpEnterTextView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(PopUpEnterTextView), null, null);
            var v = Runtime.GetNSObject<PopUpEnterTextView>(arr.ValueAt(0));

            return v;
        }

        public override void AwakeFromNib()
        {
            descriptionTextView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            contentView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            acceptButton.BackgroundColor = Utilities.Constant.ConstantColor.UiPrimary;
            acceptButton.Layer.CornerRadius = 10;
            acceptButton.TouchUpInside += (sender, e) => {
                AcceptButtonHandler?.Invoke(descriptionTextView.Text);
            };
            closeButton.TouchUpInside += (sender, e) => CloseButtonHandler?.Invoke();
            backgroundButton.TouchUpInside += (sender, e) => CloseButtonHandler?.Invoke();

            descriptionTextView.Delegate = this;
        }

        public void SetText(string title, string description)
        {
            titleLabel.Hidden = string.IsNullOrEmpty(title);
            titleLabel.Text = title;
            descriptionTextView.Text = description;
        }

        public void SetAsEditable(bool editable)
        {
            descriptionTextView.UserInteractionEnabled = editable;
            if(editable)
            {
                descriptionTextView.Layer.BorderColor = ConstantColor.UiBorderHowDoYouLikeit.CGColor;
                descriptionTextView.Layer.BorderWidth = 1.0f;
            }
        }
    }

    public partial class PopUpEnterTextView : IUITextViewDelegate
    {
        [Export("textView:shouldChangeTextInRange:replacementText:")]
        public virtual bool ShouldChangeText(UITextView textView, NSRange range, string text)
        {
            NSMutableAttributedString currentText = new NSMutableAttributedString(textView.Text);
            currentText.Replace(range, text);
            return currentText.Length <= 120;
        }
    }
}