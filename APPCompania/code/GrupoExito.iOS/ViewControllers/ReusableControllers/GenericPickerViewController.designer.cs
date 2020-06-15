// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("GenericPickerViewController")]
    partial class GenericPickerViewController
    {
        [Outlet]
        UIKit.UIButton acceptButton { get; set; }


        [Outlet]
        UIKit.UIButton cancelButton { get; set; }


        [Outlet]
        UIKit.UIView innerView { get; set; }


        [Outlet]
        UIKit.UIPickerView pickerView { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (acceptButton != null) {
                acceptButton.Dispose ();
                acceptButton = null;
            }

            if (cancelButton != null) {
                cancelButton.Dispose ();
                cancelButton = null;
            }

            if (innerView != null) {
                innerView.Dispose ();
                innerView = null;
            }

            if (pickerView != null) {
                pickerView.Dispose ();
                pickerView = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}