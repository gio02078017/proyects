using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Load.Resource.Drawable;
using Com.Bumptech.Glide.Load.Resource.Gif;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Request.Target;
using GrupoExito.Entities.Entiites.Generic.Contents;
using System;
using System.Collections.Generic;

namespace GrupoExito.Android.Adapters
{
    public class ViewPagerAdapter : PagerAdapter
    {
        #region Properties

        private Context _context { get; set; }
        private LayoutInflater layoutInflater;
        IList<Promotion> listPromotions;
        private ImageView load;
        private AnimationDrawable frameAnimation;

        #endregion

        public ViewPagerAdapter(Context context, IList<Promotion> listPromotions)
        {
            this._context = context;
            this.listPromotions = listPromotions;
        }

        public override int Count => listPromotions.Count;

        [Obsolete]
        public override Java.Lang.Object InstantiateItem(View container, int position)
        {
            layoutInflater = Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.layout_promotion_item, null);
            ImageView imageView = view.FindViewById<ImageView>(Resource.Id.ivImagePromotion);
            load = view.FindViewById<ImageView>(Resource.Id.load);
            DrawImagen(imageView, listPromotions[position].UrlImage);

            ViewPager vp = (ViewPager)container;
            vp.AddView(view, 0);
            return view;
        }

        [Obsolete]
        public override void DestroyItem(View container, int position, Java.Lang.Object @object)
        {
            ViewPager vp = (ViewPager)container;
            View view = (View)@object;
            vp.RemoveView(view);
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view.Equals(@object);
        }

        private void DrawImagen(ImageView imageView, string image)
        {
            load.SetBackgroundResource(Resource.Drawable.promotion_loader);
            frameAnimation = (AnimationDrawable)load.Background;
            frameAnimation.Start();

            load.Visibility = ViewStates.Visible;

            var requestOptions = new RequestOptions()
                .Apply(RequestOptions.SkipMemoryCacheOf(true))
                .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.Data));

            Glide.With(Application.Context)
                 .Load(image).Transition(DrawableTransitionOptions.WithCrossFade(2000))
                 .Apply(requestOptions)
                 .Listener(new MyRequestListener(frameAnimation, load)
                 {

                 })
                 .Into(imageView);

        }
    }

    public class MyRequestListener : Java.Lang.Object, IRequestListener
    {
        private AnimationDrawable frameAnimation;
        private ImageView load;

        public MyRequestListener(AnimationDrawable frameAnimation, ImageView load)
        {
            this.frameAnimation = frameAnimation;
            this.load = load;
        }

        public bool OnLoadFailed(GlideException p0, Java.Lang.Object p1, ITarget p2, bool p3)
        {

            return false;
        }

        public bool OnResourceReady(Java.Lang.Object p0, Java.Lang.Object p1, ITarget p2, DataSource p3, bool p4)
        {
            if (p0 is GifDrawable)
            {

                ((GifDrawable)p0).SetLoopCount(1);
            }
            frameAnimation.Stop();
            load.Visibility = ViewStates.Gone;
            return false;
        }
    }
}

