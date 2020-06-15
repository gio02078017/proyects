using System;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class CashViewModel : CreditCardViewModel
    {
        private EnumPaymentType cashOptionSelected;
        public EnumPaymentType CashOptionSelected
        {
            get { return cashOptionSelected; }
            set { SetProperty(ref cashOptionSelected, value); }
        }

        public Command CashTypeSelected { get; set; }

        public CashViewModel(CreditCard creditCard, string creditCardType) : base(creditCard, creditCardType)
        {
            CashTypeSelected = new Command<EnumPaymentType>(ExecuteCashTypeSelected);
        }

        private void ExecuteCashTypeSelected(EnumPaymentType enumPaymentType)
        {
            CashOptionSelected = enumPaymentType;
        }
    }
}
