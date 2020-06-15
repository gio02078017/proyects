using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Products;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Activities.InStoreServices
{
    [Activity(Label = "Servicios en almacén", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ServicesInStoreActivity : BaseActivity, Adapters.IItemMenu
    {
        #region Controls

        private RecyclerView RvMenu;
        private MenuServicesAdapter menuServicesAdapter;
        private LinearLayoutManager MenuLayoutManager;
        private TextView TvTitle;
        private NestedScrollView NsvMenu;

        #endregion

        #region Properties
        private MenusApplicationModel _servicesInStoreModel;
        private string activityName;

        #endregion

        public void OnMenuItemClicked(MenuItem menuItem)
        {
            Intent intent;

            switch (menuItem.ActionName)
            {
                case ConstMenuServicesInStore.Discounts:
                    intent = new Intent(this, typeof(MyDiscountsActivity));
                    intent.PutExtra(ConstantPreference.IsLobbyInMyDiscount, true);
                    StartActivity(intent);
                    break;
                case ConstMenuServicesInStore.CashDrawerTurn:
                    intent = new Intent(this, typeof(CashDrawerTurnActivity));
                    StartActivity(intent);
                    break;
                case ConstMenuServicesInStore.PriceChecker:
                    ValidatePermision(ConstantActivityName.PriceCheckerLocationActivity);
                    break;
                case ConstMenuServicesInStore.Stores:
                    ValidatePermision(ConstantActivityName.StoresActivity);
                    break;
                default:
                    break;
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        public override void RetryRequestPermissions(bool Retried)
        {
            if (Retried)
            {
                ValidatePermision(activityName);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityServicesInStore);
            _servicesInStoreModel = new MenusApplicationModel();
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsCarToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            FindViewById<ImageView>(Resource.Id.ivToolbarBack).Visibility = ViewStates.Visible;
            this.SetControlsProperties();
            this.EditFonts();
        }

        private void SetControlsProperties()
        {
            RlError = FindViewById<RelativeLayout>(Resource.Id.layoutError);
            NsvMenu = FindViewById<NestedScrollView>(Resource.Id.nsvMenu);
            TvError = RlError.FindViewById<TextView>(Resource.Id.tvError);
            BtnError = RlError.FindViewById<Button>(Resource.Id.btnError);
            IvError = RlError.FindViewById<ImageView>(Resource.Id.ivError);
            TvTitle = FindViewById<TextView>(Resource.Id.tvTitle);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            RvMenu = FindViewById<RecyclerView>(Resource.Id.rvMenu);
            MenuLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvMenu.NestedScrollingEnabled = false;
            RvMenu.HasFixedSize = true;
            RvMenu.SetLayoutManager(MenuLayoutManager);
            NsvMenu.ScrollTo(0, 0);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            this.GetServicesInStore();
        }

        private void GetServicesInStore()
        {
            try
            {
                var items = _servicesInStoreModel.GetMenuServicesInStore();

                if (items != null && items.Any())
                {
                    ShowBodyLayout();
                    menuServicesAdapter = new MenuServicesAdapter(items, this, this);
                    RvMenu.SetAdapter(menuServicesAdapter);
                }
                else
                {
                    ShowErrorLayout(AppMessages.ItemsNoFoundMenuMessage);
                }
            }
            catch (Exception exception)
            {
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ServicesInStoreFragment, ConstantMethodName.GetServicesInStore } };

                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitle).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
        }

        private new void ShowBodyLayout()
        {
            NsvMenu.Visibility = ViewStates.Visible;
            RlError.Visibility = ViewStates.Gone;
        }

        private new void ShowErrorLayout(string message, int resource = 0)
        {
            NsvMenu.Visibility = ViewStates.Gone;
            RlError.Visibility = ViewStates.Visible;
            TvError.Text = message;

            if (resource != 0)
            {
                IvError.SetImageResource(resource);
            }

            BtnError.Click += delegate { GetServicesInStore(); };
        }

        private void ValidatePermision(string activityName)
        {
            this.activityName = activityName;
            Intent intent;
            bool permission = GetLocationPermission();

            if (permission)
            {
                if (activityName.Equals(ConstantActivityName.PriceCheckerLocationActivity))
                {
                    intent = new Intent(this, typeof(PriceCheckerLocationActivity));
                }
                else
                {
                    intent = new Intent(this, typeof(StoresActivity));
                }

                StartActivity(intent);
            }
        }
    }
}