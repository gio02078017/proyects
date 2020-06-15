// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    [Register ("CameraViewController")]
    partial class CameraViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SessionPresetsButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cameraButton != null) {
                cameraButton.Dispose ();
                cameraButton = null;
            }

            if (cameraUnavailableLabel != null) {
                cameraUnavailableLabel.Dispose ();
                cameraUnavailableLabel = null;
            }

            if (metadataObjectTypesButton != null) {
                metadataObjectTypesButton.Dispose ();
                metadataObjectTypesButton = null;
            }

            if (previewView != null) {
                previewView.Dispose ();
                previewView = null;
            }

            if (SessionPresetsButton != null) {
                SessionPresetsButton.Dispose ();
                SessionPresetsButton = null;
            }

            if (zoomSlider != null) {
                zoomSlider.Dispose ();
                zoomSlider = null;
            }
        }
    }
}