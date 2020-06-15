using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GrupoExito.Android.ViewHolders
{
    public class RecipesViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvRecipe { get; private set; }
        public TextView TvRecipeName { get; private set; }
        public TextView TvRecipeDifficultyLabel { get; private set; }
        public TextView TvRecipeDifficulty { get; private set; }
        public TextView TvRecipeTimeLabel { get; private set; }
        public TextView TvRecipeTime { get; private set; }
        public CardView CvRecipe { get; private set; }

        public RecipesViewHolder(View itemView) : base(itemView)
        {
            IvRecipe = itemView.FindViewById<ImageView>(Resource.Id.ivRecipe);
            TvRecipeName = itemView.FindViewById<TextView>(Resource.Id.tvRecipeName);
            TvRecipeDifficulty = itemView.FindViewById<TextView>(Resource.Id.tvDifficulty);
            TvRecipeDifficultyLabel = itemView.FindViewById<TextView>(Resource.Id.tvDifficultyLabel);
            TvRecipeTime = itemView.FindViewById<TextView>(Resource.Id.tvTime);
            TvRecipeTimeLabel = itemView.FindViewById<TextView>(Resource.Id.tvTimeLabel);
            CvRecipe = itemView.FindViewById<CardView>(Resource.Id.cvRecipe);
        }
    }
}