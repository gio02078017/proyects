using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GrupoExito.Android.ViewHolders
{
    public class RecipeCategoryViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvCategoryIcon { get; private set; }
        public TextView TvCategoryTitle { get; private set; }
        public TextView TvCategoryDescription { get; private set; }
        public LinearLayout LyRecipeCategory { get; private set; }

        public RecipeCategoryViewHolder(View itemView) : base(itemView)
        {
            IvCategoryIcon = itemView.FindViewById<ImageView>(Resource.Id.ivCategory);
            TvCategoryTitle = itemView.FindViewById<TextView>(Resource.Id.tvCategoryTitle);
            TvCategoryDescription = itemView.FindViewById<TextView>(Resource.Id.tvCategoryContent);
            LyRecipeCategory = itemView.FindViewById<LinearLayout>(Resource.Id.lyRecipeCategory);
        }
    }
}