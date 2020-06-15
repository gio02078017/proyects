using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GrupoExito.Android.ViewHolders
{
    public class CategoryViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvCategoryIcon { get; private set; }
        public TextView TvCategoryName { get; private set; }
        public ImageView IvCategoryImage { get; private set; }

        public CategoryViewHolder(View itemView) : base(itemView)
        {
            IvCategoryIcon = itemView.FindViewById<ImageView>(Resource.Id.ivCategoryIcon);
            TvCategoryName = itemView.FindViewById<TextView>(Resource.Id.tvCategoryName);
            IvCategoryImage = itemView.FindViewById<ImageView>(Resource.Id.ivCategoryImage);
        }
    }
}