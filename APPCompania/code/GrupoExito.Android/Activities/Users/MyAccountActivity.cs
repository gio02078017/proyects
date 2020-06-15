using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Addresses;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.InStoreServices;
using GrupoExito.Android.Activities.Orders;
using GrupoExito.Android.Activities.Payments;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Generic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;

namespace GrupoExito.Android.Activities.Users
{
    [Activity(Label = "Mi cuenta", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyAccountActivity : BaseActivity, Adapters.IItemMenu
    {
        #region Controls

        private RecyclerView RvMyAccount;
        private MyAccountAdapter myAccountAdapter;
        private LinearLayout LyNameClientAccount;
        private TextView TvNameClientAccount, TvTypeClientAccount, TvCloseSession;
        private ImageView IvEditPerfilAccount;
        private NestedScrollView NsvMyAccount;
        private GridLayoutManager manager;
        private bool ispair = false;

        #endregion

        #region Properties

        private MyAccount MyAccount { get; set; }
        private MyAccountModel _myAccountModel;
        private ProductCarModel _productCarModel;

        #endregion

        public override void OnBackPressed()
        {
            Finish();
        }

        public void CallActivity(Intent intent)
        {
            StartActivity(intent);
        }

        public void OnMenuItemClicked(MenuItem menuItem)
        {
            Intent intent = null;

            switch (menuItem.ActionName)
            {
                case ConstMenuMyAccount.Prime:
                    intent = new Intent(this, typeof(ClientPrimeActivity));
                    this.CallActivity(intent);
                    break;
                case ConstMenuMyAccount.ChangeKey:
                    intent = new Intent(this, typeof(ChangePasswordActivity));
                    this.CallActivity(intent);
                    break;
                case ConstMenuMyAccount.MyAddresses:
                    intent = new Intent(this, typeof(MyAddressesActivity));
                    this.CallActivity(intent);
                    break;
                case ConstMenuMyAccount.MyCards:
                    intent = new Intent(this, typeof(MyCardsActivity));
                    this.CallActivity(intent);
                    break;
                case ConstMenuMyAccount.MyOrders:
                    intent = new Intent(this, typeof(MyOrdersActivity));
                    this.CallActivity(intent);
                    break;
                case ConstMenuMyAccount.ContactUs:
                    intent = new Intent(this, typeof(ContactUsActivity));
                    this.CallActivity(intent);
                    break;
                case ConstMenuMyAccount.Notifications:
                    intent = new Intent(this, typeof(NotificationsActivity));
                    this.CallActivity(intent);
                    break;
                case ConstMenuMyAccount.MyPoints:
                    intent = new Intent(this, typeof(MyPointsActivity));
                    this.CallActivity(intent);
                    break;
                case ConstMenuMyAccount.MyStickers:
                    intent = new Intent(this, typeof(MyStickersActivity));
                    this.CallActivity(intent);
                    break;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityMyAccount);
            _myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            MyAccount = new MyAccount();
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            GetMyAccount();
            UpdateToolbarName();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.MyAccount, typeof(MyAccountActivity).Name);
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
            this.CallActivity(intent);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            TvNameClientAccount = FindViewById<TextView>(Resource.Id.tvNameClientAccount);
            TvTypeClientAccount = FindViewById<TextView>(Resource.Id.tvTypeClientAccount);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            RvMyAccount = FindViewById<RecyclerView>(Resource.Id.rvMyAccount);
            NsvMyAccount = FindViewById<NestedScrollView>(Resource.Id.nsvMyAccount);
            LyNameClientAccount = FindViewById<LinearLayout>(Resource.Id.lyNameClientAccount);
            TvCloseSession = FindViewById<TextView>(Resource.Id.tvCloseSession);
            IvEditPerfilAccount = FindViewById<ImageView>(Resource.Id.ivEditPerfilAccount);

            manager = new GridLayoutManager(this, 2, GridLayoutManager.Vertical, false)
            {
                AutoMeasureEnabled = true
            };

            RvMyAccount.NestedScrollingEnabled = false;
            RvMyAccount.HasFixedSize = false;
            RvMyAccount.SetLayoutManager(manager);

            NsvMyAccount.ScrollTo(0, 0);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            TvCloseSession.Click += delegate
            {
                this.LogOut();
            };

            IvEditPerfilAccount.Click += delegate
            {
                this.GoToEditProfile();
            };
        }

        private void GoToEditProfile()
        {
            Intent intent = new Intent(this, typeof(EditProfileActivity));
            StartActivity(intent);
        }

        private void GoToClientPrime()
        {
            Intent intent = new Intent(this, typeof(ClientPrimeActivity));
            StartActivity(intent);
        }

        private void GetMyAccount()
        {
            try
            {
                MyAccount = _myAccountModel.GetMyAccount();

                TvNameClientAccount.Text = (ParametersManager.UserContext != null && ParametersManager.UserContext.FirstName != null) ?
                    StringFormat.Capitalize(StringFormat.SplitName(ParametersManager.UserContext.FirstName)) : string.Empty;

                if (AppServiceConfiguration.SiteId.Equals("carulla"))
                {
                    TvTypeClientAccount.Text = ParametersManager.UserContext != null && ParametersManager.UserContext.UserType != null ? ParametersManager.UserContext.UserType.Name : string.Empty;
                    TvTypeClientAccount.Visibility = ParametersManager.UserContext != null && ParametersManager.UserContext.UserType != null ?
                        ViewStates.Visible : ViewStates.Gone;
                }

                ispair = false;

                if (MyAccount.Menu.Count % 2 == 0)
                {
                    ispair = true;
                }

                for (int i = 0; i < MyAccount.Menu.Count; i++)
                {
                    MyAccount.Menu[i].Ispair = ispair;
                }

                myAccountAdapter = new MyAccountAdapter(MyAccount.Menu, this, this);
                RvMyAccount.SetAdapter(myAccountAdapter);
                manager.SetSpanSizeLookup(new SpanSizeLookup(ispair));
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyAccountActivity, ConstantMethodName.GetMyAccount } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void EditFonts()
        {
            TvNameClientAccount.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvTypeClientAccount.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCloseSession.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvHello).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        internal class SpanSizeLookup : GridLayoutManager.SpanSizeLookup
        {
            private bool Pair { get; set; }
            public SpanSizeLookup(bool isPair)
            {
                this.Pair = isPair;
            }

            public override int GetSpanSize(int position)
            {
                if (!Pair && position == 0)
                {
                    return 2;
                }

                return 1;
            }
        }
    }
}