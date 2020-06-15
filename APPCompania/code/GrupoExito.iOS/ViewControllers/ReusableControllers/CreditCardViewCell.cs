using System;
using System.Globalization;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class CreditCardViewCell : UITableViewCell
    {
        #region Properties
        private EventHandler caduceCreditCard;
        private EventHandler activeCaduceWarningAction;
        private NSIndexPath indexPath;
        #endregion

        public static readonly NSString Key = new NSString("CreditCardViewCell");
        public static readonly UINib Nib;
        private CreditCard cards;
        public EventHandler CaduceCreditCard { get => caduceCreditCard; set => caduceCreditCard = value; }
        public EventHandler ActiveCaduceWarningAction { get => activeCaduceWarningAction; set => activeCaduceWarningAction = value; }
        public CreditCard Cards { get => cards; set => cards = value; }

        static CreditCardViewCell()
        {
            Nib = UINib.FromName("CreditCardViewCell", NSBundle.MainBundle);
        }

        #region Constructors 

        protected CreditCardViewCell(IntPtr handle) : base(handle)
        {
            //Default constructor with argument
        }
        #endregion

        #region Overrides Methods
        [Export("awakeFromNib")]
        public void AwakeFromNib()
        {
            this.CreateShadowLayer();
            this.LoadHandlers();

        }
        #endregion

        #region Methods
        private void LoadHandlers()
        {
            alertCaduceCreditButton.TouchUpInside += AlertCaduceCreditButtonTouchUpInside;
        }


        private void LoadCaduceLabels() 
        {
            infoCaduceCreditStackView.Hidden = false;
            cardTitleMessageLabel.Hidden = false;
            alertCaduceCreditButton.Hidden = true;
            alertCaduceCreditImageView.Hidden = true;
        }

        public void LoadData(CreditCard cards, NSIndexPath indexPath)
        {
            try
            {
                this.indexPath = indexPath;
                imageCardImageView.Image = UIImage.FromFile(cards.Image);
                nameCardLabel.Text = cards.Name;
                numberCardLabel.Text = cards.NumberCard.Substring(cards.NumberCard.Length - 4);

                if (cards.IsNextCaduced == true && !cards.Type.Equals(ConstCreditCardType.Exito))
                {
                    if (cards.Selected)
                    {
                        ShowExpirationView();
                        cardTitleMessageLabel.Text = String.Format(AppMessages.CreditCardCaducedMessage, cards.Image); cardDateCaducedLabel.Hidden = false;
                        string month;
                        try
                        {
                            month = DateTimeFormatInfo.CurrentInfo.GetMonthName(int.Parse(cards.ExpirationMonth));
                        }
                        catch (Exception exception)
                        {
                            month = cards.ExpirationMonth;
                            Util.LogException(exception, ConstantReusableViewName.CreditCardViewCell, ConstantMethodName.LoadData);
                        }
                        cardDateCaducedLabel.Text = String.Format(AppMessages.CreditCardDateMessage, month + " " + cards.ExpirationYear);
                    }
                    else
                    {
                        alertCaduceCreditButton.Hidden = false;
                        alertCaduceCreditImageView.Hidden = false;
                        infoCaduceCreditStackView.Hidden = true;
                        iconCaduceImageView.Hidden = true;
                        cardTitleMessageLabel.Hidden = true;
                        cardDateCaducedLabel.Hidden = true;
                    }
                }
                else
                {
                    HideExpirationView();
                }

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.CreditCardViewCell, ConstantMethodName.LoadData);
            }
        }

        internal void HideIconDetail(bool isFromCheckout)
        {
            detailCellImageView.Hidden = isFromCheckout;
        }

        private void CreateShadowLayer()
        {
            viewContent.Layer.CornerRadius = ConstantStyle.CornerRadius;
            viewContent.Layer.BorderWidth = 1.0f;
            viewContent.Layer.BorderColor = UIColor.Clear.CGColor;
            viewContent.Layer.ShadowColor = UIColor.LightGray.CGColor;
            viewContent.Layer.ShadowOffset = new CGSize(5, 5);
            viewContent.Layer.ShadowRadius = 5.0f;
            viewContent.Layer.ShadowOpacity = 0.5f;
            viewContent.Layer.MasksToBounds = false;
        }

        private void HideExpirationView()
        {
            alertCaduceCreditButton.Hidden = true;
            alertCaduceCreditImageView.Hidden = true;
            infoCaduceCreditStackView.RemoveFromSuperview();
        }

        private void ShowExpirationView()
        {
            alertCaduceCreditButton.Hidden = true;
            alertCaduceCreditImageView.Hidden = true;
            infoCaduceCreditStackView.Hidden = false;
            iconCaduceImageView.Hidden = false;
            cardTitleMessageLabel.Hidden = false;
            cardDateCaducedLabel.Hidden = false;
        }
        #endregion

        #region Events

        private void AlertCaduceCreditButtonTouchUpInside(object sender, EventArgs e)
        {
            LoadCaduceLabels();
            activeCaduceWarningAction?.Invoke(indexPath, null);
        }

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);
            if (selected)
            {
                viewContent.BackgroundColor = ConstantColor.UiPrimary;
                nameCardLabel.TextColor = ConstantColor.DefaultSelectedText;
                numberCardLabel.TextColor = ConstantColor.DefaultSelectedText;
                cardTitleMessageLabel.TextColor = ConstantColor.DefaultSelectedText;
                cardDateCaducedLabel.TextColor = ConstantColor.DefaultSelectedText;
            }
            else
            {
                viewContent.BackgroundColor = UIColor.White;
                nameCardLabel.TextColor = ConstantColor.DefaultText;
                numberCardLabel.TextColor = ConstantColor.DefaultText;
                cardTitleMessageLabel.TextColor = ConstantColor.DefaultText;
                cardDateCaducedLabel.TextColor = ConstantColor.DefaultText;
            }

        }
        #endregion

    }
}
