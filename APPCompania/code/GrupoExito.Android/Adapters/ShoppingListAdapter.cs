using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Signature;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class ShoppingListAdapter : RecyclerView.Adapter
    {
        private IList<ShoppingList> ShoppingLists { get; set; }
        private IMyList InterfaceMyList { get; set; }
        private ShoppingListViewHolder _ShoppingListViewHolder { get; set; }
        private Context Context { get; set; }

        private Activity activity;

        public ShoppingListAdapter(IList<ShoppingList> shoppingList, Context context, Activity activity, IMyList interfaceMyList)
        {
            this.InterfaceMyList = interfaceMyList;
            this.Context = context;
            this.ShoppingLists = shoppingList;
            this.activity = activity;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemMyLists, parent, false);
            ShoppingListViewHolder _MyListViewHolder = new ShoppingListViewHolder(itemView, InterfaceMyList, ShoppingLists);
            return _MyListViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _ShoppingListViewHolder = holder as ShoppingListViewHolder;
            ShoppingList shoppingList = ShoppingLists[position];
            _ShoppingListViewHolder.TvNameList.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            _ShoppingListViewHolder.TvNameProductsList.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _ShoppingListViewHolder.TvQuantityList.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _ShoppingListViewHolder.TvNameList.Text = shoppingList.Name;
            _ShoppingListViewHolder.TvQuantityList.Text = shoppingList.QuantityProducts.ToString();
            _ShoppingListViewHolder.TvNameProductsList.Text = shoppingList.QuantityProducts.Equals("1") ? Context.Resources.GetString(Resource.String.str_product_title): Context.Resources.GetString(Resource.String.str_products_title);
            SetListIcon(holder, _ShoppingListViewHolder.IvLista, shoppingList.Description);
        }

        private void SetListIcon(RecyclerView.ViewHolder viewHolder, ImageView ivCategory, string description)
        {
            var requestOptions = new RequestOptions()
                .Apply(RequestOptions.SkipMemoryCacheOf(true))
                .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.None))
                .Apply(RequestOptions.SignatureOf(new ObjectKey(DateTime.Now.Millisecond)));

            var resource = ConvertUtilities.ResourceId(description.ToLower());

            if (resource != 0)
            {
                Glide.With(viewHolder.ItemView.Context).Load(resource).Apply(requestOptions).Thumbnail(0.5f).Into(ivCategory);
            }
            else
            {
                Glide.With(viewHolder.ItemView.Context).Load(Resource.Drawable.bookmark).Apply(requestOptions).Thumbnail(0.5f).Into(ivCategory);
            }
        }

        public override int ItemCount
        {
            get { return ShoppingLists != null ? ShoppingLists.Count() : 0; }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            RecyclerView.ViewHolder rvHolder = holder as RecyclerView.ViewHolder;
            ShoppingListViewHolder viewHolder = holder as ShoppingListViewHolder;
            if (activity != null && !activity.IsDestroyed)
            {
                Glide.With(rvHolder.ItemView.Context).Clear(viewHolder.IvLista);
            }
        }
    }
}