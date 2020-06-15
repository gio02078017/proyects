using Foundation;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Logic.Models.Addresses;
using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class AddCorrespondenceAddressView : UIView
    {
        public Action<string, string, string> ContinueAction { get; set; }
        public Action ShowCitiesAction { get; set; }
        private string phone { get; set; }
        public City City { get; set; }
        private AddressModel addressModel;

        public AddCorrespondenceAddressView (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            Initialize();
            SetHandlers();
        }

        private void Initialize()
        {
            addressModel = new AddressModel(new AddressService(DeviceManager.Instance));
            continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
            continueButton.Enabled = false;
            contentView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            continueButton.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;
            descriptionLabel.Text = "Para continuar con tu proceso de compra debes agregar una dirección de facturación. " +
                "Esta puede ser diferente a tu dirección de domicilio.";

            phoneTextField.Text = ParametersManager.UserContext.CellPhone;

            if (!string.IsNullOrEmpty(ParametersManager.UserContext.CellPhone))
            {
                string validationResult = addressModel.ValidateFieldCellPhone(ParametersManager.UserContext.CellPhone);
                if (string.IsNullOrEmpty(validationResult))
                {
                    phoneTextField.RemoveFromSuperview();
                    phoneTitleLabel.RemoveFromSuperview();
                }
            }
        }

        private void SetHandlers()
        {
            continueButton.TouchUpInside += ((o, s) =>
            {
                ContinueAction?.Invoke(cityTextField.Text, addressTextField.Text, phone);
            });

            closeButton.TouchUpInside += ((o, s) =>
            {
                this.RemoveFromSuperview();
            });

            cityButton.TouchUpInside += ((o, s) =>
            {
                ShowCitiesAction?.Invoke();
            });

            if (phoneTextField != null)
            {
                phoneTextField.EditingChanged += ((o, s) =>
                {
                    ContinueButtonEnableValidation();
                });
            }

            addressTextField.EditingChanged += ((o, s) =>
            {
                ContinueButtonEnableValidation();
            });
        }

        private void ContinueButtonEnableValidation()
        {


            if (!cityTextField.Text.TrimEnd().TrimStart().Equals(string.Empty) &&
                   !addressTextField.Text.TrimStart().TrimEnd().Equals(string.Empty) &&
               IsPhoneSet())
            {
                string cellValidationResult = addressModel.ValidateFieldCellPhone(phoneTextField.Text);

                if (string.IsNullOrEmpty(cellValidationResult))
                {
                    UserAddress userAddress = new UserAddress
                    {
                        AddressComplete = addressTextField.Text,
                        CellPhone = phone,
                        CityId = (City != null) ? City.Id : string.Empty
                    };

                    string addressValidationResult = addressModel.ValidateFieldsAddressCorrespondence(userAddress);

                    if (string.IsNullOrEmpty(addressValidationResult))
                    {
                        continueButton.Enabled = true;
                        continueButton.Layer.BackgroundColor = ConstantColor.UiPrimary.CGColor;
                    }
                }
                else
                {
                    continueButton.Enabled = false;
                    continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
                }
            }
            else
            {
                continueButton.Enabled = false;
                continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
            }
        }

        private bool IsPhoneSet()
        {
            if (phoneTextField.Superview != null)
            {
                if(string.IsNullOrEmpty(phoneTextField.Text))
                {
                    return false;
                }
                else
                {
                    phone = phoneTextField.Text;
                    return true;
                }
            }
            else
            {
                phone = ParametersManager.UserContext.CellPhone;
                return true;
            }
        }

        public void SetCity(City city)
        {
            City = city;
            cityTextField.Text = City.Name;
        }
    }
}