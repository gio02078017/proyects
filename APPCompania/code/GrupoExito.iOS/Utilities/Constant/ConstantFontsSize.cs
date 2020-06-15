using System;
namespace GrupoExito.iOS.Utilities
{
    public static class ConstantFontSize
    {
        #region Font Names
        ////Nombre de las fuentes
        private static string letterTitle = "GillSans-Bold";
        public static string LetterTitle { get => letterTitle; set => letterTitle = value; }

        private static string letterSubtitle = "GillSans-SemiBold";
        public static string LetterSubtitle { get => letterSubtitle; set => letterSubtitle = value; }

        private static string letterBody = "GillSans-Light";
        public static string LetterBody { get => letterBody; set => letterBody = value; }       

        #endregion

        #region tam font home 
        private static nfloat userNameHome = 19f;
        public static nfloat UserNameHome { get => userNameHome; set => userNameHome = value; }

        private static nfloat homeDescriptionUser = 15f;
        public static nfloat HomeDescriptionUser { get => homeDescriptionUser; set => homeDescriptionUser = value; }
        #endregion

        #region tam generic font 
        private static nfloat titleGeneric = 17f;
        public static nfloat TitleGeneric { get => titleGeneric; set => titleGeneric = value; }

        private static nfloat subtitleGeneric = 15f;
        public static nfloat SubtitleGeneric { get => subtitleGeneric; set => subtitleGeneric = value; }

        private static nfloat bodyGeneric = 14f;
        public static nfloat BodyGeneric { get => bodyGeneric; set => bodyGeneric = value; }

        private static nfloat textTitleSize = 18f;
        public static nfloat TextTitleSize { get => textTitleSize; set => textTitleSize = value; }

        private static nfloat textSubtitle1Size = 17f;
        public static nfloat TextSubtitle1Size { get => textSubtitle1Size; set => textSubtitle1Size = value; }

        private static nfloat textSubtitle2Size = 16f;
        public static nfloat TextSubtitle2Size { get => textSubtitle2Size; set => textSubtitle2Size = value; }

        private static nfloat textBodySize = 15f;
        public static nfloat TextBodySize { get => textBodySize; set => textBodySize = value; }
        #endregion

        #region tam address Font 
        private static nfloat addressTextField = 13f;
        public static nfloat AddressTextField { get => addressTextField; set => addressTextField = value; }

        private static nfloat cityTextField = 13f;
        public static nfloat CityTextField { get => cityTextField; set => cityTextField = value; }
        #endregion

        #region tam welcome fonts 
        private static nfloat welcomeTitle = 24f;
        public static nfloat WelcomeTitle { get => welcomeTitle; set => welcomeTitle = value; }

        private static nfloat welcomeMessage = 16f;
        public static nfloat WelcomeMessage { get => welcomeMessage; set => welcomeMessage = value; }

        private static nfloat welcomeTitleServiceType = 16f;
        public static nfloat WelcomeTitleServiceType { get => welcomeTitleServiceType; set => welcomeTitleServiceType = value; }

        private static nfloat welcomeAutocompleteCell = 12f;
        public static nfloat WelcomeAutocompleteCell { get => welcomeAutocompleteCell; set => welcomeAutocompleteCell = value; }
        #endregion

        #region tam Filters fonts 
        private static nfloat filterHeader = 16f;
        public static nfloat FilterHeader { get => filterHeader; set => filterHeader = value; }

        private static nfloat filterTableRow = 15f;
        public static nfloat FilterTableRow { get => filterTableRow; set => filterTableRow = value; }
        #endregion

        #region tam Products fonts 
        private static nfloat productTitle = 18f;
        public static nfloat ProductTitle { get => productTitle; set => productTitle = value; }

        private static nfloat productSubtitle = 18f;
        public static nfloat ProductSubtitle { get => productSubtitle; set => productSubtitle = value; }

        private static nfloat productBody = 16f;
        public static nfloat ProductBody { get => productBody; set => productBody = value; }
        #endregion

        #region tam Categories fonts 
        private static nfloat categoryTitle = 18f;
        public static nfloat CategoryTitle { get => categoryTitle; set => categoryTitle = value; }

        private static nfloat categorySearcher = 16f;
        public static nfloat CategorySearcher { get => categorySearcher; set => categorySearcher = value; }

        private static nfloat categorySubtitle = 14f;
        public static nfloat CategorySubtitle { get => categorySubtitle; set => categorySubtitle = value; }

        private static nfloat categoryBody = 14f;
        public static nfloat CategoryBody { get => categoryBody; set => categoryBody = value; }
        #endregion

