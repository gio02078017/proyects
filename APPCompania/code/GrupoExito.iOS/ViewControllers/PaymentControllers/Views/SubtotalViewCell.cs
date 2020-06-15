using System;

using Foundation;
using GrupoExito.Models.ViewModels.Payments;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class SubtotalViewCell : GenericCell
    {
        public static readonly NSString Key = new NSString("SubtotalViewCell");
        public static readonly UINib Nib;
        private SubtotalViewModel viewModel;

        static SubtotalViewCell()
        {
            Nib = UINib.FromName("SubtotalViewCell", NSBundle.MainBundle);
        }

        protected SubtotalViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            infoButton.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
        }

        public override void Setup(object v)
        {
            if (v is SubtotalViewModel model)
            {
                viewModel = model;

                subtotalLabel.Text = viewModel.Subtotal;
                bagTaxLabel.Text = viewModel.BagTax;

                viewModel.PropertyChanged += ViewModel_PropertyChanged;
                infoButton.TouchUpInside += viewModel.SubtotalViewModel_BagTaxInfoHandler;
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

        public override void PrepareForReuse()
        {
            infoButton.TouchUpInside -= viewModel.SubtotalViewModel_BagTaxInfoHandler;
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            viewModel = null;
        }
    }
}
