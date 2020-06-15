using System;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Models.ViewModels.Payments;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class DeliveryAddressCell : GenericCell
    {
        public static readonly NSString Key = new NSString("DeliveryAddressCell");
        public static readonly UINib Nib;
        private AddressViewModel viewModel;

        protected DeliveryAddressCell(IntPtr handle) : base(handle) { }

        static DeliveryAddressCell()
        {
            Nib = UINib.FromName("DeliveryAddressCell", NSBundle.MainBundle);
        }

        public override void AwakeFromNib()
        {
            CreateShadowLayer();
        }

        public override void Setup(object v)
        {
            if (v is AddressViewModel model)
            {
                this.viewModel = model;
                //NSAttributedString attrString = new NSAttributedString(model.AddressTitle, new UIStringAttributes {  UnderlineStyle = NSUnderlineStyle.Single });
                //titleLabel.AttributedText = attrString;
                titleLabel.Text = model.AddressTitle;
                addressLabel.Text = model.Address;
                if (!string.IsNullOrEmpty(model.Description))
                {
                    descriptionLabel.Hidden = false;
                    descriptionLabel.Text = model.Description;
                }
                else
                {
                    descriptionLabel.Hidden = true;
                }
                this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            switch (propertyName)
            {
                case nameof(viewModel.Address):
                    {
                        InvokeOnMainThread(() =>
                        {
                            addressLabel.Text = viewModel.Address;
                        });
                    }
                    break;
                case nameof(viewModel.AddressTitle):
                    {
                        InvokeOnMainThread(() =>
                        {
                            titleLabel.Text = viewModel.AddressTitle;
                        });
                    }
                    break;
            }
        }

        public override void SetSelected(bool selected, bool animated)
        {
            if(selected)viewModel.CellTouchUpInside(this, null);
        }

        private void CreateShadowLayer()
        {
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            containerView.Layer.BorderWidth = 1.0f;
            containerView.Layer.BorderColor = UIColor.Clear.CGColor;
            containerView.Layer.ShadowColor = UIColor.LightGray.CGColor;
            containerView.Layer.ShadowOffset = new CGSize(5, 5);
            containerView.Layer.ShadowRadius = 5.0f;
            containerView.Layer.ShadowOpacity = 0.5f;
            containerView.Layer.MasksToBounds = false;
        }
    }
}
