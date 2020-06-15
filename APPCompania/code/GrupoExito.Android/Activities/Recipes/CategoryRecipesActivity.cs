using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
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
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Recipes
{
    [Activity(Label = "Recetas por categoría", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CategoryRecipesActivity : BaseActivity, IRecipeAdapter
    {
        #region Controls

        private RecyclerView RvRecipes;
        private RecipeAdapter RecipeAdapter;
        private LinearLayoutManager linearLayoutManager;
        private ImageView IvCategory;
        private TextView TvCategoryName;

        #endregion

        #region Properties

        private RecipesModel _myRecipesModel;
        private RecipesResponse Response { get; set; }
        private RecipeCategory recipeCategory;

        #endregion

        protected override void OnResume()
        {
            this.GetRecipes();
            base.OnResume();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityRecipeCategory);
            _myRecipesModel = new RecipesModel(new RecipesService(DeviceManager.Instance));

            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
        }

        private async Task GetRecipes()
        {
            Bundle bundle = Intent.Extras;

            try
            {
                if (bundle != null && !string.IsNullOrEmpty(bundle.GetString(ConstantPreference.RecipeCategory)))
                {
                    recipeCategory = JsonService.Deserialize<RecipeCategory>(bundle.GetString(ConstantPreference.RecipeCategory));

                    ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                    Response = await _myRecipesModel.GetRecipesByCategory(recipeCategory.Id.ToString());

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

                        if (Response != null && Response.Recipes != null && Response.Recipes.Any())
                        {
                            Glide.With(this).Load(recipeCategory.Image).Thumbnail(0.1f).Into(IvCategory);
                            TvCategoryName.Text = recipeCategory.Name;

                            RecipeAdapter = new RecipeAdapter(Response.Recipes, this, this);
                            RvRecipes.SetAdapter(RecipeAdapter);
                            ShowBodyLayout();
                        }
                        else
                        {
                            ShowErrorLayout(AppMessages.RecipesNotFound);
                        }
                    }
                }
            }
            catch
            {
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        protected override void EventError()
        {
            OnResume();
            base.EventNoInfo();
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));

            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                       FindViewById<LinearLayout>(Resource.Id.lyBody));

            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            FindViewById<LinearLayout>(Resource.Id.lyUser).Visibility = ViewStates.Invisible;

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            IvCategory = FindViewById<ImageView>(Resource.Id.ivCategory);
            TvCategoryName = FindViewById<TextView>(Resource.Id.tvCategoryName);

            RvRecipes = FindViewById<RecyclerView>(Resource.Id.rvRecipes);
            RvRecipes.HasFixedSize = true;
            linearLayoutManager = new LinearLayoutManager(this);
            RvRecipes.SetLayoutManager(linearLayoutManager);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void EditFonts()
        {
            TvCategoryName.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        public void OnRecipeClicked(Recipe recipe)
        {
            Intent intent = new Intent(this, typeof(RecipeDetailActivity));
            intent.PutExtra(ConstantPreference.Recipe, recipe.Id);
            StartActivity(intent);
        }
    }
}