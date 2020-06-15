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
    public class RecipeCategoryAdapter : RecyclerView.Adapter
    {
        private IList<RecipeCategory> Categories { get; set; }
        private Context Context { get; set; }
        private IRecipeCategoryAdapter CategoryInterface { get; set; }

        public override int ItemCount
        {
            get { return Categories != null ? Categories.Count() : 0; }
        }

        public RecipeCategoryAdapter(IList<RecipeCategory> categories, Context context, IRecipeCategoryAdapter categoryInterface)
        {
            this.Categories = categories;
            this.Context = context;
            this.CategoryInterface = categoryInterface;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemRecipeCategory, parent, false);
            RecipeCategoryViewHolder categoryViewHolder = new RecipeCategoryViewHolder(itemView);
            categoryViewHolder.LyRecipeCategory.Click += delegate { CategoryInterface.OnCategoryClicked(Categories[categoryViewHolder.AdapterPosition]); };
            return categoryViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecipeCategoryViewHolder categoryViewHolder = holder as RecipeCategoryViewHolder;
            RecipeCategory category = Categories[position];
            Glide.With(Context).Load(category.Image).Thumbnail(0.1f).Into(categoryViewHolder.IvCategoryIcon);
            categoryViewHolder.TvCategoryTitle.Text = category.Name;
            categoryViewHolder.TvCategoryTitle.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            categoryViewHolder.TvCategoryDescription.Text = category.Description;
            categoryViewHolder.TvCategoryDescription.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }
    }
}
