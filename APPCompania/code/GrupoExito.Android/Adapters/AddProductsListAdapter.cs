
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities;
using GrupoExito.Utilities.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class AddProductsListAdapter : RecyclerView.Adapter
    {
        private IList<Product> ListProduct { get; set; }
        private IAddProductsList interfaceAddProductsList { get; set; }
        private AddProductsListViewHolder _AddProductsListViewHolder { get; set; }
        private Context Context { get; set; }

        public AddProductsListAdapter(IList<Product> ListProduct, Context context, IAddProductsList interfaceAddProductsList)
        {
            this.interfaceAddProductsList = interfaceAddProductsList;
            this.Context = context;
            this.ListProduct = ListProduct;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemProductLists, parent, false);
            AddProductsListViewHolder _AddProductsListViewHolder = new AddProductsListViewHolder(itemView, interfaceAddProductsList, ListProduct);
            return _AddProductsListViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
                 .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.All)
                 .Error(Resource.Drawable.sin_imagen_small)
                 .DontAnimate()
                 .DontTransform());

            AddProductsListViewHolder _AddProductsListViewHolder = holder as AddProductsListViewHolder;
            Product product = ListProduct[position];          
            
            _AddProductsListViewHolder.TvNameProduct.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            _AddProductsListViewHolder.TvDescriptionProducts.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _AddProductsListViewHolder.TvPriceProducts.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
             Glide.With(holder.ItemView.Context).Load(product.UrlMediumImage).Apply(requestOptions).Thumbnail(0.1f).Into(_AddProductsListViewHolder.IvIcon);
            _AddProductsListViewHolder.TvNameProduct.Text = product.Name;
            _AddProductsListViewHolder.TvPriceProducts.Text = StringFormat.ToPrice(product.Price.ActualPrice);
            _AddProductsListViewHolder.TvDescriptionProducts.Text = product.Presentation;

            if (product.Selected)
            {
                _AddProductsListViewHolder.LyIcon.SetBackgroundResource(Resource.Drawable.circle_white);
                _AddProductsListViewHolder.LyFather.SetBackgroundResource(Resource.Drawable.button_little_card_caducity);
                _AddProductsListViewHolder.IvCheck.SetImageResource(Resource.Drawable.seleccionar_primario);
                _AddProductsListViewHolder.LyCheck.SetBackgroundResource(Resource.Drawable.button_little_primary);
                _AddProductsListViewHolder.ViewLineEnd.Visibility = ViewStates.Gone;
            }
            else
            {
                _AddProductsListViewHolder.LyIcon.SetBackgroundResource(Android.Resource.Color.colorTransparent);
                _AddProductsListViewHolder.LyFather.SetBackgroundResource(Android.Resource.Color.colorTransparent);
                _AddProductsListViewHolder.IvCheck.SetImageResource(Resource.Drawable.mas_primario);
                _AddProductsListViewHolder.LyCheck.SetBackgroundResource(Resource.Drawable.button_little_transparent_with_border_primary);
                _AddProductsListViewHolder.ViewLineEnd.Visibility = ViewStates.Visible;
            }

        }

        public override int ItemCount
        {
            get { return ListProduct != null ? ListProduct.Count() : 0; }
        }     

    }
}