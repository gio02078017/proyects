using UIKit;

namespace GrupoExito.iOS.Utilities.Constant
{
    public static class ConstantColor
    {
        public static UIColor UiPrimary { get => UIColor.FromRGB(141, 198, 63); }
        public static UIColor UiGrayBackground { get => UIColor.FromRGB(231, 231, 233); }
        public static UIColor DefaultText { get => UIColor.FromRGB(1, 1, 1); }
        public static UIColor DefaultSelectedText { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor DefaultDeselectedText { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor ButtonSelected { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor CashPaymentHint { get => UIColor.FromRGB(112, 95, 124); }
        public static UIColor UIBackgroundShimmer { get => UIColor.GroupTableViewBackgroundColor; }
        public static UIColor UiBackgroundFloatingFilter { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiPageControlDot { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiWelcomeContinueButton { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiMessageError { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiMessageSuccess { get => UIColor.FromRGB(51, 159, 79); }
        public static UIColor UiMessageStatusProduct { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiMessageErrorButton { get => UIColor.FromRGB(255, 0, 0); }
        public static UIColor UiBackgroundWaitData { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiBackgroundSkipLogin { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiBackgroundChangeProductStatus { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiLaterIncomeTitleButtonsNotSelected { get => UIColor.FromRGB(239, 118, 50);  }
        public static UIColor UiLaterIncomeTitleButtonsSelected { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiOrderFilterButton { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiPriceWhitDiscount { get => UIColor.FromRGB(255, 0, 0); }
        public static UIColor UiPriceWhitOutDiscount { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiPricePrevious { get => UIColor.LightGray; }
        public static UIColor UiFilterOrderTextSelected { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiFilterOrderTextNotSelected { get => UIColor.FromRGB(25, 25, 25); }
        public static UIColor UiFilterOrderButtonSelected { get => UIColor.FromRGB(25, 25, 25); }
        public static UIColor UiFilterOrderButtonNotSelected { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiBackgroundAddressTypeNotSelected { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiBackgroundAddressTypeSelected { get => UiPrimary; }
        public static UIColor UiValidError { get => UIColor.FromRGB(255, 0, 0); }
        public static UIColor UiValidSuccess { get => UIColor.FromRGB(51, 159, 79); }
        public static UIColor UiBackgroundMyAccountRowSelected { get => UIColor.FromRGB(255, 116, 48); }
        public static UIColor UiBackgroundMyAccountRowNotSelected { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiBackgroundMyAddressRowNotSelected { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiBorderHowDoYouLikeit { get => UIColor.FromRGB(170, 170, 170); }
        public static UIColor UiBackgroundHowDoYouLikeit { get => UIColor.FromRGB(235, 235, 241); }
        public static UIColor UiBorderMeansOfPayment { get => UIColor.FromRGB(255, 116, 48); }
        public static UIColor UiBackgroundOrderQuantity { get => UIColor.FromRGB(255, 116, 48); }
        public static UIColor UiMyOrderTextColor { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiBackgroundKeepBuying { get => UIColor.FromRGB(239, 124, 50); }
        public static UIColor UiBackgroundSummarySelector { get => UIColor.FromRGB(235, 235, 241); }
        public static UIColor UiBackgroundSchedulePriceOptionSelected { get => UIColor.FromRGB(54, 175, 0); }
        public static UIColor UiBackgroundSchedulePriceOptionDeselected { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiBackgroundSummarySelectorNotSelected { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiBorderRecipientInfo { get => UIColor.FromRGB(255, 180, 100); }
        public static UIColor UiHeaderOrderCurrent { get => UIColor.FromRGB(235, 235, 241); }
        public static UIColor UiBackgroundCheckStatusNotSelected { get => UIColor.FromRGB(255, 255, 255); }
        public static UIColor UiBackgroundCheckStatusCanceled { get => UIColor.FromRGB(235, 235, 241); }
        public static UIColor UiBorderCountStatus { get => UIColor.FromRGB(0, 0, 0); }
        public static UIColor UiBorderTurnStatus { get => UIColor.FromRGB(255, 116, 48); }
        public static UIColor UiBorderCreditCardCanceledMessage { get => UIColor.FromRGB(255, 116, 48); }
        public static UIColor UiBackgroundColorNotRedeemableDiscount { get => UIColor.FromRGB(201, 200, 200);  }
        public static UIColor UiTextNotRedeemableDiscount { get => UIColor.FromRGB(126, 126, 126); }
        public static UIColor UiColorTextDiscountActivated { get => UIColor.FromRGB(0, 157, 206); }
        public static UIColor UiColorTextDiscountActivate { get => UIColor.FromRGB(255, 255, 255);  }
        public static UIColor UiColorBackgroundDiscountActivate { get => UiDiscountButtonToActivated; }
        public static UIColor UiTextColorGeneric { get => UIColor.White; }
        public static UIColor UiTextColorAddressDefault { get => UIColor.Black; }
        public static UIColor UiTextColorAddressTypeDescriptionLabelSelected { get => UIColor.Black; }
        public static UIColor UiTextColorAddressTypeDescriptionLabelNotSelected { get => UIColor.Black; }
        public static UIColor UiBorderColorGoBackWelcome { get => UIColor.White; }
        public static UIColor UIColorBackgroundCuponmania { get => UIColor.FromRGB(50, 44, 83); }
        public static UIColor UiBackgroundToastMake { get => UIColor.FromRGB(0, 157, 206); }
        public static UIColor UiBackgroundSummaryDefault { get => UIColor.FromRGB(112, 95, 124); }
        public static UIColor UiBackgroundSummaryAnimation { get => UIColor.FromRGB(141, 198, 63); }
        public static UIColor UiBorderColorButton { get => UIColor.FromRGB(81, 132, 162); }
        public static UIColor OrangeColor { get => UIColor.FromRGB(239, 124, 50); }
        public static UIColor UiDiscountButtonToActivated { get => UIColor.FromRGB(0, 157, 206); }
    }
}
