using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Barcodes;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Utilities.Resources;
using Java.Lang;
using System;
using static Android.Gms.Vision.Detector;
using static Android.Support.V4.App.ActivityCompat;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Chequeador de Precios", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PriceCheckActivity : BaseActivity, ISurfaceHolderCallback, IProcessor, IOnRequestPermissionsResultCallback
    {
        #region Controls

        private SurfaceView SurfaceView;
        private TextView TxtResult;
        private ImageView IvClose;

        #endregion

        #region Properties

        private BarcodeDetector BarcodeDetector;
        private CameraSource CameraSource;
        private const int RequestCameraPermisionID = 1001;
        private bool Capture { get; set; }
        private string DependencyId;

        #endregion

        public override void OnBackPressed()
        {
            Finish();
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == RequestCameraPermisionID)
            {
                if (grantResults[0] == Permission.Granted)
                {
                    CameraSource.Start(SurfaceView.Holder);
                }
                else
                {
                    ConvertUtilities.MessageToast(AppMessages.PermissionPriceCheckerPriceMessage, this);
                    OnBackPressed(); Finish();
                }
            }
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != (int)Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new string[]
                    {
                        Manifest.Permission.Camera
                    }, RequestCameraPermisionID);

                    return;
                }
                else
                {
                    CameraSource.Start(SurfaceView.Holder);
                }

            }
            catch (InvalidOperationException)
            {
            }
            catch (RuntimeException)
            {
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            CameraSource.Stop();
        }

        public void ReceiveDetections(Detections detections)
        {
            SparseArray barcodes = detections.DetectedItems;

            if (barcodes.Size() != 0)
            {
                TxtResult.Post(() =>
                {
                    if (((Barcode)barcodes.ValueAt(0)).RawValue.Trim().Length > 0)
                    {
                        if (!Capture)
                        {
                            Intent intent = new Intent(this, typeof(ProductDetailPriceCheckActivity));
                            intent.PutExtra(ConstantPreference.BarCode, ((Barcode)barcodes.ValueAt(0)).RawValue.Trim());
                            intent.PutExtra(ConstantPreference.DependencyId, DependencyId);
                            StartActivity(intent);
                            Capture = true;
                        }
                    }

                    CameraSource.Stop();
                });
            }
        }
        public void Release()
        {
        }

        protected override void OnResume()
        {
            base.OnResume();
            Capture = false;
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.PriceCheck, typeof(PriceCheckActivity).Name);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityPriceCheck);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.HideItemsToolbar(this);

            if (Intent.Extras != null && !string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.DependencyId)))
            {
                DependencyId = Intent.Extras.GetString(ConstantPreference.DependencyId);
                this.SetControlsProperties();
            }            
        }

        private void SetControlsProperties()
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            SurfaceView = FindViewById<SurfaceView>(Resource.Id.cameraView);
            TxtResult = FindViewById<TextView>(Resource.Id.txtResult);
            IvClose = FindViewById<ImageView>(Resource.Id.ivClose);
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                 FindViewById<RelativeLayout>(Resource.Id.rlBody),
                 AppMessages.PriceCheckMessage, AppMessages.PriceCheckAction);

            this.InitSurfaceView();
            IvToolbarBack.Click += delegate { OnBackPressed(); Finish(); };
            IvClose.Click += delegate { OnBackPressed(); Finish(); };
        }

        private void InitSurfaceView()
        {
            Bitmap bitMap = BitmapFactory.DecodeResource(ApplicationContext.Resources, Resource.Drawable.scan);
            BarcodeDetector = new BarcodeDetector.Builder(this)
                .SetBarcodeFormats(Barcode.AllFormats)
                .Build();
            CameraSource = new CameraSource
                .Builder(this, BarcodeDetector)
                .SetRequestedPreviewSize(1280, 1024)
                .SetAutoFocusEnabled(true)
                .SetRequestedFps(40.0f)
                .Build();
            SurfaceView.Holder.AddCallback(this);
            BarcodeDetector.SetProcessor(this);
        }
    }
}