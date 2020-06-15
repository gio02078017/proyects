using System;
using GrupoExito.Entities;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Helpers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class OrderDetailCollectionViewCell : UIView
    {
        #region Attributes
        public event EventHandler<bool> DisplayStateHandler;
        public event EventHandler DisplayProductsHandler;
        private bool IsTotalPriceDisplayed = true;
        #endregion

        #region Properties 
        public UIButton ProductsViews{
            get { return viewProductsButton; }
        }
        #endregion

        #region Constructors
        static OrderDetailCollectionViewCell()
        {
            //Static default Constructor this class 
        }

        protected OrderDetailCollectionViewCell(IntPtr handle) : base(handle)
        {
            //Default Constructor this class 
        }
        #endregion

        #region Methods
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }

        public void Configure()
        {
            UpdateArrow();

            displayButton.TouchUpInside += (o, s) =>
            {
                IsTotalPriceDisplayed = !IsTotalPriceDisplayed;
                UpdateArrow();

                DisplayStateHandler?.Invoke(this, IsTotalPriceDisplayed);
            };

            viewProductsButton.TouchUpInside += (o, s) =>
            {
                var handler = DisplayProductsHandler;
                if(handler != null)
                {
                    DisplayProductsHandler(this, null);
                }
            };
        }

        public void UpdateCell(Order order)
        {
            orderNumberLabel.Text = order.Id;
            totalProductsLabel.Text = order.TotalProducts.ToString();
            totalPriceLabel.Text = StringFormat.ToPrice(order.Total);
            switch(order.PaymentType){
                case (int)EnumPaymentType.Cash:
                    paymentMethodLabel.Text = "Efectivo";
                    break;
                case (int)EnumPaymentType.Both:
                    paymentMethodLabel.Text = "Efectivo/Tarjeta";
                    break;
                case (int)EnumPaymentType.Dataphone:
                    paymentMethodLabel.Text = "Tarjeta";
                    break;
            }
        }

        private void UpdateArrow()
        {
            if (IsTotalPriceDisplayed)
            {
                arrowImageView.Image = UIImage.FromBundle(ConstantImages.FlechaArribaPrimaria);
                lineView.Hidden = false;
            }
            else
            {
                arrowImageView.Image = UIImage.FromBundle(ConstantImages.FlechaAbajoTerciaria);
                lineView.Hidden = true;
            }
        }
        #endregion
    }
}
