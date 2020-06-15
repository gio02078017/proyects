// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    [Register ("ContactUsItemViewCell")]
    partial class ContactUsItemViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel storeNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton storePhoneButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (storeNameLabel != null) {
                storeNameLabel.Dispose ();
                storeNameLabel = null;
            }

            if (storePhoneButton != null) {
                storePhoneButton.Dispose ();
                storePhoneButton = null;
            }
        }
    }
}