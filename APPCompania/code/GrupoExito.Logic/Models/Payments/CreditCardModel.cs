namespace GrupoExito.Logic.Models.Paymentes
{
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Payments;
    using GrupoExito.Utilities.Contracts.Payments;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public class CreditCardModel
    {
        private ICreditCardService _creditCardService { get; set; }

        public CreditCardModel(ICreditCardService creditCardService)
        {
            _creditCardService = creditCardService;
        }

        public async Task<ResponseBase> DeleteCreditCard(CreditCard creditCard)
        {
            return await _creditCardService.DeleteCreditCard(creditCard);
        }

        public async Task<ResponseBase> DeleteCreditCardPaymentez(CreditCard creditCard)
        {
            return await _creditCardService.DeleteCreditCardPaymentez(creditCard);
        }

        public AddCardResponse GetResponseAddCardPaymentez(string value)
        {
            AddCardResponse response = null;

            if (!string.IsNullOrEmpty(value))
            {
                string responseAddCard = HttpUtility.UrlDecode(value);
                responseAddCard = responseAddCard.Replace("cardResponse=", string.Empty);
                response = JsonService.Deserialize<AddCardResponse>(responseAddCard);
            }

            return response;
        }

        public async Task<CreditCardResponse> GetAllCreditCards()
        {
            CreditCardResponse response = new CreditCardResponse();
            var responseCreditCard = await _creditCardService.GetCreditCards();
            var responseCreditCardsPaymentez = await _creditCardService.GetCreditCardsPaymentez();

            if (responseCreditCard != null && responseCreditCard.CreditCards != null && responseCreditCard.CreditCards.Any())
            {
                foreach (var item in responseCreditCard.CreditCards)
                {
                    item.Type = item.Image;
                    item.Image = this.GetImageName(item.Image);
                    item.Paymentez = false;
                    response.CreditCards.Add(item);
                }
            }

            if (responseCreditCardsPaymentez != null && responseCreditCardsPaymentez.CreditCards != null && responseCreditCardsPaymentez.CreditCards.Any())
            {
                foreach (var item in responseCreditCardsPaymentez.CreditCards)
                {
                    item.Type = item.Image;
                    item.Image = this.GetImageName(item.Image);
                    item.Paymentez = true;
                    response.CreditCards.Add(item);
                }
            }

            return response;
        }

        public async Task<CreditCardResponse> GetAllPaymentMethods()
        {
            CreditCardResponse response = await GetAllCreditCards();

            if (response != null)
            {
                response.CreditCards.Add(new CreditCard
                {
                    TypePayment = ConstTypePayment.Delivery,
                    Bin = AppMessages.PaymentDelivery
                });
            }

            return response;
        }

        private string GetImageName(string typeCard)
        {
            string imageName = string.Empty;

            switch (typeCard)
            {
                case ConstCreditCardType.Amex:
                    imageName = "amex";
                    break;
                case ConstCreditCardType.Diners:
                    imageName = "tarjeta_diners_club";
                    break;
                case ConstCreditCardType.Discover:
                    imageName = "tarjeta";
                    break;
                case ConstCreditCardType.Jcb:
                    imageName = "tarjeta";
                    break;
                case ConstCreditCardType.Mastercard:
                    imageName = "tarjeta_master_card";
                    break;
                case ConstCreditCardType.Visa:
                    imageName = "tarjeta_visa";
                    break;
                case ConstCreditCardType.Exito:
                    imageName = "tarjeta_exito";
                    break;
                case ConstCreditCardType.ExitoMastercard:
                    imageName = "tarjeta_mastercard";
                    break;
                default:
                    imageName = "tarjeta";
                    break;
            }

            return imageName;
        }
    }
}
