using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using ObjCRuntime;
using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class PaymentFooterTableView : UIView
    {
        public enum EnumDefaultCashTypeSelection
        {
            Cash,
            Dataphone
        }

        public static PaymentFooterTableView Create(EnumDefaultCashTypeSelection cashTypeSelection)
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(PaymentFooterTableView), null, null);
            var v = Runtime.GetNSObject<PaymentFooterTableView>(arr.ValueAt(0));

            v.cashButton.SetSelectedState(cashTypeSelection == EnumDefaultCashTypeSelection.Cash ? true : false);
            v.dataphoneButton.SetSelectedState(cashTypeSelection == EnumDefaultCashTypeSelection.Dataphone ? true : false);

            return v;
        }

        public override void AwakeFromNib()
        {
            cashButton.Layer.CornerRadius = 10;
            dataphoneButton.Layer.CornerRadius = 10;

            cashButton.TouchUpInside += CashButton_TouchUpInside;
            dataphoneButton.TouchUpInside += DataphoneButton_TouchUpInside;
        }

        public PaymentFooterTableView (IntPtr handle) : base (handle)
        {
        }

        void CashButton_TouchUpInside(object sender, EventArgs e)
        {
            dataphoneButton.Enabled = true;
            cashButton.Enabled = false;
        }

        void DataphoneButton_TouchUpInside(object sender, EventArgs e)
        {
            cashButton.Enabled = true;
            dataphoneButton.Enabled = false;
        }
    }
}