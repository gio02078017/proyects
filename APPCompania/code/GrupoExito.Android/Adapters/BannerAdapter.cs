using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Load.Engine.Bitmap_recycle;
using Com.Bumptech.Glide.Load.Resource.Bitmap;
using Com.Bumptech.Glide.Load.Resource.Drawable;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Generic.Contents;
using Java.Security;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class BannerAdapter : RecyclerView.Adapter
    {
        private IList<BannerPromotion> BannerList { get; set; }
        private IBanner BannerInterface { get; set; }
        private BannerViewHolder BannerViewHolder { get; set; }
        private Context Context { get; set; }

        public BannerAdapter(IList<BannerPromotion> bannerList, Context context, IBanner bannerInterface)
        {
            this.BannerInterface = bannerInterface;
            this.Context = context;
            this.BannerList = bannerList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemBanner, parent, false);
            BannerViewHolder bannerViewHolder = new BannerViewHolder(itemView, BannerInterface, BannerList);
            bannerViewHolder.CvBanner.Click += delegate { BannerInterface.OnSelectBannerItem(BannerList[bannerViewHolder.AdapterPosition]); };
            return bannerViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
                .Placeholder(Resource.Drawable.banners_home__stickers_gris)
                .Error(Resource.Drawable.banners_home__stickers_gris)
                .Fallback(Resource.Drawable.banners_home__stickers_gris)
                .Transform(new MyTransformation(Context))
                .Apply(RequestOptions.SkipMemoryCacheOf(true))
                .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.Data))                
                .FitCenter();

            BannerViewHolder = holder as BannerViewHolder;
            BannerPromotion bannerItem = BannerList[position];

            ChangeSize();

            Glide.With(AndroidApplication.Context).Load(bannerItem.Image).Transition(DrawableTransitionOptions.WithCrossFade(2000)).Apply
                (requestOptions).Thumbnail(0.5f).Into(BannerViewHolder.IvBanner);

        }

        public override int ItemCount
        {
            get { return BannerList != null ? BannerList.Count() : 0; }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            RecyclerView.ViewHolder rvHolder = holder as RecyclerView.ViewHolder;
            BannerViewHolder viewHolder = holder as BannerViewHolder;
            Glide.With(rvHolder.ItemView.Context).Clear(viewHolder.IvBanner);
        }

        public void ChangeSize()
        {
            var metrics = Context.Resources.DisplayMetrics;
            var height = metrics.HeightPixels;
            var width = metrics.WidthPixels;
            int widthBaner = (width * 80) / 100;
            LinearLayout.LayoutParams linearLayoutParams = new LinearLayout.LayoutParams(widthBaner,
            LinearLayout.LayoutParams.WrapContent);
            linearLayoutParams.SetMargins(0, 0, 0, 0);
            BannerViewHolder.LyBanner.LayoutParameters = linearLayoutParams;
        }

        private class MyTransformation : BitmapTransformation
        {
            public MyTransformation(Context context) : base(context)
            {

            }

            public override void UpdateDiskCacheKey(MessageDigest p0)
            {
            }

            protected override Bitmap Transform(IBitmapPool bitmapPool, Bitmap original, int width, int height)
            {
                Bitmap result = bitmapPool.Get(width, height, Bitmap.Config.Argb8888);
                if (result == null)
                {
                    result = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                }
                Canvas canvas = new Canvas(result);
                Paint paint = new Paint
                {
                    Alpha = 128
                };
                canvas.DrawBitmap(original, 0, 0, paint);
                return result;
            }
        }
    }
}