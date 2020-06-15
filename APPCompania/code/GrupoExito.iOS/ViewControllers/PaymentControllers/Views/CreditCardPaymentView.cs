using Foundation;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Resources;
using ObjCRuntime;
using System;
using System.Linq;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class CreditCardPaymentView : UIView
    {
        public Action AddCreditCard { get; set; }
        public Action AddExitoCreditCard { get; set; }

        public CreditCardPaymentView (IntPtr handle) : base (handle)
        {
        }

        public static CreditCardPaymentView Create(UIPickerView pickerView, UIToolbar toolbar)
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(CreditCardPaymentView), null, null);
            var v = Runtime.GetNSObject<CreditCardPaymentView>(arr.ValueAt(0));
            v.Configure(pickerView, toolbar);
            return v;
        }

        private void Configure(UIPickerView picker, UIToolbar bar)
        {
            installmentTextField.InputView = picker;
            installmentTextField.InputAccessoryView = bar;
        }

        public override void AwakeFromNib()
        {
            installmentTextField.Layer.BorderColor = UIColor.Black.CGColor;
            installmentTextField.Layer.BorderWidth = 1;
            installmentTextField.Layer.CornerRadius = 10;

            exitoCardButton.TouchUpInside += (sender, e) => AddExitoCreditCard?.Invoke();
            mastercardButton.TouchUpInside += (sender, e) => AddCreditCard?.Invoke();

            tableView.RegisterNibForCellReuse(CreditCardViewCell.Nib, CreditCardViewCell.Key);
        }

        public void ShowInstallmentView(bool show)
        {
            installmentStackView.Hidden = !show;
        }

        internal void ClearInstallmentTextField()
        {
            installmentTextField.Text = string.Empty;
        }

        public void SetDuesNumber(string dues)
        {
            installmentTextField.Text = dues;
        }

        public void ReloadData(CreditCardSource source)
        {
            if (source == null) return;

            tableView.Source = source;
            tableView.ReloadData();
            LayoutIfNeeded();

            CreditCardViewModel selectedRowViewModel = source.ViewModels.FirstOrDefault((arg) => arg.IsSelected == true);
            if (selectedRowViewModel != null)
            {
                int index = source.ViewModels.IndexOf(selectedRowViewModel);
                if (index != -1)
                {
                    tableView.SelectRow(NSIndexPath.FromRowSection(index, 0), false, UITableViewScrollPosition.Top);
                }
            }
        }

        internal void ExitoViewConfiguration(bool isExitoView)
        {
            if (AppServiceConfiguration.SiteId.Equals("exito"))
            {
                exitoCardView.Hidden = !isExitoView;
            }
            else exitoCardView.Hidden = true;

            mastercardLabel.Text = isExitoView ? AppMessages.AddSiteMasterCard : AppMessages.AddCreditCard;
            //mastercardImageView.Hidden = !isExitoView;
        }
    }
}