// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    [Register ("NotPrimeViewController")]
    partial class NotPrimeViewController
    {
        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton closeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton exitoButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelBold1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelBold2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelBold3 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelBold4 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewAnual { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewMes { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (closeButton != null) {
                closeButton.Dispose ();
                closeButton = null;
            }

            if (exitoButton != null) {
                exitoButton.Dispose ();
                exitoButton = null;
            }

            if (labelBold1 != null) {
                labelBold1.Dispose ();
                labelBold1 = null;
            }

            if (labelBold2 != null) {
                labelBold2.Dispose ();
                labelBold2 = null;
            }

            if (labelBold3 != null) {
                labelBold3.Dispose ();
                labelBold3 = null;
            }

            if (labelBold4 != null) {
                labelBold4.Dispose ();
                labelBold4 = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }

            if (viewAnual != null) {
                viewAnual.Dispose ();
                viewAnual = null;
            }

            if (viewMes != null) {
                viewMes.Dispose ();
                viewMes = null;
            }
        }
    }
}