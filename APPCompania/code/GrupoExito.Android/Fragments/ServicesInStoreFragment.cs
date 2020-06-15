using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.InStoreServices;
using GrupoExito.Android.Activities.Products;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Fragments
{
    public class ServicesInStoreFragment : Fragment, Adapters.IItemMenu, IHome
    {
        #region Controls

        private RecyclerView RvMenu;
        private MenuServicesAdapter menuServicesAdapter;
        private LinearLayoutManager MenuLayoutManager;
        private TextView TvTitle;
        private NestedScrollView NsvMenu;
        private Button BtnError;
        private RelativeLayout RlError;
        private ImageView IvError;
        private TextView TvError;

        #endregion

        #region Properties

        private Context context;
        private MenusApplicationModel _servicesInStoreModel;
        private string ActivityName { get; set; }

        #endregion

        public static ServicesInStoreFragment NewInstance(String question, String answer)
        {
            ServicesInStoreFragment servicesInStoreFragment = new ServicesInStoreFragment();
            return servicesInStoreFragment;
        }

        public override void OnResume()
        {
            base.OnResume();
            if (Activity != null)
            {
                ((MainActivity)Activity).RegisterScreen(AnalyticsScreenView.ServicesInStore);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FragmentServicesInStore, container, false);
            context = container.Context;
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _servicesInStoreModel = new MenusApplicationModel();
            this.SetControlsProperties(view, savedInstanceState);
        }

        public void OnMenuItemClicked(MenuItem menuItem)
        {
            Intent intent;

            switch (menuItem.ActionName)
            {
                case ConstMenuServicesInStore.Discounts:
                    intent = new Intent(context, typeof(MyDiscountsActivity));
                    StartActivity(intent);
                    break;
                case ConstMenuServicesInStore.CashDrawerTurn:
                    intent = new Intent(context, typeof(CashDrawerTurnActivity));
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

        private void SetControlsProperties(View view, Bundle savedInstanceState)
        {
            RlError = view.FindViewById<RelativeLayout>(Resource.Id.layoutError);
            NsvMenu = view.FindViewById<NestedScrollView>(Resource.Id.nsvMenu);
            TvError = RlError.FindViewById<TextView>(Resource.Id.tvError);
            BtnError = RlError.FindViewById<Button>(Resource.Id.btnError);
            IvError = RlError.FindViewById<ImageView>(Resource.Id.ivError);
            TvTitle = view.FindViewById<TextView>(Resource.Id.tvTitle);
            TvTitle.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);

            RvMenu = view.FindViewById<RecyclerView>(Resource.Id.rvMenu);
            MenuLayoutManager = new LinearLayoutManager(context)
            {
                AutoMeasureEnabled = true
            };
            RvMenu.NestedScrollingEnabled = false;
            RvMenu.HasFixedSize = true;
            RvMenu.SetLayoutManager(MenuLayoutManager);
            NsvMenu.ScrollTo(0, 0);
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
                    menuServicesAdapter = new MenuServicesAdapter(items, Activity, this);
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

                if (Activity != null)
                {
                    ((MainActivity)Activity).ShowAndRegisterMessageExceptions(exception, properties);
                }
            }
        }

        private void ShowBodyLayout()
        {
            NsvMenu.Visibility = ViewStates.Visible;
            RlError.Visibility = ViewStates.Gone;
        }

        private void ShowErrorLayout(string message, int resource = 0)
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
            ActivityName = activityName;
            Intent intent;
            bool permission = ((BaseActivity)Activity).GetLocationPermission();

            if (permission)
            {
                if (ActivityName.Equals(ConstantActivityName.PriceCheckerLocationActivity))
                {
                    intent = new Intent(Activity, typeof(PriceCheckerLocationActivity));
                }
                else
                {
                    intent = new Intent(Activity, typeof(StoresActivity));
                }

                StartActivity(intent);
            }
        }

        public void RefreshProductList(Product ActualProduct)
        {
            ////
        }

        public void RetryPermmision(bool retry)
        {
            if (retry)
            {
                ValidatePermision(ActivityName);
            }
        }

        public void GoToLobby()
        {
            ////
        }

        public void CallShowAddressDialog()
        {
            ////
        }
    }
}