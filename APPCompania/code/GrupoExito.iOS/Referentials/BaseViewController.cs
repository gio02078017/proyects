using UIKit;

namespace GrupoExito.iOS.Referentials
{
    public partial class BaseViewController : UIViewController
    {
        private UIActivityIndicatorView indicatorView;
        private CustomSpinnerViewController spinnerViewController;

        public BaseViewController() : base("BaseViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            spinnerViewController = new CustomSpinnerViewController();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void ShowSpinner()
        {
            if (spinnerViewController == null) return;

            AddChildViewController(spinnerViewController);
            View.AddSubview(spinnerViewController.View);
            spinnerViewController.DidMoveToParentViewController(this);
        }

        public void HideSpinner()
        {
            if (spinnerViewController == null) return;

            spinnerViewController.WillMoveToParentViewController(null);
            spinnerViewController.RemoveFromParentViewController();
            spinnerViewController.View.RemoveFromSuperview();
        }
    }
}

