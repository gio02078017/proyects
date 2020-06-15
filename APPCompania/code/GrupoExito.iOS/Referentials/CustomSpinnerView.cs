using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.Referentials
{
    public partial class CustomSpinnerView : UIView
    {
        public CustomSpinnerView (IntPtr handle) : base (handle)
        {
        }

        public static CustomSpinnerView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(CustomSpinnerView), null, null);
            var v = Runtime.GetNSObject<CustomSpinnerView>(arr.ValueAt(0));

            return v;
        }

        public override void AwakeFromNib()
        {
            UIImage[] images = Util.LoadAnimationImage(ConstantImages.FolderSpinnerLoad, ConstantViewSize.FolderSpinnerLoadCount);
            spinnerImageView.Image = images[0];
            spinnerImageView.AnimationImages = images;
            spinnerImageView.AnimationDuration = ConstantDuration.AnimationImageLoading;

            tryAgainButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            tryAgainButton.BackgroundColor = ConstantColor.UiPrimary;
        }

        public void Start()
        {
            spinnerImageView.StartAnimating();
        }

        public void Stop()
        {
            spinnerImageView.StopAnimating();
        }
    }
}