using Android.Content;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using GrupoExito.Entities.Entiites.InStoreServices;
using System.Collections.Generic;

namespace GrupoExito.Android.Adapters
{
    class GalleryStickersAdapter : PagerAdapter
    {
        private Context Context { get; set; }
        private LayoutInflater LayoutInflater { get; set; }
        private IList<StickersByPage> ListStickersByPage { get; set; }
        private bool IsImageDetail { get; set; }
        private IGalleryAdapyer InterfaceGalleryAdapter { get; set; }

        private StickersAdapter _StickersAdapter;
        private RecyclerView RvMyStickersList;
        private LinearLayoutManager linerLayoutManager { get; set; }
        private GridLayoutManager manager;

        public GalleryStickersAdapter(Context context, IList<StickersByPage> listStickersByPage, IGalleryAdapyer iGalleryAdapter = null)
        {
            this.Context = context;
            this.ListStickersByPage = listStickersByPage;
            this.InterfaceGalleryAdapter = iGalleryAdapter;
        }

        public override int Count
        {
            get { return ListStickersByPage != null ? ListStickersByPage.Count : 0; }
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == obj;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            LayoutInflater = (LayoutInflater)Context.GetSystemService(ContextWrapper.LayoutInflaterService);
            View layoutGallery = LayoutInflater.Inflate(Resource.Layout.ListItemStickersOutside, null);
            RvMyStickersList = layoutGallery.FindViewById<RecyclerView>(Resource.Id.rvMyStickersList);
            StickersByPage stickersByPage = ListStickersByPage[position];

            manager = new GridLayoutManager(Context, 5, GridLayoutManager.Vertical, false)
            {
                AutoMeasureEnabled = true
            };

            RvMyStickersList.NestedScrollingEnabled = false;
            RvMyStickersList.HasFixedSize = false;
            RvMyStickersList.SetLayoutManager(manager);
            _StickersAdapter = new StickersAdapter(stickersByPage.Stickers, Context);
            RvMyStickersList.SetAdapter(_StickersAdapter);

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