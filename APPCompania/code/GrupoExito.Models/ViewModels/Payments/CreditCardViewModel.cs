using System;
using GrupoExito.Entities.Entiites;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class CreditCardViewModel : BaseViewModel
    {
        public CreditCard CreditCard { get; }
        public bool IsSelected { get; set; }
        public string CreditCardType { get; }
        public Action<CreditCardViewModel> RowSelected { get; set; }

        public CreditCardViewModel(CreditCard creditCard, string creditCardType)
        {
            this.CreditCard = creditCard;
            this.CreditCardType = creditCardType;
        }
    }
}
