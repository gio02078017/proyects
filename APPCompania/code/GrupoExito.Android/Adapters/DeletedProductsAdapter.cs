using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class DeletedProductsAdapter : RecyclerView.Adapter
    {
        private IList<SoldOut> ListSoldOut;
        private DeletedProductsViewHolder _DeletedProductsViewHolder;
        private Context Context { get; set; }

        public DeletedProductsAdapter(IList<SoldOut> listSoldOut, Context context)
        {
            this.Context = context;
            this.ListSoldOut = listSoldOut;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemModifiedProduct, parent, false);
            DeletedProductsViewHolder _DeletedProductsViewHolder = new DeletedProductsViewHolder(itemView, ListSoldOut);
            return _DeletedProductsViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _DeletedProductsViewHolder = holder as DeletedProductsViewHolder;
            SoldOut itemSoldOut = ListSoldOut[position];
            _DeletedProductsViewHolder.TvNameProduct.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            _DeletedProductsViewHolder.TvDescriptionProducts.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _DeletedProductsViewHolder.TvQuantityChange.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _DeletedProductsViewHolder.IvLista.Visibility = ViewStates.Visible;
            _DeletedProductsViewHolder.TvQuantityChange.Visibility = ViewStates.Gone;
            _DeletedProductsViewHolder.LyLista.SetBackgroundResource(Resource.Drawable.button_gray);

            _DeletedProductsViewHolder.TvNameProduct.Text = itemSoldOut.Name;
            _DeletedProductsViewHolder.TvDescriptionProducts.Text = itemSoldOut.Presentation;

            Glide.With(AndroidApplication.Context).Load(itemSoldOut.ImagePath).Thumbnail(0.1f).Into(_DeletedProductsViewHolder.IvLista);
        }

        public override int ItemCount
        {
            get { return ListSoldOut != null ? ListSoldOut.Count() : 0; }
        }
    }
}