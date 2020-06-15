using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Signature;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class GetSoatAdapter : RecyclerView.Adapter
    {
        private IList<Soat> ListSoat { get; set; }
        private IGetSoats getSoatInterface { get; set; }
        private GetSoatViewHolder _GetSoatViewHolder { get; set; }
        private Context Context { get; set; }

        public GetSoatAdapter(IList<Soat> ListSoat, Context Context, IGetSoats getSoatInterface)
        {
            this.getSoatInterface = getSoatInterface;
            this.Context = Context;
            this.ListSoat = ListSoat;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemGetSoats, parent, false);
            GetSoatViewHolder _GetSoatViewHolder = new GetSoatViewHolder(itemView, getSoatInterface, ListSoat);

            return _GetSoatViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
                .Apply(RequestOptions.SkipMemoryCacheOf(true))
                .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.None))
                .Apply(RequestOptions.SignatureOf(new ObjectKey(DateTime.Now.Millisecond)));

            _GetSoatViewHolder = holder as GetSoatViewHolder;
            Soat itemSoat = ListSoat[position];

            _GetSoatViewHolder.TvPlateSoat.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            _GetSoatViewHolder.TvPlateSoat.Text = itemSoat.LicensePlate;
        }

        public override int ItemCount
        {
            get { return ListSoat != null ? ListSoat.Count() : 0; }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
        }
    }
}