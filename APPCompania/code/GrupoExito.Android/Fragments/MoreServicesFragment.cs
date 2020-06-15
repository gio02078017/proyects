using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.InStoreServices;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Logic.Models.Generic;
using System;
using System.Collections.Generic;

namespace GrupoExito.Android.Fragments
{
    public class MoreServicesFragment : Fragment, Adapters.IItemMenu
    {
        #region Controls

        private RecyclerView RvMenu;
        private MenuServicesAdapter MenuServicesAdapter;
        private LinearLayoutManager MenuLayoutManager;
        private TextView TvTitle;
        private NestedScrollView NsvMenu;

        #endregion

        #region Porperties
      
        private MenusApplicationModel _model { get; set; }
        private Context _context;

        #endregion

        public void OnMenuItemClicked(MenuItem menuItem)
        {
            Intent intent;

            switch (menuItem.ActionName)
            {
                case ConstMenuOtherServices.Soat:
                    intent = new Intent(_context, typeof(GetSoatsActivity));
                    StartActivity(intent);         
                    break;
                case ConstMenuOtherServices.RechargePhone:
                    break;
                default:
                    break;
            }
        }

        public static MoreServicesFragment NewInstance(String question, String answer)
        {
            MoreServicesFragment fragment = new MoreServicesFragment();
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FragmentMoreServices, container, false);
            _context = container.Context;
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = new MenusApplicationModel();
            this.SetControlsProperties(view);
        }

        public override void OnResume()
        {
            base.OnResume();
            if (Activity != null)
            {
                ((MainActivity)Activity).RegisterScreen(AnalyticsScreenView.OtherServices);
            }
        }

        private void SetControlsProperties(View view)
        {
            NsvMenu = view.FindViewById<NestedScrollView>(Resource.Id.nsvMenu);
            TvTitle = view.FindViewById<TextView>(Resource.Id.tvTitle);
            TvTitle.SetTypeface(FontManager.Instance.GetTypeFace(base.Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);

            RvMenu = view.FindViewById<RecyclerView>(Resource.Id.rvMenu);
            MenuLayoutManager = new LinearLayoutManager(_context)
            {
                AutoMeasureEnabled = true
            };
            RvMenu.NestedScrollingEnabled = false;
            RvMenu.HasFixedSize = true;
            RvMenu.SetLayoutManager(MenuLayoutManager);
            NsvMenu.ScrollTo(0, 0);
            this.GetOtherServices();
        }

        private void GetOtherServices()
        {
            try
            {
                var items = _model.GetMenuOtherServices();
                MenuServicesAdapter = new MenuServicesAdapter(items, base.Context, this);
                RvMenu.SetAdapter(MenuServicesAdapter);
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.OtherServicesActivity, ConstantMethodName.GetOtherServices } };
                ((MainActivity)Activity).ShowAndRegisterMessageExceptions(exception, properties);
            }
        }
    }
}