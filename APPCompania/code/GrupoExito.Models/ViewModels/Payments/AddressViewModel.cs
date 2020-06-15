using System;
using GrupoExito.Entities;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class AddressViewModel : BaseViewModel
    {
        private string addressTitle;
        public string AddressTitle
        {
            get { return addressTitle; }
            set { SetProperty(ref addressTitle, value); }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string description;
        public string Description { get => description; set { SetProperty(ref description, value); } }

        public EventHandler CellSelected { get; set; }

        public AddressViewModel(UserContext userContext)
        {
            if (userContext.Address != null)
            {
                AddressTitle = AppMessages.GetAddressText;
                address = userContext.Address.City + ", " + userContext.Address.AddressComplete;
                Description = userContext.Address.Description;
            }
            else if (userContext.Store != null)
            {
                AddressTitle = AppMessages.PickUpOn;
                address = userContext.Store.City + ", " + userContext.Store.Name;
                Description = string.Empty;
            }
        }

        public void UpdateData(UserContext userContext)
        {
            if (userContext.Address != null)
            {
                AddressTitle = AppMessages.GetAddressText;
                Address = userContext.Address.City + ", " + userContext.Address.AddressComplete;
                Description = userContext.Address.Description;
            }
            else if (userContext.Store != null)
            {
                AddressTitle = AppMessages.PickUpOn;
                Address = userContext.Store.City + ", " + userContext.Store.Name;
                Description = string.Empty;
            }
        }

        public void CellTouchUpInside(object sender, EventArgs e)
        {
            CellSelected?.Invoke(sender, e);
        }
    }
}
