using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Views
{
    #region Enum
    public enum ShipmentType
    {
        ExpressDelivery,
        ScheduleDelivery
    }
    #endregion

    public partial class ShipmentTypeView : UIView
    {
        #region Attributes
        public Action TouchUpAction { get; set; }
        public bool State { get; set; }
        #endregion

        #region Constructors
        public ShipmentTypeView(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods

        public static ShipmentTypeView Create()
        {
            return NSBundle.MainBundle.LoadNib(nameof(ShipmentTypeView), null, null).GetItem<ShipmentTypeView>(0);
        }

        public override void AwakeFromNib()
        {
            Layer.CornerRadius = ConstantStyle.CornerRadius / 2;
            priceBackgroundView.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;

            button.TouchUpInside += (o, e) =>
            {
                //Select();
                TouchUpAction?.Invoke();
            };
        }
        #endregion

        #region Methods
        public void Configure(ShipmentType shipmentType, bool initialState, string promisePrice, int promiseTime)
        {
            if (initialState)
            {
                Select();
            }
            else
            {
                Unselect();
            }
            bool isDeliveryType = ParametersManager.UserContext.Address != null;
            if (!isDeliveryType)
            {
                priceBackgroundView.Hidden = true;
            }
            switch (shipmentType)
            {
                case ShipmentType.ExpressDelivery:
                    titleLabel.Text = AppMessages.ExpressText;
                    descriptionLabel.Text = string.Format(AppMessages.PromiseDeliveryText, promiseTime);
                    try
                    {
                        priceLabel.Text = StringFormat.ToPrice(decimal.Parse(promisePrice));
                    }
                    catch (Exception)
                    {
                        priceLabel.Text = promisePrice;
                    }
                    iconImageView.Image = UIImage.FromFile(ConstantImages.Express);
                    break;
                case ShipmentType.ScheduleDelivery:
                    titleLabel.Text = AppMessages.ScheduledDeliveryText;
                    descriptionLabel.Text = AppMessages.ScheduleDeliveryText;
                    priceLabel.Text = AppMessages.VariablePriceText;
                    iconImageView.Image = UIImage.FromFile(ConstantImages.Programada);
                    break;
            }

            if (ParametersManager.UserContext.Store != null)
            {
                priceLabel.Text = "Gratis";
            }
        }

        public void Unselect()
        {
            BackgroundColor = ConstantColor.UiBackgroundSummarySelector;
            priceBackgroundView.BackgroundColor = ConstantColor.UiBackgroundSchedulePriceOptionDeselected;
            titleLabel.TextColor = ConstantColor.DefaultDeselectedText;
            descriptionLabel.TextColor = ConstantColor.DefaultDeselectedText;
            priceLabel.TextColor = ConstantColor.DefaultDeselectedText;
        }

        public void Select()
        {
            BackgroundColor = ConstantColor.UiPrimary;
            priceBackgroundView.BackgroundColor = ConstantColor.UiBackgroundSchedulePriceOptionSelected;
            titleLabel.TextColor = ConstantColor.DefaultSelectedText;
            descriptionLabel.TextColor = ConstantColor.DefaultSelectedText;
            priceLabel.TextColor = ConstantColor.DefaultSelectedText;
        }
        #endregion
    }
}