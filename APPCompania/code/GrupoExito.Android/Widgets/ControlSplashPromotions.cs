using Android.App;
using Android.Content;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Entities.Entiites.Generic.Contents;
using System.Collections.Generic;
using static Android.Support.V4.View.ViewPager;

namespace GrupoExito.Android.Widgets
{
    public class ControlSplashPromotions : FrameLayout, IOnPageChangeListener
    {
        #region Controls

        private ViewPager viewPager;
        private TextView buttonHide;
        private TextView btnJump;
        private ViewPagerAdapter adapter;

        #endregion

        #region Properties

        private IList<Promotion> listPromotions;
        private Context context;
        private bool endPosition;

        #endregion

        public ControlSplashPromotions(Context context) : base(context)
        {
            Initialize(context);
        }

        public ControlSplashPromotions(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context);
        }

        public void JumpToPage()
        {
            viewPager.SetCurrentItem(viewPager.CurrentItem + 1, true);

            if (endPosition)
            {
                this.Visibility = ViewStates.Gone;
            }
        }

        public TextView GetButtonHide()
        {
            return buttonHide;
        }

        public void AddImagesArray(IList<Promotion> listPromotions)
        {
            this.listPromotions = listPromotions;
            adapter = new ViewPagerAdapter(context, this.listPromotions);
            viewPager.Adapter = adapter;
            viewPager.AddOnPageChangeListener(this);
        }

        public void OnPageScrollStateChanged(int state)
        {
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            HideOrShowBtn(position, listPromotions.Count);
        }

        public void OnPageSelected(int position)
        {
        }

        private void Initialize(Context context)
        {
            this.context = context;
            this.endPosition = false;
            string infService = Context.LayoutInflaterService;
            LayoutInflater li = (LayoutInflater)Application.Context.GetSystemService(infService);
            li.Inflate(Resource.Layout.layout_control_slider_promotions, this, true);
            viewPager = FindViewById<ViewPager>(Resource.Id.vpImages);
            buttonHide = FindViewById<TextView>(Resource.Id.btnDoNotShow);
            btnJump = FindViewById<TextView>(Resource.Id.btnJump);
            btnJump.Click += delegate
            {
                JumpToPage();
            };

            buttonHide.Visibility = ViewStates.Gone;
        }

        private void HideOrShowBtn(int position, int length)
        {
            if (position >= (length - 1))
            {
                buttonHide.Visibility = ViewStates.Visible;
                this.endPosition = true;
            }
            else
            {
                buttonHide.Visibility = ViewStates.Gone;
                this.endPosition = false;
            }
        }
    }
}