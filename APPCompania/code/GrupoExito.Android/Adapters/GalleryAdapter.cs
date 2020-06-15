using Android.Content;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Signature;
using GrupoExito.Android.Activities;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Utilities.Helpers;
using System;
using System.Collections.Generic;

namespace GrupoExito.Android.Adapters
{
    class GalleryAdapter : PagerAdapter
    {
        private Context Context { get; set; }
        private LayoutInflater LayoutInflater { get; set; }
        private IList<string> ImageList { get; set; }
        private bool IsImageDetail { get; set; }
        private IGalleryAdapyer InterfaceGalleryAdapter { get; set; }

        public GalleryAdapter(Context context, IList<string> imageList, bool? isImageDetail = false, IGalleryAdapyer iGalleryAdapter = null)
        {
            this.Context = context;
            this.ImageList = imageList;
            this.IsImageDetail = isImageDetail.Value;
            this.InterfaceGalleryAdapter = iGalleryAdapter;
        }

        public override int Count
        {
            get { return ImageList != null ? ImageList.Count : 0; }
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == obj;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            var requestOptions = new RequestOptions()
                .Apply(RequestOptions.SkipMemoryCacheOf(true))
                .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.None))
                .Apply(RequestOptions.SignatureOf(new ObjectKey(DateTime.Now.Millisecond))
                .DontAnimate());

            LayoutInflater = (LayoutInflater)Context.GetSystemService(ContextWrapper.LayoutInflaterService);
            View layoutGallery = LayoutInflater.Inflate(Resource.Layout.Layout_Gallery, null);
            ImageView ivGallery = (ImageView)layoutGallery.FindViewById<ImageView>(Resource.Id.ivGallery);
            Glide.With(AndroidApplication.Context).Load(ImageList[position]).Apply(requestOptions).Thumbnail(0.1f).Into(ivGallery);
            ivGallery.Click += delegate
            {
                if (!IsImageDetail)
                {
                    Intent intent = new Intent(Context, typeof(ImageDetailActivity));
                    intent.PutExtra(ConstantPreference.Gallery, JsonService.Serialize(ImageList));
                    intent.PutExtra(ConstantPreference.GalleryPosition, position);
                    Context.StartActivity(intent);
                }
                else
                {
                    InterfaceGalleryAdapter.OnImageTouched();
                }
            };

            ViewPager viewPager = (ViewPager)container;
            viewPager.AddView(layoutGallery, 0);
            return layoutGallery;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
        {
            ViewPager viewPager = (ViewPager)container;
            View view = (View)obj;
            viewPager.RemoveView(view);
        }
    }
}