using Android.Content;
using Android.Graphics;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Utilities;
using GrupoExito.Entities;
using System.Collections.Generic;

namespace GrupoExito.Android.Adapters
{
    class TextGalleryAdapter : PagerAdapter
    {
        private Context Context;
        private LayoutInflater LayoutInflater;
        private IList<TextGallery> TextList;

        public TextGalleryAdapter(Context context, IList<TextGallery> TextList)
        {
            this.Context = context;
            this.TextList = TextList;
        }

        public override int Count
        {
            get { return TextList != null ? TextList.Count : 0; }
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == obj;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            LayoutInflater = (LayoutInflater)Context.GetSystemService(ContextWrapper.LayoutInflaterService);
            View layoutTextGallery = LayoutInflater.Inflate(Resource.Layout.LayoutTextGallery, null);
            TextView tvGalleryTitle = layoutTextGallery.FindViewById<TextView>(Resource.Id.tvGalleryTitle);
            TextView tvGalleryDescription = layoutTextGallery.FindViewById<TextView>(Resource.Id.tvGalleryDescription);

            tvGalleryTitle.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvGalleryDescription.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);

            tvGalleryTitle.Text = TextList[position].Title;
            tvGalleryDescription.Text = TextList[position].Description;

            ViewPager viewPager = (ViewPager)container;
            viewPager.AddView(layoutTextGallery, 0);
            return layoutTextGallery;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
        {
            ViewPager viewPager = (ViewPager)container;
            View view = (View)obj;
            viewPager.RemoveView(view);
        }
    }
}