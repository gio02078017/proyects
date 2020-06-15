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
    public class ModificatedUnitsAdapter : RecyclerView.Adapter
    {
        private IList<SoldOut> ListSoldOut { get; set; }
        private ModificatedUnitsViewHolder _ModificatedUnitsViewHolder { get; set; }
        private Context Context { get; set; }

        public ModificatedUnitsAdapter(IList<SoldOut> listSoldOut, Context context)
        {
            this.Context = context;
            this.ListSoldOut = listSoldOut;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemModifiedProduct, parent, false);
            ModificatedUnitsViewHolder _ModificatedUnitsViewHolder = new ModificatedUnitsViewHolder(itemView, ListSoldOut);
            return _ModificatedUnitsViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _ModificatedUnitsViewHolder = holder as ModificatedUnitsViewHolder;
            SoldOut itemSoldOut = ListSoldOut[position];
            _ModificatedUnitsViewHolder.TvNameProduct.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            _ModificatedUnitsViewHolder.TvDescriptionProducts.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _ModificatedUnitsViewHolder.TvQuantityChange.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _ModificatedUnitsViewHolder.IvDelete.Visibility = ViewStates.Gone;
            _ModificatedUnitsViewHolder.TvQuantityChange.Visibility = ViewStates.Visible;

            _ModificatedUnitsViewHolder.TvQuantityChange.Text = itemSoldOut.Quantity.ToString();
            _ModificatedUnitsViewHolder.TvNameProduct.Text = itemSoldOut.Name;
            _ModificatedUnitsViewHolder.TvDescriptionProducts.Text = itemSoldOut.Presentation;
            Glide.With(AndroidApplication.Context).Load(itemSoldOut.ImagePath).Thumbnail(0.1f).Into(_ModificatedUnitsViewHolder.IvLista);
        }

        public override int ItemCount
        {
            get { return ListSoldOut != null ? ListSoldOut.Count() : 0; }
        }
    }
}