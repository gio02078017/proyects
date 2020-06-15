using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities;
using GrupoExito.Utilities.Helpers;
using System.Collections.Generic;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Productos Eliminados", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductsDeleteActivity : BaseActivity
    {
        #region Controls

        private RecyclerView RvDeletedProducts;
        private LinearLayoutManager linerLayoutManager;
        private RemovedProductsAdapter _RemovedProductsAdapter;
        private LinearLayout LyExcuseUsDelete;
        private LinearLayout LyDeletedProducts;
        private LinearLayout LyAccept;

        #endregion

        #region Properties

        private IList<Product> ListProductos { get; set; }

        #endregion

        public override void OnBackPressed()
        {
            Finish();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityProductosDelete);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            this.SetFinishOnTouchOutside(false);
            Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();

            if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.DeleteProducts)))
            {
                ListProductos = JsonService.Deserialize<List<Product>>(Intent.Extras.GetString(ConstantPreference.DeleteProducts));
                DrawRvDeletedProducts();
            }
            else
            {
                OnBackPressed();
                Finish();
            }
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvExcuseUsDelete).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageExcuseDelete).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleDeletedProducts).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvAccept).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            RvDeletedProducts = FindViewById<RecyclerView>(Resource.Id.rvDeletedProducts);
            LyExcuseUsDelete = FindViewById<LinearLayout>(Resource.Id.lyExcuseUsDelete);
            LyDeletedProducts = FindViewById<LinearLayout>(Resource.Id.lyDeletedProducts);
            LyAccept = FindViewById<LinearLayout>(Resource.Id.lyAccept);
            LyAccept.Click += delegate { this.Continue(); };

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void Continue()
        {
            OnBackPressed();
        }

        private void DrawRvDeletedProducts()
        {
            linerLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvDeletedProducts.NestedScrollingEnabled = false;
            RvDeletedProducts.HasFixedSize = true;
            RvDeletedProducts.SetLayoutManager(linerLayoutManager);
            _RemovedProductsAdapter = new RemovedProductsAdapter(ListProductos, this);
            RvDeletedProducts.SetAdapter(_RemovedProductsAdapter);
        }

        private void ProcessDeleteResponse()
        {
        }

        private void ValidateTotalProducts()
        {
        }
    }
}