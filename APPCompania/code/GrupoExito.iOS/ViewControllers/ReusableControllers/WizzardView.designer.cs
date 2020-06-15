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
    [Register ("WizzardView")]
    partial class WizzardView
    {
        [Outlet]
        UIKit.UILabel stateOneLabel { get; set; }


        [Outlet]
        UIKit.UIView stateOneView { get; set; }


        [Outlet]
        UIKit.UILabel stateThreeLabel { get; set; }


        [Outlet]
        UIKit.UIView stateThreeView { get; set; }


        [Outlet]
        UIKit.UILabel stateTwoLabel { get; set; }


        [Outlet]
        UIKit.UIView stateTwoView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (stateOneLabel != null) {
                stateOneLabel.Dispose ();
                stateOneLabel = null;
            }

            if (stateOneView != null) {
                stateOneView.Dispose ();
                stateOneView = null;
            }

            if (stateThreeLabel != null) {
                stateThreeLabel.Dispose ();
                stateThreeLabel = null;
            }

            if (stateThreeView != null) {
                stateThreeView.Dispose ();
                stateThreeView = null;
            }

            if (stateTwoLabel != null) {
                stateTwoLabel.Dispose ();
                stateTwoLabel = null;
            }

            if (stateTwoView != null) {
                stateTwoView.Dispose ();
                stateTwoView = null;
            }
        }
    }
}