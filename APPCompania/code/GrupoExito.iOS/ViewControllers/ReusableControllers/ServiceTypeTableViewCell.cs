using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class ServiceTypeTableViewCell : UIView
    {
        #region Properties 
        public UITextField City
        {
            get { return cityTextField; }
        }

        public UITextField Address
        {
            get { return addressTextField; }
        }

        public UIActivityIndicatorView Spinner
        {
            get { return spinnerActivityIndicatorView; }
        }

        public UIButton CityActiveButton{
            get { return cityButton; }
        }

        public UIButton StoreActiveButton{
            get { return addressStoreButton; }
        }

        public UIButton ActionButton
        {
            get { return serviceTypeButton; }
        }

        public UIButton Acept{
            get { return aceptButton; }
        }

        public UIButton Cancel{
            get { return cancelButton; }
        }

        public UIView Container
        {
            get { return containerView; }
        }

        public UIActivityIndicatorView SpinnerActivityIndicator{
            get { return spinnerActivityIndicatorView; }
        }

        public UIImageView _spinnerActivityIndicatorView
        {
            get { return customSpinnerImageView; }
        }
        #endregion

        #region Constructors 
        static ServiceTypeTableViewCell()
        {
           //Static default Constructor this class
        }

        protected ServiceTypeTableViewCell(IntPtr handle) : base(handle)
        {
            //Default Constructor this class
        }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadFonts();
            this.LoadCorners();
            this.LoadColors();
        }
        #endregion

        #region Methods 
        public void LoadAnimationImages(string nameFolderAnimation, int count)
        {
            try
            {
                UIImage[] images = Util.LoadAnimationImage(nameFolderAnimation, count);
                customSpinnerImageView.Image = images[0];
                customSpinnerImageView.AnimationImages = images;
                customSpinnerImageView.AnimationDuration = 1;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceTypeTableViewCell, ConstantMethodName.LoadAnimationImages);
            }
        }

        private void LoadFonts()
        {
            try
            {
                titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyAddressTitle);
                cityLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.SubtitleGeneric);
                addressLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.SubtitleGeneric);
                cityTextField.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.CityTextField);
                addressTextField.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.AddressTextField);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceTypeTableViewCell, ConstantMethodName.LoadFonts);
            }
        }

        private void LoadCorners()
        {
            try
            {
                containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                aceptButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                cancelButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceTypeTableViewCell, ConstantMethodName.LoadCorners);
            }
        }

        private void LoadColors(){
            this.aceptButton.BackgroundColor = ConstantColor.UiPrimary;
        }
        #endregion
    }
}
