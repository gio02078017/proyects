using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Entities.Entiites.Generic.Contents;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class BannerViewHolder : RecyclerView.ViewHolder
    {
        public CardView CvBanner { get; private set; }
        public ImageView IvBanner { get; private set; }

        public LinearLayout LyBanner { get; private set; }


        public BannerViewHolder(View itemView, IBanner bannerInterface, IList<BannerPromotion> bannerItems) : base(itemView)
        {
            CvBanner = itemView.FindViewById<CardView>(Resource.Id.cvBanner);
            IvBanner = itemView.FindViewById<ImageView>(Resource.Id.ivBanner);
            LyBanner = itemView.FindViewById<LinearLayout>(Resource.Id.lyBanner);
        }
    }
}