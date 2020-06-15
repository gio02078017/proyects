using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public enum WizzardViewState
    {
        One,
        Two,
        Three
    }
    public partial class WizzardView : UIView
    {
        public WizzardView (IntPtr handle) : base (handle)
        {
        }

        public static WizzardView Create(WizzardViewState state)
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(WizzardView), null, null);
            var v = Runtime.GetNSObject<WizzardView>(arr.ValueAt(0));
            v.State(state);
            return v;
        }

        public override void AwakeFromNib()
        {
            Initialize();
        }

        private void State(WizzardViewState state)
        {
            switch (state)
            {
                case WizzardViewState.One:
                    stateOneView.BackgroundColor = ConstantColor.UiPrimary;
                    stateTwoView.BackgroundColor = ConstantColor.UiGrayBackground;
                    stateThreeView.BackgroundColor = ConstantColor.UiGrayBackground;

                    stateOneLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle1Size);
                    stateTwoLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.TextSubtitle1Size);
                    stateThreeLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.TextSubtitle1Size);
                    break;
                case WizzardViewState.Two:
                    stateOneView.BackgroundColor = ConstantColor.UiGrayBackground;
                    stateTwoView.BackgroundColor = ConstantColor.UiPrimary;
                    stateThreeView.BackgroundColor = ConstantColor.UiGrayBackground;

                    stateOneLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.TextSubtitle1Size);
                    stateTwoLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle1Size);
                    stateThreeLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.TextSubtitle1Size);
                    break;
                case WizzardViewState.Three:
                    stateOneView.BackgroundColor = ConstantColor.UiGrayBackground;
                    stateTwoView.BackgroundColor = ConstantColor.UiGrayBackground;
                    stateThreeView.BackgroundColor = ConstantColor.UiPrimary;

                    stateOneLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.TextSubtitle1Size);
                    stateTwoLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.TextSubtitle1Size);
                    stateThreeLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle1Size);
                    break;
                default:
                    break;
            }
        }

        private void Initialize()
        {
            stateOneView.Layer.CornerRadius = stateOneView.Frame.Width / 2;
            stateTwoView.Layer.CornerRadius = stateTwoView.Frame.Width / 2;
            stateThreeView.Layer.CornerRadius = stateThreeView.Frame.Width / 2;

            stateOneView.Layer.MasksToBounds = true;
            stateTwoView.Layer.MasksToBounds = true;
            stateThreeView.Layer.MasksToBounds = true;
        }
    }
}