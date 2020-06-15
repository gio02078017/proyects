using System;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Models;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class CashOnDeliveryView : UIView
    {
        #region Attributes
        private Action<int> selectedOptionHandler;
        private EnumPaymentType paymentType;
        #endregion

        #region Properties 
        public UITextField CashAmountTextField
        {
            get { return cashAmountTextField; }
        }

        public UIView HowMuchCashView
        {
            get { return howMuchCashView; }
        }
        public UILabel DateLabel
        {
            get { return dateLabel; }
        }
        public UILabel ScheduleLabel
        {
            get { return scheduleLabel; }
        }

        public Action<int> SelectedOptionHandler { get => selectedOptionHandler; set => selectedOptionHandler = value; }
        public EnumPaymentType PaymentType { get => paymentType; set => paymentType = value; }
        #endregion

        #region Constructors
        public CashOnDeliveryView(IntPtr handle) : base(handle)
        {
            //Default constructor this class with argument
        }
        #endregion

        #region Methods
        public void LoadTypeOfPaymentView()
        {
            typeOfPaymentView.LayoutIfNeeded();
            ProductUnavailableCollectionViewCell productUnavailableCollectionViewCell_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.ProductUnavailableCollectionViewCell, Self, null).GetItem<ProductUnavailableCollectionViewCell>(0);
            CGRect productUnavailableFrame = typeOfPaymentView.Frame;
            productUnavailableFrame = new CGRect(0, 0, typeOfPaymentView.Frame.Size.Width, typeOfPaymentView.Frame.Size.Height);
            productUnavailableCollectionViewCell_.Frame = productUnavailableFrame;
            productUnavailableCollectionViewCell_.Layer.CornerRadius = ConstantStyle.CornerRadius;
            typeOfPaymentView.AddSubview(productUnavailableCollectionViewCell_);

            TwoCellSelectorModel cellSelectorModel = new TwoCellSelectorModel()
            {
                Title = "",
                TitleFirstOption = "Usar datáfono",
                ValueFirstOption = "Paga con tarjeta débito o crédito",
                TitleSecondOption = "Efectivo",
                ValueSecondOption = "Entrega al domiciliario el valor total a pagar",
                BackgroundColor = ConstantColor.UiBackgroundSummarySelectorNotSelected,
                SelectedColor = ConstantColor.UiPrimary,
                UnselectedColor = ConstantColor.UiBackgroundSummarySelector,
                LeadingLength = 20
            };

            if (cellSelectorModel.SelectedColor == ConstantColor.UiPrimary)
            {
                productUnavailableCollectionViewCell_.TitleLabel.TextColor = UIColor.White;
                productUnavailableCollectionViewCell_.SubtitleLabel.TextColor = UIColor.White;
                productUnavailableCollectionViewCell_.NoReplaceTitleLabel.TextColor = UIColor.White;
                productUnavailableCollectionViewCell_.NoReplaceSubtitleLabel.TextColor = UIColor.White;

            } else if (cellSelectorModel.BackgroundColor == ConstantColor.UiBackgroundSummarySelectorNotSelected)
            {
                productUnavailableCollectionViewCell_.TitleLabel.TextColor = UIColor.Black;
                productUnavailableCollectionViewCell_.SubtitleLabel.TextColor = UIColor.Black;
                productUnavailableCollectionViewCell_.NoReplaceTitleLabel.TextColor = UIColor.Black;
                productUnavailableCollectionViewCell_.NoReplaceSubtitleLabel.TextColor = UIColor.Black;
            }

            productUnavailableCollectionViewCell_.Configure(cellSelectorModel, DataphoneSelected, CashSelected);

            //howMuchCashView.Hidden = true;

            cashOnPaymentAdviceView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            cashOnPaymentAdviceView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            cashOnPaymentAdviceView.Layer.BorderColor = UIColor.Orange.CGColor;

            PaymentType = EnumPaymentType.Dataphone;
        }
        #endregion

        #region Events
        private void DataphoneSelected(Object sender, EventArgs e)
        {
            PaymentType = EnumPaymentType.Dataphone;
            SelectedOptionHandler?.Invoke((int)EnumPaymentType.Dataphone);
        }

        private void CashSelected(Object sender, EventArgs e)
        {
            PaymentType = EnumPaymentType.Cash;
            SelectedOptionHandler?.Invoke((int)EnumPaymentType.Cash);
        }
        #endregion
    }
}