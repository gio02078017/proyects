using System;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Views
{
    public partial class PrimeCommentaryView : UIView
    {
        #region Constructors
        public PrimeCommentaryView(IntPtr handle) : base(handle)
        {
            //Default Constructor this class
        }
        #endregion

        #region Override Methods
        public override void AwakeFromNib()
        {
            Initialize();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            contentView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            contentView.Layer.BorderColor = UIColor.Orange.CGColor;
            contentView.Layer.BorderWidth = 2;
            infoLabel.Text = "PRIME: Llevaremos tu mercado sin costo una vez el total de la compra después de seleccionar el medio de pago sea mayor o igual a $50.000";
            infoLabel.TextColor = UIColor.Orange;
            iconImageView.Image = UIImage.FromFile(ConstantImages.PrimeOrange);
        }
        #endregion
    }
}