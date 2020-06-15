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
using GrupoExito.Android.Activities.Products;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoExito.Android.Fragments
{
    public class CategoriesFragment : Fragment, ICategoryAdapter
    {
        #region Controls

        private RecyclerView RvCategories;
        private CategoryAdapter CategoryAdapter;
        private TextView TvOurCategories, TvError;
        private CategoriesModel _categoriesModel;
        private Button BtnError;
        private NestedScrollView RlBody;
        private RelativeLayout RlError;
        private ImageView IvError;

        #endregion

        #region Properties

        private IList<Category> categories;

        #endregion

        public static CategoriesFragment NewInstance(String question, String answer)
        {
            CategoriesFragment categoriesFragment = new CategoriesFragment();
            return categoriesFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FragmentCategories, container, false);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            this.categories = new List<Category>();
            _categoriesModel = new CategoriesModel(new CategoriesService(DeviceManager.Instance));
            this.SetControlsProperties(view);

            ((BaseProductActivity)Activity).RunOnUiThread(async () =>
            {
                await GetCategories();
            });
        }

        public override void OnResume()
        {
            base.OnResume();
            if (Activity != null)
            {
                ((MainActivity)Activity).RegisterScreen(AnalyticsScreenView.Categories);
            }
        }

        private async Task GetCategories()
        {
            try
            {
                CategoriesResponse response = await _categoriesModel.GetCategories();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                }
                else
                {
                    ShowBodyLayout();
                    categories = response.Categories;
                    this.DrawCategoryList();
                }
            }
            catch (Exception exception)
            {
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CategoriesFragment, ConstantMethodName.GetCategories } };

                if (Activity != null)
                {
                    ((BaseProductActivity)Activity).RegisterMessageExceptions(exception, properties);
                }
            }
        }

        private void SetControlsProperties(View view)
        {
            RlError = view.FindViewById<RelativeLayout>(Resource.Id.layoutError);
            RlBody = view.FindViewById<NestedScrollView>(Resource.Id.nsvBody);
            TvError = RlError.FindViewById<TextView>(Resource.Id.tvError);
            BtnError = RlError.FindViewById<Button>(Resource.Id.btnError);
            IvError = RlError.FindViewById<ImageView>(Resource.Id.ivError);

            TvOurCategories = view.FindViewById<TextView>(Resource.Id.tvOurCategories);
            TvOurCategories.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            BtnError.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            RvCategories = view.FindViewById<RecyclerView>(Resource.Id.rvCategories);
            GridLayoutManager gridLayoutManager = new GridLayoutManager(Activity, 2, GridLayoutManager.Vertical, false);
            RvCategories.HasFixedSize = true;
            RvCategories.SetLayoutManager(gridLayoutManager);
            gridLayoutManager.AutoMeasureEnabled = true;
            RvCategories.NestedScrollingEnabled = false;
            RvCategories.HasFixedSize = false;
        }

        private void SetFonts()
        {
            BtnError.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvError.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvOurCategories.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        public void OnCategoryClicked(Category category)
        {
            var productsView = new Intent(Context, typeof(ProductsActivity));
            productsView.PutExtra(ConstantPreference.Category, JsonService.Serialize(category));
            StartActivity(productsView);
        }

        private void DrawCategoryList()
        {
            if (Activity != null)
            {
                CategoryAdapter = new CategoryAdapter(this.categories, Activity, this);

                Activity.RunOnUiThread(() =>
                {
                    RvCategories.SetAdapter(CategoryAdapter);
                });
            }
        }

        private void ShowBodyLayout()
        {
            RlBody.Visibility = ViewStates.Visible;
            RlError.Visibility = ViewStates.Gone;
        }

        private void ShowErrorLayout(string message, int resource = 0)
        {
            RlBody.Visibility = ViewStates.Gone;
            RlError.Visibility = ViewStates.Visible;
            TvError.Text = message;

            if (resource != 0)
            {
                IvError.SetImageResource(resource);
            }

            BtnError.Click += async delegate { await GetCategories(); };
        }
    }  
}