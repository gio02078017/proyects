// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    [Register ("SummaryContainerController")]
    partial class SummaryContainerController
    {
        [Outlet]
        UIKit.UIButton checkerButton { get; set; }


        [Outlet]
        UIKit.UIButton continueButton { get; set; }


        [Outlet]
        UIKit.UIButton emptyCarButton { get; set; }


        [Outlet]
        UIKit.UIButton searchButton { get; set; }


        [Outlet]
        UIKit.UITableView tableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView subtotalParent { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkerButton != null) {
                checkerButton.Dispose ();
                checkerButton = null;
            }

            if (continueButton != null) {
                continueButton.Dispose ();
                continueButton = null;
            }

            if (emptyCarButton != null) {
                emptyCarButton.Dispose ();
                emptyCarButton = null;
            }

            if (searchButton != null) {
                searchButton.Dispose ();
                searchButton = null;
            }

            if (subtotalParent != null) {
                subtotalParent.Dispose ();
                subtotalParent = null;
            }

            if (tableView != null) {
                tableView.Dispose ();
                tableView = null;
            }
        }
    }
}