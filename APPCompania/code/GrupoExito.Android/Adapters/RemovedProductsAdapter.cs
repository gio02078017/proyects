using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class RemovedProductsAdapter : RecyclerView.Adapter
    {
        private IList<Product> ListProduct;
        RemovedProductsViewHolder _RemovedProductsViewHolder;
        private Context Context { get; set; }

        public RemovedProductsAdapter(IList<Product> ListProduct, Context context)
        {
            this.Context = context;
            this.ListProduct = ListProduct;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemDeleteProduct, parent, false);
            RemovedProductsViewHolder _RemovedProductsViewHolder = new RemovedProductsViewHolder(itemView, ListProduct);
            return _RemovedProductsViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _RemovedProductsViewHolder = holder as RemovedProductsViewHolder;
            Product product = ListProduct[position];
            _RemovedProductsViewHolder.TvNameProduct.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            _RemovedProductsViewHolder.TvDescriptionProducts.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _RemovedProductsViewHolder.IvLista.Visibility = ViewStates.Visible;

            _RemovedProductsViewHolder.TvNameProduct.Text = product.Name;
            _RemovedProductsViewHolder.TvDescriptionProducts.Text = product.Description;
            Glide.With(AndroidApplication.Context).Load(product.UrlMediumImage).Thumbnail(0.1f).Into(_RemovedProductsViewHolder.IvLista);
        }

        public override int ItemCount
        {
            get { return ListProduct != null ? ListProduct.Count() : 0; }
        }
    }
}