        #region product cells fonts 
        private static nfloat previousPrice = 13f;
        public static nfloat PreviousPrice { get => previousPrice; set => previousPrice = value; }
        #endregion

        #region products Detail fonts

        private static nfloat productDetailPreviousPrice = 14f;
        public static nfloat ProductDetailPreviousPrice { get => productDetailPreviousPrice; set => productDetailPreviousPrice = value; }

        private static nfloat productDetailPrice = 20f;
        public static nfloat ProductDetailPrice { get => productDetailPrice; set => productDetailPrice = value; }

        private static nfloat productDetailCount = 12f;
        public static nfloat ProductDetailCount { get => productDetailCount; set => productDetailCount = value; }

        private static nfloat productDetailDescount = 15f;
        public static nfloat ProductDetailDescount { get => productDetailDescount; set => productDetailDescount = value; }

        private static nfloat productDetailUnitWeight = 13f;
        public static nfloat ProductDetailUnitWeight { get => productDetailUnitWeight; set => productDetailUnitWeight = value; }

        private static nfloat productDetailBlond = 15f;
        public static nfloat ProductDetailBlond { get => productDetailBlond; set => productDetailBlond = value; }
        #endregion

        #region add address fonts
        private static nfloat addressTitleBold = 20f;
        public static nfloat AddressTitleBold { get => addressTitleBold; set => addressTitleBold = value; }

        private static nfloat addressTextFieldTitle = 18f;
        public static nfloat AddressTextFieldTitle { get => addressTextFieldTitle; set => addressTextFieldTitle = value; }

        private static nfloat addressTextFieldSubTitle = 17f;
        public static nfloat AddressTextFieldSubTitle { get => addressTextFieldSubTitle; set => addressTextFieldSubTitle = value; }
        #endregion

        #region my address fonts
        private static nfloat myAddressTitle = 20f;
        public static nfloat MyAddressTitle { get => myAddressTitle; set => myAddressTitle = value; }

        private static nfloat myAddressMessageSubtitle = 15f;
        public static nfloat MyAddressMessageSubtitle { get => myAddressMessageSubtitle; set => myAddressMessageSubtitle = value; }

        private static nfloat myAddressSubtitle = 15f;
        public static nfloat MyAddressSubtitle { get => myAddressSubtitle; set => myAddressSubtitle = value; }

        private static nfloat myAddressOtherMessage = 15f;
        public static nfloat MyAddressOtherMessage { get => myAddressOtherMessage; set => myAddressOtherMessage = value; }

        private static nfloat myAddressTextFieldTitle = 18f;
        public static nfloat MyAddressTextFieldTitle { get => myAddressTextFieldTitle; set => myAddressTextFieldTitle = value; }

        private static nfloat myAddressTextFieldBody = 17f;
        public static nfloat MyAddressTextFieldBody { get => myAddressTextFieldBody; set => myAddressTextFieldBody = value; }
        #endregion

        #region ProductsByCategory
        private static nfloat productByCategoryFilter = 15f;
        public static nfloat ProductByCategoryFilter { get => productByCategoryFilter; set => productByCategoryFilter = value; }

        private static nfloat productByCategoryCountFound = 17f;
        public static nfloat ProductByCategoryCountFound { get => productByCategoryCountFound; set => productByCategoryCountFound = value; }
        #endregion

        #region LaterIncome
        private static nfloat toHomeStoreOption = 15f;
        public static nfloat ToHomeStoreOption { get => toHomeStoreOption; set => toHomeStoreOption = value; }
        #endregion

        #region PaymentConfirmation     
        private static nfloat paymentTitleFontSize = 20f;
        public static nfloat PaymentTitleFontSize { get => paymentTitleFontSize; set => paymentTitleFontSize = value; }

        private static nfloat paymentSubtitleFontSize = 17f;
        public static nfloat PaymentSubtitleFontSize { get => paymentSubtitleFontSize; set => paymentSubtitleFontSize = value; }

        private static nfloat paymentBodyFontSize = 15f;
        public static nfloat PaymentBodyFontSize { get => paymentBodyFontSize; set => paymentBodyFontSize = value; }
        #endregion

        #region StepOneView
        private static nfloat stepOneTitle = 16f;
        public static nfloat StepOneTitle { get => stepOneTitle; set => stepOneTitle = value; }

        private static nfloat stepOneSubtitle = 15f;
        public static nfloat StepOneSubtitle { get => stepOneSubtitle; set => stepOneSubtitle = value; }

        private static nfloat stepOneTitleDescription = 15f;
        public static nfloat StepOneTitleDescription { get => stepOneTitleDescription; set => stepOneTitleDescription = value; }

