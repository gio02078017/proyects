using System;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Helpers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class IncompletePrimeAmountViewController : UIViewController
    {
        #region Attributes
        private string _primeAmount;
        private decimal _costRemaining;
        #endregion

        #region Constructors
        public IncompletePrimeAmountViewController() : base("IncompletePrimeAmountViewController", null)
        {
            //Default constructor this class
        }

        public IncompletePrimeAmountViewController(string primeAmount, decimal costRemaining)
        {
            _primeAmount = primeAmount;
            _costRemaining = costRemaining;
        }
        #endregion

        #region Life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                LoadSubviews();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.IncompletePrimeAmountViewController, ConstantMethodName.ViewDidLoad);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
        #endregion

        #region Methods
        private void LoadSubviews()
        {
            missingAmountLabel.Text = "te faltan " + StringFormat.ToPrice(_costRemaining) + " para obtener tu envío gratis";
            adviceLabel.Text = "*Recuerda que si eres Prime por compras superiores a " + _primeAmount + " tu envío es gratis";
        }
        #endregion
    }
}

