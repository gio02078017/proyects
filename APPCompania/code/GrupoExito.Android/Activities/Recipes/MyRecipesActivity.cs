using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Request.Target;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Recipes;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Recipes;
using GrupoExito.Logic.Models.Recipes;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Android.Activities.Recipes
{
    [Activity(Label = "Mis recetas", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyRecipesActivity : BaseActivity, IRecipeCategoryAdapter, IRequestListener
    {
        #region Controls

        private RecyclerView RvRecipeCategories;
        private RecipeCategoryAdapter RecipeCategoriesAdapter;
        private LinearLayoutManager linearLayoutManager;
        private ImageView IvDayRecipe;
        private LinearLayout LyRecipeCategories, LyMoreOptions;
        private NestedScrollView NsvBody;
        private View ViewEnd;
        private TextView TvDayRecipe;
        private Button BtnDayRecipe;
        private ShimmerFrameLayout ShimmerFrameLayout;
        private CardView CvShimmer;

        #endregion

        #region Properties

        private RecipesModel _myRecipesModel;
        private RecipeCategoryResponse Response { get; set; }

        #endregion

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityMyRecipes);
            _myRecipesModel = new RecipesModel(new RecipesService(DeviceManager.Instance));

            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.ChangeDayRecipeSize();
            this.EditFonts();
            ShimmerFrameLayout.StartShimmer();
            this.GetRecipeCategories();
        }

        private async void GetRecipeCategories()
        {
            try
            {

                Response = await _myRecipesModel.GetCategories();

                if (Response.Result != null && Response.Result.HasErrors && Response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(Response.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    if (Response != null && Response.Categories != null && Response.Categories.Any())
                    {
                        HideProgressDialog();
                        RecipeCategoriesAdapter = new RecipeCategoryAdapter(Response.Categories, this, this);
                        RvRecipeCategories.SetAdapter(RecipeCategoriesAdapter);

                        var requestOptions = new RequestOptions()
                              .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.All)
                              .Placeholder(Resource.Drawable.sin_imagen)
                              .Error(Resource.Drawable.sin_imagen)
                              .DontAnimate()
                              .DontTransform());

                        Glide.With(this).Load(Response.Recipe.Image).Listener(this).Into(IvDayRecipe);
                        TvDayRecipe.Text = Response.Recipe.Title;
                        ShowBodyLayout();
                    }
                    else
                    {
                        HideProgressDialog();
                        ShowErrorLayout(AppMessages.RecipesNotFound);
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyRecipesActivity, ConstantMethodName.GetRecipeCategories } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void ChangeDayRecipeSize()
        {
            Display display = WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
        }

        protected override void EventError()
        {
            this.GetRecipeCategories();
            base.EventNoInfo();
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));

            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                FindViewById<NestedScrollView>(Resource.Id.nsvMyRecipes));

            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            FindViewById<LinearLayout>(Resource.Id.lyUser).Visibility = ViewStates.Invisible;

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            IvDayRecipe = FindViewById<ImageView>(Resource.Id.ivDayRecipe);
            BtnDayRecipe = FindViewById<Button>(Resource.Id.btnDayRecipe);

            ShimmerFrameLayout = FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_view_container);
            RvRecipeCategories = FindViewById<RecyclerView>(Resource.Id.rvRecipeCategories);
            RvRecipeCategories.HasFixedSize = true;
            linearLayoutManager = new LinearLayoutManager(this);
            RvRecipeCategories.SetLayoutManager(linearLayoutManager);
            NsvBody = FindViewById<NestedScrollView>(Resource.Id.nsvMyRecipes);
            ViewEnd = FindViewById(Resource.Id.viewEnd);
            TvDayRecipe = FindViewById<TextView>(Resource.Id.tvDayRecipe);
            CvShimmer = FindViewById<CardView>(Resource.Id.cvShimmer);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            BtnDayRecipe.Click += delegate { CallRecipeDetail(); };

            LyMoreOptions = FindViewById<LinearLayout>(Resource.Id.lyMoreOptions);
            LyRecipeCategories = FindViewById<LinearLayout>(Resource.Id.lyRecipeCategories);

            LyMoreOptions.Click += delegate { this.ShowRecipeCategories(); };
        }

        private void CallRecipeDetail()
        {
            if (Response != null)
            {
                Intent intent = new Intent(this, typeof(RecipeDetailActivity));
                intent.PutExtra(ConstantPreference.Recipe, Response.Recipe.Id);
                StartActivity(intent);
            }
        }

        private void ShowRecipeCategories()
        {
            LyMoreOptions.Visibility = ViewStates.Gone;
            LyRecipeCategories.Visibility = ViewStates.Visible;
            ScrollEnd();
        }

        private void EditFonts()
        {
            TvDayRecipe.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
        }

        public void OnCategoryClicked(RecipeCategory category)
        {
            Intent intent = new Intent(this, typeof(CategoryRecipesActivity));
            intent.PutExtra(ConstantPreference.RecipeCategory, JsonService.Serialize<RecipeCategory>(category));
            StartActivity(intent);
        }

        private void ScrollEnd()
        {
            Handler handler = new Handler();
            void productAction()
            {
                NsvBody.SmoothScrollingEnabled = true;
                int scrollTo = ((View)ViewEnd.Parent.Parent).Top + ViewEnd.Top + 20;
                NsvBody.SmoothScrollTo(0, scrollTo);
            }

            handler.PostDelayed(productAction, 200);
        }

        public bool OnLoadFailed(GlideException p0, Java.Lang.Object p1, ITarget p2, bool p3)
        {
            return false;
        }

        public bool OnResourceReady(Java.Lang.Object p0, Java.Lang.Object p1, ITarget p2, DataSource p3, bool p4)
        {
            ShimmerFrameLayout.StopShimmer();
            ShimmerFrameLayout.Visibility = ViewStates.Gone;
            IvDayRecipe.Visibility = ViewStates.Visible;
            IvDayRecipe.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.slide_from_left));
            LyMoreOptions.Visibility = ViewStates.Visible;
            return false;
        }
    }
}