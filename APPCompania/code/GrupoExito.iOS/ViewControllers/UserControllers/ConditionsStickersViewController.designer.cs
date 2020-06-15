// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    [Register ("ConditionsStickersViewController")]
    partial class ConditionsStickersViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView ConditionsStickersWebView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ConditionsStickersWebView != null) {
                ConditionsStickersWebView.Dispose ();
                ConditionsStickersWebView = null;
            }
        }
    }
}