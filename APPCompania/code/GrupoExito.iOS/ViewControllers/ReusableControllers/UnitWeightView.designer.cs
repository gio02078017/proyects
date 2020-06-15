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

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("UnitWeightView")]
    partial class UnitWeightView
    {
        [Outlet]
        UIKit.UIButton addButton { get; set; }


        [Outlet]
        UIKit.UILabel cantLabel { get; set; }


        [Outlet]
        UIKit.UIButton substractionButton { get; set; }


        [Outlet]
        UIKit.UIButton unitButton { get; set; }


        [Outlet]
        UIKit.UIView unitOrnamentView { get; set; }


        [Outlet]
        UIKit.UIButton weightButton { get; set; }


        [Outlet]
        UIKit.UIView weightOrnamentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView lineView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addButton != null) {
                addButton.Dispose ();
                addButton = null;
            }

            if (cantLabel != null) {
                cantLabel.Dispose ();
                cantLabel = null;
            }

            if (lineView != null) {
                lineView.Dispose ();
                lineView = null;
            }

            if (substractionButton != null) {
                substractionButton.Dispose ();
                substractionButton = null;
            }

            if (unitButton != null) {
                unitButton.Dispose ();
                unitButton = null;
            }

            if (unitOrnamentView != null) {
                unitOrnamentView.Dispose ();
                unitOrnamentView = null;
            }

            if (weightButton != null) {
                weightButton.Dispose ();
                weightButton = null;
            }

            if (weightOrnamentView != null) {
                weightOrnamentView.Dispose ();
                weightOrnamentView = null;
            }
        }
    }
}