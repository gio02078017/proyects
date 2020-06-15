using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class FilterAdapter : RecyclerView.Adapter
    {
        private IList<ProductFilter> Categories { get; set; }
        private Context Context { get; set; }
        private bool IsCategory { get; set; }
        private IFilterAdapter CategoryInterface { get; set; }

        public FilterAdapter(IList<ProductFilter> categories, Context context, IFilterAdapter categoryInterface, bool isCategory)
        {
            this.Categories = categories;
            this.Context = context;
            this.CategoryInterface = categoryInterface;
            this.IsCategory = isCategory;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemFilters, parent, false);
            FilterViewHolder filterViewHolder = new FilterViewHolder(itemView);
            return filterViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            FilterViewHolder filterViewHolder = holder as FilterViewHolder;
            ProductFilter productFilter = Categories[position];
            filterViewHolder.ChkFilterCategory.Text = productFilter.Key + " (" + productFilter.Quantity + ")";
            filterViewHolder.ChkFilterCategory.Checked = productFilter.Checked;

            if (productFilter.Checked)
            {
                filterViewHolder.ChkFilterCategory.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            }
            else
            {
                filterViewHolder.ChkFilterCategory.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            }

            filterViewHolder.ChkFilterCategory.CheckedChange += delegate
            {
                if (filterViewHolder.ChkFilterCategory.Checked)
                {
                    filterViewHolder.ChkFilterCategory.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
                }
                else
                {
                    filterViewHolder.ChkFilterCategory.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                }

                if (IsCategory)
                {
                    CategoryInterface.OnCategoryChecked(productFilter);
                }
                else
                {
                    CategoryInterface.OnBrandChecked(productFilter);
                }
            };
        }

        public override int ItemCount
        {
            get { return Categories != null ? Categories.Count() : 0; }
        }
    }
}
