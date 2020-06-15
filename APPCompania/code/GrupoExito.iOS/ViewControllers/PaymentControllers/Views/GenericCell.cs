using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public abstract class GenericCell : UITableViewCell
    {
        protected GenericCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public abstract void Setup(object v);
    }
}
