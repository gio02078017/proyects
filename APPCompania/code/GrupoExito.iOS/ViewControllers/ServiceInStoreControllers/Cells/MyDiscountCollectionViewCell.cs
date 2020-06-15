using System;
using CoreAnimation;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Resources;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Cells
{
    public partial class MyDiscountCollectionViewCell : UICollectionViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("MyDiscountCollectionViewCell");
        public static readonly UINib Nib;

        private EventHandler activeAction;
        private EventHandler desactiveAction;
        private EventHandler legalAction;
        private Discount discount;
        private bool isRedeemed;
        #endregion

        #region Properties 
        public UIButton Active { get => activeButton; }
        public EventHandler ActiveAction { get => activeAction; set => activeAction = value; }
        public EventHandler DesactiveAction { get => desactiveAction; set => desactiveAction = value; }
        public EventHandler LegalAction { get => legalAction; set => legalAction = value; }
        public Discount Discount { get => discount; set => discount = value; }

        public UIView ContainerAllViews { get => containerAllView; }
        public UILabel Plu { get => pluNumber; }

        #endregion

        #region Constructors
        static MyDiscountCollectionViewCell()
        {
            Nib = UINib.FromName("MyDiscountCollectionViewCell", NSBundle.MainBundle);
        }

        protected MyDiscountCollectionViewCell(IntPtr handle) : base(handle)
        {
            //Static default constructor this class
        }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadCorners();
            this.LoadHandlers();
            this.LoadColors();
        }
        #endregion

        #region Public Methods 
        public void LoadDiscountViewCell(Discount discount, bool previewCampaign)
        {
            this.Discount = discount;
            if (Discount != null)
            {
                LoadInitialControls();
                if (!string.IsNullOrEmpty(discount.Image))
                {
                    try
                    {
                        iconImageView.SetImage(new NSUrl(discount.Image), UIImage.FromFile(ConstantImages.SinImagenDescuento));
                    }
                    catch
                    {
                        iconImageView.Image = UIImage.FromFile(ConstantImages.SinImagenDescuento);
                    }
                }
                else
                {
                    iconImageView.Image = UIImage.FromFile(ConstantImages.SinImagenDescuento);
                }
                pluDescriptionLabel.Text = discount.Description;
                eventModeLabel.Text = discount.EventMode;
                if (!string.IsNullOrEmpty(discount.Plu) && discount.Plu != "0")
                {
                    pluNumber.Text = "PLU: {0}";
                    pluNumber.Text = string.Format(pluNumber.Text, discount.Plu);
                    pluNumber.Hidden = false;
                }
                else
                {
                    pluNumber.Hidden = true;
                }
                legalLabel.Hidden = true;
                if (discount.Future)
                {
                    activeButton.Enabled = false;
                    activeButton.BackgroundColor = ConstantColor.UiBackgroundColorNotRedeemableDiscount;
                    activeButton.SetTitleColor(ConstantColor.UiTextNotRedeemableDiscount, UIControlState.Normal);
                    activeButton.SetTitle(AppMessages.Next, UIControlState.Normal);
                }
                else if (discount.Redeemable)
                {
                    activeButton.Enabled = true;
                    if (!discount.Active)
                    {
                        activeButton.BackgroundColor = ConstantColor.UiColorBackgroundDiscountActivate;
                        activeButton.SetTitle(AppMessages.Activate, UIControlState.Normal);
                        activeButton.SetTitleColor(ConstantColor.UiColorTextDiscountActivate, UIControlState.Normal);
                    }
                    else
                    {
                        regionLabel.Superview.BackgroundColor = ConstantColor.UiColorBackgroundDiscountActivate;
                        if (discount.DaysRedeemLeft > 0)
                        {
                            regionLabel.Text = AppMessages.LeftDays + " " + discount.DaysRedeemLeft + " " + AppMessages.Days;
                        }
                        else
                        {
                            regionLabel.Text = AppMessages.ExpiresToday;
                        }
                        activeButton.SetTitle(AppMessages.Desactivate, UIControlState.Normal);
                        activeButton.BackgroundColor = UIColor.Clear;
                        activeButton.SetTitleColor(ConstantColor.UiColorTextDiscountActivated, UIControlState.Normal);
                    }
                }
                else
                {
                    activeButton.Enabled = false;
                    if (!discount.Active)
                    {
                        activeButton.BackgroundColor = ConstantColor.UiBackgroundColorNotRedeemableDiscount;
                        activeButton.Layer.BorderColor = ConstantColor.UiBackgroundColorNotRedeemableDiscount.CGColor;
                        activeButton.SetTitle(AppMessages.Activate, UIControlState.Normal);
                        activeButton.SetTitleColor(ConstantColor.UiTextNotRedeemableDiscount, UIControlState.Normal);
                    }
                    else
                    {
                        if (isRedeemed)
                        {
                            pluDescriptionLabel.TextColor = ConstantColor.UiBackgroundColorNotRedeemableDiscount;
                            eventModeLabel.TextColor = ConstantColor.UiBackgroundColorNotRedeemableDiscount;
                            eventModeLabel.Lines = 4;
                            pluNumber.TextColor = ConstantColor.UiBackgroundColorNotRedeemableDiscount;

                            activeButton.BackgroundColor = ConstantColor.UiBackgroundColorNotRedeemableDiscount;
                            activeButton.Layer.BorderColor = ConstantColor.UiBackgroundColorNotRedeemableDiscount.CGColor;
                            activeButton.SetTitle(AppMessages.Redeemed, UIControlState.Normal);
                            activeButton.SetTitleColor(ConstantColor.UiTextNotRedeemableDiscount, UIControlState.Normal);

                            legalButton.Hidden = true;
                        }
                        else
                        {
                            eventModeLabel.TextColor = ConstantColor.UiColorBackgroundDiscountActivate;
                            eventModeLabel.Lines = 4;
                            pluDescriptionLabel.TextColor = UIColor.DarkGray;
                            pluNumber.TextColor = UIColor.DarkGray;
                            activeButton.Hidden = false;
                            legalButton.Hidden = false;
                            activeButton.SetTitleColor(ConstantColor.UiColorBackgroundDiscountActivate, UIControlState.Normal);
                            activeButton.Enabled = false;
                            activeButton.BackgroundColor = UIColor.Clear;
                            if (discount.DaysRedeemLeft > 0)
                            {
                                activeButton.SetTitle(AppMessages.LeftDays + " " + discount.DaysRedeemLeft + " " + AppMessages.Days, UIControlState.Normal);
                            }
                            else
                            {
                                activeButton.SetTitle(AppMessages.ExpiresToday, UIControlState.Normal);
                            }
                        }
                    }
                }
            }
        }

        public void SetProportionalConstraint(nfloat multiplier)
        {
            NSLayoutConstraint constraint = NSLayoutConstraint.Create(proportionalWidthConstraint.FirstItem, proportionalWidthConstraint.FirstAttribute,
                                                                      proportionalWidthConstraint.Relation, proportionalWidthConstraint.SecondItem, proportionalWidthConstraint.SecondAttribute, 
                                                                       multiplier, proportionalWidthConstraint.Constant);
            constraint.Active = true;
            proportionalWidthConstraint = constraint;
            containerView.LayoutIfNeeded();
        }

        public void SetRedeemed(bool redeemed)
        {
            this.isRedeemed = redeemed;
        }
        #endregion

        #region private Methods
        private void LoadInitialControls()
        {
            eventModeLabel.TextColor = ConstantColor.UiColorBackgroundDiscountActivate;
            pluDescriptionLabel.TextColor = UIColor.Black;
            pluNumber.TextColor = UIColor.DarkGray;
            activeButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
            activeButton.Layer.BorderColor = ConstantColor.UiColorBackgroundDiscountActivate.CGColor;
            activeButton.Hidden = false;
            legalButton.Hidden = false;
            regionLabel.Superview.Layer.CornerRadius = ConstantStyle.CornerRadius;
            regionLabel.Superview.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner;
            regionLabel.Superview.ClipsToBounds = true;
            regionLabel.Superview.BackgroundColor = UIColor.White;
        }

        private void LoadCorners()
        {
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            activeButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            Util.CreateShadowLayer(containerView, 5.0f, 0.5f);
        }

        private void LoadHandlers()
        {
            activeButton.TouchUpInside += ActiveButtonTouchUpInside;
            legalButton.TouchUpInside += LegalButtonTouchUpInside;
        }

        private void LoadColors()
        {
            this.activeButton.BackgroundColor = ConstantColor.UiDiscountButtonToActivated;
        }
        #endregion

        #region Events
        private void ActiveButtonTouchUpInside(object sender, EventArgs e)
        {
            if (activeButton.Title(UIControlState.Normal).Equals(AppMessages.Activate))
            {
                ActiveAction?.Invoke(Discount, null);
            }
            else
            {
                DesactiveAction?.Invoke(Discount, null);
            }
        }

        private void LegalButtonTouchUpInside(object sender, EventArgs e)
        {
            LegalAction?.Invoke(Discount, null);
        }
        #endregion
    }
}
