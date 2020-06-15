using System;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Models;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class ProductUnavailableCollectionViewCell : UICollectionViewCell
    {
        #region Attributes
        private TwoCellSelectorModel _model;
        #endregion

        #region Properties
        public UILabel TitleLabel
        {
            get { return replaceTitleLabel; }
        }

        public UILabel SubtitleLabel
        {
            get { return replaceSubtitleLabel; }
        }

        public UILabel NoReplaceTitleLabel
        {
            get { return noReplaceTitleLabel; }
        }

        public UILabel NoReplaceSubtitleLabel
        {
            get { return noReplaceSubtitleLabel; }
        }

        public UIView NotReplaceView
        {
            get { return NoReplaceView; }
        }

        public UIView ReplaceView
        {
            get { return replaceView; }
        }

        #endregion

        #region Constructors
        static ProductUnavailableCollectionViewCell() 
        {
           //Static default Constructor this class
        }
        protected ProductUnavailableCollectionViewCell(IntPtr handle) : base(handle) 
        {
            //Default Constructor this class 
        }
        #endregion

        #region methods
        public void Configure(TwoCellSelectorModel model, EventHandler firstOptionEventHandler, EventHandler secondOptionEventHandler)
        {
            _model = model;
            replaceButton.TouchUpInside += firstOptionEventHandler;
            noReplaceButton.TouchUpInside += secondOptionEventHandler;
            replaceCheckImageView.Layer.CornerRadius = replaceCheckImageView.Frame.Size.Width / 2;
            replaceCheckImageView.ClipsToBounds = true;
            replaceCheckImageView.Layer.BorderWidth = ConstantStyle.CircumferenceBorderWidth;
            replaceCheckImageView.Layer.BorderColor = _model.BackgroundColor.CGColor;
            replaceInnerView.Layer.CornerRadius = ConstantStyle.CornerRadiusTextField;
            noReplaceCheckImageView.Layer.CornerRadius = replaceCheckImageView.Frame.Size.Width / 2;
            noReplaceCheckImageView.ClipsToBounds = true;
            noReplaceCheckImageView.Layer.BorderWidth = ConstantStyle.CircumferenceBorderWidth;
            noReplaceCheckImageView.Layer.BorderColor = _model.BackgroundColor.CGColor;
            noReplaceCheckImageView.BackgroundColor = _model.UnselectedColor;
            noReplaceInnerView.Layer.CornerRadius = ConstantStyle.CornerRadiusTextField;;
            titleLabel.Text = model.Title;
            replaceTitleLabel.Text = model.TitleFirstOption;
            replaceSubtitleLabel.Text = model.ValueFirstOption;
            noReplaceTitleLabel.Text = model.TitleSecondOption;
            noReplaceSubtitleLabel.Text = model.ValueSecondOption;
            if (model.BackgroundColor != null) this.BackgroundColor = model.BackgroundColor;
            replaceInnerView.BackgroundColor = model.SelectedColor;
            noReplaceInnerView.BackgroundColor = model.UnselectedColor;
            if(model.LeadingLength >= 0)
            {
                replaceLeadingConstraint.Constant = model.LeadingLength;
                noReplaceLeadingConstraint.Constant = model.LeadingLength;
            }
            replaceButton.TouchUpInside += (o, s) => {
                replaceInnerView.BackgroundColor = _model.SelectedColor;
                noReplaceInnerView.BackgroundColor = _model.UnselectedColor;
                noReplaceCheckImageView.BackgroundColor = _model.UnselectedColor;
                replaceCheckImageView.Image = UIImage.FromBundle(ConstantImages.SeleccionarCirculo);
                //noReplaceCheckImageView.Image = UIImage.FromBundle(ConstantImages.Seleccionar);
                noReplaceCheckImageView.Image = null;
                noReplaceTitleLabel.TextColor = UIColor.DarkGray;
                noReplaceSubtitleLabel.TextColor = UIColor.DarkGray;
                replaceTitleLabel.TextColor = ConstantColor.UiTextColorGeneric;
                replaceSubtitleLabel.TextColor = ConstantColor.UiTextColorGeneric;
            };
            noReplaceButton.TouchUpInside += (o, s) => {
                noReplaceInnerView.BackgroundColor = _model.SelectedColor;
                replaceInnerView.BackgroundColor = _model.UnselectedColor;
                replaceCheckImageView.BackgroundColor = _model.UnselectedColor;
                noReplaceCheckImageView.Image = UIImage.FromBundle(ConstantImages.SeleccionarCirculo);
                //replaceCheckImageView.Image = UIImage.FromBundle(ConstantImages.Seleccionar);
                replaceCheckImageView.Image = null;
                replaceTitleLabel.TextColor = UIColor.DarkGray;
                replaceSubtitleLabel.TextColor = UIColor.DarkGray;
                noReplaceTitleLabel.TextColor = ConstantColor.UiTextColorGeneric;
                noReplaceSubtitleLabel.TextColor = ConstantColor.UiTextColorGeneric;
            };
        }
        #endregion
    }
}