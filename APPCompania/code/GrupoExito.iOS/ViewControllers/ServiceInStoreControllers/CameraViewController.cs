using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AVCamBarcode;
using AVFoundation;
using CoreAnimation;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Resources;
using UIKit;

using static AVFoundation.AVCaptureVideoOrientation;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    enum SessionSetupResult
    {
        Success,
        NotAuthorized,
        ConfigurationFailed
    }

    class MetadataObjectLayer : CAShapeLayer
    {
        public AVMetadataObject MetadataObject { get; set; }

        bool PathContaints(CGPoint point)
        {
            var path = Path;
            return path != null && path.ContainsPoint(point, false);
        }
    }

    [Register("CameraViewController")]
    public class CameraViewController : UIViewControllerBase, IAVCaptureMetadataOutputObjectsDelegate, ItemSelectionViewControllerDelegate, IDisposable
    {

        private string dependencyId;
        private string dependecyName;
        public string DependencyId { get => dependencyId; set => dependencyId = value; }
        public string DependecyName { get => dependecyName; set => dependecyName = value; }

        const string metadataObjectTypeItemSelectionIdentifier = "MetadataObjectTypes";
        const string sessionPresetItemSelectionIdentifier = "SessionPreset";

        [Outlet("metadataObjectTypesButton")]
        UIButton MetadataObjectTypesButton { get; set; }

        [Outlet("sessionPresetsButton")]
        UIButton SessionPresetsButton { get; set; }

        [Outlet("cameraButton")]
        UIButton CameraButton { get; set; }

        [Outlet("cameraUnavailableLabel")]
        UILabel CameraUnavailableLabel { get; set; }

        [Outlet("zoomSlider")]
        UISlider ZoomSlider { get; set; }

        [Outlet("previewView")]
        PreviewView PreviewView { get; set; }

        AVCaptureDeviceInput VideoDeviceInput;
        readonly AVCaptureSession Session = new AVCaptureSession();
        readonly AVCaptureMetadataOutput MetadataOutput = new AVCaptureMetadataOutput();

        // Communicate with the session and other session objects on this queue.
        readonly DispatchQueue SessionQueue = new DispatchQueue("session queue");
        readonly DispatchQueue MetadataObjectsQueue = new DispatchQueue("metadata objects queue");

        NSTimer RemoveMetadataObjectOverlayLayersTimer;
        readonly AutoResetEvent ResetEvent = new AutoResetEvent(true);
        SessionSetupResult SetupResult = SessionSetupResult.Success;
        bool SessionRunning;
        IDisposable RunningChangeToken;
        NSObject RuntimeErrorNotificationToken;
        NSObject WasInterruptedNotificationToken;
        NSObject InterruptionEndedNotificationToken;
        readonly List<MetadataObjectLayer> MetadataObjectOverlayLayers = new List<MetadataObjectLayer>();
        Dictionary<string, AVMetadataObjectType> BarcodeTypeMap;
        Dictionary<string, NSString> PresetMap;
        UITapGestureRecognizer TapRecognizer;

        UITapGestureRecognizer OpenBarcodeURLGestureRecognizer
        {
            get
            {
                TapRecognizer = TapRecognizer ?? new UITapGestureRecognizer(OpenBarcodeUrl);
                return TapRecognizer;
            }
        }

        public CameraViewController(IntPtr handle)
            : base(handle)
        {
        }


        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.PriceCheck, nameof(CameraViewController));
        }


        public override void ViewDidLoad()
        {           
            base.ViewDidLoad();
            this.LoadExternalViews();
            this.LoadHandlers();

            MetadataObjectTypesButton.Enabled = false;
            SessionPresetsButton.Enabled = false;
            CameraButton.Enabled = false;
            ZoomSlider.Enabled = false;

            PreviewView.AddGestureRecognizer(OpenBarcodeURLGestureRecognizer);
            PreviewView.Session = Session;

            switch (AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video))
            {
                case AVAuthorizationStatus.Authorized:
                    break;
                case AVAuthorizationStatus.NotDetermined:
                    SessionQueue.Suspend();
                    AVCaptureDevice.RequestAccessForMediaType(AVMediaType.Video, granted =>
                    {
                        if (!granted)
                            SetupResult = SessionSetupResult.NotAuthorized;
                        SessionQueue.Resume();
                    });
                    break;

                default:
                    SetupResult = SessionSetupResult.NotAuthorized;
                    break;
            }
            SessionQueue.DispatchAsync(ConfigureSession);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            NavigationView.LoadControllers(false, false, true, this);
            NavigationView.HiddenCarData();
            NavigationView.HiddenAccountProfile();
            SessionQueue.DispatchAsync(() =>
            {
                switch (SetupResult)
                {
                    case SessionSetupResult.Success:
                        AddObservers();
                        Session.StartRunning();
                        SessionRunning = Session.Running;
                        break;

                    case SessionSetupResult.NotAuthorized:
                        DispatchQueue.MainQueue.DispatchAsync(() =>
                        {
                            var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.AccessDeniedCarulla, UIAlertControllerStyle.Alert);
                            alertController.AddAction(UIAlertAction.Create(AppMessages.OK, UIAlertActionStyle.Cancel, null));
                            alertController.AddAction(UIAlertAction.Create(AppMessages.Configuration, UIAlertActionStyle.Default, action =>
                            {
                                UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                            }));

                            this.PresentViewController(alertController, true, null);
                        });
                        break;

                    case SessionSetupResult.ConfigurationFailed:
                        DispatchQueue.MainQueue.DispatchAsync(() =>
                        {
                            var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.UnableToCapture, UIAlertControllerStyle.Alert);
                            alertController.AddAction(UIAlertAction.Create(AppMessages.OK, UIAlertActionStyle.Cancel, null));
                            this.PresentViewController(alertController, true, null);
                        });
                        break;
                }
            });
        }

        public override void ViewDidDisappear(bool animated)
        {
            SessionQueue.DispatchAsync(() =>
            {
                if (SetupResult == SessionSetupResult.Success)
                {
                    Session.StopRunning();
                    SessionRunning = Session.Running;
                    Dispose();
                }
            });

            base.ViewDidDisappear(animated);
        }

        public new void Dispose()
        {
            RunningChangeToken.Dispose();
            PreviewView.RegionOfInterestDidChange -= RegionOfInterestChanged;
            RuntimeErrorNotificationToken.Dispose();
            WasInterruptedNotificationToken.Dispose();
            InterruptionEndedNotificationToken.Dispose();
        }

        private void LoadExternalViews()
        {
            LoadNavigationView(this.NavigationController.NavigationBar);

        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "SelectMetadataObjectTypes")
            {
                string key(AVMetadataObjectType v) => v.ToString();
                BarcodeTypeMap = MetadataOutput.AvailableMetadataObjectTypes
                                               .GetFlags()
                                               .ToDictionary(key);
                var allItems = BarcodeTypeMap.Keys.ToArray();
                var selectedItems = MetadataOutput.MetadataObjectTypes
                                                  .GetFlags()
                                                  .Select(key)
                                                  .ToList();

                var navigationController = (UINavigationController)segue.DestinationViewController;
                var selectionCtrl = (ItemSelectionViewController)navigationController.ViewControllers[0];

                selectionCtrl.Title = "Metadata Object Types";
                selectionCtrl.Delegate = this;
                selectionCtrl.Identifier = metadataObjectTypeItemSelectionIdentifier;
                selectionCtrl.AllItems = allItems;
                selectionCtrl.SelectedItems = selectedItems;
                selectionCtrl.AllowsMultipleSelection = true;
            }
            else if (segue.Identifier == "SelectSessionPreset")
            {
                string key(NSString v) => v;
                PresetMap = AvailableSessionPresets().ToDictionary(key);
                var allItems = PresetMap.Keys.ToArray();

                var navigationController = (UINavigationController)segue.DestinationViewController;
                var selectionCtrl = (ItemSelectionViewController)navigationController.ViewControllers[0];

                selectionCtrl.Title = "Session Presets";
                selectionCtrl.Delegate = this;
                selectionCtrl.Identifier = sessionPresetItemSelectionIdentifier;
                selectionCtrl.AllItems = allItems;
                selectionCtrl.SelectedItems = new List<string> { key(Session.SessionPreset) };
                selectionCtrl.AllowsMultipleSelection = false;
            }
        }


        public override bool ShouldAutorotate()
        {
            return !PreviewView.IsResizingRegionOfInterest;
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            base.ViewWillTransitionToSize(toSize, coordinator);

            var videoPreviewLayerConnection = PreviewView.VideoPreviewLayer.Connection;

            if (videoPreviewLayerConnection != null)
            {
                var deviceOrientation = UIDevice.CurrentDevice.Orientation;

                if (!deviceOrientation.IsPortrait() && !deviceOrientation.IsLandscape())
                    return;

                var newVideoOrientation = VideoOrientationFor(deviceOrientation);
                var oldSize = View.Frame.Size;
                var oldVideoOrientation = videoPreviewLayerConnection.VideoOrientation;
                videoPreviewLayerConnection.VideoOrientation = newVideoOrientation;

                coordinator.AnimateAlongsideTransition(context =>
                {
                    var oldRegion = PreviewView.RegionOfInterest;
                    var newRegion = new CGRect();

                    if (oldVideoOrientation == LandscapeRight && newVideoOrientation == LandscapeLeft)
                    {
                        newRegion = oldRegion.WithX(oldSize.Width - oldRegion.X - oldRegion.Width);
                    }
                    else if (oldVideoOrientation == LandscapeRight && newVideoOrientation == Portrait)
                    {
                        newRegion.X = toSize.Width - oldRegion.Y - oldRegion.Height;
                        newRegion.Y = oldRegion.X;
                        newRegion.Width = oldRegion.Height;
                        newRegion.Height = oldRegion.Width;
                    }
                    else if (oldVideoOrientation == LandscapeLeft && newVideoOrientation == LandscapeRight)
                    {
                        newRegion = oldRegion.WithX(oldSize.Width - oldRegion.X - oldRegion.Width);
                    }
                    else if (oldVideoOrientation == LandscapeLeft && newVideoOrientation == Portrait)
                    {
                        newRegion.X = oldRegion.Y;
                        newRegion.Y = oldSize.Width - oldRegion.X - oldRegion.Width;
                        newRegion.Width = oldRegion.Height;
                        newRegion.Height = oldRegion.Width;
                    }
                    else if (oldVideoOrientation == Portrait && newVideoOrientation == LandscapeRight)
                    {
                        newRegion.X = oldRegion.Y;
                        newRegion.Y = toSize.Height - oldRegion.X - oldRegion.Width;
                        newRegion.Width = oldRegion.Height;
                        newRegion.Height = oldRegion.Width;
                    }
                    else if (oldVideoOrientation == Portrait && newVideoOrientation == LandscapeLeft)
                    {
                        newRegion.X = oldSize.Height - oldRegion.Y - oldRegion.Height;
                        newRegion.Y = oldRegion.X;
                        newRegion.Width = oldRegion.Height;
                        newRegion.Height = oldRegion.Width;
                    }

                    PreviewView.SetRegionOfInterestWithProposedRegionOfInterest(newRegion);
                }, context =>
                {
                    SessionQueue.DispatchAsync(() =>
                    {
                        MetadataOutput.RectOfInterest = PreviewView.VideoPreviewLayer.MapToLayerCoordinates(PreviewView.RegionOfInterest);
                    });

                    RemoveMetadataObjectOverlayLayers();
                });
            }
        }

        AVCaptureVideoOrientation VideoOrientationFor(UIDeviceOrientation deviceOrientation)
        {
            switch (deviceOrientation)
            {
                case UIDeviceOrientation.Portrait:
                    return Portrait;
                case UIDeviceOrientation.PortraitUpsideDown:
                    return PortraitUpsideDown;
                case UIDeviceOrientation.LandscapeLeft:
                    return LandscapeLeft;
                case UIDeviceOrientation.LandscapeRight:
                    return LandscapeRight;
                default:
                    throw new InvalidProgramException();
            }
        }

        #region Session Management

        void ConfigureSession()
        {
            if (SetupResult != SessionSetupResult.Success)
                return;

            Session.BeginConfiguration();

            var videoDevice = DeviceWithMediaType(AVMediaType.Video, AVCaptureDevicePosition.Back);
            var vDeviceInput = AVCaptureDeviceInput.FromDevice(videoDevice, out NSError err);

            if (err != null)
            {
                SetupResult = SessionSetupResult.ConfigurationFailed;
                Session.CommitConfiguration();
                return;
            }

            if (Session.CanAddInput(vDeviceInput))
            {
                Session.AddInput(vDeviceInput);
                VideoDeviceInput = vDeviceInput;
            }
            else
            {
                SetupResult = SessionSetupResult.ConfigurationFailed;
                Session.CommitConfiguration();
                return;
            }

            if (Session.CanAddOutput(MetadataOutput))
            {
                Session.AddOutput(MetadataOutput);
                MetadataOutput.SetDelegate(this, MetadataObjectsQueue);
                MetadataOutput.MetadataObjectTypes = MetadataOutput.AvailableMetadataObjectTypes;
                MetadataOutput.RectOfInterest = CGRect.Empty;
            }
            else
            {
                SetupResult = SessionSetupResult.ConfigurationFailed;
                Session.CommitConfiguration();
                return;
            }

            Session.CommitConfiguration();
        }

        #endregion

        #region Device Configuration

        [Action("changeCamera")]
        void ChangeCamera()
        {
            MetadataObjectTypesButton.Enabled = false;
            SessionPresetsButton.Enabled = false;
            CameraButton.Enabled = false;
            ZoomSlider.Enabled = false;
            RemoveMetadataObjectOverlayLayers();

            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                var currentVideoDevice = VideoDeviceInput.Device;
                var currentPosition = currentVideoDevice.Position;

                var preferredPosition = AVCaptureDevicePosition.Unspecified;

                switch (currentPosition)
                {
                    case AVCaptureDevicePosition.Unspecified:
                    case AVCaptureDevicePosition.Front:
                        preferredPosition = AVCaptureDevicePosition.Back;
                        break;

                    case AVCaptureDevicePosition.Back:
                        preferredPosition = AVCaptureDevicePosition.Front;
                        break;
                }

                var videoDevice = DeviceWithMediaType(AVMediaType.Video, preferredPosition);

                if (videoDevice != null)
                {
                    var vDeviceInput = AVCaptureDeviceInput.FromDevice(videoDevice, out NSError err);

                    if (err != null)
                    {
                        return;
                    }

                    Session.BeginConfiguration();
                    Session.RemoveInput(VideoDeviceInput);
                    var previousSessionPreset = Session.SessionPreset;
                    Session.SessionPreset = AVCaptureSession.PresetHigh;

                    if (Session.CanAddInput(vDeviceInput))
                    {
                        Session.AddInput(vDeviceInput);
                        VideoDeviceInput = vDeviceInput;
                    }
                    else
                    {
                        Session.AddInput(VideoDeviceInput);
                    }

                    if (Session.CanSetSessionPreset(previousSessionPreset))
                        Session.SessionPreset = previousSessionPreset;

                    Session.CommitConfiguration();
                }

                MetadataObjectTypesButton.Enabled = true;
                SessionPresetsButton.Enabled = true;
                CameraButton.Enabled = true;
                ZoomSlider.Enabled = true;
                ZoomSlider.MaxValue = (float)NMath.Min(VideoDeviceInput.Device.ActiveFormat.VideoMaxZoomFactor, 10);
                ZoomSlider.Value = (float)VideoDeviceInput.Device.VideoZoomFactor;
            });
        }

        AVCaptureDevice DeviceWithMediaType(NSString mediaType, AVCaptureDevicePosition position)
        {
            return AVCaptureDevice.DevicesWithMediaType(mediaType)
                                  .FirstOrDefault(d => d.Position == position);
        }

        [Action("zoomCameraWith:")]
        void ZoomCamera(UISlider slider)
        {
            var device = VideoDeviceInput.Device;
            VideoDeviceInput.Device.LockForConfiguration(out NSError err);

            if (err != null)
            {
                return;
            }

            device.VideoZoomFactor = slider.Value;
            device.UnlockForConfiguration();
        }

        #endregion

        #region Drawing Metadata Object Overlay Layers



        private MetadataObjectLayer CreateMetadataOverlay(AVMetadataObject metadataObject)
        {
            // Transform the metadata object so the bounds are updated to reflect those of the video preview layer.
            var transformedMetadataObject = this.PreviewView.VideoPreviewLayer.GetTransformedMetadataObject(metadataObject);

            // Create the initial metadata object overlay layer that can be used for either machine readable codes or faces.
            var metadataObjectOverlayLayer = new MetadataObjectLayer
            {
                LineWidth = 7,
                LineJoin = CAShapeLayer.JoinRound,
                MetadataObject = transformedMetadataObject,
                FillColor = this.View.TintColor.ColorWithAlpha(0.3f).CGColor,
                StrokeColor = this.View.TintColor.ColorWithAlpha(0.7f).CGColor,
            };

            if (transformedMetadataObject is AVMetadataMachineReadableCodeObject barcodeMetadataObject)
            {
                var barcodeOverlayPath = this.BarcodeOverlayPathWithCorners(barcodeMetadataObject.Corners);


                string textLayerString = null;
                if (!string.IsNullOrEmpty(barcodeMetadataObject.StringValue))
                {
                    textLayerString = barcodeMetadataObject.StringValue;
                    DetailPriceCheckerViewController detailPriceCheckerViewController = this.Storyboard.InstantiateViewController(ConstantControllersName.DetailPriceCheckerViewController) as DetailPriceCheckerViewController;
                    detailPriceCheckerViewController.Data = textLayerString;
                    detailPriceCheckerViewController.DependecyId = dependencyId;
                    detailPriceCheckerViewController.DependecyName = dependecyName;
                    this.NavigationController.PushViewController(detailPriceCheckerViewController, true);
                    Session.StopRunning();
                }
                else
                {

                }
            }

            return metadataObjectOverlayLayer;
        }

        private void LoadHandlers()
        {
            SessionPresetsButton.TouchUpInside += SessionPresetsButtonTouchUpInside;

        }

        void SessionPresetsButtonTouchUpInside(object sender, EventArgs e)
        {

            SelectStorePriceCheckerViewController selectStorePriceCheckerView = (SelectStorePriceCheckerViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.SelectStorePriceCheckerViewController);
            this.NavigationController.PopViewController(true);
        }


        CGPath BarcodeOverlayPathWithCorners(CGPoint[] corners)
        {
            var path = new CGPath();

            if (corners.Length > 0)
            {
                var start = corners[0];
                path.MoveToPoint(start);

                for (int i = 1; i < corners.Length; i++)
                {
                    var corner = corners[i];
                    path.AddLineToPoint(corner);
                }

                path.CloseSubpath();
            }

            return path;
        }

        void RemoveMetadataObjectOverlayLayers()
        {
            MetadataObjectOverlayLayers.ForEach(layer => layer.RemoveFromSuperLayer());
            MetadataObjectOverlayLayers.Clear();

            RemoveMetadataObjectOverlayLayersTimer?.Invalidate();
            RemoveMetadataObjectOverlayLayersTimer = null;
        }

        void AddMetadataOverlayLayers(IEnumerable<MetadataObjectLayer> layers)
        {
            CATransaction.Begin();
            CATransaction.DisableActions = true;

            MetadataObjectOverlayLayers.Clear();
            foreach (var l in layers)
            {
                this.PreviewView.VideoPreviewLayer.AddSublayer(l);
                this.MetadataObjectOverlayLayers.Add(l);
            }

            CATransaction.Commit();
            this.RemoveMetadataObjectOverlayLayersTimer = NSTimer.CreateScheduledTimer(TimeSpan.FromSeconds(0.01), t => RemoveMetadataObjectOverlayLayers());
        }

        void OpenBarcodeUrl(UITapGestureRecognizer openBarcodeURLGestureRecognizer)
        {
            foreach (var metadataObjectOverlayLayer in this.MetadataObjectOverlayLayers)
            {
                var location = openBarcodeURLGestureRecognizer.LocationInView(this.PreviewView);

                if (metadataObjectOverlayLayer.Path.ContainsPoint(location, false))
                {
                    if (metadataObjectOverlayLayer.MetadataObject is AVMetadataMachineReadableCodeObject barcodeMetadataObject)
                    {
                        var val = barcodeMetadataObject.StringValue;

                        if (!string.IsNullOrEmpty(val))
                        {
                            var url = NSUrl.FromString(val);
                            var sharedApp = UIApplication.SharedApplication;

                            if (sharedApp.CanOpenUrl(url))
                            {
                                sharedApp.OpenUrl(url);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region AVCaptureMetadataOutputObjectsDelegate

        private readonly AutoResetEvent resetEvent = new AutoResetEvent(true);

        [Export("captureOutput:didOutputMetadataObjects:fromConnection:")]
        public void DidOutputMetadataObjects(AVCaptureMetadataOutput captureOutput, AVMetadataObject[] metadataObjects, AVCaptureConnection connection)
        {
            if (this.resetEvent.WaitOne(0))
            {
                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    this.RemoveMetadataObjectOverlayLayers();
                    this.AddMetadataOverlayLayers(metadataObjects.Select(this.CreateMetadataOverlay));
                    this.resetEvent.Set();
                });
            }
        }

        #endregion

        #region ItemSelectionViewControllerDelegate

        public void ItemSelectionViewController(ItemSelectionViewController itemSelectionViewController, List<string> selectedItems)
        {
            var identifier = itemSelectionViewController.Identifier;

            if (identifier == metadataObjectTypeItemSelectionIdentifier)
            {
                SessionQueue.DispatchAsync(() =>
                {
                    var objectTypes = selectedItems.Select(t => BarcodeTypeMap[t]).Combine();
                    MetadataOutput.MetadataObjectTypes = objectTypes;
                });
            }
            else if (identifier == sessionPresetItemSelectionIdentifier)
            {
                SessionQueue.DispatchAsync(() =>
                {
                    Session.SessionPreset = PresetMap[selectedItems.First()];
                });
            }
        }

        #endregion

        #region Change observers

        void AddObservers()
        {
            RunningChangeToken = Session.AddObserver("running", NSKeyValueObservingOptions.New, RunningChanged);
            PreviewView.RegionOfInterestDidChange += RegionOfInterestChanged;

            var center = NSNotificationCenter.DefaultCenter;
            RuntimeErrorNotificationToken = center.AddObserver(AVCaptureSession.RuntimeErrorNotification, OnRuntimeErrorNotification, Session);
            WasInterruptedNotificationToken = center.AddObserver(AVCaptureSession.WasInterruptedNotification, OnSessionWasInterrupted, Session);
            InterruptionEndedNotificationToken = center.AddObserver(AVCaptureSession.InterruptionEndedNotification, OnSessionInterruptionEnded, Session);
        }

        void RunningChanged(NSObservedChange obj)
        {
            var isSessionRunning = ((NSNumber)obj.NewValue).BoolValue;

            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                MetadataObjectTypesButton.Enabled = isSessionRunning;
                SessionPresetsButton.Enabled = isSessionRunning;
                CameraButton.Enabled = isSessionRunning && AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video).Length > 1;
                ZoomSlider.Enabled = isSessionRunning;
                ZoomSlider.MaxValue = (float)NMath.Min(VideoDeviceInput.Device.ActiveFormat.VideoMaxZoomFactor, 8);
                ZoomSlider.Value = (float)(VideoDeviceInput.Device.VideoZoomFactor);

                if (!isSessionRunning)
                    RemoveMetadataObjectOverlayLayers();
            });
        }

        void RegionOfInterestChanged(object sender, EventArgs e)
        {
            var pv = (PreviewView)sender;
            CGRect newRegion = pv.RegionOfInterest;
            SessionQueue.DispatchAsync(() =>
            {
                MetadataOutput.RectOfInterest = PreviewView.VideoPreviewLayer.MapToMetadataOutputCoordinates(newRegion);
                DispatchQueue.MainQueue.DispatchAsync(RemoveMetadataObjectOverlayLayers);
            });
        }

        void OnRuntimeErrorNotification(NSNotification notification)
        {
            var e = new AVCaptureSessionRuntimeErrorEventArgs(notification);
            var errorVal = e.Error;
            if (errorVal == null)
                return;

            var error = (AVError)(long)errorVal.Code;

            if (error == AVError.MediaServicesWereReset)
            {
                SessionQueue.DispatchAsync(() =>
                {
                    if (SessionRunning)
                    {
                        Session.StartRunning();
                        SessionRunning = Session.Running;
                    }
                });
            }
        }

        void OnSessionWasInterrupted(NSNotification notification)
        {
            var reasonIntegerValue = ((NSNumber)notification.UserInfo[AVCaptureSession.InterruptionReasonKey]).Int32Value;
            var reason = (AVCaptureSessionInterruptionReason)reasonIntegerValue;

            if (reason == AVCaptureSessionInterruptionReason.VideoDeviceNotAvailableWithMultipleForegroundApps)
            {
                CameraUnavailableLabel.Hidden = false;
                CameraUnavailableLabel.Alpha = 0;
                UIView.Animate(0.25, () =>
                {
                    CameraUnavailableLabel.Alpha = 1;
                });
            }
        }

        void OnSessionInterruptionEnded(NSNotification notification)
        {
            if (CameraUnavailableLabel.Hidden)
            {
                UIView.Animate(0.25, () =>
                {
                    CameraUnavailableLabel.Alpha = 0;
                }, () =>
                {
                    CameraUnavailableLabel.Hidden = true;
                });
            }
        }

        #endregion

        NSString[] AvailableSessionPresets()
        {
            return AllSessionPresets().Where(p => Session.CanSetSessionPreset(p))
                                       .ToArray();
        }

        static IEnumerable<NSString> AllSessionPresets()
        {
            yield return AVCaptureSession.PresetPhoto;
            yield return AVCaptureSession.PresetLow;
            yield return AVCaptureSession.PresetMedium;
            yield return AVCaptureSession.PresetHigh;
            yield return AVCaptureSession.Preset352x288;
            yield return AVCaptureSession.Preset640x480;
            yield return AVCaptureSession.Preset1280x720;
            yield return AVCaptureSession.PresetiFrame960x540;
            yield return AVCaptureSession.PresetiFrame1280x720;
            yield return AVCaptureSession.Preset1920x1080;
            yield return AVCaptureSession.Preset3840x2160;
        }
    }
}
