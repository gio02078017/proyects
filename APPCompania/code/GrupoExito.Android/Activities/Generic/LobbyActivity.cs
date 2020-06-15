using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.InStoreServices;
using GrupoExito.Android.Activities.Recipes;
using GrupoExito.Android.Activities.Users;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Generic;
using GrupoExito.Entities.Responses.Generic;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Android.Activities.Generic
{
    [Activity(Label = "Lobby", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LobbyActivity : BaseAddressesActivity
    {
        #region Controls

        private LinearLayout LyShopping;
        private LinearLayout LyStikers;
        private LinearLayout LyDiscounts;
        private LinearLayout LyStoreServices;
        private LinearLayout LyOtherServices;
        private LinearLayout LyMenuContainer;
        private TextView TvTypeClientLobby;
        private TextView TvCloseSession;

        #endregion

        #region Properties

        public const string TAG = "MainActivity";
        private ContentsModel _contentModel;
        private PromotionResponse Response { get; set; }
        private ProductCarModel _productCarModel;

        #endregion

        public override void OnBackPressed()
        {
            FinishAndRemoveTask();
        }

        protected override void OnResume()
        {
            base.OnResume();
            this.SetUserName();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Lobby, typeof(LobbyActivity).Name);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.ActivityLobby);
            _contentModel = new ContentsModel(new ContentsService(DeviceManager.Instance));
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
           
            this.SetProperties();
            this.SetControlsProperties();
            this.PutItemsData();
            this.PutFonts();
            RegisterNotificationTags();            
        }


        private void SetProperties()
        {
            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }
        }

        private void SetControlsProperties()
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            LyShopping = FindViewById<LinearLayout>(Resource.Id.layoutShopping);
            LyStikers = FindViewById<LinearLayout>(Resource.Id.layoutStikers);
            LyDiscounts = FindViewById<LinearLayout>(Resource.Id.layoutDiscounts);
            LyStoreServices = FindViewById<LinearLayout>(Resource.Id.layoutStoreServices);
            LyOtherServices = FindViewById<LinearLayout>(Resource.Id.layoutOtherServices);
            LyMenuContainer = FindViewById<LinearLayout>(Resource.Id.lyMenuContainer);
            TvTypeClientLobby = FindViewById<TextView>(Resource.Id.tvTypeClientHome);
            TvCloseSession = FindViewById<TextView>(Resource.Id.tvCloseSession);
            LyMenuContainer.Visibility = ViewStates.Visible;

            LyShopping.Click += delegate { EventShopping(); };
            LyStikers.Click += delegate { EventStickers(); };
            LyDiscounts.Click += delegate { EventDiscounts(); };
            LyStoreServices.Click += delegate { EventStoreServices(); };
            LyOtherServices.Click += delegate { EventOtherServices(); };
            TvCloseSession.Click += delegate { LogOut(); };
        }

        private void PutItemsData()
        {
            LyShopping.FindViewById<ImageView>(Resource.Id.ivLobbyItem).SetImageResource(Resource.Drawable.compras);
            LyStikers.FindViewById<ImageView>(Resource.Id.ivLobbyItem).SetImageResource(Resource.Drawable.compras);
            LyDiscounts.FindViewById<ImageView>(Resource.Id.ivLobbyItem).SetImageResource(Resource.Drawable.conocer_descuento);
            LyStoreServices.FindViewById<ImageView>(Resource.Id.ivLobbyItem).SetImageResource(Resource.Drawable.idea_cocinar);
            LyOtherServices.FindViewById<ImageView>(Resource.Id.ivLobbyItem).SetImageResource(Resource.Drawable.servicios_almacen);

            LyShopping.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).Text = AppMessages.ShoppingTitle;
            LyStikers.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).Text = AppMessages.StickersTitle;
            LyDiscounts.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).Text = AppMessages.DiscountsTitle;
            LyStoreServices.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).Text = AppMessages.WhatCookToday;
            LyOtherServices.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).Text = AppMessages.StoreServicesTitle;

            LyShopping.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).Text = AppMessages.ShoppingMessage;
            LyStikers.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).Text = AppMessages.StickersMessage;
            LyDiscounts.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).Text = AppMessages.DiscountsMessage;
            LyStoreServices.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).Text = AppMessages.WhatCookTodayMessage;
            LyOtherServices.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).Text = AppMessages.StoreServicesMessage;
        }

        private void PutFonts()
        {
            LyShopping.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            LyStikers.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            LyDiscounts.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            LyStoreServices.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            LyOtherServices.FindViewById<TextView>(Resource.Id.tvLobbyItemTitle).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvTypeClientLobby.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            LyShopping.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            LyStikers.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            LyDiscounts.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            LyStoreServices.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            LyOtherServices.FindViewById<TextView>(Resource.Id.tvLobbyItemMessage).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                        
            FindViewById<TextView>(Resource.Id.tvLobbyTitle).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvLobbyHello).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvLobbyUser).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvTypeClientHome).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvCloseSession).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
           
        }

        private void SetUserName()
        {
            if (ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.FirstName))
            {
                var nameSplitted = ParametersManager.UserContext.FirstName.Split(' ');
                var userName = nameSplitted != null ? StringFormat.CutString(nameSplitted[0], 15) : StringFormat.CutString(ParametersManager.UserContext.FirstName, 15);
                FindViewById<TextView>(Resource.Id.tvLobbyUser).Text = StringFormat.Capitalize(userName) + "!";
            }

            if (AppServiceConfiguration.SiteId.Equals("carulla"))
            {
                TvTypeClientLobby.Text = ParametersManager.UserContext != null && ParametersManager.UserContext.UserType != null ? ParametersManager.UserContext.UserType.Name : string.Empty;
                TvTypeClientLobby.Visibility = ParametersManager.UserContext != null && ParametersManager.UserContext.UserType != null ?
                    ViewStates.Visible : ViewStates.Gone;
            }
        }

        private void EventOtherServices()
        {
            Intent intent = new Intent(this, typeof(ServicesInStoreActivity));
            StartActivity(intent);
        }

        private void EventStoreServices()
        {
            Intent intent = new Intent(this, typeof(MyRecipesActivity));
            StartActivity(intent);
        }

        private void EventDiscounts()
        {
            Intent intent = new Intent(this, typeof(MyDiscountsActivity));
            intent.PutExtra(ConstantPreference.IsLobbyInMyDiscount, true);
            StartActivity(intent);
        }

        private void EventShopping()
        {
            RunOnUiThread(() =>
            {
                if ((ParametersManager.UserContext != null && ParametersManager.UserContext.Store != null)
                || (ParametersManager.UserContext != null && ParametersManager.UserContext.Address != null))
                {
                    OnMainActiviy();
                }
                else
                {
                    OnWelcomeActivity();
                }
            });
        }

        private void EventStickers()
        {
            OnStickers();
        }

        private void OnMainActiviy()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void OnStickers()
        {
            Intent intent = new Intent(this, typeof(MyStickersActivity));
            StartActivity(intent);
        }

        private void OnWelcomeActivity()
        {
            Intent intent = new Intent(this, typeof(WelcomeActivity));
            StartActivity(intent);
        }

        protected override void OnPostResume()
        {
            base.OnPostResume();
        }

        private void LogOut()
        {
            string firebaseToken = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.FirebaseToken);
            string mobileId = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.MobileId);
            bool tutorialWelcome = DeviceManager.Instance.GetAccessPreference(ConstNameViewTutorial.Welcome, false);
            bool tutorialHome = DeviceManager.Instance.GetAccessPreference(ConstNameViewTutorial.Home, false);

            DeviceManager.Instance.DeleteAccessPreference();
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.FirebaseToken, firebaseToken);
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.MobileId, mobileId);

            DeviceManager.Instance.SaveAccessPreference(ConstNameViewTutorial.Welcome, tutorialWelcome);
            DeviceManager.Instance.SaveAccessPreference(ConstNameViewTutorial.Home, tutorialHome);

            _productCarModel.FlushCar();
            RegisterNotificationTags();
            Intent intent = new Intent(this, typeof(LoginActivity));
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            StartActivity(intent);
        }       
    }
}