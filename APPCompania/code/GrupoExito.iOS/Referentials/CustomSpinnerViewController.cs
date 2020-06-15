using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.Referentials
{
    public partial class CustomSpinnerViewController : UIViewController
    {
        public CustomSpinnerViewController() : base("CustomSpinnerViewController", null)
        {
        }

        public override void LoadView()
        {
            base.LoadView();
            var arr = NSBundle.MainBundle.LoadNib(nameof(CustomSpinnerViewController), this, null);
            var v = Runtime.GetNSObject<UIView>(arr.ValueAt(0));

            View = v;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UIImage[] images = Util.LoadAnimationImage(ConstantImages.FolderSpinnerLoad, ConstantViewSize.FolderSpinnerLoadCount);
            imageView.Image = images[0];
            imageView.AnimationImages = images;
            imageView.AnimationDuration = ConstantDuration.AnimationImageLoading;

            actionButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            actionButton.BackgroundColor = ConstantColor.UiPrimary;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            imageView.StartAnimating();
            this.NavigationController?.SetNavigationBarHidden(true, false);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            imageView.StopAnimating();
            this.NavigationController?.SetNavigationBarHidden(false, false);
        }

        public void PopFromNavigationController()
        {
            this.NavigationController?.PopViewController(false);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

