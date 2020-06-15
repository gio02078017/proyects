// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    [Register ("PopUpEnterTextView")]
    partial class PopUpEnterTextView
    {
        [Outlet]
        UIKit.UIButton acceptButton { get; set; }


        [Outlet]
        UIKit.UIButton backgroundButton { get; set; }


        [Outlet]
        UIKit.UIButton closeButton { get; set; }


        [Outlet]
        UIKit.UIView contentView { get; set; }


        [Outlet]
        UIKit.UITextView descriptionTextView { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (acceptButton != null) {
                acceptButton.Dispose ();
                acceptButton = null;
            }

            if (backgroundButton != null) {
                backgroundButton.Dispose ();
                backgroundButton = null;
            }

            if (closeButton != null) {
                closeButton.Dispose ();
                closeButton = null;
            }

            if (contentView != null) {
                contentView.Dispose ();
                contentView = null;
            }

            if (descriptionTextView != null) {
                descriptionTextView.Dispose ();
                descriptionTextView = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}