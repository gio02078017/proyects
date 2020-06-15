using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites.InStoreServices;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class StickersAdapter : RecyclerView.Adapter
    {
        private IList<Sticker> ListSticker;
        StickersViewHolder _StickersViewHolder;
        private Context Context { get; set; }

        public StickersAdapter(IList<Sticker> ListSticker, Context context)
        {
            this.Context = context;
            this.ListSticker = ListSticker;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemStickersInside, parent, false);
            StickersViewHolder _StickersViewHolder = new StickersViewHolder(itemView, ListSticker);
            return _StickersViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _StickersViewHolder = holder as StickersViewHolder;
            Sticker _Sticker = ListSticker[position];
            
            _StickersViewHolder.IvSticker.SetImageResource(_Sticker.Fill ? Resource.Drawable.sticker : Resource.Drawable.sticker_primario);
            _StickersViewHolder.TvNameSticker.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _StickersViewHolder.TvNameSticker.Text = _Sticker.Numer.ToString();
        }

        public override int ItemCount
        {
            get { return ListSticker != null ? ListSticker.Count() : 0; }
        }
    }
}