using System;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views
{
    public partial class StatusHeaderMyDiscount : UIView
    {
        #region Attributes
        public static readonly NSString Key = new NSString("StatusHeaderMyDiscount");
        public static readonly UINib Nib;
        private int optionSelected = 0;
        private CustomSegmentedControl categoryOfType;
        private EventHandler toActivateEvent;
        private EventHandler activatedEvent;
        private EventHandler redeemedEvent;
        #endregion

        #region Properties
        public EventHandler ToActivateEvent { get => toActivateEvent; set => toActivateEvent = value; }
        public EventHandler ActivatedEvent { get => activatedEvent; set => activatedEvent = value; }
        public EventHandler RedeemedEvent { get => redeemedEvent; set => redeemedEvent = value; }
        public int OptionSelected { get => optionSelected; set => optionSelected = value; }
        public Action<int> SelectedChanged { get; set; }

        #endregion

        #region Constructors
        static StatusHeaderMyDiscount()
        {
            Nib = UINib.FromName("StatusHeaderMyDiscount", NSBundle.MainBundle);
        }

        protected StatusHeaderMyDiscount(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static StatusHeaderMyDiscount Create()
        {
            return NSBundle.MainBundle.LoadNib(nameof(StatusHeaderMyDiscount), null, null).GetItem<StatusHeaderMyDiscount>(0);
        }
        #endregion

        #region Override Methods

        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            LoadExternalViews();
            LoadHandlers();
        }

        private void LoadHandlers()
        {
            toActivateButton.TouchUpInside += ToActivateButton_TouchUpInside;
            activatedButton.TouchUpInside += ActivatedButton_TouchUpInside;
            redeemedButton.TouchUpInside += RedeemedButton_TouchUpInside;
        }
        #endregion

        #region Private Methods
        private void LoadExternalViews()
        {
            if (categoryOfType == null)
            {
                categoryOfType = CustomSegmentedControl.Create();
                AssingCategoriesForOption(ConstantCuponType.ToActivate);
                if (categoryOfType.SelectedChanged == null)
                {
                    categoryOfType.SelectedChanged = CategoryOfTypeSelectedChanged;
                }
                categoryOfType.SetSelection(optionSelected);
                CGRect CategoryOfTypeFrame = new CGRect(0, 0, this.categoryOfCuponView.Frame.Size.Width, 50);
                categoryOfType.Frame = CategoryOfTypeFrame;
                categoryOfCuponView.AddSubview(categoryOfType);
            }
        }

        private void SetUnderlineTitle(int option)
        {
            switch (option)
            {
                case 1:
                    NSAttributedString toActiveAttributeText = new NSAttributedString(AppMessages.Available, new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.Single, ForegroundColor = ConstantColor.UiDiscountButtonToActivated });
                    toActivateTitleButton.SetAttributedTitle(toActiveAttributeText, UIControlState.Normal);
                    NSAttributedString activedAttributeText = new NSAttributedString(AppMessages.Activated, new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.None, ForegroundColor = ConstantColor.UiDiscountButtonToActivated });
                    activatedTitleButton.SetAttributedTitle(activedAttributeText, UIControlState.Normal);
                    NSAttributedString redeemedAttributeText = new NSAttributedString(AppMessages.Redeemeds, new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.None, ForegroundColor = ConstantColor.UiDiscountButtonToActivated });
                    redeemedTitleButton.SetAttributedTitle(redeemedAttributeText, UIControlState.Normal);
                    break;
                case 2:
                    toActiveAttributeText = new NSAttributedString(AppMessages.Available, new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.None, ForegroundColor  = ConstantColor.UiDiscountButtonToActivated });
                    toActivateTitleButton.SetAttributedTitle(toActiveAttributeText, UIControlState.Normal);
                    activedAttributeText = new NSAttributedString(AppMessages.Activated, new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.Single, ForegroundColor = ConstantColor.UiDiscountButtonToActivated });
                    activatedTitleButton.SetAttributedTitle(activedAttributeText, UIControlState.Normal);
                    redeemedAttributeText = new NSAttributedString(AppMessages.Redeemeds, new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.None, ForegroundColor = ConstantColor.UiDiscountButtonToActivated });
                    redeemedTitleButton.SetAttributedTitle(redeemedAttributeText, UIControlState.Normal);
                    break;
                case 3:
                    toActiveAttributeText = new NSAttributedString(AppMessages.Available, new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.None, ForegroundColor = ConstantColor.UiDiscountButtonToActivated });
                    toActivateTitleButton.SetAttributedTitle(toActiveAttributeText, UIControlState.Normal);
                    activedAttributeText = new NSAttributedString(AppMessages.Activated, new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.None, ForegroundColor = ConstantColor.UiDiscountButtonToActivated });
                    activatedTitleButton.SetAttributedTitle(activedAttributeText, UIControlState.Normal);
                    redeemedAttributeText = new NSAttributedString(AppMessages.Redeemeds, new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.Single, ForegroundColor = ConstantColor.UiDiscountButtonToActivated });
                    redeemedTitleButton.SetAttributedTitle(redeemedAttributeText, UIControlState.Normal);
                    break;
            }
        }
        #endregion

        #region Public Methods
        public void SetOptionSelected(int optionSelected)
        {
            this.optionSelected = optionSelected;
            categoryOfType.SetSelection(optionSelected);
        }

        public void SetCounters(int toActivate, int activated, int redeemed, int totalMaxDiscountActivated)
        {
            if(toActivate > 0)
            {
                ContainertoActivateView.Hidden = false;
                toActivateCountLabel.Text = toActivate.ToString();
                toActivateButton.Enabled = true;
            }
            else
            {
                toActivateCountLabel.Text =  "0";
                toActivateButton.Enabled = false;
            }


            if (activated > 0)
            {
                containerActivatedView.Hidden = false;
                activatedCountLabel.Text = activated.ToString();
                activatedButton.Enabled = true;

            }
            else
            {
                activatedCountLabel.Text = "0";
                activatedButton.Enabled = false;
            }

            if (redeemed > 0)
            {
                containerRedeemedView.Hidden = false;
                redeemedCountLabel.Text = redeemed.ToString();
                redeemedButton.Enabled = true;
            }
            else
            {
                redeemedCountLabel.Text = "0";
                redeemedButton.Enabled = false;
            }
            maxActivatedbyClientLabel.Text = string.Format(maxActivatedbyClientLabel.Text, totalMaxDiscountActivated);
        }

        public void SetCountersToActivate(int AlreadyPurchased, int CouldLike, int Killers)
        {
            if (AlreadyPurchased == 0)
            {
                categoryOfType.SetHiddenOption(1, true);
            }
            else
            {
                categoryOfType.SetHiddenOption(1, false);
            }
            if (CouldLike == 0)
            {
                categoryOfType.SetHiddenOption(2, true);
            }
            else
            {
                categoryOfType.SetHiddenOption(2, false);
            }
            if (Killers == 0)
            {
                categoryOfType.SetHiddenOption(3, true);
            }
            else
            {
                categoryOfType.SetHiddenOption(3, false);
            }
        }
        #endregion

        #region Events
        private void CategoryOfTypeSelectedChanged(int obj)
        {
            SelectedChanged?.Invoke(obj);
        }

        private void RedeemedButton_TouchUpInside(object sender, EventArgs e)
        {
            AssingCategoriesForOption(ConstantCuponType.Redeemed);
            RedeemedEvent?.Invoke(sender, e);
        }

        private void ActivatedButton_TouchUpInside(object sender, EventArgs e)
        {
            AssingCategoriesForOption(ConstantCuponType.Activated);
            activatedEvent?.Invoke(sender, e);
        }

        private void ToActivateButton_TouchUpInside(object sender, EventArgs e)
        {
            AssingCategoriesForOption(ConstantCuponType.ToActivate);
            toActivateEvent?.Invoke(sender, e);
        }

        private void AssingCategoriesForOption(string option)
        {
            switch (option)
            {
                case ConstantCuponType.ToActivate:
                    categoryOfType.SetTitleOption(1,"Para todos");
                    categoryOfType.SetTitleOption(2,"Para ti");
                    categoryOfType.SetTitleOption(3,"Sugeridos");
                    categoryOfType.SetAlignment(1, UITextAlignment.Center);
                    categoryOfType.SetEnableOption(1, true);
                    categoryOfType.SetHiddenSelector(1, true);
                    categoryOfType.SetHiddenOption(2, false);
                    categoryOfType.SetHiddenOption(3, false);
                    SetUnderlineTitle(1);
                    break;
                case ConstantCuponType.Activated:
                    NSMutableAttributedString titleTextAttributes = new NSMutableAttributedString("Tus descuentos ", UIFont.FromName(ConstantFontSize.LetterSubtitle, 16f));
                    NSMutableAttributedString boldText = new NSMutableAttributedString("Activados", UIFont.FromName(ConstantFontSize.LetterTitle, 16f));
                    titleTextAttributes.Append(boldText);
                    categoryOfType.SetSelection(0);
                    categoryOfType.SetTitleAttributes(1,titleTextAttributes);
                    categoryOfType.SetAlignment(1, UITextAlignment.Left);
                    categoryOfType.SetHiddenSelector(1, true);
                    categoryOfType.SetEnableOption(1, false);
                    categoryOfType.SetHiddenOption(1, false);
                    categoryOfType.SetHiddenOption(2, true);
                    categoryOfType.SetHiddenOption(3, true);
                    SetUnderlineTitle(2);
                    break;
                case ConstantCuponType.Redeemed:
                    titleTextAttributes = new NSMutableAttributedString("Tus descuentos ", UIFont.FromName(ConstantFontSize.LetterSubtitle, 16f));
                    boldText = new NSMutableAttributedString("Redimidos", UIFont.FromName(ConstantFontSize.LetterTitle, 16f));
                    titleTextAttributes.Append(boldText);
                    categoryOfType.SetSelection(0);
                    categoryOfType.SetTitleAttributes(1, titleTextAttributes);
                    categoryOfType.SetAlignment(1, UITextAlignment.Left);
                    categoryOfType.SetEnableOption(1, false);
                    categoryOfType.SetHiddenOption(1, false);
                    categoryOfType.SetHiddenSelector(1, true);
                    categoryOfType.SetHiddenOption(2, true);
                    categoryOfType.SetHiddenOption(3, true);
                    SetUnderlineTitle(3);
                    break;
            }
        }
        #endregion
    }
}
