using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Logic.Models.Generic;
using System;
using System.Collections.Generic;

namespace GrupoExito.Android.Activities.InStoreServices
{
    [Activity(Label = "Otros servicios", ScreenOrientation = ScreenOrientation.Portrait)]
    public class OtherServicesActivity : BaseActivity, Adapters.IItemMenu
    {
        #region Controls

        private RecyclerView RvMenu;
        private MenuServicesAdapter MenuServicesAdapter;
        private LinearLayoutManager MenuLayoutManager;
        private TextView TvTitle;
        private NestedScrollView NsvMenu;
        private LinearLayout LySearch;
        private LinearLayout LyError;

        #endregion

        #region Porperties

        private IList<DocumentType> DocumentTypes;
        private MenusApplicationModel _model { get; set; }

        #endregion

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        public void OnMenuItemClicked(MenuItem menuItem)
        {
            Intent intent;

            switch (menuItem.ActionName)
            {
                case ConstMenuOtherServices.Soat:
                    intent = new Intent(this, typeof(GetSoatsActivity));
                    StartActivity(intent);
                    break;
                case ConstMenuOtherServices.RechargePhone:
                    break;
                default:
                    break;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void EventError()
        {
            base.EventError();
            ShowBodyLayout();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityOtherServices);
            _model = new MenusApplicationModel();
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsCarToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            SetControlsProperties();
        }     

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            TvTitle = FindViewById<TextView>(Resource.Id.tvTitle);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            NsvMenu = FindViewById<NestedScrollView>(Resource.Id.nsvMenu);
            RvMenu = FindViewById<RecyclerView>(Resource.Id.rvMenu);
            MenuLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvMenu.NestedScrollingEnabled = false;
            RvMenu.HasFixedSize = true;
            RvMenu.SetLayoutManager(MenuLayoutManager);
            this.GetOtherServices();
            NsvMenu.ScrollTo(0, 0);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void GetOtherServices()
        {
            try
            {
                var items = _model.GetMenuOtherServices();
                MenuServicesAdapter = new MenuServicesAdapter(items, this, this);
                RvMenu.SetAdapter(MenuServicesAdapter);
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.OtherServicesActivity, ConstantMethodName.GetOtherServices } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }       
    }
}