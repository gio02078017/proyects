using System;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class AddressTableViewCell : UITableViewCell
    {
        #region Attributes
        private UserAddress defaultAddress;
        private Store defaultStore;
        #endregion

        #region Properties
        public UIImageView IconAddress
        {
            get { return iconAddressImageView; }
        }

        public UILabel TitleLabel
        {
            get { return addressTitleLabel; }
        }

        public UILabel Address
        {
            get { return addressLabel; }
        }

        public UIImageView DetailAddress
        {
            get { return detailAddressImageView; }
        }

        public UIButton EditAddress
        {
            get { return editAddressButton; }
        }

        public UserAddress DefaultAddress { get => defaultAddress; set => defaultAddress = value; }
        public Store DefaultStore { get => defaultStore; set => defaultStore = value; }
        #endregion

        #region Constructors
        static AddressTableViewCell() { }

        protected AddressTableViewCell(IntPtr handle) : base(handle) { }
        #endregion

        #region Overrides Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            //this.LoadFonts();
        }
        #endregion


        #region Methods
        public void LoadData(UserAddress address)
        {
            Address.Text = address.AddressComplete;
            if (address.IsDefaultAddress && ParametersManager.UserContext.Store == null)
            {
                BackgroundColor = ConstantColor.UiPrimary;
                Layer.CornerRadius = ConstantStyle.CornerRadius;
                LoadColorDefaultAddress();
            }
            else
            {
                BackgroundColor = ConstantColor.UiBackgroundMyAddressRowNotSelected;
            }
            if (address.Description != null)
            {
                TitleLabel.Text = address.Description;
                switch (address.Description)
                {
                    case ConstAddressType.Home:
                        if (BackgroundColor == ConstantColor.UiPrimary)
                        {
                            iconAddressImageView.Image = UIImage.FromFile(ConstantImages.casa_primario);
                        }
                        else
                        {
                            iconAddressImageView.Image = UIImage.FromFile(ConstantImages.Casa);
                        }
                        break;

                    case ConstAddressType.Office:
                        if (BackgroundColor == ConstantColor.UiPrimary)
                        {
                            iconAddressImageView.Image = UIImage.FromFile(ConstantImages.OficinaPrimario);
                        }
                        else
                        {
                            iconAddressImageView.Image = UIImage.FromFile(ConstantImages.Oficina);
                        }
                        break;
                    case ConstAddressType.Couple:
                        if (BackgroundColor == ConstantColor.UiPrimary)
                        {
                            iconAddressImageView.Image = UIImage.FromFile(ConstantImages.ParejaPrimario);
                        }
                        else
                        {
                            iconAddressImageView.Image = UIImage.FromFile(ConstantImages.Pareja);
                        }
                        break;
                    default:
                        TitleLabel.Text = ConstAddressType.Other;
                        if (BackgroundColor == ConstantColor.UiPrimary)
                        {
                            iconAddressImageView.Image = UIImage.FromFile(ConstantImages.OtroPrimario);
                        }
                        else
                        {
                            iconAddressImageView.Image = UIImage.FromFile(ConstantImages.Otro);
                        }
                        break;
                }
            }
            else
            {
                TitleLabel.Text = ConstAddressType.Home;
                iconAddressImageView.Image = UIImage.FromFile(ConstantImages.casa_primario);
            }
        }

        private void LoadFonts()
        {
            addressTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.AddressTitle);
            addressLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.Address);
        }

        private void LoadColorDefaultAddress()
        {
            addressTitleLabel.TextColor = UIColor.White;
            addressLabel.TextColor = UIColor.White;

        }
        #endregion
    }
}
