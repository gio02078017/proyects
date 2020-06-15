// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Cells
{
    [Register ("HeaderInitialAccessViewCell")]
    partial class HeaderInitialAccessViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel userNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (userNameLabel != null) {
                userNameLabel.Dispose ();
                userNameLabel = null;
            }
        }
    }
}