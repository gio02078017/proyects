using System;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Helpers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class LeftCheckedTableViewCell : GenericCell
    {
        #region Properties
        public static readonly NSString Key = new NSString("LeftCheckedTableViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Constructors
        static LeftCheckedTableViewCell()
        {
            Nib = UINib.FromName("LeftCheckedTableViewCell", NSBundle.MainBundle);
        }

        protected LeftCheckedTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Override Methods
        public override void AwakeFromNib()
        {
            Initialize();
        }

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);
            if (selected)
            {
                innerContentView.BackgroundColor = ConstantColor.UiPrimary;
                checkIconImageView.Image = UIImage.FromFile(ConstantImages.SeleccionarCirculo);
                titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersBodySize);
                priceLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyOrdersBodySize);
                titleLabel.TextColor = UIColor.White;
                priceLabel.TextColor = UIColor.White;
            }
            else
            {
                checkIconImageView.Image = null;
                checkIconImageView.BackgroundColor = ConstantColor.UiGrayBackground;
                innerContentView.BackgroundColor = ConstantColor.UiBackgroundHowDoYouLikeit;
                titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                priceLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyOrdersBodySize);
                titleLabel.TextColor = UIColor.Black;
                priceLabel.TextColor = UIColor.Black;
            }
        }
        #endregion

        #region Methods
        public void Configure(ScheduleHours hour, bool isPrime)
        {
            titleLabel.Text = hour.StartHour + " - " + hour.EndHour;

            if (!string.IsNullOrEmpty(hour.ShippingCostValue))
            {
                try
                {
                    priceLabel.Text = StringFormat.ToPrice(Convert.ToDecimal(hour.ShippingCostValue));
                }
                catch (Exception)
                {
                    priceLabel.Text = hour.ShippingCostValue;
                }
            }
            else
            {
                priceLabel.Text = hour.ShippingCostValue;
            }

            descriptionLabel.RemoveFromSuperview();

            bool isDeliveryType = ParametersManager.UserContext.Address != null;

            //if (!isPrime || !isDeliveryType)
            //{
                //primeIconImageView.RemoveFromSuperview();
            //}

            if (!isDeliveryType)
            {
                priceLabel.RemoveFromSuperview();
                titleLabel.TextAlignment = UITextAlignment.Center;
            }
        }

        private void Initialize()
        {
            checkIconImageView.Layer.CornerRadius = checkIconImageView.Frame.Size.Width / 2;
            checkIconImageView.ClipsToBounds = true;
            checkIconImageView.Layer.BorderWidth = ConstantStyle.CircumferenceBorderWidth;
            checkIconImageView.Layer.BorderColor = ConstantColor.UiFilterOrderTextSelected.CGColor;
            innerContentView.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;
        }

        public override void Setup(object v)
        {
            if (v is ScheduleHourViewModel model)
            {
                titleLabel.Text = model.Hour.StartHour + " - " + model.Hour.EndHour;

                if (!string.IsNullOrEmpty(model.Hour.ShippingCostValue))
                {
                    try
                    {
                        priceLabel.Text = StringFormat.ToPrice(Convert.ToDecimal(model.Hour.ShippingCostValue));
                    }
                    catch (Exception)
                    {
                        priceLabel.Text = model.Hour.ShippingCostValue;
                    }
                }
                else
                {
                    priceLabel.Text = model.Hour.ShippingCostValue;
                }

                descriptionLabel.RemoveFromSuperview();

                bool isDeliveryType = ParametersManager.UserContext.Address != null;

                //primeIconImageView.RemoveFromSuperview();

                if (!isDeliveryType)
                {
                    priceLabel.RemoveFromSuperview();
                    titleLabel.TextAlignment = UITextAlignment.Center;
                }
            }
        }
        #endregion
    }
}
