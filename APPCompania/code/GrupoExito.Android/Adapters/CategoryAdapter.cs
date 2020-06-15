using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class CategoryAdapter : RecyclerView.Adapter
    {
        private IList<Category> Categories { get; set; }
        private Context Context { get; set; }
        private ICategoryAdapter CategoryInterface { get; set; }
       
        public override int ItemCount
        {
            get { return Categories != null ? Categories.Count() : 0; }
        }

        public CategoryAdapter(IList<Category> categories, Context context, ICategoryAdapter categoryInterface)
        {
            this.Categories = categories;
            this.Context = context;
            this.CategoryInterface = categoryInterface;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemCategory, parent, false);
            CategoryViewHolder categoryViewHolder = new CategoryViewHolder(itemView);
            return categoryViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
                .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.All)
                .DontAnimate()
                .DontTransform());

            CategoryViewHolder categoryViewHolder = holder as CategoryViewHolder;
            Category category = Categories[position];            
            Glide.With(holder.ItemView.Context).Load(category.ImageCategory).Apply(requestOptions).Thumbnail(0.5f).Into(categoryViewHolder.IvCategoryImage);
            Glide.With(holder.ItemView.Context).Load(category.IconCategory).Apply(requestOptions).Thumbnail(0.5f).Into(categoryViewHolder.IvCategoryIcon);
            categoryViewHolder.TvCategoryName.Text = category.Name;
            categoryViewHolder.TvCategoryName.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            categoryViewHolder.IvCategoryImage.Click += delegate
            {
                CategoryInterface.OnCategoryClicked(category);
            };
        }

        public override void OnViewRecycled(Object holder)
        {
            base.OnViewRecycled(holder);
            RecyclerView.ViewHolder rvHolder = holder as RecyclerView.ViewHolder;
            CategoryViewHolder categoryViewHolder = holder as CategoryViewHolder;
            Glide.With(rvHolder.ItemView.Context).Clear(categoryViewHolder.IvCategoryImage);
            Glide.With(rvHolder.ItemView.Context).Clear(categoryViewHolder.IvCategoryIcon);
        }
    }
}
