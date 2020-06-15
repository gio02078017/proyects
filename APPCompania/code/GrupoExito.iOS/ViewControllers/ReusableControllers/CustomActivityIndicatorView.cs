using System;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class CustomActivityIndicatorView : UIView
    {
        #region Attributes
        private string codeMesage = string.Empty;
        #endregion

        #region Properties
        public UIImageView Image
        {
            get { return spinnerImageImageView; }
        }

        public UIImageView ProductAdding
        {
            get { return productAddingImageView; }
        }

        public UIView ProductAddingV
        {
            get { return productAddingView; }
        }

        public UILabel Message
        {
            get { return spinnerMessageLabel; }
        }

        public UIButton Retry
        {
            get { return retryButton; }
        }

        public string CodeMesage { get => codeMesage; set => codeMesage = value; }
        #endregion

        #region Constructors
        protected CustomActivityIndicatorView(IntPtr handle) : base(handle) 
        {
            //Default constructor with argument this class 
        }

        public CustomActivityIndicatorView() 
        {
            //Default constructor without argument this class 
        }
        #endregion

        #region Overrides Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadCorners();
            this.LoadColors();
        }
        #endregion

        #region Methods
        public void LoadImage(string nameImage)
        {
            try
            {
                spinnerImageImageView.Image = UIImage.FromFile(nameImage);
            }
            catch (Exception exception)
            {
                var error = exception.Message;
            }
        }

        public void LoadAnimationImages(string nameFolderAnimation, int count, double duration)
        {
            try
            {
                UIImage[] images = Util.LoadAnimationImage(nameFolderAnimation, count);
                spinnerImageImageView.Image = images[0];
                spinnerImageImageView.AnimationImages = images;
                spinnerImageImageView.AnimationDuration = duration;
            }
            catch (Exception exception)
            {
                var error = exception.Message;
            }
        }

        public void LoadAnimationImagesWithReverse(string nameFolderAnimation, int count)
        {
            try
            {
                UIImage[] images = Util.LoadAnimationImageWithReverse(nameFolderAnimation, count);
                spinnerImageImageView.Image = images[0];
                spinnerImageImageView.AnimationImages = images;
                spinnerImageImageView.AnimationDuration = 2;
            }
            catch (Exception exception)
            {
                var error = exception.Message;
            }
        }

        public void LoadProductAdding(Product product)
        {
            productAddingView.Hidden = false;
            productAddingImageView.Hidden = false;
            productAddingView.Layer.CornerRadius = productAddingView.Frame.Width / 2;
            productAddingImageView.Layer.CornerRadius = productAddingImageView.Frame.Width / 2;
            productAddingImageView.Layer.MasksToBounds = true;
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.UrlMediumImage))
                {
                    productAddingImageView.SetImage(new NSUrl(product.UrlMediumImage));
                }
            }
        }

        private void LoadCorners()
        {
            this.containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            retryButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadColors()
        {
            this.retryButton.BackgroundColor = ConstantColor.UiPrimary;
        }
        #endregion
    }
}

