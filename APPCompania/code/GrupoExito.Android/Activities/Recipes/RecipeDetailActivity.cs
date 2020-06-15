using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
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

namespace GrupoExito.Android.Activities.Recipes
{
    [Activity(Label = "Detalle de Receta", ScreenOrientation = ScreenOrientation.Portrait)]
    public class RecipeDetailActivity : BaseActivity
    {
        #region Controls

        private TextView TvRecipeName, TvRecipeDescription, TvDifficultyLabel,
                         TvDifficulty, TvTimeLabel, TvTime, TvIngredientsLabel,
                         TvPreparationLabel;
        private RecyclerView RvIngredients, RvPreparation;
        private RecipeStepAdapter RecipeStepIngredientsAdapter, RecipeStepPreparationAdapter;
        private LinearLayoutManager linerLayoutManager;
        private ImageView IvRecipe;
        #endregion

        #region Properties

        private RecipesModel _myRecipesModel;
        private RecipeResponse Response { get; set; }

        #endregion

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityRecipeDetail);
            _myRecipesModel = new RecipesModel(new RecipesService(DeviceManager.Instance));

            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
            await this.GetRecipe();
        }

        private async Task GetRecipe()
        {
            try
            {
                Bundle bundle = Intent.Extras;

                if (bundle != null && !string.IsNullOrEmpty(bundle.GetString(ConstantPreference.Recipe)))
                {
                    ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                    Response = await _myRecipesModel.GetRecipe(bundle.GetString(ConstantPreference.Recipe));

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
                        if (Response != null && Response.Recipe != null && Response.Recipe.Any())
                        {
                            SetRecipeInfo();
                            ShowBodyLayout();
                        }
                        else
                        {
                            ShowErrorLayout(AppMessages.RecipeNotFound);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.RecipeDetailActivity, ConstantMethodName.GetRecipe } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void SetRecipeInfo()
        {
            RecipeDetail recipeDetail = Response.Recipe[0];
            Glide.With(this).Load(recipeDetail.Image).Thumbnail(0.1f).Into(IvRecipe);
            TvRecipeName.Text = recipeDetail.Title;
            TvRecipeDescription.Text = recipeDetail.Subtitle;
            TvTime.Text = recipeDetail.PreparationTime;
            TvDifficulty.Text = recipeDetail.Difficulty;
            this.DrawIngredients(recipeDetail.Ingredients);
            this.DrawPreparation(recipeDetail.PreparationSteps);
        }

        private void DrawIngredients(IList<string> listStep)
        {

            linerLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvIngredients.NestedScrollingEnabled = false;
            RvIngredients.HasFixedSize = true;
            RvIngredients.SetLayoutManager(linerLayoutManager);
            RecipeStepIngredientsAdapter = new RecipeStepAdapter(listStep, this);
            RvIngredients.SetAdapter(RecipeStepIngredientsAdapter);
        }

        private void DrawPreparation(IList<string> listStep)
        {

            linerLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvPreparation.NestedScrollingEnabled = false;
            RvPreparation.HasFixedSize = true;
            RvPreparation.SetLayoutManager(linerLayoutManager);
            RecipeStepPreparationAdapter = new RecipeStepAdapter(listStep, this, true);
            RvPreparation.SetAdapter(RecipeStepPreparationAdapter);
        }

        protected override void EventError()
        {
            GetRecipe();
            base.EventError();
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));

            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                       FindViewById<RelativeLayout>(Resource.Id.rlBody));

            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            FindViewById<LinearLayout>(Resource.Id.lyUser).Visibility = ViewStates.Invisible;

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);

            TvRecipeName = FindViewById<TextView>(Resource.Id.tvRecipeName);
            TvRecipeDescription = FindViewById<TextView>(Resource.Id.tvRecipeDescription);
            TvDifficultyLabel = FindViewById<TextView>(Resource.Id.tvDifficultyLabel);
            TvDifficulty = FindViewById<TextView>(Resource.Id.tvDifficulty);
            TvTimeLabel = FindViewById<TextView>(Resource.Id.tvTimeLabel);
            TvTime = FindViewById<TextView>(Resource.Id.tvTime);

            TvIngredientsLabel = FindViewById<TextView>(Resource.Id.tvIngredientsLabel);
            RvIngredients = FindViewById<RecyclerView>(Resource.Id.rvIngredients);
            TvPreparationLabel = FindViewById<TextView>(Resource.Id.tvPreparationLabel);
            RvPreparation = FindViewById<RecyclerView>(Resource.Id.rvPreparation);
            IvRecipe = FindViewById<ImageView>(Resource.Id.ivRecipe);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void EditFonts()
        {
            TvRecipeName.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvIngredientsLabel.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvPreparationLabel.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvRecipeDescription.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTime.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTimeLabel.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvDifficulty.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvDifficultyLabel.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }
    }
}