        private static nfloat stepOneMessageDescrption = 13f;
        public static nfloat StepOneMessageDescrption { get => stepOneMessageDescrption; set => stepOneMessageDescrption = value; }

        private static nfloat stepOneHaveYouDoubts = 14f;
        public static nfloat StepOneHaveYouDoubts { get => stepOneHaveYouDoubts; set => stepOneHaveYouDoubts = value; }

        private static nfloat stepOneChatUs = 13f;
        public static nfloat StepOneChatUs { get => stepOneChatUs; set => stepOneChatUs = value; }

        #endregion

        #region StepTwoView
        private static nfloat stepTwoTitle = StepOneTitle;
        public static nfloat StepTwoTitle { get => stepTwoTitle; set => stepTwoTitle = value; }

        private static nfloat stepTwoSubtitle = StepOneSubtitle;
        public static nfloat StepTwoSubtitle { get => stepTwoSubtitle; set => stepTwoSubtitle = value; }

        private static nfloat stepTwoDomicileTitleDescription = 14f;
        public static nfloat StepTwoDomicileTitleDescription { get => stepTwoDomicileTitleDescription; set => stepTwoDomicileTitleDescription = value; }

        private static nfloat stepTwoDomicileNameDescrption = 14f;
        public static nfloat StepTwoDomicileNameDescrption { get => stepTwoDomicileNameDescrption; set => stepTwoDomicileNameDescrption = value; }

        private static nfloat stepTwoWhereIsDomicile = 14f;
        public static nfloat StepTwoWhereIsDomicile { get => stepTwoWhereIsDomicile; set => stepTwoWhereIsDomicile = value; }
        #endregion

        #region StepThreeView
        private static nfloat stepThreeTitle = StepOneTitle;
        public static nfloat StepThreeTitle { get => stepThreeTitle; set => stepThreeTitle = value; }

        private static nfloat stepThreeSubtitle = StepOneSubtitle;
        public static nfloat StepThreeSubtitle { get => stepThreeSubtitle; set => stepThreeSubtitle = value; }

        private static nfloat stepThreeMessageDescription = 13f;
        public static nfloat StepThreeMessageDescription { get => stepThreeMessageDescription; set => stepThreeMessageDescription = value; }

        private static nfloat stepThreeTotalPriceDescription = 13f;
        public static nfloat StepThreeTotalPriceDescription { get => stepThreeTotalPriceDescription; set => stepThreeTotalPriceDescription = value; }
        #endregion

        #region StepFourView
        private static nfloat stepFourTitle = StepOneTitle;
        public static nfloat StepFourTitle { get => stepFourTitle; set => stepFourTitle = value; }

        private static nfloat stepFourSubtitle = StepOneSubtitle;
        public static nfloat StepFourSubtitle { get => stepFourSubtitle; set => stepFourSubtitle = value; }

        private static nfloat stepFourHelpUsDescription = 13f;
        public static nfloat StepFourHelpUsDescription { get => stepFourHelpUsDescription; set => stepFourHelpUsDescription = value; }

        private static nfloat stepFourRateYouExperienceDescription = 13f;
        public static nfloat StepFourRateYouExperienceDescription { get => stepFourRateYouExperienceDescription; set => stepFourRateYouExperienceDescription = value; }
        #endregion

        #region MyOrders
        private static nfloat myOrdersTitleSize = 20f;
        public static nfloat MyOrdersTitleSize { get => myOrdersTitleSize; set => myOrdersTitleSize = value; }

        private static nfloat myOrdersSubtitle1Size = 18f;
        public static nfloat MyOrdersSubtitle1Size { get => myOrdersSubtitle1Size; set => myOrdersSubtitle1Size = value; }

        private static nfloat myOrdersSubtitle2Size = 17f;
        public static nfloat MyOrdersSubtitle2Size { get => myOrdersSubtitle2Size; set => myOrdersSubtitle2Size = value; }

        private static nfloat myOrdersSubtitle3Size = 16f;
        public static nfloat MyOrdersSubtitle3Size { get => myOrdersSubtitle3Size; set => myOrdersSubtitle3Size = value; }

        private static nfloat myOrdersBodySize = 15f;
        public static nfloat MyOrdersBodySize { get => myOrdersBodySize; set => myOrdersBodySize = value; }
        #endregion

        #region Turn
        private static nfloat textTurnSize = 35;
        public static nfloat TextTurnSize { get => textTurnSize; set => textTurnSize = value; }       
        
        #endregion

        #region AddressTableViewCell
        private static nfloat addressTitle = 15;
        public static nfloat AddressTitle { get => addressTitle; set => addressTitle = value; }

        private static nfloat address = 13;
        public static nfloat Address { get => address; set => address = value; }

        #endregion
    }
}
