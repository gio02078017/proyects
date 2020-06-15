using System;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class AddressTypeView : UIView
    {
        #region Attributes
        private string optionSelected = String.Empty;
        public string OptionSelected { get => optionSelected; set => optionSelected = value; }
        #endregion

        #region Constructors 
        static AddressTypeView()
        {
            //Default constructor without arguments
        }
        protected AddressTypeView(IntPtr handle) : base(handle)
        {
            //Default constructor with parameter
        }
        #endregion

        #region Override methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadCorners();
            //this.LoadFonts();
        }
        #endregion

        #region Methods 
        private void LoadCorners()
        {
            homeView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            officeView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            coupleView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            otherView.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadFonts()
        {
            homeLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.BodyGeneric);
            officeLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.BodyGeneric);
            coupleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.BodyGeneric);
            otherLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.BodyGeneric);
        }

        public void LoadImage(string name)
        {
            switch (name)
            {
                case ConstAddressType.Home:
                    homeButton_UpInside(null);
                    break;
                case ConstAddressType.Office:
                    officeButton_UpInside(null);
                    break;
                case ConstAddressType.Couple:
                    coupleButton_UpInside(null);
                    break;
                default:
                    otherButton_UpInside(null);
                    break;
            }
        }
        #endregion

        #region Events
        partial void homeButton_UpInside(Foundation.NSObject sender)
        {
            homeView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeSelected;
            officeView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            coupleView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            otherView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            OptionSelected = ConstAddressType.Home;

            homeImageView.Image = UIImage.FromFile(ConstantImages.casa_primario);
            officeImageView.Image = UIImage.FromFile(ConstantImages.Oficina);
            coupleImageView.Image = UIImage.FromFile(ConstantImages.Pareja);
            otherImageView.Image = UIImage.FromFile(ConstantImages.Otro);

            homeLabel.TextColor = UIColor.White;
            officeLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
            coupleLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
            otherLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
        }

        partial void officeButton_UpInside(Foundation.NSObject sender)
        {
            officeView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeSelected;
            homeView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            coupleView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            otherView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            OptionSelected = ConstAddressType.Office;

            homeImageView.Image = UIImage.FromFile(ConstantImages.Casa);
            officeImageView.Image = UIImage.FromFile(ConstantImages.OficinaPrimario);
            coupleImageView.Image = UIImage.FromFile(ConstantImages.Pareja);
            otherImageView.Image = UIImage.FromFile(ConstantImages.Otro);

            officeLabel.TextColor = UIColor.White;
            homeLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
            coupleLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
            otherLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
        }

        partial void coupleButton_UpInside(Foundation.NSObject sender)
        {
            coupleView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeSelected;
            officeView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            homeView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            otherView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            OptionSelected = ConstAddressType.Couple;

            homeImageView.Image = UIImage.FromFile(ConstantImages.Casa);
            officeImageView.Image = UIImage.FromFile(ConstantImages.Oficina);
            coupleImageView.Image = UIImage.FromFile(ConstantImages.ParejaPrimario);
            otherImageView.Image = UIImage.FromFile(ConstantImages.Otro);

            coupleLabel.TextColor = UIColor.White;
            homeLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
            officeLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
            otherLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
        }

        partial void otherButton_UpInside(Foundation.NSObject sender)
        {
            otherView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeSelected;
            officeView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            coupleView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            homeView.BackgroundColor = ConstantColor.UiBackgroundAddressTypeNotSelected;
            OptionSelected = ConstAddressType.Other;

            homeImageView.Image = UIImage.FromFile(ConstantImages.Casa);
            officeImageView.Image = UIImage.FromFile(ConstantImages.Oficina);
            coupleImageView.Image = UIImage.FromFile(ConstantImages.Pareja);
            otherImageView.Image = UIImage.FromFile(ConstantImages.OtroPrimario);


            otherLabel.TextColor = coupleLabel.TextColor = UIColor.White;
            homeLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
            officeLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;
            coupleLabel.TextColor = ConstantColor.UiTextColorAddressTypeDescriptionLabelNotSelected;

        }
        #endregion
    }
}
