using System;
using UIKit;

namespace GrupoExito.iOS.Referentials
{
    public partial class BaseTableController : UITableViewController
    {
        CustomSpinnerViewController childSpinnerController;

        public BaseTableController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            childSpinnerController = new CustomSpinnerViewController();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void AddSpinnerController()
        {
            if (childSpinnerController == null) return;
            this.PresentViewController(childSpinnerController, false, null);
        }

        public void RemoveSpinnerController()
        {
            if (childSpinnerController == null) return;
            childSpinnerController.DismissViewController(false, null);
        }
    }
}

