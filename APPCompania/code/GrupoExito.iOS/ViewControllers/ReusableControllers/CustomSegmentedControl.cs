using System;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class CustomSegmentedControl : UIView
	{
        #region Attributes
        public static readonly NSString Key = new NSString("CustomSegmentedControl");
        public static readonly UINib Nib;
        private nint buttonSelected = 0;
        #endregion

        #region Properties
        public Action<int> SelectedChanged { get; set; }
        #endregion

        #region Constructors
        static CustomSegmentedControl()
        {
            Nib = UINib.FromName("CustomSegmentedControl", NSBundle.MainBundle);
        }

        public static CustomSegmentedControl Create()
        {
            return NSBundle.MainBundle.LoadNib(nameof(CustomSegmentedControl), null, null).GetItem<CustomSegmentedControl>(0);
        }

        public CustomSegmentedControl (IntPtr handle) : base (handle){}
        #endregion

        #region Overrides Methods
        public override void AwakeFromNib()
        {
            LoadHandlers();
        }
        #endregion

        #region Public Methods
        public void SetTitleOption(int option, string title)
        {
            switch (option)
            {
                case 1:
                    option1Label.Text = title;
                    break;
                case 2:
                    option2Label.Text = title;
                    break;
                case 3:
                    option3Label.Text = title;
                    break;
            }
        }


        public void SetTitleAttributes(int option, NSAttributedString title)
        {
            switch (option)
            {
                case 1:
                    option1Label.AttributedText = title;
                    break;
                case 2:
                    option2Label.AttributedText = title;
                    break;
                case 3:
                    option3Label.AttributedText = title;
                    break;
            }
        }

        public void SetHiddenOption(int option, bool hidden)
        {
            switch (option)
            {
                case 1:
                    option1StackView.Hidden = hidden;
                    break;
                case 2:
                    option2StackView.Hidden = hidden;
                    break;
                case 3:
                    option3StackView.Hidden = hidden;
                    break;
            }
        }

        public void SetHiddenSelector(int option, bool hidden)
        {
            switch (option)
            {
                case 1:
                    selectorOption1View.Hidden = hidden;
                    break;
                case 2:
                    selectorOption2View.Hidden = hidden;
                    break;
                case 3:
                    selectorOption3View.Hidden = hidden;
                    break;
            }
        }

        public void SetEnableOption(int option, bool enabled)
        {
            switch (option)
            {
                case 1:
                    option1Button.Enabled = enabled;
                    break;
                case 2:
                    option2Button.Enabled = enabled;
                    break;
                case 3:
                    option3Button.Enabled = enabled;
                    break;
            }
        }

        public void SetSelection(nint selection)
        {
            if (selection > 2)
            {

            }
            else
            {
                buttonSelected = selection;
                UpdateView();
            }
        }

        public void SetAlignment(int option, UITextAlignment position)
        {
            switch (option)
            {
                case 1:
                    option1Label.TextAlignment = position;
                    break;
                case 2:
                    option1Label.TextAlignment = position;
                    break;
                case 3:
                    option1Label.TextAlignment = position;
                    break;
            }
        }
        #endregion

        #region Private Methods
        private void LoadHandlers()
        {
            option1Button.TouchUpInside += OptionEventTouchUpInside;
            option2Button.TouchUpInside += OptionEventTouchUpInside;
            option3Button.TouchUpInside += OptionEventTouchUpInside;
        }

        private void UpdateView()
        {
            switch (buttonSelected)
            {
                case 0:
                    option1Label.TextColor = ConstantColor.ButtonSelected;
                    option2Label.TextColor = ConstantColor.DefaultText;
                    option3Label.TextColor = ConstantColor.DefaultText;

                    selectorOption1View.Hidden = false;
                    selectorOption2View.Hidden = true;
                    selectorOption3View.Hidden = true;
                    break;
                case 1:
                    option2Label.TextColor = ConstantColor.ButtonSelected;
                    option1Label.TextColor = ConstantColor.DefaultText;
                    option3Label.TextColor = ConstantColor.DefaultText;

                    selectorOption1View.Hidden = true;
                    selectorOption2View.Hidden = false;
                    selectorOption3View.Hidden = true;
                    break;
                case 2:
                    option3Label.TextColor = ConstantColor.ButtonSelected;
                    option1Label.TextColor = ConstantColor.DefaultText;
                    option2Label.TextColor = ConstantColor.DefaultText;
                    selectorOption1View.Hidden = true;
                    selectorOption2View.Hidden = true;
                    selectorOption3View.Hidden = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Events
        void OptionEventTouchUpInside(object sender, EventArgs e)
        {
            UIButton optionTypeButton = (UIButton)sender;
            if (optionTypeButton.Tag != buttonSelected)
            {
                buttonSelected = optionTypeButton.Tag;
                UpdateView();
                SelectedChanged?.Invoke((int)optionTypeButton.Tag);
            }
        }
        #endregion
    }
}
