using System;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class UnitWeightView : UIView
    {
        #region Properties 
        public UIButton Adds
        {
            get { return addButton; }
        }

        public UIButton Substraction
        {
            get { return substractionButton; }
        }

        public UILabel Count
        {
            get { return cantLabel; }
        }

        public UIButton Unit
        {
            get { return unitButton; }
        }

        public UIButton Weight
        {
            get { return weightButton; }
        }

        #endregion

        #region Constructor
        public UnitWeightView(IntPtr handle) : base(handle) 
        {
            //Default constructor this class
        }
        #endregion

        #region Override methods
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            this.LoadColors();
            this.LoadCorners();
        }
        #endregion

        #region Methods
        public void HiddenControls(){
            addButton.Hidden = true;
            substractionButton.Hidden = true;
            cantLabel.Hidden = true;
            unitButton.Hidden = true;
            weightButton.Hidden = true;
            unitOrnamentView.Hidden = true;
            weightOrnamentView.Hidden = true;
        }

        public void selectedUnit()
        {
            unitOrnamentView.Hidden = false;
            unitButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductDetailUnitWeight);
            weightOrnamentView.Hidden = true;
            weightButton.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.ProductDetailUnitWeight);
        }

        public void selectedWeight()
        {
            unitOrnamentView.Hidden = true;
            unitButton.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.ProductDetailUnitWeight);
            weightOrnamentView.Hidden = false;
            weightButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ProductDetailUnitWeight);
        }

        public void ShowUnit()
        {
            unitButton.Hidden = false;
            unitOrnamentView.Hidden = false;
            lineView.Hidden = false;
        }

        public void HiddenUnit()
        {
            unitButton.Hidden = true;
            unitOrnamentView.Hidden = true;
            lineView.Hidden = false;
        }

        public void ShowWeight()
        {
            weightButton.Hidden = false;
            weightOrnamentView.Hidden = false;
            lineView.Hidden = false;
        }

        public void HiddenWeight()
        {
            weightButton.Hidden = true;
            weightOrnamentView.Hidden = true;
            lineView.Hidden = false;
        }

        public void HiddenUnitWeight()
        {
            unitButton.Hidden = true;
            unitOrnamentView.Hidden = true;
            weightButton.Hidden = true;
            weightOrnamentView.Hidden = true;
            lineView.Hidden = true;
        }

        public void ShowUnitWeight()
        {
            unitButton.Hidden = false;
            unitOrnamentView.Hidden = false;
            weightButton.Hidden = false;
            weightOrnamentView.Hidden = false;
            lineView.Hidden = false;
        }

        private void LoadColors()
        {
            unitOrnamentView.BackgroundColor = ConstantColor.UiPrimary;
            weightOrnamentView.BackgroundColor = ConstantColor.UiPrimary;
        }

        private void LoadCorners()
        {
            this.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }
        #endregion
    }
}

