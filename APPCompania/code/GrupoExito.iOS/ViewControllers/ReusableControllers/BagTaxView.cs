using System;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class BagTaxView : UIView
    {
        #region Constructors
        public BagTaxView (IntPtr handle) : base (handle)
        {
            //Default constructor this class
        }
        #endregion

        #region Overrides Methods
        public override void AwakeFromNib()
        {
            Initialize();
            LoadHandlers();
            LoadCorners();
            //LoadFonts();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            titleLabel.Text = AppMessages.BagTax;
            descriptionLabel.Text = AppMessages.BagTaxDisclaimer;
        }

        private void LoadCorners()
        {
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadHandlers()
        {
            acceptButton.TouchUpInside += ((o, s) =>
            {
                this.RemoveFromSuperview();
            });
        }

        private void LoadFonts()
        {
            titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle2Size);
            descriptionLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextBodySize);
            acceptButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle2Size);
        }
        #endregion
    }
}