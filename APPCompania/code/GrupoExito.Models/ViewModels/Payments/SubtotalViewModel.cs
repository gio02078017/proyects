using System;
using System.Collections.Generic;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities.Constants.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class SubtotalViewModel : BaseViewModel
    {
        private string subtotal;
        public string Subtotal
        {
            get { return subtotal; }
            set { SetProperty(ref subtotal, value); }
        }

        private string bagTax;
        public string BagTax
        {
            get { return bagTax; }
            set { SetProperty(ref bagTax, value); }
        }

        private bool bagTaxButton;
        public bool BagTaxButton
        {
            get { return bagTaxButton; }
            set { SetProperty(ref bagTaxButton, value); }
        }

        private Dictionary<string, object> summary;
        public Dictionary<string, object> Summary
        {
            get { return summary; }
            set { SetProperty(ref summary, value); }
        }

        public Command UpdateContentCommand { get; set; }
        public EventHandler BagTaxInfoHandler { get; set; }

        private ProductCarModel databaseModel;

        public SubtotalViewModel(string subtotal, string bagTax)
        {
            this.databaseModel = new ProductCarModel(ProductCarDataBase.Instance);

            decimal.TryParse(subtotal, out decimal subtotalNumber);
            decimal.TryParse(bagTax, out decimal bagTaxNumber);

            StringFormat.ToPrice(subtotalNumber);
            StringFormat.ToPrice(bagTaxNumber);

            UpdateContentCommand = new Command(ExecuteUpdateContentCommand);
        }

        public void SubtotalViewModel_BagTaxInfoHandler(object sender, EventArgs e)
        {
            BagTaxInfoHandler?.Invoke(sender, e);
        }

        private void ExecuteUpdateContentCommand()
        {
            var summaryRecalc = databaseModel.RecalculateSummary();
            this.Subtotal = StringFormat.ToPrice(decimal.Parse(summaryRecalc[ConstDataBase.TotalPrice].ToString()));
            this.BagTax = StringFormat.ToPrice(decimal.Parse(summaryRecalc[ConstDataBase.TotalTaxBag].ToString()));
            Summary = summaryRecalc;
        }
    }
}
