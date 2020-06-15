using System;
using Foundation;
using GrupoExito.Models.ViewModels.Payments;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class SubtotalView : UIView
    {
        private SubtotalViewModel viewModel;
        public SubtotalView (IntPtr handle) : base (handle)
        {
        }

        public static SubtotalView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(SubtotalView), null, null);
            var v = Runtime.GetNSObject<SubtotalView>(arr.ValueAt(0));

            return v;
        }

        public void Setup(object v)
        {
            if (v is SubtotalViewModel model)
            {
                viewModel = model;

                subtotalLabel.Text = viewModel.Subtotal;
                bagTaxLabel.Text = viewModel.BagTax;

                viewModel.PropertyChanged += ViewModel_PropertyChanged;;
                bagTaxButton.TouchUpInside += viewModel.SubtotalViewModel_BagTaxInfoHandler;
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            switch (propertyName)
            {
                case nameof(viewModel.Subtotal):
                    {
                        subtotalLabel.Text = viewModel.Subtotal;
                    }
                    break;
                case nameof(viewModel.BagTax):
                    {
                        bagTaxLabel.Text = viewModel.BagTax;
                    }
                    break;
            }
        }
    }
}