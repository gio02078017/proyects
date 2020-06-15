using System;
using GrupoExito.Models.ViewModels.Payments;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Cells
{
    public abstract class BasePaymentCell : UITableViewCell
    {
        public abstract void SetData(CreditCardViewModel viewModel);

        protected BasePaymentCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
