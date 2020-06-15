using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Bumptech.Glide;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class RecipeAdapter : RecyclerView.Adapter
    {
        private IList<Recipe> Recipes { get; set; }
        private Context Context { get; set; }
        private IRecipeAdapter RecipeInterface { get; set; }

        public override int ItemCount
        {
            get { return Recipes != null ? Recipes.Count() : 0; }
        }

        public RecipeAdapter(IList<Recipe> categories, Context context, IRecipeAdapter recipeInterface)
        {
            this.Recipes = categories;
            this.Context = context;
            this.RecipeInterface = recipeInterface;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemRecipe, parent, false);
            RecipesViewHolder recipeViewHolder = new RecipesViewHolder(itemView);
            recipeViewHolder.CvRecipe.Click += delegate { RecipeInterface.OnRecipeClicked(Recipes[recipeViewHolder.AdapterPosition]); };
            return recipeViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecipesViewHolder categoryViewHolder = holder as RecipesViewHolder;
            Recipe recipe = Recipes[position];

            categoryViewHolder.CvRecipe.PreventCornerOverlap = false;
            Glide.With(Context).Load(recipe.ShortImage).Thumbnail(0.1f).Into(categoryViewHolder.IvRecipe);
            categoryViewHolder.TvRecipeName.Text = recipe.Title;
            categoryViewHolder.TvRecipeDifficulty.Text = recipe.Difficulty;
            categoryViewHolder.TvRecipeTime.Text = recipe.PreparationTime;

            categoryViewHolder.TvRecipeName.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            categoryViewHolder.TvRecipeDifficulty.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            categoryViewHolder.TvRecipeDifficultyLabel.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            categoryViewHolder.TvRecipeTime.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            categoryViewHolder.TvRecipeTimeLabel.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }
    }
}
