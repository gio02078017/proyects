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

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    [Register ("HistoricalOrderTableViewCell")]
    partial class HistoricalOrderTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel dateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel dateValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderValueLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (dateLabel != null) {
                dateLabel.Dispose ();
                dateLabel = null;
            }

            if (dateValueLabel != null) {
                dateValueLabel.Dispose ();
                dateValueLabel = null;
            }

            if (orderLabel != null) {
                orderLabel.Dispose ();
                orderLabel = null;
            }

            if (orderValueLabel != null) {
                orderValueLabel.Dispose ();
                orderValueLabel = null;
            }
        }
    }
}