// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.HomeControllers
{
    [Register ("TermAndConditionViewController")]
    partial class TermAndConditionViewController
    {
        [Outlet]
        UIKit.UIView navigationView { get; set; }


        [Outlet]
        UIKit.UIWebView termAndConditionsWebView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (navigationView != null) {
                navigationView.Dispose ();
                navigationView = null;
            }

            if (termAndConditionsWebView != null) {
                termAndConditionsWebView.Dispose ();
                termAndConditionsWebView = null;
            }
        }
    }
}