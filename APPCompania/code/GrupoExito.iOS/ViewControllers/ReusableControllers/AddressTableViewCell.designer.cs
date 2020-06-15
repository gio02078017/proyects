// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("AddressTableViewCell")]
    partial class AddressTableViewCell
    {
        [Outlet]
        UIKit.UIButton editAddressButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel addressLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel addressTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView detailAddressImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconAddressImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addressLabel != null) {
                addressLabel.Dispose ();
                addressLabel = null;
            }

            if (addressTitleLabel != null) {
                addressTitleLabel.Dispose ();
                addressTitleLabel = null;
            }

            if (detailAddressImageView != null) {
                detailAddressImageView.Dispose ();
                detailAddressImageView = null;
            }

            if (editAddressButton != null) {
                editAddressButton.Dispose ();
                editAddressButton = null;
            }

            if (iconAddressImageView != null) {
                iconAddressImageView.Dispose ();
                iconAddressImageView = null;
            }
        }
    }
}