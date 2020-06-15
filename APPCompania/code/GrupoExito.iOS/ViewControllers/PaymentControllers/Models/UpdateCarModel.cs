namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Models
{
    public class UpdateCarModel
    {
        #region Attributes
        private string _price;
        private string _cantity;
        private bool _showNotification;
        #endregion

        #region Properties
        public string Price { get => _price; set => _price = value; }
        public string Cantity { get => _cantity; set => _cantity = value; }
        public bool ShowNotification { get => _showNotification; set => _showNotification = value; }
        #endregion

        #region Constructors
        public UpdateCarModel(string price, string cantity, bool showNotification)
        {
            this.Price = price;
            this.Cantity = cantity;
            this.ShowNotification = showNotification;
        }
        #endregion
    }
}
