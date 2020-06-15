using System;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class GenericButton : UIButton
    {
        public override bool Enabled { get { return base.Enabled; } set { base.Enabled = value; var state = value ? UIControlState.Normal : UIControlState.Disabled; ConfigureViewForState(state); } }

        public override void AwakeFromNib()
        {
            Layer.CornerRadius = 10;

            SetTitleColor(ConstantColor.DefaultSelectedText, UIControlState.Disabled);
            SetTitleColor(ConstantColor.DefaultDeselectedText, UIControlState.Normal);
        }

        public GenericButton(IntPtr handle) : base(handle)
        {
        }

        public void SetSelectedState(bool isSelected)
        {
            ConfigureViewForState(isSelected ? UIControlState.Disabled : UIControlState.Normal);
        }

        protected void ConfigureViewForState(UIControlState state)
        {
            switch (state)
            {
                case UIControlState.Normal:
                    {
                        BackgroundColor = ConstantColor.UiGrayBackground;
                        TitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.TextSubtitle1Size);
                        break;
                    }
                case UIControlState.Disabled:
                    {
                        BackgroundColor = ConstantColor.UiPrimary;
                        TitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle1Size);
                        break;
                    }
            }
        }
    }
}