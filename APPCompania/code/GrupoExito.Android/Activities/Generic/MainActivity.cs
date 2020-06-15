using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Addresses;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Fragments;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Android.Widgets;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Generic;
using GrupoExito.Entities.Entiites.Generic.Contents;
using GrupoExito.Entities.Parameters.Generic;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Generic
{
    [Activity(Label = "Home", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseProductActivity, ViewPager.IOnPageChangeListener
    {
        #region Controls

        private TabLayout Tabs;
        private ViewPager ViewPager;
        private TabAdapter TabAdapter;
        private ControlSplashPromotions controlPromotions;

        private readonly int[] TabIcons = {
            Resource.Drawable.casa_menu,
            Resource.Drawable.categorias,
            Resource.Drawable.listas,
            Resource.Drawable.tiendas,
            Resource.Drawable.menu_mas_opciones
        };

        #endregion

        #region Properties

        private int Position;
        private IHome Home;
        private bool KeepLobby { get; set; }
        private ContentsModel _contentModel;

        #endregion

        #region Protected Methods

        protected override void RefreshProductList(Product ActualProduct)
        {
            if (this.Home != null)
            {
                Home.RefreshProductList(ActualProduct);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityMain);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            _contentModel = new ContentsModel(new ContentsService(DeviceManager.Instance));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            SetTabs();
            var root = FindViewById<LinearLayout>(Resource.Id.rootLayout);
            root.RequestFocus();

            string preferenceName = ConstNameViewTutorial.Home;
            bool response = DeviceManager.Instance.GetAccessPreference(preferenceName, false);

            if (!response)
            {
                Intent intent = new Intent(this, typeof(TutorialsActivity));
                intent.PutExtra(ConstantPreference.ActivityTutorial, preferenceName);
                StartActivity(intent);
            }

            KeepLobby = Intent.GetBooleanExtra(ConstantPreference.KeepLobby, false);

            RunOnUiThread(async () =>
            {
                await GetPromotions();
            });
        }

        protected override void OnResume()
        {
            base.OnResume();

            ParametersManager.FromProductsActivity = false;
            Glide.With(ApplicationContext).OnStart();

            if (ParametersManager.ChangeAddress)
            {
                RunOnUiThread(async () =>
                {
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.DoNotShowAgain, false);
                    await GetPromotions();
                });
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Glide.With(ApplicationContext).PauseRequests();
        }

        public override void RetryRequestPermissions(bool Retried)
        {
            if (Retried)
            {
                if (this.Home != null)
                {
                    Home.RetryPermmision(Retried);
                }
            }
        }

        #endregion

        #region Public Methods

        public override void OnBackPressed()
        {
            if (Position != 0)
            {
                ViewPager.Post(() => { ViewPager.SetCurrentItem(0, false); });
            }
            else
            {
                if (KeepLobby)
                {
                    Intent intent = new Intent(this, typeof(LobbyActivity));
                    intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    FinishAndRemoveTask();
                }
            }
        }

        public void SetHomeInterface(IHome home)
        {
            this.Home = home;
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        public bool CustomizeTabs()
        {
            try
            {
                RunOnUiThread(() =>
                {
                    PutTabTextsTypeFace();
                    DefineTabViewsPosition();
                    Tabs.Visibility = ViewStates.Visible;
                    Tabs.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.slide_in_up));
                });

                return true;

            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MainActivity, ConstantMethodName.CustomizeTabs } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }

            return false;
        }

        public void OnPageScrollStateChanged(int state)
        {
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }

        public void OnPageSelected(int position)
        {
            bool searcherVisibility = position < 3 ? true : false;
            DefineSearcherVisibility(searcherVisibility);
            this.Position = position;
        }

        public void ShowAddressesDialog()
        {
            Intent intent = new Intent(this, typeof(AddressesDialogActivity));
            StartActivity(intent);
        }

        #endregion

        #region Protected Methods
        protected override void EventCloseModalMessageInfo()
        {
            base.EventCloseModalMessageInfo();
            MessageInfoDialog.Hide();
            if (this.Home != null)
            {
                Home.CallShowAddressDialog();
            }
        }
        #endregion

        #region Private Methods    

        private void GoToLobby()
        {
            if (this.Home != null)
            {
                if (Position == 0)
                {
                    Home.GoToLobby();
                }
            }
        }

        private void SetControlsProperties()
        {
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.mainToolbar);
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetSearcher(FindViewById<LinearLayout>(Resource.Id.searcher));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            FindViewById<TextView>(Resource.Id.tvSearcher).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<ImageView>(Resource.Id.ivToolbarBack).Visibility = ViewStates.Gone;
            FindViewById<ImageView>(Resource.Id.ivLogo).Click += delegate { GoToLobby(); };
            ViewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            ViewPager.OffscreenPageLimit = 5;
        }

        private void EventHideArrow(bool ArrowVisible)
        {
            FindViewById<ImageView>(Resource.Id.ivToolbarBack).Visibility = ViewStates.Gone;
            if (ArrowVisible)
            {
                FindViewById<ImageView>(Resource.Id.ivToolbarBack).Visibility = ViewStates.Visible;
            }
        }

        private void SetTabs()
        {
            RunOnUiThread(() =>
            {
                TabAdapter = new TabAdapter(SupportFragmentManager);
                TabAdapter.AddFragment(new HomeFragment(), Resources.GetString(Resource.String.str_home_menu));
                TabAdapter.AddFragment(new CategoriesFragment(), Resources.GetString(Resource.String.str_categories));
                TabAdapter.AddFragment(new ShoppingListsFragment(), Resources.GetString(Resource.String.str_lists));
                TabAdapter.AddFragment(new ServicesInStoreFragment(), Resources.GetString(Resource.String.str_your_store));
                TabAdapter.AddFragment(new MoreServicesFragment(), Resources.GetString(Resource.String.str_more));
                ViewPager.AddOnPageChangeListener(this);
                ViewPager.Adapter = TabAdapter;
                this.DrawMenu();
            });
        }

        private void DrawMenu()
        {
            Tabs = FindViewById<TabLayout>(Resource.Id.tabs);
            Tabs.SetupWithViewPager(ViewPager);
        }

        private TextView GetTabText(int position)
        {
            return (TextView)(((LinearLayout)((LinearLayout)Tabs.GetChildAt(0)).GetChildAt(position)).GetChildAt(1));
        }

        private ImageView GetTabIcon(int position)
        {
            return (ImageView)(((LinearLayout)((LinearLayout)Tabs.GetChildAt(0)).GetChildAt(position)).GetChildAt(0));
        }

        private void PutTabTextsTypeFace()
        {
            for (int i = 0; i < Tabs.TabCount; i++)
            {
                TextView customTab = (TextView)LayoutInflater.Inflate(Resource.Layout.tab, null);
                customTab.Text = TabAdapter.FragmentTitleList[i];
                Drawable img = ContextCompat.GetDrawable(this, TabIcons[i]);
                img.SetBounds(0, 0, 50, 50);
                customTab.SetCompoundDrawables(null, img, null, null);
                customTab.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
                ViewGroup.LayoutParams layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                customTab.LayoutParameters = layoutParams;
                Tabs.GetTabAt(i).SetCustomView(customTab);
            }
        }

        private void DefineTabViewsPosition()
        {
            for (int i = 0; i < Tabs.TabCount; i++)
            {
                Tabs.GetTabAt(i).CustomView.ScaleY = -1;
            }
        }

        private async Task GetPromotions()
        {
            try
            {
                var parameters = new PromotionParameters();
                var Response = await _contentModel.GetPromotions(parameters);

                if (Response.Result != null && Response.Result.HasErrors && Response.Result.Messages != null)
                {
                }
                else
                {
                    if (Response.Promotions != null && Response.Promotions.Any())
                    {
                        if (Response.HaveNewPromotion)
                        {
                            bool value = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.DoNotShowAgain, false);

                            if (!value)
                            {
                                ShowPromotions(Response.Promotions);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MainActivity, ConstantMethodName.GetPromotions } };
                RegisterMessageExceptions(exception, properties);

            }
        }

        private void ShowPromotions(IList<Promotion> listPromotions)
        {
            controlPromotions = FindViewById<ControlSplashPromotions>(Resource.Id.cPromotions);
            controlPromotions.Visibility = ViewStates.Visible;
            controlPromotions.AddImagesArray(listPromotions);
            controlPromotions.GetButtonHide().Text = AppMessages.MessageBtnDoNotShowAgain;
            controlPromotions.GetButtonHide().SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            controlPromotions.GetButtonHide().Click += delegate
            {
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.DoNotShowAgain, true);
                controlPromotions.Visibility = ViewStates.Gone;
            };
        }

        public void RegisterScreen(string nameScreen)
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, nameScreen, typeof(MainActivity).Name);
        }

        #endregion
    }
}
