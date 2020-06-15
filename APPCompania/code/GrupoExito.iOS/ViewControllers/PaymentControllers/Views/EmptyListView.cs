using Foundation;
using ObjCRuntime;
using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class EmptyListView : UIView
    {
        public Action AddProductsHandler { get; set; }
        public EmptyListView (IntPtr handle) : base (handle)
        {
        }

        public static EmptyListView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(EmptyListView), null, null);
            var v = Runtime.GetNSObject<EmptyListView>(arr.ValueAt(0));

            return v;
        }

        public override void AwakeFromNib()
        {
            addProductsButton.BackgroundColor = Utilities.Constant.ConstantColor.UiPrimary;
            addProductsButton.Layer.CornerRadius = 10;
            addProductsButton.TouchUpInside += (sender, e) => {
                RemoveFromSuperview();
                AddProductsHandler?.Invoke();
            };
        }
    }
}