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
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class HistoricalProductAdapter : RecyclerView.Adapter
    {
        private IList<Product> Products { get; set; }
        private Context Context { get; set; }
        private IHistoricalProducts ProductsInterface { get; set; }

        public HistoricalProductAdapter(IList<Product> products, Context context, IHistoricalProducts productsInterface)
        {
            this.Products = products;
            this.Context = context;
            this.ProductsInterface = productsInterface;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemHistoricalOrderDetail, parent, false);
            HistoricalProductViewHolder HistoricalOrderHolder = new HistoricalProductViewHolder(itemView, ProductsInterface, Products);
            return HistoricalOrderHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
             .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.None)
             .DontAnimate()
             .DontTransform());

            HistoricalProductViewHolder productViewHolder = holder as HistoricalProductViewHolder;
            Product product = Products[position];
            productViewHolder.TvNameProductHistorical.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            productViewHolder.TvQuantityProductHistorical.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvPriceProductHistorical.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            productViewHolder.TvHowLikeHistorical.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            productViewHolder.TvMessageHistorical.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Italic);
            productViewHolder.CkProduct.Checked = product.Selected;
            Glide.With(holder.ItemView.Context).Load(product.UrlMediumImage).Apply(requestOptions).Thumbnail(0.5f).Into(productViewHolder.IvProduct);
            productViewHolder.TvNameProductHistorical.Text = product.Name;
            productViewHolder.TvQuantityProductHistorical.Text = Convert.ToString(product.Quantity) + " Unds.";
            productViewHolder.TvPriceProductHistorical.Text = product.SalePrice;

            if (product.Note != null)
            {
                productViewHolder.LyHowLike.Visibility = ViewStates.Visible;
                productViewHolder.TvMessageHistorical.Text = product.Note;
            }
            else
            {
                productViewHolder.LyHowLike.Visibility = ViewStates.Gone;
            }
        }

        public void EventCheck()
        {
        }

        public override int ItemCount
        {
            get { return Products != null ? Products.Count() : 0; }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            RecyclerView.ViewHolder rvHolder = holder as RecyclerView.ViewHolder;
            HistoricalProductViewHolder viewHolder = holder as HistoricalProductViewHolder;
            Glide.With(rvHolder.ItemView.Context).Clear(viewHolder.IvProduct);
        }
    }
}